using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// WndStationCalib.xaml 的交互逻辑
    /// </summary>
    public partial class WndStationCalib : Window
    {
        static WndStationCalib _instance = null;
        static object _locker = new object();

        public static WndStationCalib GetInstance()
        {
            if(_instance==null)
            {
                lock(_locker)
                {
                    if (_instance == null)
                        _instance = new WndStationCalib();
                }
            }
            return _instance;
        }

        private WndStationCalib()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
