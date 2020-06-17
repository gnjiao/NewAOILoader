
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
using System.Drawing;
using DealRobot;
using DealPLC;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using DealComprehensive;
using SetPar;
using SetComprehensive;
using DealFile;
using BasicClass;
using DealConfigFile;

namespace Main
{
    partial class MainWindow
    {
        #region  定义
        //



        //bool 
        #endregion  定义

        /// <summary>
        /// 长时间监控,30s监控一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TM30000_LongMonitor_Tick(object sender, EventArgs e)
        {
            try
            {
                base.TM30000_LongMonitor_Tick(null, null);


            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 短时间监控,200ms循环一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TM200_ShortMonitor_Tick(object sender, EventArgs e)
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
