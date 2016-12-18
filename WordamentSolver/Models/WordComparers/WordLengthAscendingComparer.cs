namespace WordamentSolver.Models.WordComparers
{
    public sealed class WordLengthAscendingComparer : WordComparer
    {
        public override string Name => "word length ascending";

        public override int Compare(Word x, Word y)
            => x.WordLength != y.WordLength
            ? x.WordLength.CompareTo(y.WordLength)
            : WordComparer.Alphabet.Compare(x, y);
    }
}
