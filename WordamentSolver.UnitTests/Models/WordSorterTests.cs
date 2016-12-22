using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WordamentSolver.Models;
using WordamentSolver.Models.WordSorters;

namespace WordamentSolver.UnitTests.Models
{
    [TestClass]
    public sealed class WordSorterTests
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
            Assert.AreEqual(solution.Words[0].String, "ADO");

            solution.SortWords(WordSorter.Points);
            Assert.AreEqual(solution.Words[0].String, "ADOPTER");

            solution.SortWords(WordSorter.WordLength);
            Assert.AreEqual(solution.Words[0].String, "ADOPTEE");

            solution.SortWords(WordSorter.PathLength);
            Assert.AreEqual(solution.Words[0].String, "MITERER");

            solution.SortWords(WordSorter.PointsOverWordLength);
            Assert.AreEqual(solution.Words[0].String, "ADOPT");

            solution.SortWords(WordSorter.PointsOverPathLength);
            Assert.AreEqual(solution.Words[0].String, "DAG");

            solution.SortWords(WordSorter.StartPositionByPoints);
            Assert.AreEqual(solution.Words[0].String, "ADOPTER");

            solution.SortWords(WordSorter.StartPositionByWordLength);
            Assert.AreEqual(solution.Words[0].String, "ADOPTEE");

            solution.SortWords(WordSorter.StartPositionByPointsOverWordLength);
            Assert.AreEqual(solution.Words[0].String, "ADOPT");

            solution.SortWords(WordSorter.StartPositionByPointsOverPathLength);
            Assert.AreEqual(solution.Words[0].String, "ADO");

            solution.SortWords(WordSorter.StartLetterByPoints);
            Assert.AreEqual(solution.Words[6].String, "DOPER");

            solution.SortWords(WordSorter.StartLetterByWordLength);
            Assert.AreEqual(solution.Words[118].String, "TOP");

            solution.SortWords(WordSorter.StartLetterByPointsOverWordLength);
            Assert.AreEqual(solution.Words[7].String, "DOG");

            solution.SortWords(WordSorter.StartLetterByPointsOverPathLength);
            Assert.AreEqual(solution.Words[8].String, "DOT");

            solution.SortWords(WordSorter.WordLengthAscending);
            Assert.AreEqual(solution.Words[3].String, "DAG");

            solution.SortWords(WordSorter.StartPositionByWordLengthAscending);
            Assert.AreEqual(solution.Words[118].String, "FETE");
        }

        [TestMethod]
        public void ApproximateBestPathSorterFindsABetterPathThanOtherWordSorters()
        {
            var board = new Board(4, 4, p => _tileStrings2[p], p => null);
            board.GuessTilePoints();
            var solution = new Solution(board);
            double totalDistanceFromWords = solution.Words.Sum(w => w.PathLength);

            solution.SortWords(WordSorter.ApproximateBestPath);
            double totalDistanceForApproximateBestPathSorter = totalDistanceFromWords
                + ApproximateBestPathSorter.CalculateTotalDistanceBetweenWords(solution.Words);

            foreach (WordSorter wordSorter in WordSorter.All
                .Where(ws => ws != WordSorter.ApproximateBestPath))
            {
                solution.SortWords(wordSorter);
                double totalDistanceForWordSorter = totalDistanceFromWords + ApproximateBestPathSorter.CalculateTotalDistanceBetweenWords(solution.Words);

                Assert.IsTrue(totalDistanceForApproximateBestPathSorter < totalDistanceForWordSorter);
            }
        }
    }
}
