using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordamentSolver.Models;

namespace WordamentSolver.UnitTests.Models
{
    [TestClass]
    public sealed class SolutionTests
    {
        private static string[] _simpleTileStrings = new[]
        {
            "A", "B", "E", "F",
            "G", "K", "U", "O",
            "L", "E", "M", "D",
            "R", "A", "F", "O"
        };

        private static string[] _complexTileStrings = new[]
        {
            "-ING", "I/S", "E", "R/P",
            "REA-", "D/E", "Y/U", "FE-",
            "LA", "E/S", "M/O", "-ED",
            "R/T", "A/T", "H", "O/B"
        };

        [TestMethod]
        public void SimpleSolution()
        {
            var board = new Board(4, 4, p => _simpleTileStrings[p], p => null);
            board.GuessTilePoints();
            var solution = new Solution(board, WordSorter.Points);

            Assert.AreEqual(solution.Words[0].String, "AGLEAM");
            Assert.AreEqual(solution.Words[0].Points, 32);
            Assert.AreEqual(solution.Words[4].String, "BAKER");
            Assert.AreEqual(solution.Words[4].Points, 24);
            Assert.AreEqual(solution.Words[109].String, "ERA");
            Assert.AreEqual(solution.Words[109].Points, 5);
            Assert.AreEqual(solution.TotalPoints, 1319);
            Assert.AreEqual(solution.WordsFound, 110);
        }

        [TestMethod]
        public void ComplexSolution()
        {
            var board = new Board(4, 4, p => _complexTileStrings[p], p => null);
            board.GuessTilePoints();
            var solution = new Solution(board, WordSorter.Points);

            Assert.AreEqual(solution.Words[0].String, "HOUSESAT");
            Assert.AreEqual(solution.Words[0].Points, 360);
            Assert.AreEqual(solution.Words[4].String, "RASURES");
            Assert.AreEqual(solution.Words[4].Points, 242);
            Assert.AreEqual(solution.Words[8].String, "STOURES");
            Assert.AreEqual(solution.Words[8].Points, 242);
            Assert.AreEqual(solution.TotalPoints, 61847);
            Assert.AreEqual(solution.WordsFound, 618);
        }
    }
}
