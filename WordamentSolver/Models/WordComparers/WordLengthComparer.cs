namespace WordamentSolver.Models.WordComparers
{
    public sealed class WordLengthComparer : WordComparer
    {
        public override string Name => "word length (w.l.)";

        public override int Compare(Word x, Word y)
            => x.WordLength != y.WordLength
            ? y.WordLength.CompareTo(x.WordLength)
            : WordComparer.Alphabet.Compare(x, y);
    }
}
