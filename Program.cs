using System;
using System.Windows.Forms;

namespace Taskkiller
{
    static class Program
    {
        public static TaskkillerMain MainContext;
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainContext = new TaskkillerMain();
            Application.Run(MainContext);
        }
    }
}
