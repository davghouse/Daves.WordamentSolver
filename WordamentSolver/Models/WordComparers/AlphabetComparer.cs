namespace WordamentSolver.Models.WordComparers
{
    public sealed class AlphabetComparer : WordComparer
    {
        public override string Name => "alphabet";

        public override int Compare(Word x, Word y)
            => x.String.CompareTo(y.String);
    }
}
