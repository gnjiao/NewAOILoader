using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using System.Threading;
using DealFile;
using DealConfigFile;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DealPLC;
using Common;
using System.Text.RegularExpressions;

namespace Main
{
    partial class MainWindow
    {
        /// <summary>
        /// 新建一个Task来初始化CIM端口
        /// </summary>
        public override void Init_CIM()
        {


        }

        /// <summary>
        /// 注册
        /// </summary>
        public override void LoginEvent_CIM()
        {
            try
            {


            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 关闭CIM端口
        /// </summary>
        public override void CloseCIM()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

    }
}
