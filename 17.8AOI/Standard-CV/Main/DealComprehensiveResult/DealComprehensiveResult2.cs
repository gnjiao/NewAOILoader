using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using System.Threading.Tasks;
using DealFile;
using DealComprehensive;
using Common;
using SetPar;
using ParComprehensive;
using BasicClass;
using Camera;
using System.Collections;
using DealResult;
using DealConfigFile;
using DealCalibrate;
using DealRobot;
using DealMath;
using DealImageProcess;
using BasicComprehensive;
using System.Diagnostics;
using BasicDisplay;
using Main_EX;
using DealLog;
using System.Windows;

namespace Main
{
    public partial class DealComprehensiveResult2 : BaseDealComprehensiveResult_Main
    {
        
        #region 定义

  
        #endregion 定义

       
        #region 位置1拍照
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun1(
            TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 1;
            bool blResult = true;

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
               if(ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    ShowState(string.Format("空跑模式，相机{0}第1次拍照默认ok", g_NoCamera));
                    LogicRobot.L_I.WriteRobotCMD(new Point4D(-540, -200, -40, 0), Protocols.BotCmd_StationPos);
                    LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_PreciseOK);
                    return StateComprehensive_enum.True;
                }

                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                     g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = htResult[Camera1Match1] as BaseResult;

                if (!DealTypeResult(result))
                {
                    ShowAlarm("精定位相机1拍照NG!");
                    LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_PreciseNG);
                    return StateComprehensive_enum.False;
                }

                Pt2Mark1.DblValue1 = result.X;
                Pt2Mark1.DblValue2 = result.Y;
                Camera1Done = true;
                DualLocation(index);
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录
               
            }
        }
        #endregion 位置1拍照

        #region 位置2拍照
        public override StateComprehensive_enum DealComprehensiveResultFun2(
            TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 2;
            bool blResult = true;

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                //标定基准位
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = htResult[Camera1Match1] as BaseResult;

                if (!DealTypeResult(result))
                {
                    ShowAlarm("精定位相机1第二次拍照NG!");
                    LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_PreciseNG);
                    return StateComprehensive_enum.False;
                }

                Pt2Mark1.DblValue1 = result.X;
                Pt2Mark1.DblValue2 = result.Y;
                Camera1Done = true;
                CalibDual(index);
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录

                Display(Pos_enum.Pos1, htResult, blResult, sw);

                #endregion 显示和日志记录
            }
        }
        #endregion 位置2拍照

        #region 位置3拍照
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun3(
            TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 3;
            bool blResult = true;

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                //标定旋转中心
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = htResult[Camera2Match1] as BaseResult;

                if (!DealTypeResult(result))
                {
                    ShowAlarm("精定位相机2标定旋转中心拍照NG!");
                    LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_PreciseNG);
                    MessageBox.Show("标定旋转中心失败，请重新标定!", "提示");
                    return StateComprehensive_enum.False;
                }

                Pt2MarkRC.DblValue1 = result.X;
                Pt2MarkRC.DblValue2 = result.Y;
                CalibRC();

                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置3拍照

        #region 位置4拍照
        public override StateComprehensive_enum DealComprehensiveResultFun4(TriggerSource_enum trigerSource_e
            , int index, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 4;
            bool blResult = true;

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                     g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = htResult[Camera1Match1] as BaseResult;

                if (!DealTypeResult(result))
                {
                    ShowAlarm("精定位相机1拍照NG!");
                    LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_PreciseNG);
                    return StateComprehensive_enum.False;
                }

                if (index == 1)
                {
                    Pt2Mark1CalibStd.DblValue1 = result.X;
                    Pt2Mark1CalibStd.DblValue2 = result.Y;                    
                }
                else
                {
                    Pt2Mark1Calib.DblValue1 = result.X;
                    Pt2Mark1Calib.DblValue2 = result.Y;
                   
                }

                //Camera1Done = true;
                AxisCalib(index);
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
            finally
            {
                #region 显示和日志记录
                Display(Pos_enum.Pos1, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置4拍照
    }
}
