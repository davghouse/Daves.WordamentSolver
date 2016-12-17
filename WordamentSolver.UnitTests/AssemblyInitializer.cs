using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordamentSolver.Helpers;
using WordamentSolver.Models;

namespace WordamentSolver.UnitTests
{
    [TestClass]
    public static class AssemblyInitializer
    {
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext context)
        {
            Solution.SetDictionary(FileHelper.ReadDictionaryFile());
        }
    }
}
