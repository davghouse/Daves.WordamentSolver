namespace WordamentSolver.Models.WordComparers
{
    public sealed class StartPositionByWordLengthComparer : WordComparer
    {
        public override string Name => "start position by: word length";

        public override int Compare(Word x, Word y)
            => x.StartPosition != y.StartPosition
            ? x.StartPosition.CompareTo(y.StartPosition)
            : WordComparer.WordLength.Compare(x, y);
    }
}
