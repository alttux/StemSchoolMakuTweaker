using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MakuTweaker
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool openForm3First = Properties.Settings.Default.sleep;

            if (openForm3First)
            {
                Application.Run(new Form3());
            }
            else
            {
                Application.Run(new Form1());
            }
        }
    }
}
