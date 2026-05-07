using System.Collections.Generic;
using Game.Pawn;
using Game.PlayerInput;
using UnityEngine;

namespace Game.Interaction
{
    public class PawnConnectionController : PlayerInteractionHandler
    {
        [SerializeField]
        private LineRenderer lineRendererPrefab;

        [SerializeField]
        private Transform linesParent;
        
        [SerializeField]
        private Camera playerCamera;

        private List<PawnController> _pawns = new List<PawnController>();
        
        private PawnConnectionSystem _connectionSystem;

        private PawnConnector _sourceConnector;
        private PawnConnector _hoveredConnector;

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
            UpdateHoveredConnector(inputHandler);
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
                _sourceConnector = pawnConnector;
                var pawnController = _sourceConnector.Owner;
                pawnController.HighlightConnector(_sourceConnector);
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

        private void UpdateHoveredConnector(IPlayerInput inputHandler)
        {
            if (!IsConnectionSelectionActive)
                return;

            if (!_playerRaycaster.DoCameraMouseRaycast(inputHandler.MousePosition, out var hitInfo))
            {
                ResetHoveredConnector();
                return;
            }

            if (!hitInfo.collider.gameObject.TryGetComponent<PawnConnector>(out var pawnConnector))
            {
                ResetHoveredConnector();
                return;
            }

            if (_hoveredConnector == pawnConnector)
                return;

            if (_hoveredConnector != null)
                ResetHoveredConnector();

            if (_connectionSystem.CanConnect(_sourceConnector, pawnConnector))
            {
                _hoveredConnector = pawnConnector;
                var pawnController = _hoveredConnector.Owner;
                pawnController.HighlightConnector(_hoveredConnector);
            }
        }

        private void ResetConnectionSelection()
        {
            ResetSourceConnector();
            ResetHoveredConnector();
        }

        private void ResetSourceConnector()
        {
            if (_sourceConnector != null)
            {
                var pawnController = _sourceConnector.Owner;
                pawnController.ClearConnectorHighlight(_sourceConnector);
            }

            _sourceConnector = null;
        }

        private void ResetHoveredConnector()
        {
            if (_hoveredConnector != null)
            {
                var pawnController = _hoveredConnector.Owner;
                pawnController.ClearConnectorHighlight(_hoveredConnector);
            }

            _hoveredConnector = null;
        }

        private void OnPawnDestroyed(PawnController pawn)
        {
            _connectionSystem.RemovePawnConnections(pawn);
            pawn.PawnDestroyed -= OnPawnDestroyed;
            _pawns.Remove(pawn);
        }
        
    }
}
