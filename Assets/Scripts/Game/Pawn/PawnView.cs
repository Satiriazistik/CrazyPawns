using UnityEngine;

namespace Game.Pawn
{
    public class PawnView : MonoBehaviour
    {
        private Material _deleteMaterial;
        private Material _activeConnectorMaterial;

        [SerializeField]
        private Renderer[] pawnGraphics;

        [SerializeField]
        private ConnectorVisualBinding[] connectorVisualBindings;

        private Material[] _basePawnMaterials;
        private Material[] _baseConnectorMaterials;
        
        public void Initialize(Material deleteMaterial, Material activeConnectorMaterial)
        {
            _deleteMaterial = deleteMaterial;
            _activeConnectorMaterial = activeConnectorMaterial;

            _basePawnMaterials = new Material[pawnGraphics.Length];
            for (int i = 0; i < _basePawnMaterials.Length; i++)
                _basePawnMaterials[i] = pawnGraphics[i].sharedMaterial;

            _baseConnectorMaterials = new Material[connectorVisualBindings.Length];
            for (int i = 0; i < _baseConnectorMaterials.Length; i++)
                _baseConnectorMaterials[i] = connectorVisualBindings[i].Renderer.sharedMaterial;
        }

        public void ApplyDeleteMaterial()
        {
            for (int i = 0; i < pawnGraphics.Length; i++)
                pawnGraphics[i].material = _deleteMaterial;
            
        }

        public void ApplyBaseMaterial()
        {
            for (int i = 0; i < pawnGraphics.Length; i++)
                pawnGraphics[i].material = _basePawnMaterials[i];
        }

        public void HighlightConnector(PawnConnector connector)
        {
            for (int i = 0; i < connectorVisualBindings.Length; i++)
            {
                var binding = connectorVisualBindings[i];
                if (binding.Connector == connector)
                {
                    binding.Renderer.material = _activeConnectorMaterial;
                    break;
                }
            }
        }

        public void ClearHighlightConnector(PawnConnector connector)
        {
            for (int i = 0; i < connectorVisualBindings.Length; i++)
            {
                var binding = connectorVisualBindings[i];
                if (binding.Connector == connector)
                {
                    binding.Renderer.material = _baseConnectorMaterials[i];
                    break;
                }
            }
        }

        public void ClearConnectorHighlights()
        {
            for (int i = 0; i < connectorVisualBindings.Length; i++)
                connectorVisualBindings[i].Renderer.material = _baseConnectorMaterials[i];
        }
        
    }
}