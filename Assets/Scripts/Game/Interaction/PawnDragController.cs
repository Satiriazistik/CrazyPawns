using Game.Board;
using Game.Pawn;
using Game.PlayerInput;
using UnityEngine;

namespace Game.Interaction
{
    public class PawnDragController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputHandler inputHandler;

        [SerializeField]
        private Camera playerCamera;

        [SerializeField]
        private LayerMask raycastTargetLayers;

        [SerializeField]
        private float raycastMaxDistance = 50f;
        
        private PlayerRaycaster _playerRaycaster;

        private PawnController _currentPawn;

        private BoardBounds _boardBounds;

        private Plane _worldPlane;

        private bool _initialized;

        public void Initialize(BoardBounds boardBounds)
        {
            _boardBounds = boardBounds;
            
            _playerRaycaster = new PlayerRaycaster(playerCamera, raycastTargetLayers, raycastMaxDistance);

            _worldPlane = new Plane(Vector3.up, Vector3.zero);
            
            _initialized = true;
        }
        
        private void Update()
        {
            if (!_initialized)
                return;
            
            inputHandler.HandleUpdate();
            PlayerDragUpdate();
        }

        private void PlayerDragUpdate()
        {
            if (inputHandler.IsInteractionStarted)
                PlayerInteractionStarted();
            
            if (inputHandler.IsInteractionHeld)
                PlayerInteractionHeld();
            
            if (inputHandler.IsInteractionEnded)
                PlayerInteractionEnded();
        }
        
        private void PlayerInteractionStarted()
        {
            if (!_playerRaycaster.DoCameraMouseRaycast(inputHandler.MousePosition, out var hitInfo))
                return;

            var pawnController = hitInfo.collider.transform.GetComponentInParent<PawnController>();
            if (pawnController == null)
            {
                Debug.LogError($"Collider: {hitInfo.collider.name} is on Pawn layer, but has no {nameof(PawnController)} component in parent. Pawn dragging is impossible.", hitInfo.collider);
                return;
            }

            _currentPawn = pawnController;
        }

        private void PlayerInteractionHeld()
        {
            if (_currentPawn == null)
                return;

            if (TryGetMouseWorldPoint(out var pawnPosition))
            {
                _currentPawn.PawnTransform.position = pawnPosition;
                
                bool isOnBoard = _boardBounds.Contains(pawnPosition);
                _currentPawn.SetState(isOnBoard ? PawnState.OnBoard : PawnState.OutOfBoard);
            }
        }

        private void PlayerInteractionEnded()
        {
            if (_currentPawn == null)
                return;
            
            if (_currentPawn.CurrentPawnState == PawnState.OutOfBoard)
                Destroy(_currentPawn.gameObject);
            
            _currentPawn = null;
        }
        
        private bool TryGetMouseWorldPoint(out Vector3 point)
        {
            var ray = playerCamera.ScreenPointToRay(inputHandler.MousePosition);

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