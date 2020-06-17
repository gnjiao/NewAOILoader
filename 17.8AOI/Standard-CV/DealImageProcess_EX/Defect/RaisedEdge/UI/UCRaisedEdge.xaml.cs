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
using DealImageProcess;

namespace DealImageProcess_EX
{
    /// <summary>
    /// UCRaisedEdge.xaml 的交互逻辑
    /// </summary>
    public partial class UCRaisedEdge : BaseUCImageProcess
    {
        #region 定义
        ParRaisedEdge g_ParRaisedEdge = null;
        List<CellReference> g_cellExecute_L = new List<CellReference>();
        List<string> g_CellStdEdge_L = new List<string>();
        List<string> g_CellMCrossLine_L = new List<string>();

        #endregion 定义

        #region 初始化
        public UCRaisedEdge()
        {
            InitializeComponent();
            //显示控件权限设置按钮
            //Init_ImEnbale(imEnable);

            NameClass = "UCRaisedEdge";
        }

        public void Init(ParRaisedEdge par, List<CellReference> cellExecute_L, List<CellHObjectReference> cellHObject_L)
        {
            g_ParRaisedEdge = par;
            g_AuthorityCtr_L = par.g_AuthorityCtr_L;
            g_cellExecute_L = cellExecute_L;

            g_CellMCrossLine_L.Clear();
            for (int i = 0; i < cellExecute_L.Count; i++)
            {
                if (cellExecute_L[i].Info.Contains("M直线交点"))
                {
                    g_CellMCrossLine_L.Add(cellExecute_L[i].NameCell);
                }
            }
          
            ShowPar_Invoke();
        }
        #endregion 初始化

        #region 调整参数
        private void cboOutlineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (cboOutlineType.IsMouseOver)
                {
                    g_ParRaisedEdge.OutlineType = cboOutlineType.SelectedIndex;
                    //控件使能
                    if (g_ParRaisedEdge.OutlineType == 0) // C角
                    {

                    }
                    else //R角
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

       
        private void cboPolygon1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (cboPolygon1.IsMouseOver)
                {
                    g_ParRaisedEdge.NameCellPolygon1 = cboPolygon1.SelectedValue.ToString(); 
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 检测位置
        /// </summary>
        private void cboPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (cboPosition.IsMouseOver)
                {
                    g_ParRaisedEdge.Position = cboPosition.SelectedValue.ToString().Split(':')[1].Trim();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        private void cboDefectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (cboDefectType.IsMouseOver)
                {
                    switch (cboDefectType.SelectedIndex)
                    {
                        case 0:
                            g_ParRaisedEdge.DefectType = "Fin";
                            break;
                        case 1:
                            g_ParRaisedEdge.DefectType = "Shell";
                            break;
                        default:
                            g_ParRaisedEdge.DefectType = "Fin";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        //判断数偶数
        public static bool IsOdd(int n)
        {
            return Convert.ToBoolean(n % 2);
        } 

        #endregion 调整参数

        #region 显示
        public override void ShowPar()
        {
            try
            {
                //显示直线交点单元
                ShowCellMCrossLineEdge();           
                cboOutlineType.SelectedIndex = g_ParRaisedEdge.OutlineType;
                //控件使能
                if (g_ParRaisedEdge.OutlineType == 0) // C角
                {
                }
                else //R角
                {

                }

                switch (g_ParRaisedEdge.DefectType)
                {
                    case "Fin":
                        cboDefectType.SelectedIndex = 0;
                        break;
                    case "Shell":
                        cboDefectType.SelectedIndex = 1;
                        break;
                    default:
                        cboDefectType.SelectedIndex = 0;
                        break;
                }

                //检测位置
                cboPosition.Text = g_ParRaisedEdge.Position;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 显示轮廓的M直线交点单元
        /// </summary>
        void ShowCellMCrossLineEdge()
        {
            try
            {
                if (g_cellExecute_L.Count == 0)
                {
                    return;
                }
                cboPolygon1.ItemsSource = g_CellMCrossLine_L;
                //判断是否包含M直线交点的单元
                if (g_CellMCrossLine_L.Contains(g_ParRaisedEdge.NameCellPolygon1)
                    && g_ParRaisedEdge.NameCellPolygon1 != "")
                {
                    cboPolygon1.Text = g_ParRaisedEdge.NameCellPolygon1;//
                }
                else
                {
                    cboPolygon1.Text = g_CellMCrossLine_L[0];
                    g_ParRaisedEdge.NameCellPolygon1 = cboPolygon1.Text;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        #endregion 显示


    }
}
