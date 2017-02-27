using System;
using System.Windows.Forms;
using Daves.WordamentSolver.Helpers;
using Daves.WordamentSolver.Models;
using Daves.WordamentSolver.Presenters;
using Daves.WordamentSolver.Views;

namespace Daves.WordamentSolver
{
    internal static class ApplicationEntryPoint
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Solution.SetDictionary(FileHelper.ReadDictionaryFile());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            var mainForm = new SolverForm();
            var mainFormPresenter = new SolverPresenter(mainForm);
            Application.Run(mainForm);
        }
    }
}
