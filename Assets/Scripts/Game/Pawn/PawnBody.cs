using UnityEngine;

namespace Game.Pawn
{
    public class PawnBody : MonoBehaviour
    {
        public PawnController Owner => _owner;
        
        private PawnController _owner;

        public void Initialize(PawnController controller)
        {
            _owner = controller;
        }
        
    }
}