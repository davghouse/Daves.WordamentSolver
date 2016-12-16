using System;
using System.Windows.Forms;
using WordamentSolver.Presenters;
using WordamentSolver.Views;

namespace WordamentSolver
{
    internal static class ApplicationEntryPoint
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new SolverForm();
            var mainFormPresenter = new SolverPresenter(mainForm);
            Application.Run(mainForm);
        }
    }
}
