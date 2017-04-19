using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVVC_VCCV_Helper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm form = new MainForm();
            form.USTFile = args.FirstOrDefault();
            if (form.USTFile == null)
            {
                form.USTFile = @"../../../Test_tempfile.ust";
            }
            Application.Run(form);
            Application.Run(new MainForm());
        }
    }
}
