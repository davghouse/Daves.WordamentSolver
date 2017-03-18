using Daves.WordamentSolver.WordSorters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Daves.WordamentSolver.UnitTests.WordSorters
{
    [TestClass]
    public class WordSorterTests
    {
        private static string[] _tileStrings1 = new[]
        {
            "A", "D/E", "F", "E",
            "G", "O", "P", "R",
            "M", "T", "E", "N",
            "I", "F", "E", "R"
        };

        private static string[] _tileStrings2 = new[]
        {
            "E", "L", "R", "D",
            "P", "O", "M", "Y",
            "S", "A", "R", "E",
            "S", "D", "E", "C"
        };

        [TestMethod]
        public void WordSorters()
        {
            var board = new Board(4, 4, p => _tileStrings1[p], p => null);
            board.GuessTilePoints();
            var solution = new Solution(board);

            solution.SortWords(WordSorter.Alphabet);
            Assert.AreEqual("ADO", solution.Words[0].String);

            solution.SortWords(WordSorter.Points);
            Assert.AreEqual("ADOPTER", solution.Words[0].String);

            solution.SortWords(WordSorter.WordLength);
            Assert.AreEqual("ADOPTEE", solution.Words[0].String);

            solution.SortWords(WordSorter.PathLength);
            Assert.AreEqual("MITERER", solution.Words[0].String);

            solution.SortWords(WordSorter.PointsOverWordLength);
            Assert.AreEqual("ADOPT", solution.Words[0].String);

            solution.SortWords(WordSorter.PointsOverPathLength);
            Assert.AreEqual("DAG", solution.Words[0].String);

            solution.SortWords(WordSorter.StartPositionByPoints);
            Assert.AreEqual("ADOPTER", solution.Words[0].String);

            solution.SortWords(WordSorter.StartPositionByWordLength);
            Assert.AreEqual("ADOPTEE", solution.Words[0].String);

            solution.SortWords(WordSorter.StartPositionByPointsOverWordLength);
            Assert.AreEqual("ADOPT", solution.Words[0].String);

            solution.SortWords(WordSorter.StartPositionByPointsOverPathLength);
            Assert.AreEqual("ADO", solution.Words[0].String);

            solution.SortWords(WordSorter.StartLetterByPoints);
            Assert.AreEqual("DOPER", solution.Words[6].String);

            solution.SortWords(WordSorter.StartLetterByWordLength);
            Assert.AreEqual("TOP", solution.Words[118].String);

            solution.SortWords(WordSorter.StartLetterByPointsOverWordLength);
            Assert.AreEqual("DOG", solution.Words[7].String);

            solution.SortWords(WordSorter.StartLetterByPointsOverPathLength);
            Assert.AreEqual("DOT", solution.Words[8].String);

            solution.SortWords(WordSorter.WordLengthAscending);
            Assert.AreEqual("DAG", solution.Words[3].String);

            solution.SortWords(WordSorter.StartPositionByWordLengthAscending);
            Assert.AreEqual("FETE", solution.Words[118].String);
        }

        [TestMethod]
        public void ApproximateBestPathSorterFindsABetterPathThanOtherWordSorters()
        {
            ApproximateBestPathSorterFindsABetterPathThanOtherWordSorters(new Board(4, 4, p => _tileStrings2[p], p => null));
            ApproximateBestPathSorterFindsABetterPathThanOtherWordSorters(new Board(4, 4, p => SolutionTests.ComplexTileStrings[p], p => null));
        }

        private void ApproximateBestPathSorterFindsABetterPathThanOtherWordSorters(Board board)
        {
            board.GuessTilePoints();
            var solution = new Solution(board);

            solution.SortWords(WordSorter.ApproximateBestPath);
            double totalDistanceForApproximateBestPathSorter = ApproximateBestPathSorter.CalculateTotalPathLength(solution.Words);

            foreach (WordSorter wordSorter in WordSorter.All
                .Where(ws => ws != WordSorter.ApproximateBestPath))
            {
                solution.SortWords(wordSorter);
                double totalDistanceForWordSorter = ApproximateBestPathSorter.CalculateTotalPathLength(solution.Words);

                Assert.IsTrue(totalDistanceForApproximateBestPathSorter < totalDistanceForWordSorter);
            }
        }
    }
}
