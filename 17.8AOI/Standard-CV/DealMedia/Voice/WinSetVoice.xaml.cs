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
using DealMedia;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using ControlLib;
using BasicClass;

namespace DealMedia
{
    /// <summary>
    /// VoicePlayer.xaml 的交互逻辑
    /// </summary>
    public partial class WinSetVoice : BaseMetroWindow
    {
        #region 窗体单实例
        private static WinSetVoice g_WinSetVoice = null;
        public static WinSetVoice GetWinInst()
        {
            try
            {
                if (g_WinSetVoice == null)
                {
                    g_WinSetVoice = new WinSetVoice();
                }
                return g_WinSetVoice;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinSetVoice", ex);
                return null;
            }
        }      
        #endregion 窗体单实例

        #region 定义
        bool isFocus = false;
        string g_PathFile = "";
        List<MusicLog> Log_L = new List<MusicLog>();
        #endregion 定义

        #region 初始化
        public WinSetVoice()
        {
            InitializeComponent();           
        }

        private void BaseMetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ParVoice.P_I.ReadParIni();
                ShowPar_Invoke();
            }
            catch (Exception ex)
            {
                
            }
        }    
        
        #endregion 初始化

        

        #region 播放控制
        //开始播放，
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                base.IndexP = this.dgVoice.SelectedIndex;
                UCVoicePlay.U_I.StartPlay_Invoke(ParVoice.P_I.BasicParVoice_L[base.IndexP].NameVoice);
            }
            catch
            {

            }
        }
      
        //停止播放
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            UCVoicePlay.U_I.Stop();
        }
        #endregion 播放控制

        #region 循环次数
        private void dudNumPlay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                DoubleUpDown dud = (DoubleUpDown)sender;
                if (dud.IsMouseOver)
                {
                    base.IndexP = dgVoice.SelectedIndex;
                    ParVoice.P_I.BasicParVoice_L[base.IndexP].NumPlay = (int)dud.Value;
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion 循环次数

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParVoice.P_I.WriteParIni();
            }
            catch (Exception ex)
            {
                
            }
        }

        #region 显示
        public override void ShowPar()
        {
            try
            {
                dgVoice.ItemsSource = ParVoice.P_I.BasicParVoice_L;
                dgVoice.Items.Refresh();
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion 显示

        private void BaseMetroWindow_Closing(object sender, CancelEventArgs e)
        {
            g_WinSetVoice = null;
        }

        
    }

    public class MusicLog : INotifyPropertyChanged
    {
        public string log = "";
        public string Log
        {
            get
            {
                return this.log;
            }
            set
            {
                if (this.log != value)
                {
                    this.log = value;
                    OnPropertyChanged("Log");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }        
        }
    }
}
