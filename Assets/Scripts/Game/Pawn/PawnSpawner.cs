using System.Collections.Generic;
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
        public List<PawnController> PawnInstances => _pawns;
        
        [SerializeField]
        private GameObject pawnPrefab;
        [SerializeField]
        private Transform parent;

        private List<PawnController> _pawns = new List<PawnController>();
        
#if UNITY_EDITOR
        [Header("Editor Preview")]
        [SerializeField]
        private CrazyPawnSettings crazyPawnSettings;
        [SerializeField]
        private Color gizmoColor = Color.red;
#endif
        
        public IReadOnlyList<PawnController> Spawn(int count, ISpawnArea spawnArea)
        {
            for (int i = 0; i < _pawns.Count; i++)
                Destroy(_pawns[i].gameObject);
            
            _pawns.Clear();
            
            for (int i = 0; i < count; i++)
            {
                var position = spawnArea.GetRandomPoint();
                var pawnInstance = Instantiate(pawnPrefab, position, Quaternion.identity, parent);
                if (!pawnInstance.TryGetComponent<PawnController>(out var pawnController))
                {
                    Debug.LogError($"Cannot find {nameof(PawnController)} component on pawn instance.", pawnInstance);
                    continue;
                }
                
                _pawns.Add(pawnController);
            }

            return _pawns;
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