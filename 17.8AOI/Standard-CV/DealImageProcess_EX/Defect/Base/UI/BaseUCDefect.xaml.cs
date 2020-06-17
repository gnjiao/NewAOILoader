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
using MahApps.Metro.Controls;
using Common;
using DealFile;
using BasicClass;
using System.IO;
using Camera;
using BasicComprehensive;
using System.Collections;
using HalconDotNet;
using System.Threading;
using System.Threading.Tasks;
using DealResult;
using DealImageProcess;

namespace DealImageProcess_EX
{
    /// <summary>
    /// UCCopperxaml.xaml 的交互逻辑
    /// </summary>
    public partial class BaseUCDefect : BaseUCImageProcess
    {
        #region 定义
        BaseParDefect g_BaseParDefect = null;

        #endregion 定义

        #region 初始化
        public BaseUCDefect()
        {
            InitializeComponent();
        }

        public void Init(BaseParDefect baseParDefect)
        {
            g_BaseParDefect = baseParDefect;
            ShowPar_Invoke();
        }
        #endregion 初始化

        #region 参数设置
        //最小灰度
        private void dudMinGray_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMinGray.IsMouseOver)
                {
                    g_BaseParDefect.MinGray = Math.Round((double)dudMinGray.Value, 0);
                    dudMinGray.Value = g_BaseParDefect.MinGray;
                }
            }
            catch (Exception ex)
            {
                 Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMaxGray_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMaxGray.IsMouseOver)
                {
                    g_BaseParDefect.MaxGray = Math.Round((double)dudMaxGray.Value, 0);
                    dudMaxGray.Value = g_BaseParDefect.MaxGray;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }
       //开运算闭运算
        private void dudOpenRadius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudOpenRadius.IsMouseOver)
                {
                    g_BaseParDefect.OpenRadius = Math.Round((double)dudOpenRadius.Value, 0);
                    dudOpenRadius.Value = g_BaseParDefect.OpenRadius;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudCloseRadius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudCloseRadius.IsMouseOver)
                {
                    g_BaseParDefect.CloseRadius = Math.Round((double)dudCloseRadius.Value, 0);
                    dudCloseRadius.Value = g_BaseParDefect.CloseRadius;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }
        //面积
        private void dudMinArea_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMinArea.IsMouseOver)
                {
                    g_BaseParDefect.MinArea = Math.Round((double)dudMinArea.Value, 0);
                    dudMinArea.Value = g_BaseParDefect.MinArea;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMaxArea_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMaxArea.IsMouseOver)
                {
                    g_BaseParDefect.MaxArea = Math.Round((double)dudMaxArea.Value, 0);
                    dudMaxArea.Value = g_BaseParDefect.MaxArea;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMinCircularity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMinCircularity.IsMouseOver)
                {
                    g_BaseParDefect.DblMinCircularity = Math.Round((double)dudMinCircularity.Value, 2);
                    dudMinCircularity.Value = g_BaseParDefect.DblMinCircularity;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMaxCircularity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMaxCircularity.IsMouseOver)
                {
                    g_BaseParDefect.DblMaxCircularity = Math.Round((double)dudMaxCircularity.Value, 2);
                    dudMaxCircularity.Value = g_BaseParDefect.DblMaxCircularity;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMinRectangularity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMinRectangularity.IsMouseOver)
                {
                    g_BaseParDefect.DblMinRectangularity = Math.Round((double)dudMinRectangularity.Value, 2);
                    dudMinRectangularity.Value = g_BaseParDefect.DblMinRectangularity;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMaxRectangularity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMaxRectangularity.IsMouseOver)
                {
                    g_BaseParDefect.DblMaxRectangularity = Math.Round((double)dudMaxRectangularity.Value, 2);
                    dudMaxRectangularity.Value = g_BaseParDefect.DblMaxRectangularity;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMinWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMinWidth.IsMouseOver)
                {
                    g_BaseParDefect.DblMinWidth = Math.Round((double)dudMinWidth.Value, 2);
                    dudMinWidth.Value = g_BaseParDefect.DblMinWidth;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMaxWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMaxWidth.IsMouseOver)
                {
                    g_BaseParDefect.DblMaxWidth = Math.Round((double)dudMaxWidth.Value, 2);
                    dudMaxWidth.Value = g_BaseParDefect.DblMaxWidth;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMinHeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMinHeight.IsMouseOver)
                {
                    g_BaseParDefect.DblMinHeight = Math.Round((double)dudMinHeight.Value, 2);
                    dudMinHeight.Value = g_BaseParDefect.DblMinHeight;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMaxHeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMaxHeight.IsMouseOver)
                {
                    g_BaseParDefect.DblMaxHeight = Math.Round((double)dudMaxHeight.Value, 2);
                    dudMaxHeight.Value = g_BaseParDefect.DblMaxHeight;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMinX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMinX.IsMouseOver)
                {
                    g_BaseParDefect.DblMinX = Math.Round((double)dudMinX.Value, 2);
                    dudMinX.Value = g_BaseParDefect.DblMinX;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMaxX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMaxX.IsMouseOver)
                {
                    g_BaseParDefect.DblMaxX = Math.Round((double)dudMaxX.Value, 2);
                    dudMaxX.Value = g_BaseParDefect.DblMaxX;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMinY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMinY.IsMouseOver)
                {
                    g_BaseParDefect.DblMinY = Math.Round((double)dudMinY.Value, 2);
                    dudMinY.Value = g_BaseParDefect.DblMinY;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        private void dudMaxY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMaxY.IsMouseOver)
                {
                    g_BaseParDefect.DblMaxY = Math.Round((double)dudMaxY.Value, 2);
                    dudMaxY.Value = g_BaseParDefect.DblMaxY;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }

        #endregion 参数设置

        #region 显示       
        public override void ShowPar()
        {
            try
            {
                dudMinGray.Value = g_BaseParDefect.MinGray;
                dudMaxGray.Value = g_BaseParDefect.MaxGray;
                dudOpenRadius.Value = g_BaseParDefect.OpenRadius;
                dudCloseRadius.Value = g_BaseParDefect.CloseRadius;
                dudMinArea.Value = g_BaseParDefect.MinArea;
                dudMaxArea.Value = g_BaseParDefect.MaxArea;
                dudMaxCircularity.Value = g_BaseParDefect.DblMaxCircularity;
                dudMaxCircularity.Value = g_BaseParDefect.DblMaxCircularity;
                dudMinRectangularity.Value = g_BaseParDefect.DblMinRectangularity;
                dudMaxRectangularity.Value = g_BaseParDefect.DblMaxRectangularity;
                dudMinWidth.Value = g_BaseParDefect.DblMinWidth;
                dudMaxWidth.Value = g_BaseParDefect.DblMaxWidth;
                dudMinHeight.Value = g_BaseParDefect.DblMinHeight;
                dudMaxHeight.Value = g_BaseParDefect.DblMaxHeight;
                dudMinX.Value = g_BaseParDefect.DblMinX;
                dudMaxX.Value = g_BaseParDefect.DblMaxX;
                dudMinY.Value = g_BaseParDefect.DblMinY;
                dudMaxY.Value = g_BaseParDefect.DblMaxY;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinBlob", ex);
            }
        }
        #endregion 显示

    }
}
