using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtauLib;

namespace CVVC_VCCV_Helper
{
    static class CVVCPlus_French
    {
        //public static readonly Regex French_RE = new Regex(@"(.*?)(a|ai|e|eh|en|eu|i|in|o|on|oo|ou|u|ui|oi)(.*?)");
        public static readonly string vowels_RE_str = @"ai|eh|en|eu|in|on|oo|ou|ui|oi|a|e|i|o|u";
        public static readonly string consonants_RE_str = @"sh|b|d|f|g|j|k|l|m|n|p|r|s|t|v|w|y|z";
        public static readonly string starting_blends_RE_str =
            @"br|bl|bz|dr|dl|fr|fl|gr|gl|gz|j|jr|jl|kr|kl|ks|mr|ml|nr|nl|pr|pl|ps|sr|sl|sk|shr|shl|tr|tl|ts|vr|vl|zr|zl";
        public static readonly string ending_blends_RE_str =
            @"rb|lb|zb|rd|ld|rf|lf|rg|lg|zg|rj|lj|rk|lk|sk|rl|rm|lm|zm|sm|rn|ln|zn|sn|rp|lp|zp|sp|rs|ls|rsh|lsh|rt|lt|st|rv|lv|rz|lz";

        public static readonly string not_to_be_confused_for_nasals_RE_str = @"e|i|o";
        public static readonly Regex not_to_be_confused_for_nasals_RE = new Regex(not_to_be_confused_for_nasals_RE_str);
        public static readonly string the_nasal_consonant = @"n";

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


        /*public static readonly List<string> vowels = new List<string>(new string[]
        {
             "a", "ai", "e", "eh", "en", "eu", "i", "in", "o", "on", "oo", "ou", "u", "ui", "oi"
        });
        public static readonly List<string> consonants = new List<string>(new string[]
        {
            "b","d","f","g","j","k","l","m","n","p","r","s","sh","t","v","z"
        });
        public static readonly List<string> starting_clusters = new List<string>(new string[]
        {
            "br", "bl", "bz", "dr", "dl", "fr", "fl", "gr", "gl", "gz", "jr", "jl", "kr", "kl", "ks", "mr", "ml", "nr", "nl",
            "pr", "pl", "ps", "sr", "sl", "sk", "shr", "shl", "tr", "tl", "ts", "vr", "vl", "zr", "zl"
        });
        public static readonly List<string> ending_clusters = new List<string>(new string[]
        {
            "rb", "lb", "zb", "rd", "ld", "rf", "lf", "rg", "lg", "zg", "rj", "lj", "rk", "lk", "sk", "rl", "rm", "lm", "zm", "sm",
            "rn", "ln", "zn", "sn", "rp", "lp", "zp", "sp", "rs", "ls", "rsh", "lsh", "rt", "lt", "st", "rv", "lv", "rz", "lz"
        });*/

        /*
    /// <summary>
    /// split a French syllable into its onset, nucleus, and coda
    /// or a (null, null, null) tuple if the lyric can't be recognized
    /// </summary>
    /// <param name="lyric"></param>
    /// <returns></returns>
    private static Tuple<List<string>, string, List<string>> split_CVVC_French(string lyric)
    {
        var toReturnOnset = new List<string>();
        var toReturnCoda = new List<string>();
        var toReturnNucleus = "";
        var all_starting_clusters = consonants.Concat(starting_clusters);
        var max_starting_cluster_length = all_starting_clusters.Max(s => s.Length);
        var all_ending_clusters = consonants.Concat(ending_clusters);
        var next_lyric = lyric;

        // find onset
        var continue_searching = true;
        while (continue_searching) {
            continue_searching = false;
            // find a single onset
            for (var i = 0; i < max_starting_cluster_length; i++)
            {
                if (all_starting_clusters.Any(
                    prefix => lyric.StartsWith(prefix) && prefix.Length == max_starting_cluster_length
                    ))
                {
                    // iterates upwards, so at the end of the for loop, next_lyric is the maximum
                    next_lyric = lyric.Substring(i);
                    continue_searching = true;
                }
            }
            // if (continue_searching) toReturnOnset.Add(lyric.Substring(0, i));
            lyric = next_lyric;
        }

        // find a single onset
        for (var i = 0; i < max_starting_cluster_length; i++)
        {
            if (all_starting_clusters.Any(
                prefix => lyric.StartsWith(prefix) && prefix.Length == max_starting_cluster_length
                ))
            {
                // iterates upwards, so at the end of the for loop, next_lyric is the maximum
                toReturnNucleus = lyric.Substring(i);
                next_lyric = lyric.Substring(i);
                continue_searching = true;
            }
        }
        lyric = next_lyric;

        // otherwise, return as expected
        return new Tuple<List<string>, string, List<string>>(toReturnOnset,toReturnNucleus,toReturnCoda);
    }*/

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

