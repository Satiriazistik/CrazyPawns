using System;
using PlayerInput;
using UnityEngine;

namespace Interaction
{
    public class PlayerCameraController : PlayerInteractionHandler
    {
        [SerializeField]
        private float zoomMaxDistance;
        [SerializeField]
        private float zoomMinDistance;
        [SerializeField]
        private float zoomSpeed;
        [SerializeField]
        private float cameraMoveSpeed;

        private Transform _cameraTransform;

        private Vector3 _targetCameraPosition;

        protected override void Awake()
        {
            base.Awake();
            _cameraTransform = playerCamera.transform;
        }

        protected override void OnInteractionUpdate(IPlayerInput inputHandler)
        {
            base.OnInteractionUpdate(inputHandler);
            CameraZoomUpdate(inputHandler.ScrollDelta, inputHandler.MousePosition);
        }

        protected override void OnInteractionStarted(IPlayerInput inputHandler)
        {
            
        }

        protected override void OnInteractionHeld(IPlayerInput inputHandler)
        {
            if (InteractionBlocked)
                return;

            CameraMoveUpdate(inputHandler.MouseDelta);
        }

        protected override void OnInteractionEnded(IPlayerInput inputHandler)
        {
            
        }

        private void CameraMoveUpdate(Vector3 mouseDelta)
        {
            var forward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
            if (Mathf.Approximately(forward.sqrMagnitude, 0f))
                forward = Vector3.ProjectOnPlane(_cameraTransform.up, Vector3.up).normalized;
            var right = _cameraTransform.right;
            var cameraMove = -(right * mouseDelta.x + forward * mouseDelta.y) * cameraMoveSpeed;

            _cameraTransform.position += cameraMove;
        }

        private void CameraZoomUpdate(float scrollDelta, Vector3 mousePosition)
        {
            if (Mathf.Approximately(scrollDelta, 0f))
                return;

            if (!TryGetMouseWorldPoint(mousePosition, out var worldPoint))
                return;

            var direction = (worldPoint - _cameraTransform.position).normalized;
            var nextPosition = _cameraTransform.position + direction * (scrollDelta * zoomSpeed);
            var distanceToPlane = nextPosition.y;
            if (distanceToPlane < zoomMinDistance || distanceToPlane > zoomMaxDistance)
                return;

            _cameraTransform.position = nextPosition;
        }
        
    }
}