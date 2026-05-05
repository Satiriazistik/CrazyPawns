using CrazyPawn;
using Game.Board;
using UnityEngine;

namespace Bootstrap
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField]
        private CrazyPawnSettings settings;
        
        [SerializeField]
        private BoardView boardView;

        private void Awake()
        {
            InitializeGameBoard();
        }

        private void InitializeGameBoard()
        {
            var generationConfig = new BoardGenerationConfig(settings.CheckerboardSize, settings.CheckerboardSize, BoardConstants.CELL_SIZE,
                settings.BlackCellColor, settings.WhiteCellColor);
            
            boardView.Initialize(generationConfig);
        }
    }
}