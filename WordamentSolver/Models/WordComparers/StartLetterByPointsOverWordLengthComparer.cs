namespace WordamentSolver.Models.WordComparers
{
    public sealed class StartLetterByPointsOverWordLengthComparer : WordComparer
    {
        public override string Name => "start letter by: points / word length";

        public override int Compare(Word x, Word y)
            => x.StartLetter != y.StartLetter
            ? x.StartLetter.CompareTo(y.StartLetter)
            : WordComparer.PointsOverWordLength.Compare(x, y);
    }
}
