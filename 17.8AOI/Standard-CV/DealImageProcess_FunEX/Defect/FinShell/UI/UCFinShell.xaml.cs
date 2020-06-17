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
    /// UCFinShell.xaml 的交互逻辑
    /// </summary>
    public partial class UCFinShell : BaseUCImageProcess
    {
        #region 定义
        ParFinShell g_ParFinShell = null;
        List<CellReference> g_cellExecute_L = new List<CellReference>();
        #endregion 定义
        #region 初始化
        public UCFinShell()
        {
            InitializeComponent();

            //显示控件权限设置按钮
            //Init_ImEnbale(imEnable);

            NameClass = "UCFinShell";
        }
        public void Init(ParFinShell par, List<CellReference> cellExecute_L, List<CellHObjectReference> cellHObject_L)
        {
            g_ParFinShell = par;
            g_AuthorityCtr_L = par.g_AuthorityCtr_L;
            g_cellExecute_L = cellExecute_L;
            //是否记录
            uCSetRecord.Init(par, cellHObject_L);
            ShowPar_Invoke();
        }
        #endregion 初始化

        #region 调整参数

        private void cboActualEdge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (cboActualEdge.IsMouseOver)
                {
                    //g_ParFinShell.NameCellActualEdge = cboActualEdge.SelectedValue.ToString();
                    g_ParFinShell.NameCellActualEdge = "C8";
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        private void dudWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudWidth.IsMouseOver)
                {
                    dudWidth.Value = Math.Round((double)dudWidth.Value, 0);
                    g_ParFinShell.Width_Paral = (int)dudWidth.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudOuterStdShift_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudOuterStdShift.IsMouseOver)
                {
                    dudOuterStdShift.Value = Math.Round((double)dudOuterStdShift.Value, 1);
                    g_ParFinShell.OuterStdShift_Paral = (double)dudOuterStdShift.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudInnerStdShift_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudInnerStdShift.IsMouseOver)
                {
                    dudInnerStdShift.Value = Math.Round((double)dudInnerStdShift.Value, 1);
                    g_ParFinShell.InnerStdShift_Paral = (double)dudInnerStdShift.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void cboWorkingRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (cboWorkingRegion.IsMouseOver)
                {
                    //g_ParFinShell.WorkingRegion = cboWorkingRegion.SelectedValue.ToString();

                    switch (cboWorkingRegion.SelectedIndex)
                    {
                        case 0:
                            g_ParFinShell.WorkingRegion = "Outer";
                            break;
                        case 1:
                            g_ParFinShell.WorkingRegion = "Inner";
                            break;
                        default:
                            g_ParFinShell.WorkingRegion = "Outer";
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudClosingCircle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudClosingCircle.IsMouseOver)
                {
                    dudClosingCircle.Value = Math.Round((double)dudClosingCircle.Value, 0);
                    g_ParFinShell.ClosingCircle = (int)dudClosingCircle.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudOpeningCircle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudOpeningCircle.IsMouseOver)
                {
                    dudOpeningCircle.Value = Math.Round((double)dudOpeningCircle.Value, 0);
                    g_ParFinShell.OpeningCircle = (int)dudOpeningCircle.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudMinWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMinWidth.IsMouseOver)
                {
                    dudMinWidth.Value = Math.Round((double)dudMinWidth.Value, 0);
                    g_ParFinShell.MinWidth = (int)dudMinWidth.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudMaxWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMaxWidth.IsMouseOver)
                {
                    dudMaxWidth.Value = Math.Round((double)dudMaxWidth.Value, 0);
                    g_ParFinShell.MaxWidth = (int)dudMaxWidth.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudMinHeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMinHeight.IsMouseOver)
                {
                    dudMinHeight.Value = Math.Round((double)dudMinHeight.Value, 0);
                    g_ParFinShell.MinHeight = (int)dudMinHeight.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudMaxHeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMaxHeight.IsMouseOver)
                {
                    dudMaxHeight.Value = Math.Round((double)dudMaxHeight.Value, 0);
                    g_ParFinShell.MaxHeight = (int)dudMaxHeight.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudMinArea_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMinArea.IsMouseOver)
                {
                    dudMinArea.Value = Math.Round((double)dudMinArea.Value, 0);
                    g_ParFinShell.MinArea = (int)dudMinArea.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudMaxArea_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudMaxArea.IsMouseOver)
                {
                    dudMaxArea.Value = Math.Round((double)dudMaxArea.Value, 0);
                    g_ParFinShell.MaxArea = (int)dudMaxArea.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void cboSmallestRect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (cboSmallestRect.IsMouseOver)
                {
                    g_ParFinShell.SmallestSurround_e = (SmallestSurround_enum)cboSmallestRect.SelectedIndex;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void cboTypeOutCoord_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (cboTypeOutCoord.IsMouseOver)
                {
                    string[] str = (cboTypeOutCoord.SelectedValue.ToString()).Split(':');
                    g_ParFinShell.TypeOutCoord = str[1].Trim();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        #endregion 调整参数


        #region 显示
        public override void ShowPar()
        {
            try
            {
                ShowCellActualEdge();
                //设置控件权限
                //Init_EnbaleAuthority(gdRoot);
                //cboActualEdge.Text = g_ParFinShell.NameCellActualEdge;
                cboActualEdge.Text = "C8";
                dudWidth.Value = g_ParFinShell.Width_Paral;
                dudOuterStdShift.Value = g_ParFinShell.OuterStdShift_Paral;
                dudInnerStdShift.Value = g_ParFinShell.InnerStdShift_Paral;

                switch (g_ParFinShell.WorkingRegion)
                {
                    case "Outer":
                        cboWorkingRegion.SelectedIndex = 0;
                        break;
                    case "Inner":
                        cboWorkingRegion.SelectedIndex = 1;
                        break;
                    default:
                        cboWorkingRegion.SelectedIndex = 0;
                        break;
                }

                dudClosingCircle.Value = g_ParFinShell.ClosingCircle;
                dudOpeningCircle.Value = g_ParFinShell.OpeningCircle;
                dudMinWidth.Value = g_ParFinShell.MinWidth;
                dudMaxWidth.Value = g_ParFinShell.MaxWidth;
                dudMinHeight.Value = g_ParFinShell.MinHeight;
                dudMaxHeight.Value = g_ParFinShell.MaxHeight;
                dudMinArea.Value = g_ParFinShell.MinArea;
                dudMaxArea.Value = g_ParFinShell.MaxArea;

                cboSmallestRect.SelectedIndex = (int)g_ParFinShell.SmallestSurround_e;

                cboTypeOutCoord.Text = g_ParFinShell.TypeOutCoord;//输出坐标类型                
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 显示实际轮廓单元
        /// </summary>
        void ShowCellActualEdge()
        {
            try
            {
                //Cell
                //cboActualEdge.ItemsSource = g_cellExecute_L;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        #endregion 显示
        





        

    }
}
