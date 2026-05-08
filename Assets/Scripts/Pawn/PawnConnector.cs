using UnityEngine;

namespace Pawn
{
    public class PawnConnector : MonoBehaviour
    {
        public PawnController Owner { get; private set; }

        public Transform ConnectorTransform { get; private set; }

        public void Initialize(PawnController owner)
        {
            Owner = owner;
            ConnectorTransform = transform;
        }
    }
}