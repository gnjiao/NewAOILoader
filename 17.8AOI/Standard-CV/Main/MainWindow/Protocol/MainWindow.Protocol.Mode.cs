using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    partial class Protocols
    {
        /// <summary>
        /// 运行设定模式-是否读取二维码
        /// </summary>
        public static bool DefaultQrCodeOK
        {
            get
            {
                return !DealCIM.PostParams.P_I.BlCodeOn;//ParSetWork.P_I.WorkSelect_L[1].BlSelect;
            }
        }

        public static bool IfPassCodeNG
        {
            get
            {
                return DealCIM.PostParams.P_I.BlPassVerifyCode && DealCIM.PostParams.P_I.BlPassVerifyCodeEnabled;//ParSetWork.P_I.WorkSelect_L[4].BlSelect;
            }
        }
    }
}
