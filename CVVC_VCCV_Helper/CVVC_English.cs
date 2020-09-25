using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using UtauLib;

namespace CVVC_VCCV_Helper
{
    public static class CVVC_English
    {
        private static readonly string Starting_Consonant_str =
            @"th|tth|sh|zh|ch|[bpmfvwdtlnszjgkyhrw]";
        private static readonly string Vowel_str =
            @"[aeiuEo6@AIO8Q&13]";

        public static readonly Regex Starting_Consonant_RE = new Regex(Starting_Consonant_str);
        public static readonly Regex Vowel_RE = new Regex(Vowel_str);
        public static readonly Regex Syllable_RE =
            new Regex("(.*)" +
                "(" + Vowel_str + ")" +
                "(.*)"
                );
        public static readonly Regex Get_Starting_Consonant_RE =
            new Regex("^(" + Starting_Consonant_str + ")");

        /// <summary>
        /// Return the current note, split up as necessary to match the prev and next notes
        /// (Returns a new copy of all notes; the inputs are unchanged)
        /// 
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="curr"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static List<UtauNote> GetConnectingNotes(UtauNote prev, UtauNote curr, UtauNote next)
        {
            CVVC_English_Syllable lyric_split = CVVC_English_Syllable.SplitSyllable(curr.Lyric);
            string nextlyric_start = CVVC_English_Syllable.StartingConsonant(next.Lyric);

            List<UtauNote> toReturn = new List<UtauNote>();
            //if unrecognized, return the note untouched
            if (lyric_split.Onset == null && lyric_split.Nucleus == null)
            {
                return new List<UtauNote> { new UtauNote(curr) };
            }

            // add in the base note
            UtauNote CV = new UtauNote(curr);
            CV.Lyric = lyric_split.Onset + lyric_split.Nucleus;
            toReturn.Add(CV);

            // add in the VC note (first figuring out what C to add in)
            UtauNote VC = new UtauNote(curr);
            string VC_consonant;
            if (lyric_split.Coda != "")
            {
                VC_consonant = lyric_split.Coda;
            } else if (nextlyric_start != "")
            {
                VC_consonant = nextlyric_start;
            } else
            {
                VC_consonant = null;
            }

            if (VC_consonant != null)
            {
                VC.Lyric = lyric_split.Nucleus + VC_consonant;
                VC.Length = 120; //TODO: something more clever with lengths. For now, 120 is a 16th note
                toReturn.Add(VC);
                CV.Length = CV.Length - 120;
            }

            // adjust first sound if previous note is a rest (and this note has no starting consonant)
            if (lyric_split.Onset == "" && (prev == null || prev.IsRest))
            {
                toReturn[0].Lyric = "-" + toReturn[0].Lyric;
            }
            // adjust last sound if following note is a rest (and this note has no ending consonant)
            if (lyric_split.Coda == "" && (next == null || next.IsRest))
            {
                VC.Lyric = lyric_split.Nucleus + "-";
                VC.Length = 120; //TODO: something more clever with lengths. For now, 120 is a 16th note
                toReturn.Add(VC);
                CV.Length = CV.Length - 120;
            }

            //TODO: vowel-vowel sounds like V
            //TODO: double-check what's going on with the UtauNote copy constructor and why the tempo is being copied from the next note or something
            //for now, hack:
            toReturn.ForEach(note =>
            {
                //note.MainValues.Remove("Tempo"); //remove if exists
            });
            return toReturn;
        }


        private class CVVC_English_Syllable
        {
            public String Onset;
            public String Nucleus;
            public String Coda;

            private CVVC_English_Syllable()
            {
                Onset = "";
                Nucleus = "";
                Coda = "";
            }
            private CVVC_English_Syllable(string onset, string nucleus, string coda)
            {
                Onset = onset;
                Nucleus = nucleus;
                Coda = coda;
            }

            /// <summary>
            /// takes a lyric and splits it into its onset/nucleus/coda parts
            /// returns a CVVC_English_Syllable of [null, null, null] if not recognized
            /// </summary>
            /// <param name="lyric"></param>
            /// <returns></returns>
            public static CVVC_English_Syllable SplitSyllable(string lyric)
            {
                var matches = Syllable_RE.Match(lyric).Groups;
                var result = new CVVC_English_Syllable();

                if (matches.Count < 3)
                {
                    // not recognized
                    result.Onset = null;
                    result.Nucleus = null;
                    result.Coda = null;
                    return new CVVC_English_Syllable((string)null, null, null);
                }

                result.Onset = matches[1].Value;
                result.Nucleus = matches[2].Value;
                result.Coda = matches[3].Value;

                // otherwise, return as expected
                return result;
            }

            /// <summary>
            /// takes a lyric and gets just the starting consonant, or the empty string if not recognized
            /// </summary>
            /// <param name="lyric"></param>
            /// <returns></returns>
            public static string StartingConsonant(string lyric)
            {
                var matches = Get_Starting_Consonant_RE.Match(lyric).Groups;

                if (matches.Count < 1)
                {
                    // not recognized
                    return "";
                }

                // otherwise, return as expected
                return matches[1].Value;
            }

            public override string ToString()
            {
                return
                    "SYLLABLE:\n    " +
                    Onset + ";\n    " +
                    Nucleus + ";\n    " +
                    Coda + ";\n" +
                    "END SYLLABLE";
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as CVVC_English_Syllable);
            }
            public bool Equals(CVVC_English_Syllable obj)
            {
                if (obj == null) return false;

                return obj.Onset == this.Onset &&
                     obj.Nucleus == this.Nucleus &&
                     obj.Coda == this.Coda;
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }
        }
    }
}
