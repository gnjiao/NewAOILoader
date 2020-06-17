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
using System.IO;
using BasicClass;
using System.Threading.Tasks;
using System.Threading;

namespace DealMedia
{
    /// <summary>
    /// UCVoicePlay.xaml 的交互逻辑
    /// </summary>
    public partial class UCVoicePlay : UserControl
    {
        #region 静态类实例
        public static UCVoicePlay U_I = new UCVoicePlay();
        #endregion 静态类实例

        #region 定义
        MediaPlayer M_I = new MediaPlayer();

        int NumPlay = 0;
        public string NameVoice = "";

        Mutex mt_Play = new Mutex();
        #endregion 定义

        #region 初始化
        public UCVoicePlay()
        {
            InitializeComponent();
            M_I.MediaEnded += new EventHandler(M_I_MediaEnded);
        }

       
        #endregion 初始化

        #region 开始播放
        public void StartPlay_Task()
        {
            if (!ParVoice.P_I[NameVoice].BlExceute)
            {
                return;
            }
            this.Dispatcher.Invoke(new Action(StartPlay));
        }
        public void StartPlay_Task(object name)
        {
            if (!ParVoice.P_I[NameVoice].BlExceute)
            {
                return;
            }
            this.Dispatcher.Invoke(new StrAction(StartPlay), name.ToString());
        }
        public void StartPlay_Invoke(string name)
        {
            try
            {
                if (!ParVoice.P_I[name].BlExceute)
                {
                    return;
                }
                NameVoice = name;
                this.Dispatcher.Invoke(new StrAction(StartPlay), name);
            }
            catch (Exception  ex)
            {
                
            }            
        }

        void StartPlay(string name)
        {
            mt_Play.WaitOne();
            try
            {
                string path = ParVoice.P_I.PathRootVoice + name + ".mp3";
                //如果没有文件退出
                if (!File.Exists(path))
                {
                    return;
                }
                Uri pathVoice = new Uri(path, UriKind.RelativeOrAbsolute);
                M_I.Open(pathVoice);
                M_I.Play();
            }
            catch
            {

            }
            finally
            {
                mt_Play.ReleaseMutex();
            }
        }

        void StartPlay()
        {
            mt_Play.WaitOne();
            try
            {
                string path = ParVoice.P_I.PathRootVoice + NameVoice + ".mp3";
                //如果没有文件退出
                if (!File.Exists(path))
                {
                    return;
                }
                Uri pathVoice = new Uri(path, UriKind.RelativeOrAbsolute);
                M_I.Open(pathVoice);
                M_I.Play();
            }
            catch
            {

            }
            finally
            {
                mt_Play.ReleaseMutex();
            }
        }
        #endregion 开始播放

        #region 结束
        void M_I_MediaEnded(object sender, EventArgs e)
        {
            try
            {
                NumPlay++;
                if (NumPlay < ParVoice.P_I[NameVoice].NumPlay)
                {
                    StartPlay_Invoke(NameVoice);
                }
                else
                {
                    NumPlay = 0;
                }
            }
            catch (Exception ex)
            {

            }
        }

        //停止播放
        public void Stop()
        {
            M_I.Stop();
        }

        //关闭
        void Close()
        {
            M_I.Close();
        }
        #endregion 结束
    }
}
