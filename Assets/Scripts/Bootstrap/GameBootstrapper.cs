using CrazyPawn;
using Game.Board;
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

        private void Awake()
        {
            InitializeGameBoard();
            SpawnPawns();
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
            
            pawnSpawner.Spawn(settings.InitialPawnCount, spawnArea);
        }
        
    }
}