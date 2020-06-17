using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using Main_EX;
using DealConfigFile;

namespace Main
{
    public partial class MainWindow
    {
        /// <summary>
        /// 调用手动模拟运行
        /// </summary>
        public override void TriggerPC()
        {
            try
            {
                bool blNew = false;

                if (ParCameraWork.NumCamera > 4 && ParCameraWork.NumCamera < 9)
                {
                    WinTrrigerComprehensive win = WinTrrigerComprehensive.GetWinInst(out blNew);
                    if (blNew)
                    {
                        BaseDealComprehensiveResult[] baseDealComprehensiveResult = new BaseDealComprehensiveResult[8] {
                        DealComprehensiveResult1.D_I,
                        DealComprehensiveResult2.D_I ,
                        DealComprehensiveResult3.D_I,
                        DealComprehensiveResult4.D_I,
                        DealComprehensiveResult5.D_I,
                        DealComprehensiveResult6.D_I,
                        DealComprehensiveResult7.D_I,
                        DealComprehensiveResult8.D_I};
                        win.Init(g_UCStateWork, baseDealComprehensiveResult);

                        #region 注册事件
                        win.Camera1_event += new TrrigerSourceAction_del(DealComprehensive_Camera1_event);
                        win.Camera2_event += new TrrigerSourceAction_del(DealComprehensive_Camera2_event);
                        win.Camera3_event += new TrrigerSourceAction_del(DealComprehensive_Camera3_event);
                        win.Camera4_event += new TrrigerSourceAction_del(DealComprehensive_Camera4_event);
                        win.Camera5_event += new TrrigerSourceAction_del(DealComprehensive_Camera5_event);
                        win.Camera6_event += new TrrigerSourceAction_del(DealComprehensive_Camera6_event);
                        win.Camera7_event += new TrrigerSourceAction_del(DealComprehensive_Camera7_event);
                        win.Camera8_event += new TrrigerSourceAction_del(DealComprehensive_Camera8_event);

                        //包含Index
                        win.Camera1_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera1_event);
                        win.Camera2_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera2_event);
                        win.Camera3_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera3_event);
                        win.Camera4_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera4_event);
                        win.Camera5_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera5_event);
                        win.Camera6_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera6_event);
                        win.Camera7_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera7_event);
                        win.Camera8_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera8_event);
                        #endregion 注册事件
                    }
                    win.Show();
                }
                else if (ParCameraWork.NumCamera < 9)
                {
                    WinTrrigerComprehensiveSmall win = WinTrrigerComprehensiveSmall.GetWinInst(out blNew);
                    if (blNew)
                    {
                        BaseDealComprehensiveResult[] baseDealComprehensiveResult = new BaseDealComprehensiveResult[4] {
                        DealComprehensiveResult1.D_I,
                        DealComprehensiveResult2.D_I ,
                        DealComprehensiveResult3.D_I,
                        DealComprehensiveResult4.D_I,
};
                        win.Init(g_UCStateWork, baseDealComprehensiveResult);

                        #region 注册事件
                        win.Camera1_event += new TrrigerSourceAction_del(DealComprehensive_Camera1_event);
                        win.Camera2_event += new TrrigerSourceAction_del(DealComprehensive_Camera2_event);
                        win.Camera3_event += new TrrigerSourceAction_del(DealComprehensive_Camera3_event);
                        win.Camera4_event += new TrrigerSourceAction_del(DealComprehensive_Camera4_event);

                        //包含Index
                        win.Camera1_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera1_event);
                        win.Camera2_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera2_event);
                        win.Camera3_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera3_event);
                        win.Camera4_index_event += new TrrigerSourceIntAction_del(DealComprehensive_Camera4_event);
                        #endregion 注册事件
                    }
                    win.Show();
                }
                else
                {
                    WinTrrigerComprehensiveFull win = WinTrrigerComprehensiveFull.GetWinInst(out blNew);
                    if (blNew)
                    {
                        BaseDealComprehensiveResult[] baseDealComprehensiveResult = new BaseDealComprehensiveResult[12] {
                        DealComprehensiveResult1.D_I,
                        DealComprehensiveResult2.D_I ,
                        DealComprehensiveResult3.D_I,
                        DealComprehensiveResult4.D_I,
                        DealComprehensiveResult5.D_I,
                        DealComprehensiveResult6.D_I ,
                        DealComprehensiveResult7.D_I,
                        DealComprehensiveResult8.D_I,

                        DealComprehensiveResult9.D_I,
                        DealComprehensiveResult10.D_I ,
                        DealComprehensiveResult11.D_I,
                        DealComprehensiveResult12.D_I,
};
                        win.Init(g_UCStateWork, baseDealComprehensiveResult);
                    }
                    win.Show();
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
    }
}
