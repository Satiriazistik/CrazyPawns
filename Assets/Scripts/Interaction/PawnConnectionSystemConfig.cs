using System.Collections.Generic;
using Pawn;
using UnityEngine;

namespace Interaction
{
    public class PawnConnectionSystemConfig
    {
        public IReadOnlyList<PawnController> Pawns { get; private set; }
        public LineRenderer LinePrefab { get; private set; }
        public Transform LinesParent { get; private set; }

        public PawnConnectionSystemConfig(
            IReadOnlyList<PawnController> pawns,
            LineRenderer linePrefab,
            Transform linesParent)
        {
            Pawns = pawns;
            LinePrefab = linePrefab;
            LinesParent = linesParent;
        }
    }
}