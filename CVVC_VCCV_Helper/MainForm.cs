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
        public bool Valid = true; // if something goes wrong, don't start the plugin at all
        public bool Testing = false; // if testing, print out instead of writing back to disk
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
            List<string> options = new List<string>() { "CVVC Chinese", "CVVC+ French", "VCCV English", "CVVC English" };
            dictionary_combo.Items.AddRange(options.ToArray());
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void go_btn_Click(object sender, EventArgs e)
        {
            Func<UtauNote, UtauNote, UtauNote, List<UtauNote>> selected_dictionary;
            Console.WriteLine(dictionary_combo);
            Console.WriteLine(dictionary_combo.SelectedItem);
            Console.WriteLine((string) dictionary_combo.SelectedItem);
            switch ((string) dictionary_combo.SelectedItem)
            {
                case "CVVC+ French":
                    selected_dictionary = CVVCPlus_French.GetConnectingNotes;
                    Console.WriteLine("Choosing CVVC+ French");
                    break;
                case "VCCV English":
                    selected_dictionary = VCCV_English.GetConnectingNotes;
                    Console.WriteLine("Choosing VCCV English");
                    break;
                case "CVVC English":
                    selected_dictionary = CVVC_English.GetConnectingNotes;
                    Console.WriteLine("Choosing CVVC English");
                    break;
                case "CVVC Chinese":
                    selected_dictionary = CVVC_Chinese.GetConnectingNotes;
                    Console.WriteLine("Choosing CVVC Chinese");
                    break;
                default:
                    selected_dictionary = CVVC_Chinese.GetConnectingNotes;
                    Console.WriteLine((string)dictionary_combo.SelectedItem);
                    Console.WriteLine("No selection detected... Choosing CVVC Chinese");
                    break;
            }

            _notesList = addConnectingSounds(_notesList, selected_dictionary);

            string output =
                String.Join("\n", _preamble) + "\n" +
                String.Join("\n", _notesList.Select(i => i.ToString()));

            if (Testing)
            {
                Console.WriteLine(output);
            } else
            {
                File.WriteAllText(USTFile, output, Encoding.GetEncoding("shift_jis"));
            }

            Close();
        }

        private void about_btn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version 0.2\n\nSee https://github.com/ViolaBuddy/CVVC_VCCV_Helper for more information.");
        }

        /// <summary>
        /// create a new list of UtauNotes with the connecting notes inserted.
        /// the input list is unchanged, both at a surface and at a deep level
        /// precondition: the last note (and only the last note) in the input has number NEXT
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static List<UtauNote> addConnectingSounds(List<UtauNote> input,
            Func<UtauNote, UtauNote, UtauNote, List<UtauNote>> getConnectingotes)
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
                
                List<UtauNote> connectingLyrics = getConnectingotes(
                    i==0 ? null : input[i-1],
                    input[i],
                    input[i+1]);
                output.AddRange(connectingLyrics);
            }
            return output;
        }
    }
}
