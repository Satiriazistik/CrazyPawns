using UnityEngine;

namespace PlayerInput
{
    public class PlayerInputHandler : MonoBehaviour, IPlayerInput
    {
        public Vector3 MousePosition { get; private set; }
        public Vector3 MouseDelta { get; private set; }

        public bool IsInteractionStarted { get; private set; }
        public bool IsInteractionHeld { get; private set; }
        public bool IsInteractionEnded { get; private set; }
        
        public float ScrollDelta { get; private set; }

        [SerializeField]
        private KeyCode interactionKeyCode = KeyCode.Mouse0;
        
        private Vector3 _previousMousePosition;
        
        public void HandleUpdate()
        {
            MousePosition = Input.mousePosition;

            MouseDelta = MousePosition - _previousMousePosition;
            
            _previousMousePosition = MousePosition;

            ScrollDelta = Input.mouseScrollDelta.y;
            
            IsInteractionStarted = Input.GetKeyDown(interactionKeyCode);
            IsInteractionHeld = Input.GetKey(interactionKeyCode);
            IsInteractionEnded = Input.GetKeyUp(interactionKeyCode);
        }
    }
}