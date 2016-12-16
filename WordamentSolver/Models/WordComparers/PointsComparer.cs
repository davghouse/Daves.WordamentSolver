namespace WordamentSolver.Models.WordComparers
{
    public sealed class PointsComparer : WordComparer
    {
        public override string Name => "points";

        public override int Compare(Word x, Word y)
            => x.Points != y.Points
            ? y.Points.CompareTo(x.Points)
            : WordComparer.Alphabet.Compare(x, y);
    }
}
