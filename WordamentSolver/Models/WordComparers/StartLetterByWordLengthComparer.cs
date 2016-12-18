namespace WordamentSolver.Models.WordComparers
{
    public sealed class StartLetterByWordLengthComparer : WordComparer
    {
        public override string Name => "start letter by: word length";

        public override int Compare(Word x, Word y)
            => x.StartLetter != y.StartLetter
            ? x.StartLetter.CompareTo(y.StartLetter)
            : WordComparer.WordLength.Compare(x, y);
    }
}
