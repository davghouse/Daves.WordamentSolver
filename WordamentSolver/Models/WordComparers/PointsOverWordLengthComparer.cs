namespace WordamentSolver.Models.WordComparers
{
    public sealed class PointsOverWordLengthComparer : WordComparer
    {
        public override string Name => "points / w.l.";

        public override int Compare(Word x, Word y)
            => x.PointsOverWordLength != y.PointsOverWordLength
            ? y.PointsOverWordLength.CompareTo(x.PointsOverWordLength)
            : WordComparer.Alphabet.Compare(x, y);
    }
}
