using System;
using System.Windows.Forms;

namespace GTA_Vice_City_kodai
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main());

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace);

            }
        }
    }
}
