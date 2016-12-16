namespace WordamentSolver.Models.WordComparers
{
    public sealed class StartPositionByPointsOverWordLengthComparer : WordComparer
    {
        public override string Name => "start position by: points / w.l.";

        public override int Compare(Word x, Word y)
            => x.StartPosition != y.StartPosition
            ? x.StartPosition.CompareTo(y.StartPosition)
            : WordComparer.PointsOverWordLength.Compare(x, y);
    }
}
