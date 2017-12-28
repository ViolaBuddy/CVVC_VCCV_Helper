using System;
using CVVC_VCCV_Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;
using UtauLib;

namespace VCCV_English_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void VCCV_English_Syllable_test()
        {
            Console.WriteLine("STARTING VCCV ENGLISH SYLLABLE TEST");

            // reflection to test private stuff
            var parent_type = typeof(VCCV_English);
            var inner_type = parent_type.GetNestedType("VCCV_English_Syllable", BindingFlags.NonPublic);
            PrivateType inner_type_private = new PrivateType(inner_type);

            //1
            var result = inner_type_private.InvokeStatic("SplitSyllable", BindingFlags.NonPublic, "lak");
            var expected = new PrivateObject(Activator.CreateInstance(inner_type, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { "l", "a", "k" },
                null, null));
            Assert.AreEqual<string>((string)expected.Invoke("ToString"), result.ToString());
            
            //2
            result = inner_type_private.InvokeStatic("SplitSyllable", BindingFlags.NonPublic, "la");
            expected = new PrivateObject(Activator.CreateInstance(inner_type, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] {
                    new List<string> { "l" } ,
                    new List<string> { "a" } ,
                    new List<string> { }
                },
                null, null));
            Assert.AreEqual<string>((string)expected.Invoke("ToString"), result.ToString());

            //3
            result = inner_type_private.InvokeStatic("SplitSyllable", BindingFlags.NonPublic, "ak");
            expected = new PrivateObject(Activator.CreateInstance(inner_type, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] {
                    new List<string> { } ,
                    new List<string> { "a" } ,
                    new List<string> { "k" }
                },
                null, null));
            Assert.AreEqual<string>((string)expected.Invoke("ToString"), result.ToString());

            //4
            result = inner_type_private.InvokeStatic("SplitSyllable", BindingFlags.NonPublic, "stak");
            expected = new PrivateObject(Activator.CreateInstance(inner_type, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { "st", "a", "k" },
                null, null));
            Assert.AreEqual<string>((string)expected.Invoke("ToString"), result.ToString());

            //5
            /*result = inner_type_private.InvokeStatic("SplitSyllable", BindingFlags.NonPublic, "b6lbz");
            expected = new PrivateObject(Activator.CreateInstance(inner_type, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] {
                    new List<string> { "b" } ,
                    new List<string> { "6" } ,
                    new List<string> { "l", "bz" }
                },
                null, null));
            Assert.AreEqual<string>((string)expected.Invoke("ToString"), result.ToString());*/

            //6
            result = inner_type_private.InvokeStatic("SplitSyllable", BindingFlags.NonPublic, "str1ng");
            expected = new PrivateObject(Activator.CreateInstance(inner_type, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { "str", "1", "ng" },
                null, null));
            Assert.AreEqual<string>((string)expected.Invoke("ToString"), result.ToString());

            //7
            result = inner_type_private.InvokeStatic("SplitSyllable", BindingFlags.NonPublic, "pyongz");
            expected = new PrivateObject(Activator.CreateInstance(inner_type, BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] { "py", "o", "ngz" },
                null, null));
            Assert.AreEqual<string>((string)expected.Invoke("ToString"), result.ToString());

            Console.WriteLine("ENDING VCCV ENGLISH SYLLABLE TEST");
        }

        [TestMethod]
        public void VCCV_English_test()
        {
            Console.WriteLine("STARTING VCCV ENGLISH GetConnectingNotes TEST");
            

            Console.WriteLine("ENDING VCCV ENGLISH GetConnectingNotes TEST");
        }
    }
}
