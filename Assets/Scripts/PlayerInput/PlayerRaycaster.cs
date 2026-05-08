using UnityEngine;

namespace PlayerInput
{
    public class PlayerRaycaster
    {
        private Camera _playerCamera;
        private LayerMask _raycastLayers;
        private float _raycastDistance;
        
        public PlayerRaycaster(Camera playerCamera, LayerMask raycastLayers, float raycastDistance)
        {
            _playerCamera = playerCamera;
            _raycastLayers = raycastLayers;
            _raycastDistance = raycastDistance;
        }
        
        public bool DoCameraMouseRaycast(Vector3 mousePosition, out RaycastHit hitInfo)
        {
            var cameraRay = _playerCamera.ScreenPointToRay(mousePosition);

            return Physics.Raycast(cameraRay, out hitInfo, _raycastDistance, _raycastLayers);
        }
    }
}