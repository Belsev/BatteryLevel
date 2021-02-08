using System;
using System.Windows.Forms;

namespace BatteryLevelOnKey
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool silent = args.Length > 0 && args[0] == "-silent";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var mainForm = new MainForm();
            mainForm.Start();
            if (silent)
            {
                mainForm.FormClosed += MainForm_FormClosed;
                Application.Run();
            }
            else
            {
                Application.Run(mainForm);
            }
        }

        private static void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
