using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Linq;

namespace UtauLib
{
    public class UtauNote
    {
        // Regexes for parsing
        private static readonly Regex Number_RE = new Regex(@"^\[#(NEXT|PREV|\d+)\]$");
        //all property names are purely alphabetic (I think) but the values can be anything (I hope C# handles Unicode correctly)
        private static readonly Regex MainValues_RE = new Regex(@"^([a-zA-Z]+)=(.*)$");
        private static readonly Regex AtValues_RE = new Regex(@"^@([a-zA-Z]+)=(.*)$");

        public string Number { get; set; }
        public OrderedDictionary MainValues { get; set; }
        public OrderedDictionary AtValues { get; set; } //values that are preceeded by an @ sign in the UST
        public bool IsRest { get { return (string) MainValues["Lyric"] == "R" || (string)MainValues["Lyric"] == "r"; } }
        public string Lyric {
            get { return (string)MainValues["Lyric"]; }
            set { MainValues["Lyric"] = value; }
        }
        public int Length {
            get { return Int32.Parse((string) MainValues["Length"]); }
            set { MainValues["Length"] = value.ToString(); }
        }

        /// <summary>
        /// Create a note from the raw UST data.
        /// The first line of the UST data is [#XXXX]
        /// The rest of the lines are of the form Property=Value
        /// or @property=Value
        /// </summary>
        public UtauNote(string[] rawUst)
        {
            MainValues = new OrderedDictionary();
            AtValues = new OrderedDictionary();

            // parse the Number
            var i = 0;
            var matches = Number_RE.Matches(rawUst[i]);
            if (matches.Count <= 0)
            {
                throw new ArgumentException("First line of rawUst must be a number line");
            }
            Number = matches[0].Groups[1].Value;

            // parse the Main Values
            i = 1;
            if (i < rawUst.Length)
                matches = MainValues_RE.Matches(rawUst[i]);
            while (i < rawUst.Length && matches.Count > 0)
            {
                MainValues.Add(matches[0].Groups[1].Value, matches[0].Groups[2].Value);

                i++;
                if (i < rawUst.Length)
                    matches = MainValues_RE.Matches(rawUst[i]);
            }

            // parse the At Values - I assume the At values all come after the Main values
            if (i < rawUst.Length)
                matches = AtValues_RE.Matches(rawUst[i]);
            while (i < rawUst.Length && matches.Count > 0)
            {
                AtValues.Add(matches[0].Groups[1].Value, matches[0].Groups[2].Value);

                i++;
                if (i < rawUst.Length)
                    matches = AtValues_RE.Matches(rawUst[i]);
            }

            if (i < rawUst.Length)
            {
                string output = "Not everything has been parsed!\n";
                foreach(var item in rawUst)
                {
                    output = output + item;
                }

                throw new ArgumentException(output + "\n\n");
            }
        }

        /// <summary>
        /// Copy constructor: the internal fields are also completely recreated (deep copy)
        /// </summary>
        /// <param name="u"></param>
        public UtauNote(UtauNote u)
        {
            Number = u.Number;

            MainValues = new OrderedDictionary();
            foreach (DictionaryEntry de in u.MainValues)
            {
                MainValues.Add(de.Key, de.Value);
            }

            AtValues = new OrderedDictionary();
            foreach (DictionaryEntry de in u.AtValues)
            {
                AtValues.Add(de.Key, de.Value);
            }
        }

        /// <summary>
        /// Returns the note as a string formatted as the UST would want it
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            List<string> toReturn = new List<string>();
            toReturn.Add("[#" + Number + "]");

            foreach (System.Collections.DictionaryEntry item in MainValues)
            {
                toReturn.Add(item.Key + "=" + item.Value);
            }

            foreach (System.Collections.DictionaryEntry item in AtValues)
            {
                toReturn.Add("@" + item.Key + "=" + item.Value);
            }
            
            return String.Join("\n", toReturn);
        }

        /// <summary>
        /// Returns whether or not this note is equal to obj
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(UtauNote other)
        {
            Console.WriteLine("AAAAAAAAAAAAAAAAAaa");
            return other != null &&
                this.Lyric == other.Lyric &&
                this.Length == other.Length &&
                this.Number == other.Number &&
                this.IsRest == other.IsRest;
            // This is nonstrict: it only test for the main properties
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
            string[] preamble = new string[i];
            for(var j = 0; j < i; j++)
            {
                preamble[j] = rawUst[j];
            }

            //handle the other notes
            int lasti = i;
            i++;
            List<UtauNote> allnotes = new List<UtauNote>();
            while (i < rawUst.Length)
            {
                while (i < rawUst.Length && !Number_RE.IsMatch(rawUst[i]))
                {
                    // skip past parts that aren't note delimiters
                    i++;
                }

                //copy the relevant data into a new array
                string[] noteArg = new string[i - lasti];
                Array.Copy(rawUst, lasti, noteArg, 0, i - lasti);
                
                allnotes.Add(new UtauNote(noteArg));
                lasti = i;
                i++;
            }

            return new Tuple<string[], List<UtauNote>>(preamble, allnotes);
        }

        /// <summary>
        /// Renumbers the given UtauNote List from zero, in place, skipping over PREV and NEXT notes
        /// </summary>
        /// <param name="input"></param>
        public static void Renumber(List<UtauNote> input)
        {
            int counter = 0;
            foreach(var item in input)
            {
                if (item.Number == "PREV" || item.Number == "NEXT")
                    continue;
                item.Number = String.Format("{0:0000}", item.Number);
                counter++;
            }
        }
    }
}
