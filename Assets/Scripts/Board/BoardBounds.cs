using UnityEngine;

namespace Board
{
    public class BoardBounds
    {
        private float _halfSize;

        public BoardBounds(float boardSize)
        {
            _halfSize = boardSize * 0.5f;
        }

        public bool Contains(Vector3 point)
        {
            return Mathf.Abs(point.x) <= _halfSize &&
                   Mathf.Abs(point.z) <= _halfSize;
        }
    }
}