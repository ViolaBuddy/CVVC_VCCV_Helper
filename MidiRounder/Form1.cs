using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtauLib;

namespace MidiRounder
{
    public partial class Form1 : Form
    {
        // properties and variables
        private string _USTFile;
        private string[] _preamble;
        private List<UtauNote> _notesList;
        public bool Valid = true; // if something goes wrong, don't start the plugin at all
        public bool Testing = false; // if testing, print out instead of writing back to disk
        public string USTFile
        {
            get
            {
                return _USTFile;
            }
            set
            {
                _USTFile = value;
                if (value == null)
                {
                    _preamble = null;
                    _notesList = null;
                    return;
                }

                var lines = File.ReadAllLines(_USTFile, Helpers.Shift_JIS);
                var parsed = UtauLib.UtauNote.parseUSTFile(lines);
                _preamble = parsed.Item1;
                _notesList = parsed.Item2;
            }
        }

        public Form1()
        {

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(UtauNote note in _notesList) {
                // 80 is the length of a sixth of a quarter note
                // round to the nearest multiple of 80
                note.Length = ((note.Length + 80 / 2) / 80) * 80;
            }

            string output =
                String.Join("\n", _preamble) + "\n" +
                String.Join("\n", _notesList.Select(i => i.ToString()));

            if (Testing)
            {
                Console.WriteLine(output);
            }
            else
            {
                File.WriteAllText(USTFile, output, Encoding.GetEncoding("shift_jis"));
            }

            Close();
        }
    }
}
