using PlayerInput;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interaction
{
    public class PlayerInteractionController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputHandler inputHandler;
        
        [SerializeField] 
        private PlayerInteractionHandler[] objectInteractionHandlers;

        [SerializeField]
        private PlayerInteractionHandler playerCameraController;

        private void Update()
        {
            inputHandler.HandleUpdate();

            var objectInteractionIsActive = false;
            for (int i = 0; i < objectInteractionHandlers.Length; i++)
            {
                var handler = objectInteractionHandlers[i];
                handler.HandleUpdate(inputHandler);
                objectInteractionIsActive |= handler.InteractionIsActive;
            }

            playerCameraController.InteractionBlocked = objectInteractionIsActive;
            playerCameraController.HandleUpdate(inputHandler);
        }
    }
}