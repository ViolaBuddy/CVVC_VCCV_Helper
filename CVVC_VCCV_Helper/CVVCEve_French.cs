using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtauLib;

namespace CVVC_VCCV_Helper
{
    static class CVVCEve_French
    {
        //public static readonly Regex French_RE = new Regex(@"(.*?)(a|ai|e|eh|en|eu|i|in|o|on|oo|ou|u|ui|oi)(.*?)");
        public static readonly string vowels_RE_str = @"a|A|O|e|E|&|@|i|I|o|W|3|U|u|8";
        public static readonly string consonants_RE_str = @"b|d|ch|f|g|h|j|k|l|m|n|p|r|rr|r'|s|sh|t|v|w|x|y|z";
        public static readonly string starting_blends_RE_str =
            @"bl|br|by|dr|dy|fl|fr|fy|gl|gr|gy|hy|kl|kr|kw|ky|ly|my|ny|pl|pr|py|ry|rry|r'y|sk|st|sy|tr|vr|zy";
        public static readonly string ending_blends_RE_str =
            @"bl|br|dr|fl|fr|gl|gr|kl|kr|pl|pr|sk|st|tr|vr";

        public static readonly Regex vowels_RE = new Regex(vowels_RE_str);
        public static readonly Regex consonants_RE = new Regex(consonants_RE_str);
        public static readonly Regex starting_blends_RE = new Regex(starting_blends_RE_str);
        public static readonly Regex ending_blends_RE = new Regex(ending_blends_RE_str);

        public static readonly Regex syllable_RE =
            new Regex("(" + starting_blends_RE_str + "|" + consonants_RE_str + ")?" +
                "(" + vowels_RE_str + ")" +
                "(" + ending_blends_RE_str + "|" + consonants_RE_str + ")?"
                );
        public static readonly Regex get_starting_consonant_RE =
            new Regex("^(" + consonants_RE_str + ")");


        /// Return the current note, split up as necessary to match the prev and next notes
        /// (Returns a new copy of all notes; the inputs are unchanged)
        public static List<UtauNote> GetConnectingNotes(UtauNote prev, UtauNote curr, UtauNote next)
        {

            CVVCEve_French_Syllable lyric_split = CVVCEve_French_Syllable.SplitSyllable(curr.Lyric);
            string nextlyric_start = CVVCEve_French_Syllable.StartingConsonant(next.Lyric);


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
            }
            
            else if (nextlyric_start != "")
            {
                VC_consonant = " " + nextlyric_start;
            }
            else
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
            if (prev == null || prev.IsRest)
            {
                toReturn[0].Lyric = "- " + toReturn[0].Lyric;
            }

            //TODO: double-check what's going on with the UtauNote copy constructor and why the tempo is being copied from the next note or something
            //for now, hack:
            toReturn.ForEach(note =>
            {
                //note.MainValues.Remove("Tempo"); //remove if exists
            });
            return toReturn;
        }

        private class CVVCEve_French_Syllable
        {
            public String Onset;
            public String Nucleus;
            public String Coda;

            private CVVCEve_French_Syllable()
            {
                Onset = "";
                Nucleus = "";
                Coda = "";
            }
            private CVVCEve_French_Syllable(string onset, string nucleus, string coda)
            {
                Onset = onset;
                Nucleus = nucleus;
                Coda = coda;
            }

            /// takes a lyric and splits it into its onset/nucleus/coda parts
            /// returns a CVVC_English_Syllable of [null, null, null] if not recognized
            public static CVVCEve_French_Syllable SplitSyllable(string lyric)
            {
                var matches = syllable_RE.Match(lyric).Groups;
                var result = new CVVCEve_French_Syllable();

                if (matches.Count < 3)
                {
                    // not recognized
                    result.Onset = null;
                    result.Nucleus = null;
                    result.Coda = null;
                    return new CVVCEve_French_Syllable((string)null, null, null);
                }

                result.Onset = matches[1].Value;
                result.Nucleus = matches[2].Value;
                result.Coda = matches[3].Value;

                // otherwise, return as expected
                return result;
            }

            /// takes a lyric and gets just the starting consonant, or the empty string if not recognized
            public static string StartingConsonant(string lyric)
            {
                var matches = get_starting_consonant_RE.Match(lyric).Groups;

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
                return Equals(obj as CVVCEve_French_Syllable);
            }
            public bool Equals(CVVCEve_French_Syllable obj)
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
