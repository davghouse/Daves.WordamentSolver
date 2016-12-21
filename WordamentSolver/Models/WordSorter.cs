using System.Collections.Generic;
using WordamentSolver.Models.WordSorters;

namespace WordamentSolver.Models
{
    public abstract class WordSorter
    {
        public static readonly PointsSorter Points = new PointsSorter();
        public static readonly AlphabetSorter Alphabet = new AlphabetSorter();
        public static readonly WordLengthSorter WordLength = new WordLengthSorter();
        public static readonly PathLengthSorter PathLength = new PathLengthSorter();
        public static readonly PointsOverWordLengthSorter PointsOverWordLength = new PointsOverWordLengthSorter();
        public static readonly PointsOverPathLengthSorter PointsOverPathLength = new PointsOverPathLengthSorter();
        public static readonly StartPositionByPointsSorter StartPositionByPoints = new StartPositionByPointsSorter();
        public static readonly StartPositionByWordLengthSorter StartPositionByWordLength = new StartPositionByWordLengthSorter();
        public static readonly StartPositionByPointsOverWordLengthSorter StartPositionByPointsOverWordLength = new StartPositionByPointsOverWordLengthSorter();
        public static readonly StartPositionByPointsOverPathLengthSorter StartPositionByPointsOverPathLength = new StartPositionByPointsOverPathLengthSorter();
        public static readonly StartLetterByPointsSorter StartLetterByPoints = new StartLetterByPointsSorter();
        public static readonly StartLetterByWordLengthSorter StartLetterByWordLength = new StartLetterByWordLengthSorter();
        public static readonly StartLetterByPointsOverWordLengthSorter StartLetterByPointsOverWordLength = new StartLetterByPointsOverWordLengthSorter();
        public static readonly StartLetterByPointsOverPathLengthSorter StartLetterByPointsOverPathLength = new StartLetterByPointsOverPathLengthSorter();
        public static readonly WordLengthAscendingSorter WordLengthAscending = new WordLengthAscendingSorter();
        public static readonly StartPositionByWordLengthAscendingSorter StartPositionByWordLengthAscending = new StartPositionByWordLengthAscendingSorter();

        public static readonly IReadOnlyList<WordSorter> All = new WordSorter[]
        {
            Points, Alphabet, WordLength, PathLength, PointsOverWordLength, PointsOverPathLength,
            StartPositionByPoints, StartPositionByWordLength, StartPositionByPointsOverWordLength, StartPositionByPointsOverPathLength,
            StartLetterByPoints, StartLetterByWordLength, StartLetterByPointsOverWordLength, StartLetterByPointsOverPathLength,
            WordLengthAscending, StartPositionByWordLengthAscending
        };

        public abstract string Name { get; }

        public abstract void Sort(Word[] words);
    }
}
