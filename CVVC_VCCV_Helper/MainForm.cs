using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using UtauLib;

namespace CVVC_VCCV_Helper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public string USTFile { get; set; }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void go_btn_Click(object sender, EventArgs e)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            var lines = File.ReadAllLines(USTFile, Helpers.Shift_JIS);

            Console.WriteLine("\n\n STARTING PARSE \n\n");
            var parsed = UtauLib.UtauNote.parseUSTFile(lines);
            Console.WriteLine("Done!");
            parsed.Item2.ForEach(
                (k) => Console.WriteLine(k + "\n")
            );


            //File.WriteAllLines(USTFile, lines, Encoding.GetEncoding("shift_jis"));
            Close();
        }
    }
}
