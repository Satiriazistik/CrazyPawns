using UnityEngine;

namespace Game.Board
{
    public class BoardGenerationConfig
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public float CellSize { get; private set; }

        public Color ColorA { get; private set; }
        public Color ColorB { get; private set; }

        public BoardGenerationConfig(int width, int height, float cellSize, Color colorA, Color colorB)
        {
            Width = width;
            Height = height;

            CellSize = cellSize;

            ColorA = colorA;
            ColorB = colorB;
        }
    }
}