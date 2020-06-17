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
using BasicComprehensive;
using DealImageProcess;
using BasicClass;
using System.Collections;
using DealConfigFile;

namespace DealImageProcess_EX
{
    /// <summary>
    /// UCLinesDotsPosNegInspect.xaml 的交互逻辑
    /// </summary>
    public partial class UCLinesDotsPosNegInspect : BaseUCImageProcess
    {

        #region 定义
        ParLinesDotsPosNegInspect g_ParLinesDotsDPNIns = null;
        List<CellReference> g_cellExecute_L = new List<CellReference>();

        Hashtable HtCellRef_Mult = null;//算法单元名称
        Hashtable HtResult_Mult = null;//结果哈希表

        //List
        List<string> CellInfo_L = new List<string>();
        #endregion 定义

        #region 初始化
        public UCLinesDotsPosNegInspect()
        {
            InitializeComponent();
        }

        public void Init(ParLinesDotsPosNegInspect par, List<CellReference> cellExecute_L, List<CellHObjectReference> cellHObject_L, Hashtable htCellRef_Mult, Hashtable htResult_MultC)
        {
            try
            {
                g_ParLinesDotsDPNIns = par;
                g_AuthorityCtr_L = par.g_AuthorityCtr_L;
                g_cellExecute_L = cellExecute_L;

                HtCellRef_Mult = htCellRef_Mult;
                HtResult_Mult = htResult_MultC;

                uCUseCalib.Init(par, cellExecute_L);//使用校准
                uCSetRecord.Init(par, cellHObject_L);//设置记录
                ShowPar_Invoke();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        #region 参数调整

        private void dudDotsTh_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudDotsTh.IsMouseOver)
                {
                    dudDotsTh.Value = (int)dudDotsTh.Value;
                    g_ParLinesDotsDPNIns.DotsTh = (int)dudDotsTh.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudLinesTh_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudLinesTh.IsMouseOver)
                {
                    dudLinesTh.Value = (int)dudLinesTh.Value;
                    g_ParLinesDotsDPNIns.LinesTh = (int)dudLinesTh.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudClosingRadius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudClosingRadius.IsMouseOver)
                {
                    dudClosingRadius.Value = (int)dudClosingRadius.Value;
                    g_ParLinesDotsDPNIns.ClosingRadius = (int)dudClosingRadius.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudOpeningRadius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudOpeningRadius.IsMouseOver)
                {
                    dudOpeningRadius.Value = (int)dudOpeningRadius.Value;
                    g_ParLinesDotsDPNIns.OpeningRadius = (int)dudOpeningRadius.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudSizeTh_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudSizeTh.IsMouseOver)
                {
                    dudSizeTh.Value = (int)dudSizeTh.Value;
                    g_ParLinesDotsDPNIns.SizeTh = (int)dudSizeTh.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudDotsLinesSeperateTh_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudDotsLinesSeperateTh.IsMouseOver)
                {
                    dudDotsLinesSeperateTh.Value = Math.Round((double)dudDotsLinesSeperateTh.Value, 1);
                    g_ParLinesDotsDPNIns.DotsLinesSeperateTh = (double)dudDotsLinesSeperateTh.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudUpExtend_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudUpExtend.IsMouseOver)
                {
                    dudUpExtend.Value = Math.Round((double)dudUpExtend.Value, 1);
                    g_ParLinesDotsDPNIns.UpExtend = (double)dudUpExtend.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudLeftExtend_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudLeftExtend.IsMouseOver)
                {
                    dudLeftExtend.Value = Math.Round((double)dudLeftExtend.Value, 1);
                    g_ParLinesDotsDPNIns.LeftExtend = (double)dudLeftExtend.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudDownExtend_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudDownExtend.IsMouseOver)
                {
                    dudDownExtend.Value = Math.Round((double)dudDownExtend.Value, 1);
                    g_ParLinesDotsDPNIns.DownExtend = (double)dudDownExtend.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void dudRightExtend_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (dudRightExtend.IsMouseOver)
                {
                    dudRightExtend.Value = Math.Round((double)dudRightExtend.Value, 1);
                    g_ParLinesDotsDPNIns.RightExtend = (double)dudRightExtend.Value;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 相机序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboChangeCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboChangeCamera.IsMouseOver)
                {
                    g_ParLinesDotsDPNIns.NoCameraMult = cboChangeCamera.SelectedIndex + 1;

                    //根据切换的相机序号，选择显示单元格
                    GetCellByNoCamera(g_ParLinesDotsDPNIns.NoCameraMult);
                    ShowInfo_Mult();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void cboCellData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if (cboCellData.IsMouseOver)
                {
                    g_ParLinesDotsDPNIns.CellRefImage_Mult = g_CellData_L[cboCellData.SelectedIndex];
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
                GetCellByNoCamera(g_ParLinesDotsDPNIns.NoCameraMult);

                ShowInfo_Mult();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 切换相机序号
        /// </summary>
        /// <param name="i"></param>
        public void GetCellByNoCamera(int noCamera)
        {
            try
            {                
                string key = "CellExe_Cam" + noCamera.ToString();

                List<CellReference> cell_L = (List<CellReference>)HtCellRef_Mult[key];
                g_CellData_L.Clear();
                CellInfo_L.Clear();
                for (int i = 0; i < cell_L.Count; i++)
                {
                    if (cell_L[i].TypeResultCell_e == TypeResultCell_enum.ImageResult
                        || cell_L[i].TypeResultCell_e == TypeResultCell_enum.ImagePre)
                    {
                        g_CellData_L.Add(cell_L[i]);
                        CellInfo_L.Add(cell_L[i].Info);
                    }
                }             
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        void ShowInfo_Mult()
        {
            try
            {
                //显示
                cboCellData.ItemsSource = CellInfo_L;
                cboCellData.Items.Refresh();

                if (g_ParLinesDotsDPNIns.CellRefImage_Mult.Info.Contains("C"))
                {
                    cboCellData.Text = g_ParLinesDotsDPNIns.CellRefImage_Mult.Info;
                }
                else
                {
                    cboCellData.SelectedIndex = 0;
                    g_ParLinesDotsDPNIns.CellRefImage_Mult = g_CellData_L[0];
                }

                //相机序号
                cboChangeCamera.ItemsSource = Camera_L;
                cboChangeCamera.Items.Refresh();
                cboChangeCamera.SelectedIndex = g_ParLinesDotsDPNIns.NoCameraMult - 1;

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示

      
    }
}
