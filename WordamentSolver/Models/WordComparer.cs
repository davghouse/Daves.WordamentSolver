using System.Collections.Generic;
using WordamentSolver.Models.WordComparers;

namespace WordamentSolver.Models
{
    public abstract class WordComparer : IComparer<Word>
    {
        public static readonly WordComparer Alphabet = new AlphabetComparer();
        public static readonly WordComparer Points = new PointsComparer();
        public static readonly WordComparer WordLength = new WordLengthComparer();
        public static readonly WordComparer PathLength = new PathLengthComparer();
        public static readonly WordComparer PointsOverWordLength = new PointsOverWordLengthComparer();
        public static readonly WordComparer PointsOverPathLength = new PointsOverPathLengthComparer();
        public static readonly WordComparer StartPositionByPoints = new StartPositionByPointsComparer();
        public static readonly WordComparer StartPositionByWordLength = new StartPositionByWordLengthComparer();
        public static readonly WordComparer StartPositionByPointsOverWordLength = new StartPositionByPointsOverWordLengthComparer();
        public static readonly WordComparer StartPositionByPointsOverPathLength = new StartPositionByPointsOverPathLengthComparer();
        public static readonly WordComparer StartLetterByPoints = new StartLetterByPointsComparer();
        public static readonly WordComparer StartLetterByWordLength = new StartLetterByWordLengthComparer();
        public static readonly WordComparer StartLetterByPointsOverWordLength = new StartLetterByPointsOverWordLengthComparer();
        public static readonly WordComparer StartLetterByPointsOverPathLength = new StartLetterByPointsOverPathLengthComparer();
        public static readonly WordComparer WordLengthAscending = new WordLengthAscendingComparer();
        public static readonly WordComparer StartPositionByWordLengthAscending = new StartPositionByWordLengthAscendingComparer();

        public static readonly IReadOnlyList<WordComparer> All = new[]
        {
            Alphabet, Points, WordLength, PathLength, PointsOverWordLength, PointsOverPathLength,
            StartPositionByPoints, StartPositionByWordLength, StartPositionByPointsOverWordLength, StartPositionByPointsOverPathLength,
            StartLetterByPoints, StartLetterByWordLength, StartLetterByPointsOverWordLength, StartLetterByPointsOverPathLength,
            WordLengthAscending, StartPositionByWordLengthAscending
        };

        public abstract string Name { get; }

        public abstract int Compare(Word x, Word y);
    }
}
