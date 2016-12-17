namespace WordamentSolver.Models.WordComparers
{
    public sealed class StartPositionByPointsOverPathLengthComparer : WordComparer
    {
        public override string Name => "start position by: points / p.l.";

        public override int Compare(Word x, Word y)
            => x.StartPosition != y.StartPosition
            ? x.StartPosition.CompareTo(y.StartPosition)
            : WordComparer.PointsOverPathLength.Compare(x, y);
    }
}
