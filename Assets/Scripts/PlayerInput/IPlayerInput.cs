using UnityEngine;

namespace PlayerInput
{
    public interface IPlayerInput
    {
        Vector3 MousePosition { get; }
        Vector3 MouseDelta { get; }
        float ScrollDelta { get; }

        bool IsInteractionStarted { get; }
        bool IsInteractionHeld { get; }
        bool IsInteractionEnded { get; }
    }
}