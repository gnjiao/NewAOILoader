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
using System.Windows.Shapes;
using Camera;
using System.Threading;
using System.Threading.Tasks;
using Common;
using DealRobot;
using DealFile;
using DealComprehensive;
using SetPar;
using System.IO;
using ParComprehensive;
using DealConfigFile;
using DealPLC;
using DealCalibrate;
using DealComInterface;
using BasicClass;
using DealDisplay;
using BasicDisplay;
using System.Diagnostics;
using DealImageProcess;
using DealHelp;
using DealLog;
using Main_EX;

namespace Main
{
    /// <summary>
    /// StartUpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StartUpWindow : BaseStartWin
    {        
        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public StartUpWindow()
        {
            InitializeComponent();

            //调试时隐藏启动界面图片
            HideImage();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetPriority();//设置程序优先级位高

            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            base.Init();            
        }
        #endregion 初始化

        #region 打开相机
        /// <summary>
        /// 打开相机
        /// </summary>
        public override void OpenCamera()
        {
            try
            {
                ParCamera1.P_I.ReadIni();//相机参数
                Camera1.C_I.Init(ParCamera1.P_I);
                if (Camera1.C_I.OpenCamera())
                {
                    ParCamera1.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 2)
                {
                    return;
                }
                ParCamera2.P_I.ReadIni();//相机参数
                Camera2.C_I.Init(ParCamera2.P_I);
                if (Camera2.C_I.OpenCamera())
                {
                    ParCamera2.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 3)
                {
                    return;
                }
                ParCamera3.P_I.ReadIni();//相机参数
                Camera3.C_I.Init(ParCamera3.P_I);
                if (Camera3.C_I.OpenCamera())
                {
                    ParCamera3.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 4)
                {
                    return;
                }
                ParCamera4.P_I.ReadIni();//相机参数
                Camera4.C_I.Init(ParCamera4.P_I);
                if (Camera4.C_I.OpenCamera())
                {
                    ParCamera4.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 5)
                {
                    return;
                }
                ParCamera5.P_I.ReadIni();//相机参数
                Camera5.C_I.Init(ParCamera5.P_I);
                if (Camera5.C_I.OpenCamera())
                {
                    ParCamera5.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 6)
                {
                    return;
                }
                ParCamera6.P_I.ReadIni();//相机参数
                Camera6.C_I.Init(ParCamera6.P_I);
                if (Camera6.C_I.OpenCamera())
                {
                    ParCamera6.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 7)
                {
                    return;
                }
                ParCamera7.P_I.ReadIni();//相机参数
                Camera7.C_I.Init(ParCamera7.P_I);
                if (Camera7.C_I.OpenCamera())
                {
                    ParCamera7.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                if (ParCameraWork.NumCamera < 8)
                {
                    return;
                }
                ParCamera8.P_I.ReadIni();//相机参数
                Camera8.C_I.Init(ParCamera8.P_I);
                if (Camera8.C_I.OpenCamera())
                {
                    ParCamera8.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);

                //大于8个相机
                if (ParCameraWork.NumCamera < 9)
                {
                    return;
                }
                ParCamera9.P_I.ReadIni();//相机参数
                Camera9.C_I.Init(ParCamera9.P_I);
                if (Camera9.C_I.OpenCamera())
                {
                    ParCamera9.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);


                if (ParCameraWork.NumCamera < 10)
                {
                    return;
                }
                ParCamera10.P_I.ReadIni();//相机参数
                Camera10.C_I.Init(ParCamera10.P_I);
                if (Camera10.C_I.OpenCamera())
                {
                    ParCamera10.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);


                if (ParCameraWork.NumCamera < 11)
                {
                    return;
                }
                ParCamera11.P_I.ReadIni();//相机参数
                Camera11.C_I.Init(ParCamera11.P_I);
                if (Camera11.C_I.OpenCamera())
                {
                    ParCamera11.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);


                if (ParCameraWork.NumCamera < 12)
                {
                    return;
                }
                ParCamera12.P_I.ReadIni();//相机参数
                Camera12.C_I.Init(ParCamera12.P_I);
                if (Camera12.C_I.OpenCamera())
                {
                    ParCamera12.P_I.BlOpenCamera = true;
                }
                Thread.Sleep(50);
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(new Action(() =>
                    {
                        WinError.GetWinInst().ShowError("相机加载报错:" + ex.Message);
                    }));
            }
            finally
            {
                if (RegeditCamera.R_I.BlOffLineCamera)
                {
                    ParCamera1.P_I.BlOpenCamera = true;
                    ParCamera2.P_I.BlOpenCamera = true;
                    ParCamera3.P_I.BlOpenCamera = true;
                    ParCamera4.P_I.BlOpenCamera = true;
                    ParCamera5.P_I.BlOpenCamera = true;
                    ParCamera6.P_I.BlOpenCamera = true;
                    ParCamera7.P_I.BlOpenCamera = true;
                    ParCamera8.P_I.BlOpenCamera = true;

                    ParCamera9.P_I.BlOpenCamera = true;
                    ParCamera10.P_I.BlOpenCamera = true;
                    ParCamera11.P_I.BlOpenCamera = true;
                    ParCamera12.P_I.BlOpenCamera = true;
                }
                g_BlFinishCamera = true;
                FinishInit();
            }
        }
        #endregion 打开相机        

        /// <summary>
        /// 加载完成
        /// </summary>
        public override void FinishInit()
        {            
            if (g_BlFinishCamera
                && g_BlFinishComprehensive1
                && g_BlFinishComprehensive2
                && g_BlFinishComprehensive3
                && g_BlFinishComprehensive4
                && g_BlFinishComprehensive5
                && g_BlFinishComprehensive6
                && g_BlFinishComprehensive7
                && g_BlFinishComprehensive8
                && g_BlFinishOthers)
            {
                //通知主线程自己已经启动完毕
                Program.s_mre.Set();
            }
        }

        #region 调试状态隐藏图像
        void HideImage()
        {
            try
            {
                string path = new DirectoryInfo("../").FullName;
               
                //时间
                if (path.Contains("bin")
                    && path.Contains("Standard"))
                {
                    imStart.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 调试状态隐藏图像
    }
}
