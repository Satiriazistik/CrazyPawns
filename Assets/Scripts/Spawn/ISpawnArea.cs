using UnityEngine;

namespace Spawn
{
    public interface ISpawnArea
    {
        public Vector3 GetRandomPoint();
    }
}