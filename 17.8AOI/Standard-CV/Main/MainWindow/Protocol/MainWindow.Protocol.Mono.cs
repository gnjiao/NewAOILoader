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
        const string key_std_Cam1Com = "std4";
        const string key_std_MonoCom = "std5";
        /// <summary>
        /// 当前机台默认下游平台和来料玻璃角度一致，所以为0
        /// </summary>
        public static double MonoAngle => Cell_Down;

        /// <summary>
        /// 在单目处的玻璃x
        /// </summary>
        public static double MonoGlassX
        {
            get => MonoAngle % 180 == 0 ? ConfGlassX : ConfGlassY;
        }

        /// <summary>
        /// 在单目处的玻璃y
        /// </summary>
        public static double MonoGlassY
        {
            get => MonoAngle % 180 == 0 ? ConfGlassY : ConfGlassX;
        }

        public static double Cam1ComX => ParStd.Value1(key_std_Cam1Com);
        public static double Cam1ComY => ParStd.Value2(key_std_Cam1Com);
        public static double Cam1ComR => ParStd.Value3(key_std_Cam1Com);

        public static double MonoComX => ParStd.Value1(key_std_MonoCom);
        public static double MonoComY => ParStd.Value2(key_std_MonoCom);
        public static double MonoComR => ParStd.Value3(key_std_MonoCom);
    }
}
