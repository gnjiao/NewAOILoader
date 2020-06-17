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
using DealConfigFile;
using BasicClass;
using DealComInterface;
using ParComprehensive;
using DealDisplay;
using BasicDisplay;
using DealTool;
using DealLog;
using DealHelp;
using System.Reflection;
using BasicComprehensive;
using DealAlgorithm;
using DealCalibrate;
using DealCommunication;
using DealDraw;
using DealGeometry;
using DealGrabImage;
using DealImageProcess;
using DealInOutput;
using DealMath;
using DealResult;
using DealWorkFlow;
using DealFile;
using Main_EX;
using NLog;

namespace Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WinMainSmall2 : MainWindow
    {
        #region 定义


        #endregion 定义        

        #region 其他
        /// <summary>
        /// CIM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmiCim_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 其他

        #region CIM模拟

        #endregion CIM模拟

        #region 操作设置
        private void epSetWork_Collapsed(object sender, RoutedEventArgs e)
        {

        }

        private void epSetWork_Expanded(object sender, RoutedEventArgs e)
        {

        }



        #endregion 操作设置        

        private void dpData_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (popData.IsOpen)
            {
                popData.IsOpen = false;
            }
            else
            {
                popData.IsOpen = true;
            }
        }
    }
}
