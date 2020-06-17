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
using DealFile;
using DealImageProcess;
using BasicClass;
using BasicComprehensive;

namespace DealImageProcess_EX
{
    /// <summary>
    /// UCSmoothing.xaml 的交互逻辑
    /// </summary>
    public partial class UCSmoothing : BaseUCImageProcess
    {
        #region 定义
        ParSmooth g_Smoothing = null;
        List<CellReference> g_cellExecute_L = new List<CellReference>();
        List<string> g_CellStdEdge_L = new List<string>();
        List<string> g_CellMCrossLine_L = new List<string>();
        List<string> g_CellMStraightLine_L = new List<string>();
        #endregion 定义

        #region 初始化
        public UCSmoothing()
        {
            InitializeComponent();
            //显示控件权限设置按钮
            //Init_ImEnbale(imEnable);

            NameClass = "UCSmoothing";
        }

        public void Init(ParSmooth par, List<CellReference> cellExecute_L, List<CellHObjectReference> cellHObject_L)
        {
            g_Smoothing = par;
            g_AuthorityCtr_L = par.g_AuthorityCtr_L;
            g_cellExecute_L = cellExecute_L;
            g_CellStdEdge_L.Clear();
            for (int i = 0; i < cellExecute_L.Count; i++)
            {
                if (cellExecute_L[i].Info.Contains("形状匹配"))
                {
                    g_CellStdEdge_L.Add(cellExecute_L[i].NameCell);
                }
            }

            g_CellMCrossLine_L.Clear();
            for (int i = 0; i < cellExecute_L.Count; i++)
            {
                if (cellExecute_L[i].Info.Contains("M直线交点"))
                {
                    g_CellMCrossLine_L.Add(cellExecute_L[i].NameCell);
                }
            }

            g_CellMStraightLine_L.Clear();
            for (int i = 0; i < cellExecute_L.Count; i++)
            {
                if (cellExecute_L[i].Info.Contains("M直线"))
                {
                    g_CellMStraightLine_L.Add(cellExecute_L[i].NameCell);
                }
            }
            ShowPar_Invoke();
        }
        #endregion 初始化

        #region 参数调整       
        /// <summary>
        /// 平滑系数
        /// </summary>
        private void dudSmoothValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudSmoothValue.IsMouseOver)
                {
                    g_Smoothing.SmoothValue = (int)dudSmoothValue.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 迭代次数
        /// </summary>
        private void dudNum_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudNum.IsMouseOver)
                {
                    g_Smoothing.Num = (int)dudNum.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        
        /// <summary>
        /// 缺陷剔除阈值
        /// </summary>
        private void dudSelectAreaLow_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudSelectAreaLow.IsMouseOver)
                {
                    g_Smoothing.SelectAreaLow = (int)dudSelectAreaLow.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 轮廓平移值
        /// </summary>
        private void dudGapIn_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudGapIn.IsMouseOver)
                {
                    g_Smoothing.GapIn = (int)dudGapIn.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudGapOut_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudGapOut.IsMouseOver)
                {
                    g_Smoothing.GapOut = (int)dudGapOut.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 缺陷面积筛选
        /// </summary>
        private void dudErrorAreaValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudErrorAreaValue.IsMouseOver)
                {
                    g_Smoothing.ErrorAreaValue = (int)dudErrorAreaValue.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 缺陷膨胀值
        /// </summary>
        private void dudDilationDefectValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudDilationDefectValue.IsMouseOver)
                {
                    g_Smoothing.DilationDefectValue = (int)dudDilationDefectValue.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 参数调整


        #region 显示
        public override void ShowPar()
        {
            try
            {
                dudNum.Value = g_Smoothing.Num;
                dudSmoothValue.Value = g_Smoothing.SmoothValue;
                dudGapIn.Value = g_Smoothing.GapIn;
                dudGapOut.Value = g_Smoothing.GapOut;
                dudErrorAreaValue.Value = g_Smoothing.ErrorAreaValue;
                dudDilationDefectValue.Value = g_Smoothing.DilationDefectValue;
                dudSelectAreaLow.Value = g_Smoothing.SelectAreaLow;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示

    }
}
