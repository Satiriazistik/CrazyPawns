using UnityEngine;

namespace Game.PlayerInput
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector3 MousePosition { get; private set; }

        public bool IsInteractionStarted { get; private set; }
        public bool IsInteractionHeld { get; private set; }
        public bool IsInteractionEnded { get; private set; }

        [SerializeField]
        private KeyCode interactionKeyCode = KeyCode.Mouse0;
        
        public void HandleUpdate()
        {
            MousePosition = Input.mousePosition;
            
            IsInteractionStarted = Input.GetKeyDown(interactionKeyCode);
            IsInteractionHeld = Input.GetKey(interactionKeyCode);
            IsInteractionEnded = Input.GetKeyUp(interactionKeyCode);
        }
    }
}