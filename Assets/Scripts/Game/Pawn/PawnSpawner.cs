using CrazyPawn;
using Game.Spawn;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Pawn
{
    public class PawnSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject pawnPrefab;
        [SerializeField]
        private Transform parent;

#if UNITY_EDITOR
        [Header("Editor Preview")]
        [SerializeField]
        private CrazyPawnSettings crazyPawnSettings;
        [SerializeField]
        private Color gizmoColor = Color.red;
#endif
        
        public void Spawn(int count, ISpawnArea spawnArea)
        {
            for (int i = 0; i < count; i++)
            {
                var position = spawnArea.GetRandomPoint();
                Instantiate(pawnPrefab, position, Quaternion.identity, parent);
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (crazyPawnSettings == null)
                return;

            if (Application.isPlaying)
                return;
            
            Handles.color = gizmoColor;
            Handles.DrawSolidDisc(Vector3.zero, Vector3.up, crazyPawnSettings.InitialZoneRadius);
        }
#endif
        
    }
}