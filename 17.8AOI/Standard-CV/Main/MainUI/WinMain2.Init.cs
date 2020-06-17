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
    partial class WinMain2
    {
        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public WinMain2()
        {
            NameClass = "WinMain2";

            InitializeComponent();

            //初始化Main_EX
            Init_Main_EX();

            //是否最大化窗体 
            MaxWinMain();

            //事件注册
            Event_Init();

            //调试时取消窗体最前
            TopFalse();

            //小窗体
            ShowSmallWin();

            if (Authority.Authority_e != Authority_enum.Manufacturer)
            {
                cmiOffline.Visibility = Visibility.Hidden;
            }

            //显示软件更新时间
            this.Title = this.Title + "        更新时间(Update):" + FunAbout.F_I.TimeChange;
        }


        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowState("软件启动");

            //设定默认登录权限
            SetDefaultLogin();

            //初始化主界面控件显示
            Init_UIMainShow();

            //指示是否处于离线状态
            ChangeColorOffLine();

            //初始化尺寸
            InitWinSize();

            //初始化
            Task task = new Task(Init);
            task.Start();
        }

        /// <summary>
        /// 初始化MainWin的基类加载
        /// </summary>
        void Init_Main_EX()
        {
            try
            {
                #region 初始化Main_EX
                //控件赋值

                //Grid
                g_GdDisplay = gdDisplay;
                g_GdCamera = gdCamera;
                g_GdInfo = gdInfo;

                g_GdMenu = gdMenu;

                //相机综合设置
                g_CmiComprehensive = cmiComprehensive;

                //MenuItem
                //相机综合设置
                g_CmiCamera1 = cmiCamera1;
                g_CmiCamera2 = cmiCamera2;
                g_CmiCamera3 = cmiCamera3;
                g_CmiCamera4 = cmiCamera4;
                g_CmiCamera5 = cmiCamera5;
                g_CmiCamera6 = cmiCamera6;
                g_CmiCamera7 = cmiCamera7;
                g_CmiCamera8 = cmiCamera8;

                #region 大于八个相机
                g_CmiCamera9 = cmiCamera9;
                g_CmiCamera10 = cmiCamera10;
                g_CmiCamera11 = cmiCamera11;
                g_CmiCamera12 = cmiCamera12;
                if (ParCameraWork.NumCamera < 9)
                {
                    cmiCamera9.Height = 0;
                    cmiCamera10.Height = 0;
                    cmiCamera11.Height = 0;
                    cmiCamera12.Height = 0;
                }
                else if (ParCameraWork.NumCamera == 9)
                {
                    cmiCamera10.Height = 0;
                    cmiCamera11.Height = 0;
                    cmiCamera12.Height = 0;
                }
                else if (ParCameraWork.NumCamera == 10)
                {
                    cmiCamera11.Height = 0;
                    cmiCamera12.Height = 0;
                }
                else if (ParCameraWork.NumCamera == 11)
                {
                    cmiCamera12.Height = 0;
                }
                #endregion 大于八个相机

                g_CimCameraWork = cimCameraWork;
                g_CimDisplayImage = cimDisplayImage;
                g_CimSetCameraPar = cmiSetCameraPar;

                //配置参数
                g_CmiConfig = miConfig;
                g_CmiConfigPar = cmiConfigPar;
                g_CimAdjust = cimAdjust;
                g_CimStd = cimStd;
                g_CimTypeWork = cimTypeWork;
                g_CimManageConfigPar = cimManageConfigPar;
                //g_CimNewModel = cimNewModel;
                g_CimChangeModel = cimChangeModel;

                //通信
                g_CmiCommunication = cmiCommunication;

                g_CmiPLC = cmiPLC;
                g_CmiRobot = cmiRobot;
                g_CmiRobot2 = cmiRobot2;
                g_CmiComInterface = cmiComInterface;
                g_CmiIO = cmiIO;

                //系统设置
                g_CimSystem = cimSystem;

                g_CimSetLogin = cimSetLogin;
                g_CmiFolder = cmiFolder;
                g_CmiMemory = cmiMemory;
                g_CmiPathRoot = cmiPathRoot;
                g_CmiMonitorSpace = cmiMonitorSpace;
                g_CmiSetSys = cmiSetSys;
                g_CmiRecover = cmiRecover;
                g_CmiLog = cmiLog;//设置日志

                //手动运行
                g_CmiManual = cmiManual;
                g_CmiManualPC = cmiManualPC;
                g_CmiManualPLC = cmiManualPLC;
                g_CmiManualComInterface = cmiManualComInterface;
                g_CmiManualRobot = cmiManualRobot;
                g_CmiManualRobot2 = cmiManualRobot2;

                g_CmiPLCValue = cmiPLCValue;
                g_CmiPLCLog = cmiPLCLog;
                g_CmiPLCReadWrite = cmiPLCReadWrite;

                //Others
                g_CmiOther = cmiOther;

                //离线模式
                g_CmiOffline = cmiOffline;
                g_CmiPLCOffline = cmiPLCOffline;
                g_CmiRobotOffline = cmiRobotOffline;
                g_CmiComPortOffline = cmiComPortOffline;
                g_CmiCameraOffline = cmiCameraOffline;

                //登陆
                g_ImLogin = imLogin;
                g_LbLogin = lbLogin;
                g_PPLogin = ppLogin;//登录弹框
                g_UCLogin = uCLogin;//登录控件

                g_CmiMaxWin = cmiMaxWin;

                g_UCStateWork = uCStateWork;//显示状态信息
                g_UCAlarm = uCAlarm;//显示报警信息
                g_UCParProduct = uCParProduct;//显示产品参数
                g_LblStateRun = lblStateRun;//软件状态显示

                g_LbStateMachine = lbStateMachine;//显示设备状态
                g_PPState = ppStateSoft;
                g_UCStateSoft = uCStateSoft;

                g_UCDisplayMainResult = uCResult;//结果控件                
                #endregion 初始化Main_EX

                #region 边栏
                g_TxtState = txtState;
                g_TxtAlarm = txtAlarm;
                g_TxtMannual = txtMannual;
                g_TxtData = txtData;
                #endregion 边栏
                //更改语言设置
                InitLanguage();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 边栏
        private void dpState_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ppState.IsOpen)
            {
                ppState.IsOpen = false;
            }
            else
            {
                ppState.IsOpen = true;
            }
        }


        private void dpAlarm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ppAlarm.IsOpen)
            {
                ppAlarm.IsOpen = false;
            }
            else
            {
                ppAlarm.IsOpen = true;
            }
        }


        private void dpManual_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (popManual.IsOpen)
            {
                popManual.IsOpen = false;
            }
            else
            {
                popManual.IsOpen = true;
            }
        }

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

        private void dpCom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (popCom.IsOpen)
            {
                popCom.IsOpen = false;
            }
            else
            {
                popCom.IsOpen = true;
            }
        }
        #endregion 边栏
    }
}
