namespace WordamentSolver.Models.WordComparers
{
    public sealed class StartLetterByPointsComparer : WordComparer
    {
        public override string Name => "start letter by: points";

        public override int Compare(Word x, Word y)
            => x.StartLetter != y.StartLetter
            ? x.StartLetter.CompareTo(y.StartLetter)
            : WordComparer.Points.Compare(x, y);
    }
}
