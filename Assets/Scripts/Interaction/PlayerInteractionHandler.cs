using PlayerInput;
using UnityEngine;

namespace Interaction
{
    public abstract class PlayerInteractionHandler : MonoBehaviour
    {
        public bool InteractionIsActive { get; protected set; }

        public bool InteractionBlocked { get; set; }

        [SerializeField]
        protected Camera playerCamera;
        
        [SerializeField]
        protected LayerMask raycastTargetLayers;
        
        [SerializeField]
        protected float raycastMaxDistance = 50f;
        
        protected PlayerRaycaster _playerRaycaster;
        
        private Plane _worldPlane;

        protected virtual void Awake()
        {
            _worldPlane = new Plane(Vector3.up, Vector3.zero);
        }

        public void HandleUpdate(IPlayerInput inputHandler)
        {
            OnInteractionUpdate(inputHandler);
            
            if (inputHandler.IsInteractionStarted)
                OnInteractionStarted(inputHandler);

            if (inputHandler.IsInteractionHeld)
                OnInteractionHeld(inputHandler);

            if (inputHandler.IsInteractionEnded)
                OnInteractionEnded(inputHandler);
        }
        
        protected virtual void OnInteractionUpdate(IPlayerInput inputHandler) { }
        
        protected abstract void OnInteractionStarted(IPlayerInput inputHandler);
        protected abstract void OnInteractionHeld(IPlayerInput inputHandler);
        protected abstract void OnInteractionEnded(IPlayerInput inputHandler);
        
        protected bool TryGetMouseWorldPoint(Vector3 mousePosition, out Vector3 point)
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