using UnityEngine;

namespace Game.PlayerInput
{
    public interface IPlayerInput
    {
        Vector3 MousePosition { get; }
        bool IsInteractionStarted { get; }
        bool IsInteractionHeld { get; }
        bool IsInteractionEnded { get; }
    }
}