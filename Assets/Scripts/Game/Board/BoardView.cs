using CrazyPawn;
using UnityEngine;

namespace Game.Board
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BoardView : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter meshFilter;
        
#if UNITY_EDITOR

        [Header("Editor preview")]
        [SerializeField]
        private Color gizmosColor = Color.green;
        [SerializeField]
        private CrazyPawnSettings crazyPawnSettings;
        
#endif
        public void Initialize(BoardGenerationConfig generationConfig)
        {
            if (meshFilter == null)
                meshFilter = GetComponent<MeshFilter>();

            var boardMesh = BoardMeshGenerator.GenerateBoardMesh(generationConfig);
            meshFilter.mesh = boardMesh;
        }
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (crazyPawnSettings == null)
                return;

            if (Application.isPlaying)
                return;

            Gizmos.color = gizmosColor;
            
            var boardPosition = transform.position;
            Gizmos.DrawCube(boardPosition, new Vector3(crazyPawnSettings.CheckerboardSize * BoardConstants.CELL_SIZE, 0.02f, crazyPawnSettings.CheckerboardSize * BoardConstants.CELL_SIZE));
        }

#endif
        
    }
}