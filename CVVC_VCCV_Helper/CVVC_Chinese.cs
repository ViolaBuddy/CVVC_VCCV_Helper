using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtauLib;

namespace CVVC_VCCV_Helper
{
    static class CVVC_Chinese
    {
        public static readonly Regex Chinese_RE = new Regex(@"(zh|ch|sh|[bpmfdtnlgkhjqxzcsrwy]|)(.+?)$");

        /// <summary>
        /// split a Chinese character's pinyin into an initial and a final
        /// or a (null, null) tuple if the lyric can't be recognized
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        private static Tuple<string, string> split_CVVC_Chinese(string lyric)
        {
            var matches = Chinese_RE.Match(lyric).Groups;

            if (matches.Count <= 1)
            {
                // not recognized
                return new Tuple<string, string>(null, null);
            }

            // otherwise, return as expected
            return new Tuple<string, string>(matches[1].Value, matches[2].Value);
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
            Tuple<string, string> lyric_split = split_CVVC_Chinese(curr.Lyric);
            Tuple<string, string> nextlyric_split = split_CVVC_Chinese(next.Lyric);
            if (next.IsRest)
            {
                nextlyric_split = new Tuple<string, string>("R", null);
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
            string conn = GetConnectingLyric(lyric_split.Item1, lyric_split.Item2, nextlyric_split.Item1, nextlyric_split.Item2);
            
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

        /// <summary>
        /// Returns the connecting lyric, or null if not valid
        /// If both of XXX_onset and XXX_rime are null, assume that this note is a rest
        /// </summary>
        /// <param name="last_onset"></param>
        /// <param name="last_rime"></param>
        /// <param name="next_onset"></param>
        /// <param name="next_rime"></param>
        /// <returns></returns>
        private static string GetConnectingLyric(string last_onset, string last_rime, string next_onset, string next_rime)
        {
            if ((last_onset == null && last_rime == null) || last_rime == "R" || last_rime == "r")
            {
                return null;
            }
            if (next_onset == null && next_rime == null)
            {
                next_onset = "R";
            }

            //there's probably a better way of doing this
            if (last_rime == "iu")
            {
                last_rime = "ou";
            }
            else if (last_rime == "ui")
            {
                last_rime = "ei";
            }
            else if (last_rime == "v")
            {
                last_rime = "yu";
            }
            else if (last_rime == "o" || last_rime == "uo")
            {
                last_rime = "wo";
            }
            else if (last_rime == "ue")
            {
                last_rime = "yue";
            }
            else if (last_rime == "uai")
            {
                last_rime = "ai";
            }
            else if (last_rime == "uan")
            {
                if (last_onset == "j" || last_onset == "q" || last_onset == "x" || last_onset == "y")
                {
                    last_rime = "yuan";
                }
                else
                {
                    last_rime = "an";
                }
            }
            else if (last_rime == "ia" || last_rime == "ua")
            {
                last_rime = "a";
            }
            else if (last_rime == "ie")
            {
                last_rime = "ye";
            }
            else if (last_rime == "ian")
            {
                last_rime = "yan";
            }
            else if (last_rime == "iang")
            {
                last_rime = "an";
            }
            else if (last_rime == "in")
            {
                last_rime = "yin";
            }
            else if (last_rime == "ing")
            {
                last_rime = "ying";
            }
            else if (last_rime == "iao")
            {
                last_rime = "ao";
            }
            else if (last_rime == "ong" || last_rime == "iong")
            {
                last_rime = "yong";
            }
            else if (last_rime == "i")
            {
                if (last_onset == "z" || last_onset == "c" || last_onset == "s")
                {
                    last_rime = "-i";
                }
                else if (last_onset == "zh" || last_onset == "ch" || last_onset == "sh" || last_onset == "r")
                {
                    last_rime = "h-i";
                }
                else
                {
                    last_rime = "yi";
                }
            }
            else if (last_rime == "u")
            {
                if (last_onset == "j" || last_onset == "q" || last_onset == "x" || last_onset == "y")
                {
                    last_rime = "yu";
                }
                else
                {
                    last_rime = "wu";
                }
            }

            if (next_onset == "y" && next_rime == "u")
            {
                next_onset = "yu";
            }

            return last_rime + " " + next_onset;
        }
    }
}
