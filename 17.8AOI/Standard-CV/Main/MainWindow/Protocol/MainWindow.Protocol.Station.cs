using BasicClass;
using Common;
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
        #region properties
        const string RefrenceStation1 = "Station1";
        const string RefrenceStation2 = "Station2";

        public static string StationDataPath => ParPathRoot.PathRoot
            + "Store\\产品参数\\" + ComConfigPar.C_I.NameModel + "\\Station.xml";
        #endregion

        #region adj
        public static int AjdX_Station1
        {
            get => (int)ParAdjust.Value1(RefrenceStation1);
            set
            {
                ParAdjust.SetValue1(RefrenceStation1, value);
            }
        }

        public static int AjdY_Station1
        {
            get => (int)ParAdjust.Value2(RefrenceStation1);
            set
            {
                ParAdjust.SetValue2(RefrenceStation1, value);
            }
        }

        public static int AjdZ_Station1
        {
            get => (int)ParAdjust.Value3(RefrenceStation1);
            set
            {
                ParAdjust.SetValue2(RefrenceStation1, value);
            }
        }

        public static int AjdR_Station1
        {
            get => (int)ParAdjust.Value4(RefrenceStation1);
            set
            {
                ParAdjust.SetValue4(RefrenceStation1, value);
            }
        }

        public static int AjdX_Station2
        {
            get => (int)ParAdjust.Value1(RefrenceStation2);
            set
            {
                ParAdjust.SetValue1(RefrenceStation2, value);
            }
        }

        public static int AjdY_Station2
        {
            get => (int)ParAdjust.Value2(RefrenceStation2);
            set
            {
                ParAdjust.SetValue2(RefrenceStation2, value);
            }
        }

        public static int AjdZ_Station2
        {
            get => (int)ParAdjust.Value3(RefrenceStation2);
            set
            {
                ParAdjust.SetValue3(RefrenceStation2, value);
            }
        }

        public static int AjdR_Station2
        {
            get => (int)ParAdjust.Value4(RefrenceStation2);
            set
            {
                ParAdjust.SetValue4(RefrenceStation2, value);
            }
        }
        #endregion
    }
}
