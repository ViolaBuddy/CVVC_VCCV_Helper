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
        public static readonly Regex French_RE = new Regex(@"(.*?)(a|ai|e|eh|en|eu|i|in|o|on|oo|ou|u|ui|oi)(.*?)");
        public static readonly Regex vowels_RE = new Regex(@"(a|ai|e|eh|en|eu|i|in|o|on|oo|ou|u|ui|oi)");
        public static readonly Regex consonants_RE = new Regex(@"(b|d|f|g|j|k|l|m|n|p|r|s|sh|t|v|z)");
        public static readonly Regex starting_blends_RE = new Regex(
            @"(br|bl|bz|dr|dl|fr|fl|gr|gl|gz|j|jr|jl|kr|kl|ks|mr|ml|nr|nl|pr|pl|ps|sr|sl|sk|shr|shl|tr|tl|ts|vr|vl|zr|zl)"
        );
        public static readonly Regex ending_blends_RE = new Regex(
            @"(rb|lb|zb|rd|ld|rf|lf|rg|lg|zg|rj|lj|rk|lk|sk|rl|rm|lm|zm|sm|rn|ln|zn|sn|rp|lp|zp|sp|rs|ls|rsh|lsh|rt|lt|st|rv|lv|rz|lz)"
        );
        public static readonly List<string> vowels = new List<string>(new string[]
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
        });

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
        }

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
            Console.WriteLine("DOING CVVC+ FRENCH STUFF!");

            Console.WriteLine("Note: this is not yet implemented.");

            Tuple<string, string, string> lyric_split = null;// split_CVVC_French(curr.Lyric);
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

            return toReturn;
        }

    }
}
