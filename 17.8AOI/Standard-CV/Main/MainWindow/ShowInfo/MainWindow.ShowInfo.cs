#define CIM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlLib;
using Common;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Camera;
using HalconDotNet;
using DealFile;
using DealPLC;
using SetPar;
using BasicClass;
using DealLog;
using DealConfigFile;
using Main_EX;

namespace Main
{
    partial class MainWindow
    {
        #region 定义

        #endregion 定义

        #region 定时器刷新
        /// <summary>
        /// 定时器，每300ms刷新一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TM300_Refresh_Tick(object sender, EventArgs e)
        {
            try
            {
                base.TM300_Refresh_Tick(sender, e);

                //添加自定义方法
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 定时器刷新

        #region 显示结果
        /// <summary>
        /// 重载初始化
        /// </summary>
        //public override void Init_MainResult()
        //{

        //}
        #endregion 显示结果       
    }
}
