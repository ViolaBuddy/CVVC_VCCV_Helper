using System;
using CVVC_VCCV_Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;
using UtauLib;

namespace VCCV_English_Test
{
    [TestClass]
    public class UnitTestArpasing
    {
        [TestMethod]
        public void Arpasing_English_SplitSyllable_test()
        {
            Console.WriteLine("STARTING Arpasing ENGLISH SYLLABLE TEST");

            // reflection to test private stuff
            var parent_type = typeof(Arpasing_English);
            var inner_type = parent_type.GetNestedType("Arpasing_English_Syllable", BindingFlags.NonPublic);
            PrivateType inner_type_private = new PrivateType(inner_type);

            //1
            var result = inner_type_private.InvokeStatic("SplitSyllable", BindingFlags.NonPublic, "t uw");
            var expected = new PrivateObject(Activator.CreateInstance(inner_type, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] {
                    new List<string> { "t", "uw" }
                },
                null, null));
            Assert.AreEqual<string>((string)expected.Invoke("ToString"), result.ToString());

            //2
            result = inner_type_private.InvokeStatic("SplitSyllable", BindingFlags.NonPublic, "t uw n");
            expected = new PrivateObject(Activator.CreateInstance(inner_type, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] {
                    new List<string> { "t", "uw", "n" }
                },
                null, null));

