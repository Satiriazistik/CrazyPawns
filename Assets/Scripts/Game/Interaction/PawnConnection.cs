using Game.Pawn;
using UnityEngine;

namespace Game.Interaction
{
    public class PawnConnection
    {
        public bool IsActive;

        public PawnConnector ConnectorA;
        public PawnConnector ConnectorB;

        public LineRenderer LineRenderer { get; private set; }

        public PawnConnection(LineRenderer lineRenderer)
        {
            LineRenderer = lineRenderer;
        }
        
        public void Clear()
        {
            IsActive = false;
            
            ConnectorA = null;
            ConnectorB = null;
            
            LineRenderer.gameObject.SetActive(false);
        }
    }
}