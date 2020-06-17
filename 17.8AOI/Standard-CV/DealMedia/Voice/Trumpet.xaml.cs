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
using System.Windows.Forms;

namespace DealMedia
{
    /// <summary>
    /// Trumpet.xaml 的交互逻辑
    /// </summary>
    public partial class Trumpet : Window
    {
        #region 定义
        //定时器
        Timer timer = new Timer();

        int time = 0;
        #endregion 定义

        #region 初始化
        public Trumpet(string PathVoice)
        {
            try
            {
                InitializeComponent();

                timer.Interval = 500;
                timer.Enabled = true;
                timer.Tick += new EventHandler(timer_Tick);
                //VarVoice.V_I.PathVoice = PathVoice;
            }
            catch
            {

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {                
                timer.Start();
                //Voice.Voice_Inst.Start_Invoke();                
            }
            catch
            {

            }
        }
        #endregion 初始化

        //定时关闭
        void timer_Tick(object sender, EventArgs e)
        {
            time++;
            //if (time == (int)(4 * RegeditPar.R_I.intNumCycVoice))
            //{
            //    Action inst = new Action(CloseAll);
            //    this.Dispatcher.Invoke(inst);
            //}
        }
        void CloseAll()
        {
            timer.Stop();
            timer.Enabled = false;
            this.Close();
        }
    }
}
