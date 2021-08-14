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
    public static class Arpasing_English
    {
        //private static readonly string Consonant_str =
        //    @"th|dh|jh|sh|zh|ch|hh|dx|el|[bpmfvwdtlnszgkyrq]|-";
        //private static readonly string Vowel_str =
        //    @"aa|eh|ih|ah|iy|uw|uh|ao|ae|ax|er|ey|ay|ow|oy|aw";
        //private static readonly string Phoneme_str =
        //    Consonant_str + "|" + Vowel_str;

        //public static readonly Regex Starting_Consonant_RE = new Regex(Consonant_str);
        //public static readonly Regex Vowel_RE = new Regex(Vowel_str);
        //public static readonly Regex Syllable_RE =
        //    new Regex("(" + Phoneme_str + ")(?: (" + Phoneme_str + "))*");
        private static readonly string[] Consonants = new string[] { "th", "dh", "jh", "sh", "zh", "ch", "hh", "dx", "el", "b", "p", "m", "f",
            "v", "w", "d", "t", "l", "n", "s", "z", "g", "k", "y", "r", "q", "-" };
        private static readonly string[] Vowels  = new string[] { "aa", "eh", "ih", "ah", "iy", "uw", "uh", "ao", "ae", "ax", "er", "ey", "ay", "ow", "oy", "aw" };


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
            Arpasing_English_Syllable currsyllable = Arpasing_English_Syllable.SplitSyllable(curr.Lyric);
            //if unrecognized, return the note untouched
            if (currsyllable == null || curr.IsRest)
            {
                return new List<UtauNote> { new UtauNote(curr) };
            }

            //default to hyphen if the prev/next is a rest or doesn't exist
            string prevsyllable_end = "-";
            string nextsyllable_start = "-";
            if (prev != null && !prev.IsRest)
            {
                prevsyllable_end = null;
            }
            if (next != null && !next.IsRest)
            {
                var nextsyllable = Arpasing_English_Syllable.SplitSyllable(next.Lyric);
                nextsyllable_start = nextsyllable[0];
            }

            // identify the vowel (if more than one, choose any - here, we arbitrarily choose the last one)
            // if none, default to first phoneme
            int nucleus_index = 0;
            for (var i = 0; i < currsyllable.Count; i++)
            {
                if (Vowels.Contains(currsyllable[i]))
                {
                    nucleus_index = i;
                }
            }

            // build up the notes to return
            List<UtauNote> toReturn = new List<UtauNote>();
            UtauNote note;

            if (prevsyllable_end != null)
            {
                // add in the note connecting to the previous note, if it exists
                note = new UtauNote(curr);
                note.Lyric = prevsyllable_end + " " + currsyllable[0];
                note.Length = 60; //TODO: something more clever with lengths. For now, 60 is a 32nd note
                toReturn.Add(note);
            }
            else if(nucleus_index == 0)
            {
                // we start with a vowel, and we don't have a - to start from
                // so we add it in manually
                note = new UtauNote(curr);
                note.Lyric = currsyllable[0];
                note.Length = 60; //TODO: something more clever with lengths. For now, 60 is a 32nd note
                toReturn.Add(note);
            }
            else
            {
                // we have no initial note, so decrement the nucleus_index
                nucleus_index -= 1;
            }

            for (var i = 0; i < currsyllable.Count - 1; i++)
            {
                note = new UtauNote(curr);
                note.Lyric = currsyllable[i] + " " + currsyllable[i + 1];
                // set the note length to be 60 for now; we'll find the nucleus_index-th one and extend it at the end
                note.Length = 60; //TODO: something more clever with lengths. For now, 60 is a 32nd note
                toReturn.Add(note);
            }

            // add in the note connecting to the next note
            note = new UtauNote(curr);
            note.Lyric = currsyllable[currsyllable.Count - 1] + " " + nextsyllable_start;
            note.Length = 60; //TODO: something more clever with lengths. For now, 60 is a 32nd note
            toReturn.Add(note);

            toReturn[nucleus_index].Length = curr.Length - (toReturn.Count - 1) * 60; // TODO: does not handle the case when this ends up shorter than the original note

            return toReturn;
        }

        private class Arpasing_English_Syllable : List<string>
        {
            // actually it's no different from a normal List<string>, but to parallel other Syllable classes from, we still define this class

            private Arpasing_English_Syllable() : base()
            {
            }

            private Arpasing_English_Syllable(IEnumerable<string> input) : base()
            {
                this.AddRange(input);
            }

            /// <summary>
            /// Create a syllable, represented in here as a List of strings
            /// </summary>
            /// <param name="lyric"></param>
            /// <returns></returns>
            public static Arpasing_English_Syllable SplitSyllable(string lyric)
            {
                return new Arpasing_English_Syllable(lyric.Split(' '));
                //var matches = Syllable_RE.Match(lyric).Groups;
                //if (matches.Count == 0)
                //{
                //    return null;
                //}
                //var toAdd = matches.Cast<Group>().ToList().Select(x => x.Value).ToList();
                //var toReturn = new Arpasing_English_Syllable(toAdd.GetRange(1,toAdd.Count-1));
                //return toReturn;
            }

            public override string ToString()
            {
                return
                    "SYLLABLE:\n    " +
                    string.Join<string>("\n    ", this) +
                    "\nEND SYLLABLE";
            }
        }
    }
}
