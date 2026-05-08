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

        private void Update()
        {
            inputHandler.HandleUpdate();

            for (int i = 0; i < objectInteractionHandlers.Length; i++)
            {
                var handler = objectInteractionHandlers[i];
                handler.HandleUpdate(inputHandler);
            }
        }
    }
}