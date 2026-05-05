using UnityEngine;

namespace Game.Spawn
{
    public interface ISpawnArea
    {
        public Vector3 GetRandomPoint();
    }
}