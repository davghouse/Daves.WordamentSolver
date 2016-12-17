using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordamentSolver.Models;

namespace WordamentSolver.UnitTests.Models
{
    [TestClass]
    public sealed class WordComparerTests
    {
        private static string[] _tileStrings = new[]
        {
            "A", "D/E", "F", "E",
            "G", "O", "P", "R",
            "M", "T", "E", "N",
            "I", "F", "E", "R"
        };

        [TestMethod]
        public void VerifyWordComparers()
        {
            var board = new Board(4, 4, p => _tileStrings[p], p => null);
            board.GuessTilePoints();
            var solution = new Solution(board);

            solution.SortWords(WordComparer.Alphabet);
            Assert.AreEqual(solution.Words[0].String, "ADO");

            solution.SortWords(WordComparer.Points);
            Assert.AreEqual(solution.Words[0].String, "ADOPTER");

            solution.SortWords(WordComparer.WordLength);
            Assert.AreEqual(solution.Words[0].String, "ADOPTEE");

            solution.SortWords(WordComparer.PathLength);
            Assert.AreEqual(solution.Words[0].String, "MITERER");

            solution.SortWords(WordComparer.PointsOverWordLength);
            Assert.AreEqual(solution.Words[0].String, "ADOPT");

            solution.SortWords(WordComparer.PointsOverPathLength);
            Assert.AreEqual(solution.Words[0].String, "DAG");

            solution.SortWords(WordComparer.StartPositionByPoints);
            Assert.AreEqual(solution.Words[0].String, "ADOPTER");

            solution.SortWords(WordComparer.StartPositionByWordLength);
            Assert.AreEqual(solution.Words[0].String, "ADOPTEE");

            solution.SortWords(WordComparer.StartPositionByPointsOverWordLength);
            Assert.AreEqual(solution.Words[0].String, "ADOPT");

            solution.SortWords(WordComparer.StartPositionByPointsOverPathLength);
            Assert.AreEqual(solution.Words[0].String, "ADO");

            solution.SortWords(WordComparer.StartLetterByPoints);
            Assert.AreEqual(solution.Words[6].String, "DOPER");

            solution.SortWords(WordComparer.StartLetterByWordLength);
            Assert.AreEqual(solution.Words[118].String, "TOP");

            solution.SortWords(WordComparer.StartLetterByPointsOverWordLength);
            Assert.AreEqual(solution.Words[7].String, "DOG");

            solution.SortWords(WordComparer.StartLetterByPointsOverPathLength);
            Assert.AreEqual(solution.Words[8].String, "DOT");

            solution.SortWords(WordComparer.WordLengthAscending);
            Assert.AreEqual(solution.Words[3].String, "DAG");

            solution.SortWords(WordComparer.StartPositionByWordLengthAscending);
            Assert.AreEqual(solution.Words[118].String, "FETE");
        }
    }
}
