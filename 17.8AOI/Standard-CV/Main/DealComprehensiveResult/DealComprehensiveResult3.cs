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


namespace Main
{
    public partial class DealComprehensiveResult3 : BaseDealComprehensiveResult_Main
    {  
        #region 定义
        //double            


        #endregion 定义

       

        #region 位置1拍照
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun1(TriggerSource_enum trigerSource_e,
            int index, out Hashtable htResult)
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
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    return DealResult(1, string.Format("相机{0}空跑默认OK", g_NoCamera));
                }

                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                     g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = htResult[Camera2Match1] as BaseResult;

                if (!DealTypeResult(result))
                {
                    ShowAlarm("精定位相机2拍照NG!");
                    LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_PreciseNG);
                    return StateComprehensive_enum.False;
                }

                Pt2Mark2.DblValue1 = result.X;
                Pt2Mark2.DblValue2 = result.Y;
                Camera2Done = true;
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

                //g_UCDisplayCamera.ShowResult("NG\nX=20\nY=100\n", false);
                //long t = g_UCDisplayCamera.SaveBit_Screen("D:\\");
                //ShowState_Hidden("保存屏幕截图时间:" + t.ToString());
            }
        }
        #endregion 位置1拍照

        #region 位置2拍照
        public override StateComprehensive_enum DealComprehensiveResultFun2(TriggerSource_enum trigerSource_e,
            int index, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 2;
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义

            try
            {

                //标定基准位
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                    g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos1, out htResult);
                BaseResult result = htResult[Camera2Match1] as BaseResult;

                if (!DealTypeResult(result))
                {
                    ShowAlarm("精定位相机2第二次拍照NG!");
                    LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_PreciseNG);
                    return StateComprehensive_enum.False;
                }

                Pt2Mark2.DblValue1 = result.X;
                Pt2Mark2.DblValue2 = result.Y;
                Camera2Done = true;
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
        public override StateComprehensive_enum DealComprehensiveResultFun3(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 3;
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
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
            bool blResult = true;//结果是否正确
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
                    Pt2Mark2CalibStd.DblValue1 = result.X;
                    Pt2Mark2CalibStd.DblValue2 = result.Y;
                }
                else
                {
                    Pt2Mark2Calib.DblValue1 = result.X;
                    Pt2Mark2Calib.DblValue2 = result.Y;

                }
                Camera2Done = true;
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
