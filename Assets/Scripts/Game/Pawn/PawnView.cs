using UnityEngine;

namespace Game.Pawn
{
    public class PawnView : MonoBehaviour
    {
        private Material _deleteMaterial;
        private Material _activeConnectorMaterial;

        [SerializeField]
        private Renderer[] pawnGraphics;

        private Material[] _baseMaterials;
        
        public void Initialize(Material deleteMaterial, Material activeConnectorMaterial)
        {
            _deleteMaterial = deleteMaterial;
            _activeConnectorMaterial = activeConnectorMaterial;

            _baseMaterials = new Material[pawnGraphics.Length];
            for (int i = 0; i < _baseMaterials.Length; i++)
                _baseMaterials[i] = pawnGraphics[i].sharedMaterial;
        }

        public void ApplyDeleteMaterial()
        {
            for (int i = 0; i < pawnGraphics.Length; i++)
                pawnGraphics[i].material = _deleteMaterial;
            
        }

        public void ApplyBaseMaterial()
        {
            for (int i = 0; i < pawnGraphics.Length; i++)
                pawnGraphics[i].material = _baseMaterials[i];
        }
        
    }
}