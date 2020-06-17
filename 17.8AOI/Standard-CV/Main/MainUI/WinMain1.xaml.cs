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
using DealMath;

namespace Main
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WinMain1 : MainWindow
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string a=Microsoft.VisualBasic.Strings.StrConv("参数", Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);

            g_BaseUCDisplaySum.TriggerOnOff(false);
            //1234
            //4567

            //重启相机
            //g_BaseUCDisplaySum.g_BaseUCDisplayCameras[2].MiRestartCamera_Click(null,null);

            //if(((UCDisplaySum8)g_BaseUCDisplaySum).BlRealImage)
            //{
            //    ((UCDisplaySum8)g_BaseUCDisplaySum).StopRealImage();
            //}
            //((UCDisplaySum8)g_BaseUCDisplaySum).SetDisplay1256();
            //new Task(new Action(()=>
            //{
            //    Thread.Sleep(500);
            //    //初始化抓图
            //    Init_Camera();
            //})).Start();

            // CreateUIDisplay1234(gdCamera);
            //g_BaseUCDisplaySum.InitRealGrabImage();
            //ParSetDisplay.P_I[0].TransDisplayImage_e = TransDisplayImage_enum.MirrorX;
            //ParCamera1.P_I.Serial_Camera = "相机1";
            //ParCamera2.P_I.Serial_Camera = "相机2";
            //ParCamera3.P_I.Serial_Camera = "相机3";
            //ParCamera4.P_I.Serial_Camera = "相机4";
        }

        private void tb_Checked(object sender, RoutedEventArgs e)
        {
            //WinLogImage.GetWinInst().Show();
            WinMsgBox.ShowMsgBox("123", false);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ParComprehensive8.P_I.ReadXmlXDoc();//综合处理参数               
                                                //double[] a = new double[] {1,2,3,4 ,5,6};
                                                //double[] b = new double[] { 1, 2, 3, 4 ,5,5};
                                                //double[,] t= DealMath.Matrix_F.MultMat(a,new int[2]{ 3,2},b,new int[] { 2,3});
                                                //FunBackup.F_I.DeleteBUParcomprehsive(6);
                                                //new Task(new Action(()=>
                                                //{
                                                //    Thread.Sleep(4000);

            //    WinMsgBox.ShowMsgBox("123", false);
            //})).Start();

            //WinMsgBox.ShowMsgBox("123", false);

            //bool blNew = false;
            //WinStateAndAlarm win = WinStateAndAlarm.GetWinInst(out blNew);
            //win.Show();

            //WinComprehensiveFull.GetWinInst().Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            g_BaseUCDisplaySum.TriggerOnOff(true);
        }

        private void cimRobotPoints_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!EngineerReturn())
                {
                    return;
                }

                if (ParSetDisplay.P_I.TypeScreen_e != TypeScreen_enum.S800)
                {
                    WinSetRobotPoints winSetAdjustWork = WinSetRobotPoints.GetWinInst();
                    winSetAdjustWork.Show();
                }
                else
                {
                    WinSetRobotPoints winSetAdjustWork = WinSetRobotPoints.GetWinInst();
                    winSetAdjustWork.Show();
                }

                //按钮日志
                FunLogButton.P_I.AddInfo("cimAdjust" + g_CimAdjust.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void cimRobotAdj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //权限返回
                if (!NullReturn())
                {
                    return;
                }

                if (ParSetDisplay.P_I.TypeScreen_e != TypeScreen_enum.S800)
                {
                    WinSetRobotAdj winSetAdjustWork = WinSetRobotAdj.GetWinInst();
                    winSetAdjustWork.Show();
                }
                else
                {
                    WinSetRobotAdj winSetAdjustWork = WinSetRobotAdj.GetWinInst();
                    winSetAdjustWork.Show();
                }

                //按钮日志
                FunLogButton.P_I.AddInfo("cimAdjust" + g_CimAdjust.Header.ToString(), "Main窗体");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
    }
}
