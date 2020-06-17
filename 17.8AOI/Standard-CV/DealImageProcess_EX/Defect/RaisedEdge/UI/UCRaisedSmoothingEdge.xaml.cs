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

namespace DealImageProcess_EX
{
    /// <summary>
    /// UCCrossLines.xaml 的交互逻辑
    /// </summary>
    public partial class UCRaisedSmoothingEdge : BaseUCImageProcess
    {
        #region 初始化
        public UCRaisedSmoothingEdge()
        {
            InitializeComponent();
        }

        public void Init(ParRaisedEdgeSmooth par, List<CellReference> cellExe_L, List<CellHObjectReference> cellHObject_L)
        {
            try
            {
                uCRaisedEdge.Init(par.g_ParRaisedEdge, cellExe_L, cellHObject_L);
                uCSmoothing.Init(par.g_ParSmooth, cellExe_L, cellHObject_L);

                uCUseCalib.Init(par, cellExe_L);//使用校准
                uCSetRecord.Init(par, cellHObject_L);//设置记录
                ShowPar_Invoke();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion 初始化

       
    }
}
