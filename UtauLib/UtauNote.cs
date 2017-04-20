using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace UtauLib
{
    public class UtauNote
    {
        // Regexes for parsing
        private static readonly Regex Number_RE = new Regex(@"\[#(NEXT|PREV|\d+)\]");

        public string Number { get; set; }
        public OrderedDictionary MainValues { get; set; }
        public OrderedDictionary AtValues { get; set; } //values that are preceeded by an @ sign in the UST
        private string[] alldata;

        /// <summary>
        /// Create a note from the raw UST data.
        /// The first line of the UST data is [#XXXX]
        /// The rest of the lines are of the form Property=Value
        /// or @property=Value
        /// </summary>
        public UtauNote(string[] rawUst)
        {
            alldata = rawUst;
            //rawUst[0];
        }

        public override string ToString()
        {
            return String.Join("\n", alldata);
        }

        /// <summary>
        /// Splits the given UST File into a preamble followed by some number of notes
        /// The preamble is returned as the first of the tuple, and the list of notes as the second of the tuple
        /// </summary>
        /// <param name="rawUst"></param>
        /// <returns></returns>
        public static Tuple<string[], List<UtauNote>> parseUSTFile(string[] rawUst)
        {
            //handle the preamble
            int i = 0;
            while (!Number_RE.IsMatch(rawUst[i]))
            {
                i++;
            }
            string[] preamble = new ArraySegment<string>(rawUst, 0, i).Array;


            //handle the other notes
            int lasti = i;
            i++;
            List<UtauNote> allnotes = new List<UtauNote>();
            while (i < rawUst.Length)
            {
                while (i < rawUst.Length && !Number_RE.IsMatch(rawUst[i]))
                {
                    i++;
                }
                allnotes.Add(new UtauNote(new ArraySegment<string>(rawUst, lasti, i-lasti).Array));
                
                lasti = i;
                i++;
                Console.WriteLine(i);
            }

            Console.WriteLine("Done!");
            return new Tuple<string[], List<UtauNote>>(preamble, allnotes); // TODO
        }
    }
}
