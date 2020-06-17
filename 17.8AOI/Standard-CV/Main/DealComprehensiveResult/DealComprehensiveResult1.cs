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
using DealImageProcess;
using System.Diagnostics;
using BasicDisplay;
using Main_EX;
using DealGrabImage;
using DealAlgorithm;
using DealLog;
using NLog.Config;

namespace Main
{
    public partial class DealComprehensiveResult1 : BaseDealComprehensiveResult_Main
    {
        #region 定义
        //double            



        #endregion 定义

        #region 位置1拍照
        /// <summary>
        /// 
        /// </summary>
        public override StateComprehensive_enum DealComprehensiveResultFun1(
            TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            PosNow_e = Pos_enum.Pos1;//当前位置
            bool blResult = true;

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义
            try
            {
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    FinishPhotoPLC(1);
                    ShowState(string.Format("空跑模式，相机{0}第1次拍照默认ok", g_NoCamera));
                    return StateComprehensive_enum.True;
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

        #region 各相机转心 Point2D
        /// <summary>
        /// 相机1旋转中心
        /// </summary>
        public static Point2D PRCCam1
        {
            get
            {
                List<string> nameCell = ParComprehensive1.P_I.GetAllNameCellByType("旋转中心变换");

                if (nameCell == null || nameCell.Count == 0)
                {
                    return new Point2D(ParAdjust.Value1("adj18"), ParAdjust.Value2("adj18"));
                }
                else
                {
                    ParCalibRotate par = ParComprehensive1.P_I.GetCellParCalibrate(nameCell[0]) as ParCalibRotate;
                    return par.PointRC;
                }
            }
            set
            {
                List<string> nameCell = ParComprehensive1.P_I.GetAllNameCellByType("旋转中心变换");

                if (nameCell == null || nameCell.Count == 0)
                {
                    ParAdjust.SetValue1("adj18", value.DblValue1);
                    ParAdjust.SetValue2("adj18", value.DblValue2);
                }
                else
                {
                    ParCalibRotate par = ParComprehensive1.P_I.GetCellParCalibrate(nameCell[0]) as ParCalibRotate;
                    if (par != null)
                    {
                        par.XRC = value.DblValue1;
                        par.YRC = value.DblValue2;
                        ParComprehensive1.P_I.WriteXmlDoc(par.NameCell);
                    }
                }

            }
        }

        /// <summary>
        /// 相机2旋转中心
        /// </summary>
        public static Point2D PRCCam2 = new Point2D();

        /// <summary>
        /// 相机3旋转中心
        /// </summary>
        public static Point2D PRCCam3 = new Point2D();

        /// <summary>
        /// 相机4旋转中心
        /// </summary>
        public static Point2D PRCCam4
        {
            get
            {
                List<string> nameCell = ParComprehensive4.P_I.GetAllNameCellByType("旋转中心变换");

                if (nameCell == null || nameCell.Count == 0)
                {
                    return new Point2D(ParAdjust.Value1("adj18"), ParAdjust.Value2("adj18"));
                }
                else
                {
                    ParCalibRotate par = ParComprehensive4.P_I.GetCellParCalibrate(nameCell[0]) as ParCalibRotate;
                    return par.PointRC;
                }
            }
            set
            {
                List<string> nameCell = ParComprehensive4.P_I.GetAllNameCellByType("旋转中心变换");

                if (nameCell == null || nameCell.Count == 0)
                {
                    ParAdjust.SetValue1("adj18", value.DblValue1);
                    ParAdjust.SetValue2("adj18", value.DblValue2);
                }
                else
                {
                    ParCalibRotate par = ParComprehensive4.P_I.GetCellParCalibrate(nameCell[0]) as ParCalibRotate;
                    if (par != null)
                    {
                        par.XRC = value.DblValue1;
                        par.YRC = value.DblValue2;
                        ParComprehensive4.P_I.WriteXmlDoc(par.NameCell);
                    }
                }

            }
        }
        #endregion
        #endregion 位置1拍照

        #region 位置2拍照
        public override StateComprehensive_enum DealComprehensiveResultFun2(
            TriggerSource_enum trigerSource_e, out Hashtable htResult)
        {
            #region 定义
            htResult = g_HtResult;
            PosNow_e = Pos_enum.Pos2;//当前位置
            bool blResult = true;//结果是否正确
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            #endregion 定义

            try
            {
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    FinishPhotoPLC(1);
                    ShowState(string.Format("空跑模式，相机{0}第2次拍照默认ok", g_NoCamera));
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.OffsetX, 4, new double[] { 0.1, 0.2, 0.3, 1 });
                    return StateComprehensive_enum.True;
                }

                if (!DealLocation((int)PtType_Mono.AutoMark2, StrMonoMatch2, Pos_enum.Pos2, out htResult))
                {
                    FinishPhotoPLC(2);
                    return StateComprehensive_enum.False;
                }

                FinishPhotoPLC(1);
                //MonoR = CalcMonoAngleOffset(pt2MarkArray[(int)PtType_Mono.AutoMark1], pt2MarkArray[(int)PtType_Mono.AutoMark2], Protocols.ConfGlassY, false);
                double[] delta = MonocularLation.CalcDeltaWithAngle(pt2MarkArray[(int)PtType_Mono.AutoMark1], pt2MarkArray[(int)PtType_Mono.AutoMark2],
                    Protocols.ConfGlassY, AMP, new Point2D(Protocols.ConfCodeX, Protocols.ConfCodeY));

                //xy都由plc的搬运臂补偿
                //偏差
                double deltaX = -(480 - pt2MarkArray[(int)PtType_Mono.AutoMark2].DblValue1) * AMP;
                double deltaY = (640 - pt2MarkArray[(int)PtType_Mono.AutoMark2].DblValue2) * AMP;
                ShowState("相机1补偿X:" + deltaX.ToString("f3") + ",Y:" + deltaY.ToString("f3"));
                ShowState("相机1角度导致的偏差X:" + delta[0].ToString("f3") + ",Y:" + delta[1].ToString("f3"));
                WritePLC(1, (int)DataRegister1.OffsetX, 4,
                    new double[] { delta[0] + deltaX+Protocols.Cam1ComX,
                        delta[1] + deltaY+Protocols.Cam1ComY,
                        delta[2]+Protocols.Cam1ComR,
                        1 });

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
                //if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                //{
                //    FinishPhotoPLC(1);
                //    ShowState(string.Format("空跑模式，相机{0}第3次拍照默认ok", g_NoCamera));
                //    LogicPLC.L_I.WriteRegData1((int)DataRegister1.OffsetX, 3, new double[] { 0.1, 0.2, 0.3 });
                //    LogicPLC.L_I.WriteRegData1((int)DataRegister1.OffsetConfirm, 1);
                //    return StateComprehensive_enum.True;
                //}

                //if (!DealLocation((int)PtType_Mono.AutoMark3, StrMonoMatch2, Pos_enum.Pos2, out htResult))
                //{
                //    FinishPhotoPLC(2);
                //    return StateComprehensive_enum.False;
                //}

                //FinishPhotoPLC(1);
                //double[] delta = CalcMonoOffset(pt2MarkArray[(int)PtType_Mono.AutoMark3]);
                //WritePLC(1, (int)DataRegister1.OffsetX, 4, new double[] { delta[0], delta[1], MonoR, 1 });

                //Point2D origin1 = new Point2D(519.767, 280.716);
                //Point2D origin2 = new Point2D(497.875, 514.6);

                //Point2D y2_1 = new Point2D(519.556, 327.833);
                //Point2D y2_2 = new Point2D(496.793, 562.702);

                //Point2D x1y2_1 = new Point2D(543.863, 328.21);
                //Point2D x1y2_2 = new Point2D(522.383, 562.555);

                //double len = Math.Sqrt((y2_2.DblValue1 - origin2.DblValue1) * (y2_2.DblValue1 - origin2.DblValue1) +
                //    (y2_2.DblValue2 - origin2.DblValue2) * (y2_2.DblValue2 - origin2.DblValue2));
                //double amp = 2 / len;

                //double deltaX = y2_2.DblValue1 - origin2.DblValue1;
                //double deltaY = y2_2.DblValue2 - origin2.DblValue2;
                //double deltaZ = 0;
                //double deltaR = 0;

                //double[] value = new double[4] { deltaX / len * amp, deltaY / len * amp, deltaZ, deltaR };

                //AxisDirectionService.GetInstance().SetCo(AXIS.Y, value);
                //AxisDirectionService.GetInstance().Save();

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
                Display(Pos_enum.Pos3, htResult, blResult, sw);
                #endregion 显示和日志记录
            }
        }
        #endregion 位置3拍照

        #region 位置4拍照
        public override StateComprehensive_enum DealComprehensiveResultFun4(TriggerSource_enum trigerSource_e, int index, out Hashtable htResult)
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
