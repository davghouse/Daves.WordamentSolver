using Microsoft.VisualStudio.TestTools.UnitTesting;
using Daves.WordamentSolver.Helpers;
using Daves.WordamentSolver.Models;

namespace Daves.WordamentSolver.UnitTests
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
