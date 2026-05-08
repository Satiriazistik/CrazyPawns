using Board;
using Pawn;
using PlayerInput;
using UnityEngine;

namespace Interaction
{
    public class PawnDragController : PlayerInteractionHandler
    {
        private PawnController _currentPawn;

        private BoardBounds _boardBounds;
        
        public void Initialize(BoardBounds boardBounds)
        {
            _boardBounds = boardBounds;
            
            _playerRaycaster = new PlayerRaycaster(playerCamera, raycastTargetLayers, raycastMaxDistance);
        }
        
        protected override void OnInteractionStarted(IPlayerInput inputHandler)
        {
            if (!_playerRaycaster.DoCameraMouseRaycast(inputHandler.MousePosition, out var hitInfo))
                return;

            if (!hitInfo.collider.gameObject.TryGetComponent<PawnBody>(out var pawnBody))
                return;

            if (pawnBody.Owner == null)
            {
                Debug.LogError($"{nameof(PawnBody)} has no owner assigned.", pawnBody);
                return;
            }

            _currentPawn = pawnBody.Owner;

            InteractionIsActive = true;
        }

        protected override void OnInteractionHeld(IPlayerInput inputHandler)
        {
            if (_currentPawn == null)
                return;

            if (TryGetMouseWorldPoint(inputHandler.MousePosition, out var pawnPosition))
            {
                _currentPawn.PawnTransform.position = pawnPosition;
                
                bool isOnBoard = _boardBounds.Contains(pawnPosition);
                _currentPawn.SetState(isOnBoard ? PawnState.OnBoard : PawnState.OutOfBoard);
            }
        }

        protected override void OnInteractionEnded(IPlayerInput inputHandler)
        {
            if (_currentPawn == null)
                return;
            
            if (_currentPawn.CurrentPawnState == PawnState.OutOfBoard)
                _currentPawn.DestroyPawn();
            
            _currentPawn = null;
            
            InteractionIsActive = false;
        }
    }
}