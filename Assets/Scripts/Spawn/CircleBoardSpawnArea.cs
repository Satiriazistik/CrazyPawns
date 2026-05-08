using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawn
{
    public class CircleBoardSpawnArea : ISpawnArea
    {
        private IntersectArea _intersectArea;
        private Vector3 _spawnAreaCenter;
        private float _spawnAreaRadiusSqr;

        private const int MAX_FIND_POINT_ATTEMPTS = 100;

        public CircleBoardSpawnArea(SpawnAreaConfig spawnAreaConfig)
        {
            _spawnAreaCenter = spawnAreaConfig.AreaCenter;
            _spawnAreaRadiusSqr = spawnAreaConfig.AreaRadius * spawnAreaConfig.AreaRadius;

            _intersectArea = GetCircleBoardIntersectArea(_spawnAreaCenter, spawnAreaConfig.AreaRadius, spawnAreaConfig.BoardCenter, spawnAreaConfig.BoardSize);
            
            if (_intersectArea == null)
                throw new InvalidOperationException("Spawn area does not intersect board bounds.");
        }

        public Vector3 GetRandomPoint()
        {
            for (int i = 0; i < MAX_FIND_POINT_ATTEMPTS; i++)
            {
                var point = new Vector3(
                    Random.Range(_intersectArea.XMin, _intersectArea.XMax),
                    0,
                    Random.Range(_intersectArea.ZMin, _intersectArea.ZMax)
                );

                if (IsInsideCircle(point))
                    return point;
            }

            return _intersectArea.Center;
        }

        private bool IsInsideCircle(Vector3 point)
        {
            var dx = point.x - _spawnAreaCenter.x;
            var dz = point.z - _spawnAreaCenter.z;

            return dx * dx + dz * dz <= _spawnAreaRadiusSqr;
        }

        private IntersectArea GetCircleBoardIntersectArea(Vector3 circleCenter, float circleRadius, Vector3 boardCenter, Vector3 boardSize)
        {
            var halfSizeX = boardSize.x * 0.5f;
            var boardXMin = boardCenter.x - halfSizeX;
            var boardXMax = boardCenter.x + halfSizeX;

            var halfSizeZ = boardSize.z * 0.5f;
            var boardZMin = boardCenter.z - halfSizeZ;
            var boardZMax = boardCenter.z + halfSizeZ;

            var circleXMin = circleCenter.x - circleRadius;
            var circleXMax = circleCenter.x + circleRadius;

            var circleMinZ = circleCenter.z - circleRadius;
            var circleMaxZ = circleCenter.z + circleRadius;

            var xMin = Mathf.Max(boardXMin, circleXMin);
            var xMax = Mathf.Min(boardXMax, circleXMax);

            var zMin = Mathf.Max(boardZMin, circleMinZ);
            var zMax = Mathf.Min(boardZMax, circleMaxZ);
            
            if (xMin >= xMax || zMin >= zMax)
                return null;
            
            return new IntersectArea(xMin, xMax, zMin, zMax);
        }

        private class IntersectArea
        {
            public float XMin { get; private set; }
            public float XMax { get; private set; }
            public float ZMin { get; private set; }
            public float ZMax { get; private set; }

            public IntersectArea(float xMin, float xMax, float zMin, float zMax)
            {
                XMin = xMin;
                XMax = xMax;
                ZMin = zMin;
                ZMax = zMax;
            }
            
            public Vector3 Center => new Vector3((XMin + XMax) * 0.5f, 0f, (ZMin + ZMax) * 0.5f);
        }
    }
}