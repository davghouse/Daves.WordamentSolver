namespace WordamentSolver.Models.WordComparers
{
    public sealed class PointsOverPathLengthComparer : WordComparer
    {
        public override string Name => "points / path length";

        public override int Compare(Word x, Word y)
            => x.PointsOverPathLength != y.PointsOverPathLength
            ? y.PointsOverPathLength.CompareTo(x.PointsOverPathLength)
            : WordComparer.Alphabet.Compare(x, y);
    }
}
