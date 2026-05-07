using System;
using UnityEngine;

namespace Game.Pawn
{
    public class PawnController : MonoBehaviour
    {

        public Action<PawnController> PawnDestroyed;
        
        public Transform PawnTransform => _pawnTransform;

        public PawnState CurrentPawnState => _currentPawnState;

        public PawnConnector[] PawnConnectors => pawnConnectors;

        [SerializeField]
        private PawnView pawnView;

        [SerializeField]
        private PawnBody pawnBody;
        
        [SerializeField] 
        private PawnConnector[] pawnConnectors;
        
        private Transform _pawnTransform;

        private PawnState _currentPawnState = PawnState.OnBoard;

        private void OnDestroy()
        {
            PawnDestroyed?.Invoke(this);
        }

        public void Initialize(Material deleteMaterial, Material activeConnectorMaterial)
        {
            _pawnTransform = transform;

            pawnBody.Initialize(this);
            pawnView.Initialize(deleteMaterial, activeConnectorMaterial);

            for (int i = 0; i < pawnConnectors.Length; i++)
                pawnConnectors[i].Initialize(this);
        }

        public void SetState(PawnState state)
        {
            if (state == _currentPawnState)
                return;
            
            switch (state)
            {
                case PawnState.OnBoard:
                    pawnView.ApplyBaseMaterial();
                    break;
                case PawnState.OutOfBoard:
                    pawnView.ApplyDeleteMaterial();
                    break;
                default:
                    Debug.LogWarning($"Trying to set unsupported pawn state: {state}.", gameObject);
                    break;
            }
            
            _currentPawnState = state;
        }

        public void HighlightConnector(PawnConnector connector)
        {
            pawnView.HighlightConnector(connector);
        }

        public void ClearConnectorHighlight(PawnConnector connector)
        {
            pawnView.ClearHighlightConnector(connector);
        }

        public void ClearAllConnectorHighlights()
        {
            pawnView.ClearConnectorHighlights();
        }

        public void DestroyPawn() => Destroy(gameObject);
    }
}