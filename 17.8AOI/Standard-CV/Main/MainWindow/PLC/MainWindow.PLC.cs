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
using ParComprehensive;
using BasicClass;
using DealConfigFile;
using Main_EX;

namespace Main
{
    /// <summary>
    /// PLC的相关定义和实现都在WinInitMain类里面 20181219-zx
    /// </summary>
    partial class MainWindow
    {
        #region PLC触发响应
        /// <summary>
        /// 报警
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void LogicPLC_Inst_PLCAlarm_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowState("设备发送报警信息!");
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 物料信息
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void L_I_PLCMaterial_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {

                //ShowState("设备发送物料信息!");
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 语音信息
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="i"></param>
        protected override void L_I_VoiceState_event(TriggerSource_enum trrigerSource_e, int i)
        {
            try
            {
                ShowVoice(i);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设备状态
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="intState"></param>
        protected override void LogicPLC_Inst_PLCState_event(TriggerSource_enum trrigerSource_e, int intState)
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
        /// 机器人状态
        /// </summary>
        /// <param name="trrigerSource_e"></param>
        /// <param name="intState"></param>
        protected override void LogicPLC_Inst_RobotState_event(TriggerSource_enum trrigerSource_e, int intState)
        {

        }

        /// <summary>
        /// 数据超限
        /// </summary>
        /// <param name="str"></param>
        protected override void L_I_WriteDataOverFlow(string str)
        {
            ShowAlarm("PLC输出数据超出范围");

            LogicPLC.L_I.PCAlarm();
        }
        #endregion PLC触发响应

        #region PLC换型相关
        /// <summary>
        /// 换型的时候写入PLC的值
        /// </summary>
        public override void WritePLCModelPar()
        {
            try
            {
                ShowState("发送取片t轴角度:" + Protocols.AxisT_PickFromPlat);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_PickFromPlat, Protocols.AxisT_PickFromPlat);
                ShowState("发送双目t轴角度:" + Protocols.AxisT_Precise);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_Precise, Protocols.AxisT_Precise);
                ShowState("发送工位1放片t轴角度:" + Protocols.AxisT_PlaceToAOI[0]);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_PlaceToAOI1, Protocols.AxisT_PlaceToAOI[0]);
                ShowState("发送工位2放片t轴角度:" + Protocols.AxisT_PlaceToAOI[1]);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_PlaceToAOI2, Protocols.AxisT_PlaceToAOI[1]);
                ShowState("发送工位3放片t轴角度:" + Protocols.AxisT_PlaceToAOI[2]);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_PlaceToAOI3, Protocols.AxisT_PlaceToAOI[2]);
                ShowState("发送工位4放片t轴角度:" + Protocols.AxisT_PlaceToAOI[3]);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_PlaceToAOI4, Protocols.AxisT_PlaceToAOI[3]);
                ShowState("发送工位1取片t轴角度:" + Protocols.AxisT_PickFromAOI[0]);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_PickFromAOI1, Protocols.AxisT_PickFromAOI[0]);
                ShowState("发送工位2取片t轴角度:" + Protocols.AxisT_PickFromAOI[1]);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_PickFromAOI2, Protocols.AxisT_PickFromAOI[1]);
                ShowState("发送工位3取片t轴角度:" + Protocols.AxisT_PickFromAOI[2]);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_PickFromAOI3, Protocols.AxisT_PickFromAOI[2]);
                ShowState("发送工位4取片t轴角度:" + Protocols.AxisT_PickFromAOI[3]);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_PickFromAOI4, Protocols.AxisT_PickFromAOI[3]);
                ShowState("发送下游放片t轴角度:" + Protocols.AxisT_PlaceToDown);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_PlaceToPlat, Protocols.AxisT_PlaceToDown);
                ShowState("发送旋转中心标定t轴角度:" + Protocols.AxisT_CalibRC);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_CalibRC, Protocols.AxisT_CalibRC);

                LogicPLC.L_I.WriteRegData1((int)DataRegister1.RecipeOK, 1);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        #endregion PLC换型相关
    }
}
