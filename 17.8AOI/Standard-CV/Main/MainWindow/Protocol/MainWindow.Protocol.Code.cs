using DealConfigFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    partial class Protocols
    {
        static string codeWaitTime = "adj13";

        public static int CodeWaitTimeBefore
        {
            get
            {
                return (int)ParAdjust.Value1(codeWaitTime);
            }
        }

        public static int CodeWaitTimeAfter
        {
            get
            {
                return (int)ParAdjust.Value2(codeWaitTime);
            }
        }
    }
}