            CVVCPlus_French_Syllable lyric_split = CVVCPlus_French_Syllable.SplitSyllable(curr.Lyric);
            string nextlyric_start = CVVCPlus_French_Syllable.StartingConsonant(next.Lyric);


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
                VC_consonant = nextlyric_start;
            }
            else
            {
                VC_consonant = null;
            }

            if (VC_consonant != null)
            {
                var matches = not_to_be_confused_for_nasals_RE.IsMatch(lyric_split.Nucleus);

                if (matches && VC_consonant == the_nasal_consonant)
                {
                    //SPECIAL CASE: handle non-nasals followed by n
                    VC.Lyric = lyric_split.Nucleus + " " + VC_consonant;
                } else
                {
                    //ORDINARY CASE
                    VC.Lyric = lyric_split.Nucleus + VC_consonant;
                }

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

            //TODO: double-check what's going on with the UtauNote copy constructor and why the tempo is being copied from the next note or something
            //for now, hack:
            toReturn.ForEach(note =>
            {
                //note.MainValues.Remove("Tempo"); //remove if exists
            });
            return toReturn;

            /*Tuple<string, string, string> lyric_split = null;// split_CVVC_French(curr.Lyric);
            Tuple<string, string, string> nextlyric_split = null;// split_CVVC_French(next.Lyric);
            if (next.IsRest)
            {
                nextlyric_split = null;// new Tuple<string, string>("R", null);
            }

             List<UtauNote> toReturn = new List<UtauNote>();

            // add in the base note
            UtauNote CV = new UtauNote(curr);
            if (prev == null || prev.IsRest)
            {
                CV.Lyric = "- " + CV.Lyric;
            }
            toReturn.Add(CV);

            // if necessary, add in the connecting note
            string conn = null;// GetConnectingLyric(lyric_split.Item1, lyric_split.Item2, nextlyric_split.Item1, nextlyric_split.Item2);
            
            if (conn != null)
            {
                UtauNote VC = new UtauNote(curr);
                VC.Lyric = conn;
                CV.Length = CV.Length - 60; //TODO: something more clever with lengths. For now, 60 is a 32th note
                VC.Length = 60;
                toReturn.Add(VC);
            }

            return toReturn;*/
        }

        private class CVVCPlus_French_Syllable
        {
            public String Onset;
            public String Nucleus;
            public String Coda;

            private CVVCPlus_French_Syllable()
            {
                Onset = "";
                Nucleus = "";
                Coda = "";
            }
            private CVVCPlus_French_Syllable(string onset, string nucleus, string coda)
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
            public static CVVCPlus_French_Syllable SplitSyllable(string lyric)
            {
                var matches = syllable_RE.Match(lyric).Groups;
                var result = new CVVCPlus_French_Syllable();

                if (matches.Count < 3)
                {
                    // not recognized
                    result.Onset = null;
                    result.Nucleus = null;
                    result.Coda = null;
                    return new CVVCPlus_French_Syllable((string)null, null, null);
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
                return Equals(obj as CVVCPlus_French_Syllable);
            }
            public bool Equals(CVVCPlus_French_Syllable obj)
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
