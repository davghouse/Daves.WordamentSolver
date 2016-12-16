namespace WordamentSolver.Models.WordComparers
{
    public sealed class StartPositionByPointsComparer : WordComparer
    {
        public override string Name => "start position by: points";

        public override int Compare(Word x, Word y)
            => x.StartPosition != y.StartPosition
            ? x.StartPosition.CompareTo(y.StartPosition)
            : WordComparer.Points.Compare(x, y);
    }
}
