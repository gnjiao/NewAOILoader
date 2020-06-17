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
using DealConfigFile;
using BasicClass;
using DealComInterface;
using ParComprehensive;

namespace Main
{
    public partial class MainWindow
    {
        #region 定义
        bool g_BlClearAuto = false;//是否已经清除
        #endregion 定义

        #region 初始化
        /// <summary>
        /// 初始化参数
        /// </summary>
        public override void Init_Custom()
        {
            try
            {
                BaseDealComprehensiveResult_Main.LoadCstData();
                StationDataManager.StationDataMngr.read_station_data();
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// 换型时需要处理的
        /// </summary>
        public override void InitNewModel_Custom()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion 初始化

        #region 显示
        /// <summary>
        /// 自定义显示
        /// </summary>
        public override void ShowCustom()
        {

        }
        #endregion 显示

        #region 关闭
        /// <summary>
        /// 关闭
        /// </summary>
        public override void Close_Custom()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion 关闭
    }
}
