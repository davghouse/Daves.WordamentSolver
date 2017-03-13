using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Daves.WordamentSolver.UnitTests
{
    [TestClass]
    public class SimpleSolutionTests
    {
        [TestMethod]
        public void BasicSolution()
        {
            var board = new Board(4, 4, p => SolutionTests.BasicTileStrings[p], p => null);
            var solution = new SimpleSolution(board);

            Assert.AreEqual(110, solution.TotalWords);
        }

        [TestMethod]
        public void ComplexSolution()
        {
            var board = new Board(4, 4, p => SolutionTests.ComplexTileStrings[p], p => null);
            var solution = new SimpleSolution(board);

            Assert.AreEqual(618, solution.TotalWords);
        }

        [TestMethod]
        public void PathMultiplicitySolution()
        {
            var board = new Board(3, 3, p => SolutionTests.PathMultiplicityTileStrings[p], p => null);
            var solution = new SimpleSolution(board);

            Assert.AreEqual(9, solution.TotalWords);
        }
    }
}
