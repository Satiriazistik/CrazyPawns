using System;
using UnityEngine;

namespace Game.Pawn
{
    public class PawnController : MonoBehaviour
    {
        public Transform PawnTransform => _pawnTransform;

        public PawnState CurrentPawnState => _currentPawnState;

        [SerializeField]
        private PawnView pawnView;
        
        private Transform _pawnTransform;

        private PawnState _currentPawnState = PawnState.OnBoard;

        public void Initialize(Material deleteMaterial, Material activeConnectorMaterial)
        {
            _pawnTransform = transform;
            
            pawnView.Initialize(deleteMaterial, activeConnectorMaterial);
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
    }
}