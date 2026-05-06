using CrazyPawn;
using Game.Board;
using Game.Interaction;
using Game.Pawn;
using Game.Spawn;
using UnityEngine;

namespace Bootstrap
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField]
        private CrazyPawnSettings settings;
        
        [SerializeField]
        private BoardView boardView;
        
        [SerializeField]
        private PawnSpawner pawnSpawner;

        [SerializeField]
        private PawnDragController pawnDragController;
        
        private void Awake()
        {
            InitializeGameBoard();
            SpawnPawns();
            InitializeDragController();
        }

        private void InitializeGameBoard()
        {
            var generationConfig = new BoardGenerationConfig(settings.CheckerboardSize, settings.CheckerboardSize, BoardConstants.CELL_SIZE,
                settings.BlackCellColor, settings.WhiteCellColor);
            
            boardView.Initialize(generationConfig);
        }

        private void SpawnPawns()
        {
            var boardCenter = boardView.transform.position;
            var boardSideSize = settings.CheckerboardSize * BoardConstants.CELL_SIZE;
            var boardSize = new Vector3(boardSideSize, 0, boardSideSize);
            var spawnAreaConfig = new SpawnAreaConfig(Vector3.zero, settings.InitialZoneRadius, boardCenter, boardSize);
            var spawnArea = new CircleBoardSpawnArea(spawnAreaConfig);
            
            var pawnInstances = pawnSpawner.Spawn(settings.InitialPawnCount, spawnArea);
            foreach (var pawnInstance in pawnInstances)
                pawnInstance.Initialize(settings.DeleteMaterial, settings.ActiveConnectorMaterial);
        }

        private void InitializeDragController()
        {
            var boardBounds = new BoardBounds(settings.CheckerboardSize * BoardConstants.CELL_SIZE);
            pawnDragController.Initialize(boardBounds);
        }
        
    }
}