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
using System.IO;
using System.Diagnostics;
using BasicDisplay;
using Main_EX;


namespace Main
{
    public partial class DealComprehensiveResult4 : BaseDealComprehensiveResult_Main
    {        

        #region 定义
        


        #endregion 定义

  
        #region 位置1拍照
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public override StateComprehensive_enum DealComprehensiveResultFun1(TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            //int pos = 1;
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                if(ParStateSoft.StateMachine_e== StateMachine_enum.NullRun)
                {
                    return DealResult(1, string.Format("空跑模式，相机{0}第1次拍照默认ok", g_NoCamera));
                }

                if (!DealLocation((int)PtType_Mono.AutoMark1, StrMonoMatch1, Pos_enum.Pos1, out htResult))
                {
                    FinishPhotoPLC(2);
                    return StateComprehensive_enum.False;
                }

                FinishPhotoPLC(1);
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
        public override StateComprehensive_enum DealComprehensiveResultFun2(TriggerSource_enum trigerSource_e, out Hashtable htResult)
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
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    return DealResult(1, string.Format("空跑模式，相机{0}第2次拍照默认ok", g_NoCamera));
                }

                if (!DealLocation((int)PtType_Mono.AutoMark2, StrMonoMatch2, Pos_enum.Pos2, out htResult))
                {
                    FinishPhotoPLC(2);
                    return StateComprehensive_enum.False;
                }

                CalcMonoAngleCom();
                FinishPhotoPLC(1);
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

                Display(Pos_enum.Pos2, htResult, blResult, sw);

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
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    LogicPLC.L_I.WriteRegData2((int)DataRegister1.MonoOffsetX, 3, new double[] { 0.1, 0.2, 0.3 });
                    LogicPLC.L_I.WriteRegData2((int)DataRegister1.MonoOffsetConfirm, 1);
                    return DealResult(1, string.Format("空跑模式，相机{0}第3次拍照默认ok", g_NoCamera));
                }

                if (!DealLocation((int)PtType_Mono.AutoMark3, StrMonoMatch2, Pos_enum.Pos2, out htResult))
                {
                    FinishPhotoPLC(2);
                    return StateComprehensive_enum.False;
                }

                CalcMonoDeviation();
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

                Display(Pos_enum.Pos2, htResult, blResult, sw);

                #endregion 显示和日志记录
            }
        }
        #endregion 位置3拍照

        #region 位置4拍照
        public override StateComprehensive_enum DealComprehensiveResultFun4(TriggerSource_enum trigerSource_e, out Hashtable htResult)
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
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, Pos_enum.Pos4, out htResult);

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

                Display(Pos_enum.Pos4, htResult, blResult, sw);

                #endregion 显示和日志记录
            }
        }
        #endregion 位置4拍照


    }
}
