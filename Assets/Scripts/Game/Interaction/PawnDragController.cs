using Game.Board;
using Game.Pawn;
using Game.PlayerInput;
using UnityEngine;

namespace Game.Interaction
{
    public class PawnDragController : PlayerInteractionHandler
    {
        [SerializeField]
        private Camera playerCamera;

        private PawnController _currentPawn;

        private BoardBounds _boardBounds;

        private Plane _worldPlane;

        public void Initialize(BoardBounds boardBounds)
        {
            _boardBounds = boardBounds;
            
            _playerRaycaster = new PlayerRaycaster(playerCamera, raycastTargetLayers, raycastMaxDistance);

            _worldPlane = new Plane(Vector3.up, Vector3.zero);
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
        }
        
        private bool TryGetMouseWorldPoint(Vector3 mousePosition, out Vector3 point)
        {
            var ray = playerCamera.ScreenPointToRay(mousePosition);

            if (_worldPlane.Raycast(ray, out var distance))
            {
                point = ray.GetPoint(distance);
                return true;
            }

            point = default;
            return false;
        }
        
    }
}