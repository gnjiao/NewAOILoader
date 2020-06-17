using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using HalconDotNet;
using System.Threading.Tasks;
using DealPLC;
using Common;
using DealRobot;
using System.IO;
using DealFile;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using DealComprehensive;
using SetPar;
using BasicClass;
using DealConfigFile;

namespace Main
{
    /// <summary>
    /// 20181219-zx,
    /// </summary>
    partial class MainWindow
    {
        #region 定义

        #endregion 定义

        /// <summary>
        /// 保留触发1
        /// </summary>
        protected override void LogicPLC_Inst_Reserve1_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {
                LogicPLC.L_I.WriteRegData1((int)DataRegister1.CodeResult, 1);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发2
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve2_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

                SendInsertData(i);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发3 
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve3_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ChangCSTSationNum(i); 
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发4
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve4_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ChangeCol();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发5
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve5_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发6
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve6_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留6");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发7
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve7_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留7");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发8
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve8_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留8");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发9
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve9_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留9");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发10
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve10_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留10");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }            
        }

        /// <summary>
        /// 保留触发11,发送泡棉
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve11_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留11");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }


        /// <summary>
        /// 保留触发12
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve12_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留12");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发13
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve13_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留13");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发14
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve14_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留14");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发15
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve15_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留15");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发16
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve16_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留16");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发17
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve17_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留17");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发18
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve18_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留18");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发19
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve19_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留19");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 保留触发20
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_Reserve20_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("触发保留20");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
    }
}
