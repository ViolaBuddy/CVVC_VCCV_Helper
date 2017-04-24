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
using System.Text.RegularExpressions;
using UtauLib;

namespace CVVC_VCCV_Helper
{
    public partial class MainForm : Form
    {
        // properties and variables
        private string _USTFile;
        private string[] _preamble;
        private List<UtauNote> _notesList;
        public bool Valid = true; //if something goes wrong, don't start the plugin at all
        public string USTFile {
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

                //check the last note is a NEXT
                if(_notesList.LastOrDefault().Number != "NEXT")
                {
                    Valid = false;
                    MessageBox.Show("Make sure you don't select the last note in the song.\n\n" +
                        "If you want to run this plugin on the whole song, add a rest after all the notes but don't select the rest.");
                }
            }
        }

        // methods
        public MainForm()
        {
            InitializeComponent();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void go_btn_Click(object sender, EventArgs e)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());

            addConnectingSounds(_notesList);

            //File.WriteAllLines(USTFile, lines, Encoding.GetEncoding("shift_jis"));
            Close();
        }

        /// <summary>
        /// create a new list of UtauNotes with the connecting notes inserted.
        /// the input list is unchanged, both at a surface and at a deep level
        /// precondition: the last note (and only the last note) in the input has number NEXT
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static List<UtauNote> addConnectingSounds(List<UtauNote> input)
        {
            List<UtauNote> output = new List<UtauNote>();
            for(var i = 0; i < input.Count; i++)
            {
                // if the note is PREV or NEXT, just plop it back in our output
                // remember the precond. says that the last item has to be NEXT, so i+1 later down will not raise IndexOutOfBounds
                if (input[i].Number == "PREV" || input[i].Number == "NEXT")
                {
                    output.Add(new UtauNote(input[i]));
                    continue;
                }
                
                List<UtauNote> connectingLyrics = CVVC_Chinese.GetConnectingNotes(
                    i==0 ? null : input[i-1].Lyric,
                    input[i].Lyric,
                    input[i+1].Lyric);

                output.AddRange(connectingLyrics);
            }
            return output;
        }
    }
}
