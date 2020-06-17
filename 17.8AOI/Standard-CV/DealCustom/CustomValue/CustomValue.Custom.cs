using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public partial class CustomValue
    {
        #region 静态类实例
        public CustomValue C_I = new CustomValue();
        #endregion 静态类实例

        #region 定义
        //bool
        public bool BlIdlePlate1 = false;//平台1空闲
        public bool BlIdlePlate2 = false;//平台2空闲
        public bool BlSafeLoad = false;//平台2空闲
        public int PosLoad = 0;//上料平台位置
        #endregion 定义
    }
}
