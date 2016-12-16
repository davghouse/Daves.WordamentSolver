namespace WordamentSolver.Models.WordComparers
{
    public sealed class StartPositionByWordLengthAscendingComparer : WordComparer
    {
        public override string Name => "start position by: w.l. ascending";

        public override int Compare(Word x, Word y)
            => x.StartPosition != y.StartPosition
            ? x.StartPosition.CompareTo(y.StartPosition)
            : WordComparer.WordLengthAscending.Compare(x, y);
    }
}
