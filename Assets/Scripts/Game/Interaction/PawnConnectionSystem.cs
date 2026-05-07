using System.Collections.Generic;
using Game.Pawn;
using UnityEngine;

namespace Game.Interaction
{
    public class PawnConnectionSystem
    {
        private List<PawnConnection> _currentPawnConnection = new List<PawnConnection>();

        private PawnConnectionSystemConfig _systemConfig;

        private const int CONNECTION_POOL_SIZE = 20;
        
        public PawnConnectionSystem(PawnConnectionSystemConfig systemConfig)
        {
            _systemConfig = systemConfig;
            
            for (int i = 0; i < CONNECTION_POOL_SIZE; i++)
            {
                var line = Object.Instantiate(systemConfig.LinePrefab, systemConfig.LinesParent);
                line.gameObject.SetActive(false);

                _currentPawnConnection.Add(new PawnConnection(line));
            }
        }

        public void UpdateConnections()
        {
            for (int i = 0; i < _currentPawnConnection.Count; i++)
            {
                var connection = _currentPawnConnection[i];
                if (!connection.IsActive)
                    continue;
                
                connection.LineRenderer.SetPosition(0, connection.ConnectorA.ConnectorTransform.position);
                connection.LineRenderer.SetPosition(1, connection.ConnectorB.ConnectorTransform.position);
            }
        }

        public bool CanConnect(PawnConnector connectorA, PawnConnector connectorB)
        {
            if (connectorA.Owner == connectorB.Owner)
                return false;
            
            for (int i = 0; i < _currentPawnConnection.Count; i++)
            {
                var connection = _currentPawnConnection[i];
                if (!connection.IsActive)
                    continue;
                
                var hasConnectionA = connection.ConnectorA == connectorA || connection.ConnectorB == connectorA;
                var hasConnectionB = connection.ConnectorA == connectorB || connection.ConnectorB == connectorB;
                if (hasConnectionA && hasConnectionB)
                    return false;
            }

            return true;
        }

        public void AddConnection(PawnConnector connectorA, PawnConnector connectorB)
        {
            var targetConnection = GetFreePawnConnection();
            targetConnection.IsActive = true;
            targetConnection.ConnectorA = connectorA;
            targetConnection.ConnectorB = connectorB;
            targetConnection.LineRenderer.SetPosition(0, connectorA.ConnectorTransform.position);
            targetConnection.LineRenderer.SetPosition(1, connectorB.ConnectorTransform.position);
            targetConnection.LineRenderer.gameObject.SetActive(true);
        }

        public void RemovePawnConnections(PawnController pawnController)
        {
            for (int i = 0; i < _currentPawnConnection.Count; i++)
            {
                var connection = _currentPawnConnection[i];
                if (!connection.IsActive)
                    continue;
                
                if (connection.ConnectorA.Owner == pawnController || connection.ConnectorB.Owner == pawnController)
                    connection.Clear();
            }
        }

        private PawnConnection GetFreePawnConnection()
        {
            for (int i = 0; i < _currentPawnConnection.Count; i++)
            {
                var connection = _currentPawnConnection[i];
                if (!connection.IsActive)
                    return connection;
            }
            
            var line = Object.Instantiate(_systemConfig.LinePrefab, _systemConfig.LinesParent);
            line.gameObject.SetActive(false);

            var additionalConnection = new PawnConnection(line);
            _currentPawnConnection.Add(additionalConnection);

            return additionalConnection;
        }
        
    }
}