using Game.PlayerInput;
using UnityEngine;

namespace Game.Interaction
{
    public abstract class PlayerInteractionHandler : MonoBehaviour
    {
        [SerializeField]
        protected LayerMask raycastTargetLayers;
        
        [SerializeField]
        protected float raycastMaxDistance = 50f;
        
        protected PlayerRaycaster _playerRaycaster;
        
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
        
        protected virtual void OnInteractionUpdate(IPlayerInput input) { }
        
        protected abstract void OnInteractionStarted(IPlayerInput inputHandler);
        protected abstract void OnInteractionHeld(IPlayerInput inputHandler);
        protected abstract void OnInteractionEnded(IPlayerInput inputHandler);
    }
}