using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidiRounder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Form1 form = new Form1();
                form.USTFile = args.FirstOrDefault();
                if (form.USTFile == null)
                {
                    //form.USTFile = @"../../../test_temp2.ust";
                    form.USTFile = @"../../../test_broken.ust";
                    form.Testing = true;
                }
                if (form.Valid)
                {
                    Application.Run(form);
                }
            }
            finally
            {

            }
        }
    }
}
