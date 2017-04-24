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
        /// or null if the lyric can't be recognized
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        private static Tuple<string, string> split_CVVC_Chinese(string lyric)
        {
            var matches = Chinese_RE.Match(lyric).Groups;

            if (matches.Count <= 1)
            {
                // not recognized
                return null;
            }

            // otherwise, return as expected
            return new Tuple<string, string>(matches[1].Value, matches[2].Value);
        }

        public static List<UtauNote> GetConnectingNotes(string prevlyric, string lyric, string nextlyric)
        {
            //TODO!

            return null;
        }
    }
}
