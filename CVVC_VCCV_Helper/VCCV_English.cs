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
    public static class VCCV_English
    {
        private static readonly string Sonorant_Consonant_str =
            @"ng|[mnlrwy]";
        private static readonly string Non_Sonorant_Consonant_str =
            @"th|dh|sh|zh|ch|dd|[bpmfvdtlszjgkrwy]";
        private static readonly string Starting_Only_Consonant_str =
            @"sk|sp|st|sm|sn|hh|h";
        private static readonly string Ending_Only_Consonant_str =
            @"nk";
        private static readonly string Onset_Cluster_str =
            @"bl|fl|gl|kl|pl|sl|spl|br|dr|fr|gr|kr|pr|tr|shr|skr|spr|str|thr|" +
            @"sf|dw|kw|gw|sw|tw|vw|skw|thw|by|fy|gy|ky|py|my|ny|vy|zy|sky|spy";
        private static readonly string Coda_Cluster_str =
            @"bd|bz|ft|fts|fs|gd|gz|ks|kt|kst|ps|pt|vz|vd|zd|znt|cht|chz|ts|dz|" +
            @"jd|dhd|sht|ths|md|mf|mfs|mp|mps|mpt|mz|ngz|nk|nks|nkth|sk|st|sts|sks|skt|" +
            @"nd|ns|nt|nts|nz|nj|njd|nch|ncht|nth|lb|lf|lk|lp|ls|lt|lv|lz|lch|lsh|lth";
        private static readonly string Pure_Vowel_str =
            @"[aeiuEo69@x]";
        private static readonly string Diphthong_Vowel_str =
            @"[0AIO8Q&13]";

        private static readonly string Onset_str =
            Onset_Cluster_str + "|" + Starting_Only_Consonant_str + "|" + Non_Sonorant_Consonant_str
            + "|" + Sonorant_Consonant_str + "|" /* + empty*/;
        private static readonly string Coda_str =
            Coda_Cluster_str + "|" + Ending_Only_Consonant_str + "|" + Non_Sonorant_Consonant_str
            + "|" + Sonorant_Consonant_str + "|" /* + empty*/;
        private static readonly string Vowel_str =
            Pure_Vowel_str + "|" + Diphthong_Vowel_str;

        public static readonly Regex Sonorant_Consonant_RE = new Regex(Sonorant_Consonant_str);
        public static readonly Regex Non_Sonorant_Consonant_RE = new Regex(Non_Sonorant_Consonant_str);
        public static readonly Regex Starting_Only_Consonant_RE = new Regex(Starting_Only_Consonant_str);
        public static readonly Regex Ending_Only_Consonant_RE = new Regex(Ending_Only_Consonant_str);
        public static readonly Regex Onset_Clusters_RE = new Regex(Onset_Cluster_str);
        public static readonly Regex Coda_Clusters_RE = new Regex(Coda_Cluster_str);
        public static readonly Regex Pure_Vowels_RE = new Regex(Pure_Vowel_str);
        public static readonly Regex Diphthong_Vowels_RE = new Regex(Diphthong_Vowel_str);

        public static readonly Regex Onset_RE = new Regex(Onset_str);
        public static readonly Regex Coda_RE = new Regex(Coda_str);
        public static readonly Regex Vowel_RE = new Regex(Vowel_str);

        public static readonly Regex Ending_Simple_Consonant_RE =
            new Regex(
                @"^(?:" + Non_Sonorant_Consonant_str + "|" + Sonorant_Consonant_str +
                ")"
            );
        public static readonly Regex Syllable_RE =
            new Regex("((?:" + Onset_str + ")+)" +
                "(" + Vowel_str + ")" +
                "((?:" + Coda_str + ")+)"
                );

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
            VCCV_English_Syllable lyric_split = VCCV_English_Syllable.SplitSyllable(curr.Lyric);
            VCCV_English_Syllable nextlyric_split = VCCV_English_Syllable.SplitSyllable(next.Lyric);

            List<UtauNote> toReturn = new List<UtauNote>();
            //if unrecognized, return the note untouched
            if (lyric_split.Onset[0] == null && lyric_split.Nucleus[0] == null)
            {
                return new List<UtauNote> { new UtauNote(curr) };
            }

            // add in the base note
            UtauNote CV = new UtauNote(curr);
            CV.Lyric = lyric_split.Onset[0] + lyric_split.Nucleus[0];
            toReturn.Add(CV);


            // if necessary, add a blending vowel _CV
            string editedV = GetStartingBlendVowel(lyric_split.Onset[0], lyric_split.Nucleus[0]);
            if (editedV != null)
            {
                UtauNote _CV = new UtauNote(curr);
                _CV.Lyric = editedV;
                _CV.Length = CV.Length - 60; //TODO: something more clever with lengths. For now, 60 is a 32th note
                CV.Lyric = lyric_split.Onset[0];
                CV.Length = 60;
                toReturn.Add(_CV);
                CV = _CV; //relabel references
            }

            // figure out the ending
            List<string> ending_split = EndingParser(lyric_split.Coda[0]);
            if (ending_split.Count == 0)
            {
                string newNote = GetConnectingSound(lyric_split.Nucleus[0], lyric_split.Coda[0], nextlyric_split.Onset[0], nextlyric_split.Nucleus[0]);
                UtauNote C_V = new UtauNote(curr);
                if (newNote != null)
                {
                    CV.Length = CV.Length - 60; //TODO: something more clever with lengths. For now, 60 is a 32th note
                    C_V.Lyric = newNote;
                    C_V.Length = 60;
                    toReturn.Add(C_V);
                }
                else if (nextlyric_split.Nucleus[0] == null)
                {
                    UtauNote V_rest = new UtauNote(curr);
                    CV.Length = CV.Length - 60; //TODO: something more clever with lengths. For now, 60 is a 32th note
                    V_rest.Lyric = lyric_split.Nucleus[0];
                    V_rest.Length = 60;
                    toReturn.Add(V_rest);
                }
            } else if (ending_split.Count == 1)
            {
                string newNote = GetEndingSound(lyric_split.Nucleus[0], lyric_split.Coda[0]);
                UtauNote VC = new UtauNote(curr);
                if (newNote != null)
                {
                    CV.Length = CV.Length - 60; //TODO: something more clever with lengths. For now, 60 is a 32th note
                    VC.Lyric = newNote;
                    VC.Length = 60;
                    if (Sonorant_Consonant_RE.IsMatch(lyric_split.Coda[0]) && !next.IsRest)
                    {
                        // on ending sonorants, add a dash before a C_C for blending
                        // this doesn't apply if the next sound is a rest
                        VC.Lyric = newNote + "-";
                    }
                    toReturn.Add(VC);
                }

                newNote = GetConnectingSound(lyric_split.Nucleus[0], lyric_split.Coda[0], nextlyric_split.Onset[0], nextlyric_split.Nucleus[0]);
                UtauNote C_V = new UtauNote(curr);
                if (newNote != null)
                {
                    CV.Length = CV.Length - 60; //TODO: something more clever with lengths. For now, 60 is a 32th note
                    C_V.Lyric = newNote;
                    C_V.Length = 60;
                    toReturn.Add(C_V);
                }
            } else
            {
                // ending consonant cluster(s) of some sort
                // TODO: more than one consonant cluster is not yet supported
                // e.g. b6lbz, which should be [b6][6l-][lb-][bz] is currently just [b6][6l-][lbz] but lbz is not a single sound

                string newNote = GetEndingSound(lyric_split.Nucleus[0], ending_split[0]);
                UtauNote VC = new UtauNote(curr);
                if (newNote != null)
                {
                    CV.Length = CV.Length - 60; //TODO: something more clever with lengths. For now, 60 is a 32th note
                    VC.Lyric = newNote + "-";
                    VC.Length = 60;
                    toReturn.Add(VC);
                }
                
                UtauNote VCC = new UtauNote(curr);
                if (newNote != null)
                {
                    CV.Length = CV.Length - 60; //TODO: something more clever with lengths. For now, 60 is a 32th note
                    VCC.Lyric = string.Join("", ending_split);
                    VCC.Length = 60;
                    toReturn.Add(VCC);
                }

                newNote = GetConnectingSound(ending_split[ending_split.Count - 2], ending_split[ending_split.Count - 1],
                    nextlyric_split.Onset[0], nextlyric_split.Nucleus[0]);
                UtauNote C_V = new UtauNote(curr);
                if (newNote != null)
                {
                    CV.Length = CV.Length - 60; //TODO: something more clever with lengths. For now, 60 is a 32th note
                    C_V.Lyric = newNote;
                    C_V.Length = 60;
                    toReturn.Add(C_V);
                }
            }


            // adjust first sound if previous note is a rest
            if (prev == null || prev.IsRest)
            {
                toReturn[0].Lyric = "-" + toReturn[0].Lyric;
            } else if (lyric_split.Onset[0] == "")
            {
                // if it's not following a rest but it starts with a vowel
                toReturn[0].Lyric = "_" + toReturn[0].Lyric;
            }
            // adjust last sound if following note is a rest
            if (next == null || next.IsRest)
            {
                toReturn[toReturn.Count-1].Lyric = toReturn[toReturn.Count - 1].Lyric + "-";
            }

            foreach (UtauNote n in toReturn.Skip(1))
            {
                n.MainValues.Remove("Tempo");
                //Remove tempo marking from any except the first (which is where it was originally)
            }

            return toReturn;
        }

        /// <summary>
        /// splits up a VCC ending into its component simple syllables
        /// </summary>
        /// <param name="ending"></param>
        /// <returns></returns>
        private static List<string> EndingParser(string ending)
        {
            List<string> toReturn = new List<string>();

            string theMatch = Ending_Simple_Consonant_RE.Match(ending).Value;
            while (theMatch != "")
            {
                toReturn.Add(theMatch);

                ending = Ending_Simple_Consonant_RE.Replace(ending, "");
                theMatch = Ending_Simple_Consonant_RE.Match(ending).Value;
            }

            return toReturn;
        }

        /// <summary>
        /// Returns the _CV or _V necessary after the given CCV
        /// If the CCV is actually a plain CV, return null instead
        /// If either of CCV or CV are null, also return null
        /// </summary>
        /// <param name="CCV"></param>
        /// <param name="V"></param>
        /// <returns></returns>
        private static string GetStartingBlendVowel(string CCV, string V)
        {
            if (CCV == null || V == null) return null;

            if (!Onset_Clusters_RE.IsMatch(CCV))
            {
                //not a CCV sound
                return null;
            }
            else if (CCV == "sf")
            {
                //sf is special because there aren't any _fV sounds
                return "_" + V;
            }
            else
            {
                //otherwise, take the last character and plop it before the vowel
                return "_" + CCV[CCV.Length - 1] + V;
            }
        }
        
        
        /// <summary>
        /// Returns the VC ending lyric, or null if not valid
        /// If last_V and last_VC are null, then the last note is a rest
        /// (If only one of them is null, this is an invalid input)
        /// Note that this is different from the empty string, which represents
        /// the syllable not having a coda/onset.
        /// </summary>
        /// <param name="last_V"></param>
        /// <param name="last_VC"></param>
        /// <returns></returns>
        private static string GetEndingSound(string last_V, string last_VC)
        {
            // if note is a rest
            if (last_V == null)
            {
                // R - (anything) as in "R ta" -> [R][-ta]
                return null;
            }

            // the normal case (not a rest)
            if (last_VC != "")
            {
                // K - C, as in "tak da" -> [ta][ak][da]
                return last_V + last_VC;
            }
            else
            {
                // V - C, as in "ta da" -> [ta][a d][a] and "tQ da" -> [tQ][Q d][da]
                return null;
            }
        }


        /// <summary>
        /// Returns the VC connecting lyric, or null if not valid
        /// If last_V and last_VC are null, then the last note is a rest
        /// (If only one of them is null, this is an invalid input)
        /// Likewise with next_CV and next_V
        /// Note that this is different from the empty string, which represents
        /// the syllable not having a coda/onset.
        /// 
        /// last_V might be a sonorant consonant or a true vowel
        /// </summary>
        /// <param name="last_V"></param>
        /// <param name="last_VC"></param>
        /// <param name="next_CV"></param>
        /// <param name="next_V"></param>
        /// <returns></returns>
        private static string GetConnectingSound(string last_V, string last_VC, string next_CV, string next_V)
        {
            // first, if next_CV is actually a CCV, drop the second C
            if (next_CV != null && Onset_Clusters_RE.IsMatch(next_CV))
            {
                next_CV = next_CV.Substring(0, next_CV.Length - 1);
            }


            // if either note is a rest
            if (last_V == null || next_V == null)
            {
                // R - (anything) as in "R ta" -> [R][-ta]
                return null;
            }


            // the normal case (neither is a rest)
            if (next_CV == "")
            {
                if (last_VC == "" && !Pure_Vowels_RE.IsMatch(last_V))
                {
                    // H - V, as in "kQ a" -> [kQ][Qa][a]
                    // TODO: I think the Qa is supposed to go at the start of the next syllable, but oh well
                    return last_V + next_V;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (last_VC != "" && Sonorant_Consonant_RE.IsMatch(last_VC))
                {
                    // N - C, as in "tam da" -> [ta][am][m d][da]
                    // TODO: return two strings
                    return last_VC + " " + next_CV;
                }
                else if (last_VC == "")
                {
                    // V - C, as in "ta da" -> [ta][a d][a] and "tQ da" -> [tQ][Q d][da]
                    return last_V + " " + next_CV;
                }
                else
                {
                    return null;
                }
            }
        }



        private class VCCV_English_Syllable
        {
            public List<string> Onset;
            public List<string> Nucleus;
            public List<string> Coda;

            private VCCV_English_Syllable()
            {
                Onset = new List<string>();
                Nucleus = new List<string>();
                Coda = new List<string>();
            }
            private VCCV_English_Syllable(string onset, string nucleus, string coda)
            {
                Onset = new List<string>(); Onset.Add(onset);
                Nucleus = new List<string>(); Nucleus.Add(nucleus);
                Coda = new List<string>(); Coda.Add(coda);
            }
            private VCCV_English_Syllable(List<string> onset, List<string> nucleus, List<string> coda)
            {
                Onset = new List<string>(onset);
                Nucleus = new List<string>(nucleus);
                Coda = new List<string>(coda);
            }

            /// <summary>
            /// takes a lyric and splits it into its onset/nucleus/coda parts
            /// returns a VCCV_English_Syllable of [null, null, null] if not recognized
            /// </summary>
            /// <param name="lyric"></param>
            /// <returns></returns>
            public static VCCV_English_Syllable SplitSyllable(string lyric)
            {
                var matches = Syllable_RE.Match(lyric).Groups;
                var result = new VCCV_English_Syllable();

                if (matches.Count < 3)
                {
                    // not recognized
                    result.Onset = null;
                    result.Nucleus = null;
                    result.Coda = null;
                    return new VCCV_English_Syllable((string)null, null, null);
                }

                result.Onset.Add(matches[1].Value);
                result.Nucleus.Add(matches[2].Value);
                result.Coda.Add(matches[3].Value); //TODO: recognize multiple ending sounds

                // otherwise, return as expected
                return result;
            }

            public override string ToString()
            {
                return
                    "SYLLABLE:\n    " +
                    string.Join<string>(",", Onset) + ";\n    " +
                    string.Join<string>(",", Nucleus) + ";\n    " +
                    string.Join<string>(",", Coda) + ";\n" +
                    "END SYLLABLE";
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as VCCV_English_Syllable);
            }
            public bool Equals(VCCV_English_Syllable obj)
            {
                if (obj == null) return false;

                return obj.Onset.SequenceEqual(this.Onset) &&
                     obj.Nucleus.SequenceEqual(this.Nucleus) &&
                     obj.Coda.SequenceEqual(this.Coda);
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }
        }
    }
}
