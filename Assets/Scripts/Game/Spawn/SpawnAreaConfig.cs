using UnityEngine;

namespace Game.Spawn
{
    public class SpawnAreaConfig
    {
        public Vector3 AreaCenter { get; private set; }
        public float AreaRadius { get; private set; }

        public Vector3 BoardCenter { get; private set; }
        public Vector3 BoardSize { get; private set; }

        public SpawnAreaConfig(Vector3 areaCenter, float areaRadius, Vector3 boardCenter, Vector3 boardSize)
        {
            AreaCenter = areaCenter;
            AreaRadius = areaRadius;

            BoardCenter = boardCenter;
            BoardSize = boardSize;
        }
    }
}