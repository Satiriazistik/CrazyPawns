using UnityEngine;

namespace Game.Board
{
    public static class BoardMeshGenerator
    {
        private const int QUAD_VERTEX_COUNT = 4;
        private const int QUAD_TRIANGLE_COUNT = 6;

        public static Mesh GenerateBoardMesh(BoardGenerationConfig config)
        {
            var width = config.Width;
            var height = config.Height;
            var cellSize = config.CellSize;
            var colorA = config.ColorA;
            var colorB = config.ColorB;
            
            var mesh = new Mesh();
            
            var vertices = new Vector3[width * height * QUAD_VERTEX_COUNT];
            var triangles = new int[width * height * QUAD_TRIANGLE_COUNT];
            var vColors = new Color[vertices.Length];

            var offsetX = -cellSize * width * 0.5f;
            var offsetZ = -cellSize * height * 0.5f;
            var origin = new Vector3(offsetX, 0, offsetZ);

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    var quadIndex = h * width + w;
                    var vertexIndex = quadIndex * QUAD_VERTEX_COUNT;
                    var triangleIndex = quadIndex * QUAD_TRIANGLE_COUNT;

                    var vertexColor = (h + w) % 2 == 0 ? colorA : colorB;

                    var pos = origin + new Vector3(w * cellSize, 0, h * cellSize);
                    var right = new Vector3(cellSize, 0, 0);
                    var forward = new Vector3(0, 0, cellSize);

                    //---Vertex generation---
                    //Order: left bottom -> left top -> right top -> right bottom
                    vertices[vertexIndex + 0] = pos;
                    vertices[vertexIndex + 1] = pos + forward;
                    vertices[vertexIndex + 2] = pos + right + forward;
                    vertices[vertexIndex + 3] = pos + right;

                    //---Vertex painting---
                    vColors[vertexIndex + 0] = vertexColor;
                    vColors[vertexIndex + 1] = vertexColor;
                    vColors[vertexIndex + 2] = vertexColor;
                    vColors[vertexIndex + 3] = vertexColor;

                    //---Triangle generation---
                    triangles[triangleIndex + 0] = vertexIndex + 0;
                    triangles[triangleIndex + 1] = vertexIndex + 1;
                    triangles[triangleIndex + 2] = vertexIndex + 2;

                    triangles[triangleIndex + 3] = vertexIndex + 0;
                    triangles[triangleIndex + 4] = vertexIndex + 2;
                    triangles[triangleIndex + 5] = vertexIndex + 3;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.colors = vColors;
            
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
