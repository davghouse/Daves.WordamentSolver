using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Daves.WordamentSolver.UnitTests
{
    [TestClass]
    public static class AssemblyInitializer
    {
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext context)
            => Solution.SetDictionary();
    }
}
