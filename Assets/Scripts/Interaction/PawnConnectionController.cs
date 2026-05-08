using System.Collections.Generic;
using Pawn;
using PlayerInput;
using UnityEngine;

namespace Interaction
{
    public class PawnConnectionController : PlayerInteractionHandler
    {
        [SerializeField]
        private LineRenderer lineRendererPrefab;

        [SerializeField]
        private Transform linesParent;

        private List<PawnController> _pawns = new List<PawnController>();
        
        private PawnConnectionSystem _connectionSystem;

        private PawnConnector _sourceConnector;

        private bool IsConnectionSelectionActive => _sourceConnector != null;

        private bool _initialized;

        private void OnDestroy()
        {
            for (int i = 0; i < _pawns.Count; i++)
            {
                var pawn = _pawns[i];
                if (pawn != null)
                    pawn.PawnDestroyed -= OnPawnDestroyed;
            }
            
            _pawns.Clear();
        }

        private void Update()
        {
            if (!_initialized)
                return;
            
            _connectionSystem.UpdateConnections();
        }

        public void Initialize(IReadOnlyList<PawnController> pawns)
        {
            var config = new PawnConnectionSystemConfig(pawns, lineRendererPrefab, linesParent);
            _connectionSystem = new PawnConnectionSystem(config);

            _playerRaycaster = new PlayerRaycaster(playerCamera, raycastTargetLayers, raycastMaxDistance);
            
            _pawns.AddRange(pawns);

            for (int i = 0; i < _pawns.Count; i++)
                _pawns[i].PawnDestroyed += OnPawnDestroyed;
            
            _initialized = true;
        }
        
        protected override void OnInteractionUpdate(IPlayerInput inputHandler)
        {
            base.OnInteractionUpdate(inputHandler);
            InteractionIsActive = IsConnectionSelectionActive;

            if (IsConnectionSelectionActive)
                HighlightAllValidConnectors();
        }
        
        protected override void OnInteractionStarted(IPlayerInput inputHandler)
        {
            if (!_playerRaycaster.DoCameraMouseRaycast(inputHandler.MousePosition, out var hitInfo))
            {
                if (IsConnectionSelectionActive)
                    ResetConnectionSelection();
                
                return;
            }

            if (!hitInfo.collider.gameObject.TryGetComponent<PawnConnector>(out var pawnConnector))
            {
                ResetConnectionSelection();
                return;
            }

            if (!IsConnectionSelectionActive)
            {
                SetSourceConnector(pawnConnector);
                return;
            }

            if (_connectionSystem.CanConnect(_sourceConnector, pawnConnector))
                _connectionSystem.AddConnection(_sourceConnector, pawnConnector);

            ResetConnectionSelection();
        }

        protected override void OnInteractionHeld(IPlayerInput inputHandler)
        {
            
        }

        protected override void OnInteractionEnded(IPlayerInput inputHandler)
        {
            if (!IsConnectionSelectionActive)
                return;

            if (!_playerRaycaster.DoCameraMouseRaycast(inputHandler.MousePosition, out var hitInfo))
            {
                ResetConnectionSelection();
                return;
            }

            if (!hitInfo.collider.gameObject.TryGetComponent<PawnConnector>(out var pawnConnector))
            {
                ResetConnectionSelection();
                return;
            }
            
            if (_sourceConnector == pawnConnector)
                return;

            if (_connectionSystem.CanConnect(_sourceConnector, pawnConnector))
                _connectionSystem.AddConnection(_sourceConnector, pawnConnector);

            ResetConnectionSelection();
        }

        private void HighlightAllValidConnectors()
        {
            for (int i = 0; i < _pawns.Count; i++)
            {
                var pawnController = _pawns[i];
                var connectors = _pawns[i].PawnConnectors;
                for (int j = 0; j < connectors.Length; j++)
                {
                    var connector = connectors[j];
                    if (_connectionSystem.CanConnect(_sourceConnector, connector))
                        pawnController.HighlightConnector(connector);
                }
            }
        }

        private void ResetConnectorsHighlight()
        {
            for (int i = 0; i < _pawns.Count; i++)
            {
                var pawnController = _pawns[i];
                pawnController.ClearAllConnectorHighlights();
            }
        }
        
        private void ResetConnectionSelection()
        {
            ResetConnectorsHighlight();
            ResetSourceConnector();
        }

        private void SetSourceConnector(PawnConnector connector) => _sourceConnector = connector; 
        private void ResetSourceConnector() => _sourceConnector = null;

        private void OnPawnDestroyed(PawnController pawn)
        {
            _connectionSystem.RemovePawnConnections(pawn);
            pawn.PawnDestroyed -= OnPawnDestroyed;
            _pawns.Remove(pawn);
        }
        
    }
}