            //3
            result = inner_type_private.InvokeStatic("SplitSyllable", BindingFlags.NonPublic, "s t r ay p d");
            expected = new PrivateObject(Activator.CreateInstance(inner_type, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] {
                    new List<string> { "s", "t", "r", "ay", "p", "d" }
                },
                null, null));
            Assert.AreEqual<string>((string)expected.Invoke("ToString"), result.ToString());

            Console.WriteLine("ENDING Arpasing ENGLISH SYLLABLE TEST");
        }

        [TestMethod]
        public void VCCV_English_test()
        {
            Console.WriteLine("STARTING ARPASING ENGLISH GetConnectingNotes TEST");

            //// reflection to test private stuff
            //var parent_type = typeof(VCCV_English);
            //var inner_type = parent_type.GetNestedType("VCCV_English_Syllable", BindingFlags.NonPublic);
            //PrivateType inner_type_private = new PrivateType(inner_type);

            //1
            var prev_note = (UtauNote) null;
            var input_note = new UtauNote(new string[] {
                "[#0010]",
                "Length=380",
                "Lyric=f ay n",
                "NoteNum=50"
            });
            var next_note = new UtauNote(new string[] {
                "[#0011]",
                "Length=240",
                "Lyric=R",
                "NoteNum=46"
            });
            var result = Arpasing_English.GetConnectingNotes(prev_note, input_note, next_note);
            var expected = new List<UtauNote>(new UtauNote[] {
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=- f",
                    "NoteNum=50",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=200",
                    "Lyric=f ay",
                    "NoteNum=50",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=ay n",
                    "NoteNum=50",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=n -",
                    "NoteNum=50",
                }),
                });
            CollectionAssert.AreEqual(expected.ConvertAll<string>(x => x.ToString()), result.ConvertAll<string>(x => x.ToString()));

            //2
            prev_note = null;
            input_note = new UtauNote(new string[] {
                "[#0010]",
                "Length=380",
                "Lyric=ao r g",
                "NoteNum=50"
            });
            next_note = new UtauNote(new string[] {
                "[#0011]",
                "Length=240",
                "Lyric=R",
                "NoteNum=46"
            });
            result = Arpasing_English.GetConnectingNotes(prev_note, input_note, next_note);
            expected = new List<UtauNote>(new UtauNote[] {
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=200",
                    "Lyric=- ao",
                    "NoteNum=50",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=ao r",
                    "NoteNum=50",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=r g",
                    "NoteNum=50",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=g -",
                    "NoteNum=50",
                }),
                });
            CollectionAssert.AreEqual(expected.ConvertAll<string>(x => x.ToString()), result.ConvertAll<string>(x => x.ToString()));

            //3
            prev_note = new UtauNote(new string[] {
                "[#0009]",
                "Length=240",
                "Lyric=R",
                "NoteNum=46"
            });
            input_note = new UtauNote(new string[] {
                "[#0010]",
                "Length=380",
                "Lyric=f l ay",
                "NoteNum=50"
            });
            next_note = new UtauNote(new string[] {
                "[#0011]",
                "Length=240",
                "Lyric=m iy",
                "NoteNum=46"
            });
            result = Arpasing_English.GetConnectingNotes(prev_note, input_note, next_note);
            expected = new List<UtauNote>(new UtauNote[] {
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=- f",
                    "NoteNum=50",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=f l",
                    "NoteNum=50",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=200",
                    "Lyric=l ay",
                    "NoteNum=50",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=ay m",
                    "NoteNum=50",
                }),
                });
            CollectionAssert.AreEqual(expected.ConvertAll<string>(x => x.ToString()), result.ConvertAll<string>(x => x.ToString()));

            //4
            prev_note = new UtauNote(new string[] {
                "[#0009]",
                "Length=380",
                "Lyric=y uw",
                "NoteNum=46"
            });
            input_note = new UtauNote(new string[] {
                "[#0010]",
                "Length=380",
                "Lyric=m ah s t",
                "NoteNum=46"
            });
            next_note = new UtauNote(new string[] {
                "[#0011]",
                "Length=380",
                "Lyric=b iy",
                "NoteNum=46"
            });
            result = Arpasing_English.GetConnectingNotes(prev_note, input_note, next_note);
            expected = new List<UtauNote>(new UtauNote[] {
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=200",
                    "Lyric=m ah",
                    "NoteNum=46",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=ah s",
                    "NoteNum=46",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=s t",
                    "NoteNum=46",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=t b",
                    "NoteNum=46",
                }),
                });
            result.ForEach(x => Console.WriteLine(x));
            CollectionAssert.AreEqual(expected.ConvertAll<string>(x => x.ToString()), result.ConvertAll<string>(x => x.ToString()));


            //5
            prev_note = new UtauNote(new string[] {
                "[#0009]",
                "Length=380",
                "Lyric=y uw",
                "NoteNum=46"
            });
            input_note = new UtauNote(new string[] {
                "[#0010]",
                "Length=380",
                "Lyric=aa r",
                "NoteNum=46"
            });
            next_note = null;
            result = Arpasing_English.GetConnectingNotes(prev_note, input_note, next_note);
            expected = new List<UtauNote>(new UtauNote[] {
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=260",
                    "Lyric=aa",
                    "NoteNum=46",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=aa r",
                    "NoteNum=46",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=r -",
                    "NoteNum=46",
                }),
                });
            result.ForEach(x => Console.WriteLine(x));
            CollectionAssert.AreEqual(expected.ConvertAll<string>(x => x.ToString()), result.ConvertAll<string>(x => x.ToString()));

            //6
            prev_note = new UtauNote(new string[] {
                "[#0009]",
                "Length=380",
                "Lyric=R",
                "NoteNum=46"
            });
            input_note = new UtauNote(new string[] {
                "[#0010]",
                "Length=380",
                "Lyric=aa r",
                "NoteNum=46"
            });
            next_note = null;
            result = Arpasing_English.GetConnectingNotes(prev_note, input_note, next_note);
            expected = new List<UtauNote>(new UtauNote[] {
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=260",
                    "Lyric=- aa",
                    "NoteNum=46",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=aa r",
                    "NoteNum=46",
                }),
                new UtauNote(new string[] {
                    "[#0010]",
                    "Length=60",
                    "Lyric=r -",
                    "NoteNum=46",
                }),
                });
            result.ForEach(x => Console.WriteLine(x));
            CollectionAssert.AreEqual(expected.ConvertAll<string>(x => x.ToString()), result.ConvertAll<string>(x => x.ToString()));

            Console.WriteLine("ENDING ARPASING ENGLISH GetConnectingNotes TEST");
        }
    }
}
