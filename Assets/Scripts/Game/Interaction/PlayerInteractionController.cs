using Game.PlayerInput;
using UnityEngine;

namespace Game.Interaction
{
    public class PlayerInteractionController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInputHandler inputHandler;
        
        [SerializeField] 
        private PlayerInteractionHandler[] interactionHandlers;
        
        private void Update()
        {
            inputHandler.HandleUpdate();
            
            for (int i = 0; i < interactionHandlers.Length; i++)
                interactionHandlers[i].HandleUpdate(inputHandler);
        }
    }
}