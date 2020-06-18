using BasicClass;
using DealAlgorithm;
using DealCalibrate;
using DealComprehensive;
using DealPLC;
using DealResult;
using DealRobot;
using Main_EX;
using ParComprehensive;
using System;
using System.Collections;

namespace Main
{
    public partial class BaseDealComprehensiveResult_Main
    {
        #region properties
        protected double MonoR = 0;

        public const double AxisCalibOffset = 5;
        #endregion

        #region 单相机双次定位
        #region 接口
        /// <summary>
        /// 将当前单目运行的结果记录到点位数组对应的位置
        /// </summary>
        /// <param name="index">数组存放该数据的索引</param>
        /// <param name="cellName">算子序号</param>
        /// <param name="pos">拍照位置</param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public bool DealLocation(int index, string cellName, Pos_enum pos, out Hashtable htResult)
        {
            htResult = null;
            try
            {
                #region 空跑
                if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
                {
                    ShowState(string.Format("相机{0}第{1}次拍照空跑默认ok", g_NoCamera, index + 1));
                    FinishPhotoPLC(1);
                    return true;
                }
                #endregion
                //保存当前匹配结果到指定的点中
                if (!SaveMatchResult(index, cellName, pos, out htResult))
                {
                    ShowState(string.Format("相机{0}第{1}次拍照失败", g_NoCamera, index + 1));
                    FinishPhotoPLC(CameraResult.NG);
                    return false;
                }

                ShowState(string.Format("相机{0}第{1}次拍照成功", g_NoCamera, index + 1));
                FinishPhotoPLC(CameraResult.OK);
                return true;
            }
            catch (Exception ex)
            {
                FinishPhotoPLC(CameraResult.NG);
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 单目相机偏差计算
        /// </summary>
        /// <param name="index">基准值的索引，计算结果与该基准值进行计算，需要与标定时的index一致</param>
        /// <param name="baseParComprehensive">拍照相机的算子参数，用于索引旋转中心的算子</param>
        /// <param name="result"></param>
        /// <returns></returns>
        //public bool MonoCalc(int index, BaseParComprehensive baseParComprehensive, out double[] result)
        //{
        //    result = new double[3] { 0, 0, 0 };
        //    try
        //    {
        //        //TODO 根据产品两角，以及旋转中心和基准值计算偏差        

        //        if (!Calc(pt2MarkArray[(int)PtType_Mono.AutoMark1],
        //            pt2MarkArray[(int)PtType_Mono.AutoMark2], index, baseParComprehensive, out result))
        //        {
        //            ShowState(string.Format("相机{0}偏差计算出错", g_NoCamera));
        //            return false;
        //        }

        //        FinishPhotoPLC(CameraResult.OK);
        //        ShowState(string.Format("单目相机偏差：X:{0},Y:{1},R{2}", result[0], result[1], result[2]));
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        FinishPhotoPLC(CameraResult.NG);
        //        Log.L_I.WriteError(NameClass, ex);
        //        return false;
        //    }
        //}

        /// <summary>
        /// 校验角度是否在可接受范围内，单目标定过程中，除了标定旋转中心，之前每次拍照理论上玻璃都应该是0度
        /// </summary>
        /// <param name="disMark">mark间距</param>
        /// <param name="thread">判断阈值</param>
        /// <returns></returns>
        //public bool VerifyAngleCom(double disMark, double thread)
        //{
        //    try
        //    {
        //        double r = ModuleBase.GetCurAngleByY(disMark, AMP,
        //            pt2MarkArray[(int)PtType_Mono.AutoMark2], pt2MarkArray[(int)PtType_Mono.AutoMark3]);
        //        ShowState(string.Format("角度补正后，当前角度为:{0},阈值{1}", (-r).ToString(ReservDigits), thread));

        //        if (Math.Abs(r) > thread)
        //            return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.L_I.WriteError(NameClass, ex);
        //        return false;
        //    }
        //    return true;
        //}

        /// <summary>
        /// 标定旋转中心
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="baseParComprehensive"></param>
        /// <returns></returns>
        public bool CalibRC(string cellName, BaseParComprehensive baseParComprehensive)
        {


            ShowState(string.Format("开始计算旋转中心"));
            //if (ModelParams.IfUseRealRC && !CalibRotateCenter(cellName, pt2MarkArray[(int)PtType_Mono.R180Mark2],
            //    pt2MarkArray[(int)PtType_Mono.RcMark2], -ModelParams.RCCalibAngle, baseParComprehensive))
            //{
            //    ShowState("旋转中心计算失败");
            //    return false;
            //}

            ////验证计算旋转中心时PLC控制玻璃旋转的角度是否与设定的计算角度一致
            //if (!VerifyRotateCenter(pt2MarkArray[(int)PtType_Mono.R180Mark1],
            //    pt2MarkArray[(int)PtType_Mono.R180Mark2],
            //    pt2MarkArray[(int)PtType_Mono.RcMark1],
            //    pt2MarkArray[(int)PtType_Mono.RcMark2], -ModelParams.RCCalibAngle, 0.2))
            //{
            //    ShowAlarm("旋转中心角度校验失败，标定结果可能存在偏差，请人工确认");
            //}

            ShowState(string.Format("相机{0}标定结束", g_NoCamera));
            return true;
        }
        #endregion

        /// <summary>
        /// 保存对应单元格的匹配结果
        /// </summary>
        /// <param name="index">当前坐标对应的数组索引</param>
        /// <param name="cellName">M直线算子序号</param>
        /// <param name="pos">拍照位置</param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public bool SaveMatchResult(int index, string cellName, Pos_enum pos, out Hashtable htResult)
        {
            htResult = null;
            try
            {
                StateComprehensive_enum stateComprehensive_e = g_BaseDealComprehensive.DealComprehensivePosNoDisplay(g_UCDisplayCamera, g_HtUCDisplay, pos, out htResult);
                ResultCrossLines result = htResult[cellName] as ResultCrossLines;

                if (!DealTypeResult(result))
                    return false;
                //保存当前匹配结果
                pt2MarkArray[index].DblValue1 = result.X;
                pt2MarkArray[index].DblValue2 = result.Y;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 计算偏差
        /// </summary>
        /// <param name="mark1">单目定位中的角1</param>
        /// <param name="mark2">单目定位中的角2，即用于标定以及计算偏差的角</param>
        /// <param name="index">基准值索引</param>
        /// <param name="baseParComprehensive"></param>
        /// <param name="pt3Result"></param>
        /// <returns></returns>
        public bool Calc(Point2D mark1, Point2D mark2, int index, BaseParComprehensive baseParComprehensive, out double[] pt3Result)
        {
            //return MonocularLation.CalcDeviationY(mark1, mark2, AMP,
            //    ModelParams.MonoGlassX, strCamera5RC, index, baseParComprehensive, out pt3Result);
            double r;

            r = ModuleBase.GetCurAngleByY(Protocols.MonoGlassX, AMP, mark2, mark1);
            return MonocularLation.CalcDeviationY(mark2, r, AMP,
                StrMonoRC, index, baseParComprehensive, out pt3Result);
        }

        /// <summary>
        /// 单目相机第1，2次拍照时，发送角度补偿给plc
        /// </summary>
        /// <param name="index">基准值的索引</param>
        /// <param name="IfAdjustAngle"></param>
        /// <returns></returns>
        public StateComprehensive_enum CalcMonoAngleCom()
        {
            try
            {
                //根据calibmark计算当前角度，根据目前机台设计，此处全部用的deltaY进行角度计算，预留了X的接口
                MonoAngleCom = -ModuleBase.GetCurAngleByX(Protocols.MonoGlassY, AMP,
                    pt2MarkArray[(int)PtType_Mono.AutoMark1], pt2MarkArray[(int)PtType_Mono.AutoMark2]);
                //此处未作镜像处理


                //发送角度结果
                //镜像角度取反

                //WritePLC(2, (int)DataRegister2.CalibDeltaR, ModelParams.IfMirrior ? r : -r);
                WritePLC(1, (int)DataRegister1.MonoOffsetX, 4, new double[4] { 0, 0, MonoAngleCom, 1 });
                ShowState(string.Format("相机{0}角度偏差为，r:{1}", g_NoCamera, MonoAngleCom.ToString(ReservDigits)));
                return StateComprehensive_enum.True;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return StateComprehensive_enum.False;
            }
        }

        public double CalcMonoAngleOffset(Point2D pt2Mark1, Point2D pt2Mark2, double disMark, bool horizontol)
        {
            double r = 0;
            try
            {
                if (horizontol)
                {
                    r = -ModuleBase.GetCurAngleByY(disMark, AMP, pt2Mark1, pt2Mark2);
                }
                else
                {
                    r = -ModuleBase.GetCurAngleByX(disMark, AMP, pt2Mark1, pt2Mark2);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            return r;
        }

        public void CalcMonoDeviation()
        {
            try
            {
                //x由fork补偿，如果玻璃有正的像素偏差，fork应当朝正方向去交接
                double deltaX = (pt2MarkArray[(int)PtType_Mono.AutoMark3].DblValue1 - 480) * AMP;
                //y由搬运补偿，如果玻璃有正的像素偏差，fork
                double deltaY = (pt2MarkArray[(int)PtType_Mono.AutoMark3].DblValue2 - 640) * AMP;
                ShowState(string.Format("相机4单目偏差{0}，{1}", deltaX.ToString("f3"), deltaY.ToString("f3")));

                WritePLC(1, (int)DataRegister1.MonoOffsetX, 4,
                    new double[4] { deltaX + Protocols.MonoComX,
                        deltaY + Protocols.MonoComY,
                        MonoAngleCom + Protocols.MonoComR,
                        1 });
                //Point4D pt = new Point4D(MainParProduct.M_I.R_posInsert.DblValue1 + deltaX,
                //    MainParProduct.M_I.R_posInsert.DblValue2 + deltaY,
                //    MainParProduct.M_I.R_posInsert.DblValue3,
                //    MainParProduct.M_I.R_posInsert.DblValue4);
                //ShowState(string.Format("发送机器人插栏交接位置：{0},{1}", pt.DblValue1, pt.DblValue2));

                //LogicRobot.L_I.WriteRobotCMD(pt, ModelParams.cmd_VerifyData);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public double[] CalcMonoOffset(Point2D pt2Mark)
        {
            double[] value = new double[2];
            value[0] = (480 - pt2Mark.DblValue1) * AMP;
            value[1] = (pt2Mark.DblValue2 - 640) * AMP;

            return value;
        }
        #endregion

        #region axiscalib
        /// <summary>
        /// 轴标定的数据计算函数
        /// <para></para>
        /// </summary>
        /// <param name="index"></param>
        public void AxisCalib(int index)
        {
            if (Camera1Done & Camera2Done)
            {
                if (index == 1) return;
                AXIS type = AXIS.X;
                double deltaX, deltaY, deltaZ, deltaR, len, amp;
                double[] value = new double[] { };
                switch (index)
                {
                    case 2:
                        type = AXIS.X;
                        //如果用达到点-基准点，获得的符号是轴的方向，这边要做的是补偿
                        //所以应当取反
                        deltaX = -(Pt2Mark1Calib.DblValue1 - Pt2Mark1CalibStd.DblValue1) / AxisCalibOffset;
                        deltaY = -(Pt2Mark1Calib.DblValue2 - Pt2Mark1CalibStd.DblValue2) / AxisCalibOffset;
                        deltaZ = 0;
                        deltaR = 0;

                        //len = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                        //amp = 1 / len;
                        AxisSerivce.GetInstance().SetA1(0, deltaX);
                        AxisSerivce.GetInstance().SetB1(0, deltaY);

                        //value = new double[4] { deltaX / len * amp, deltaY / len * amp, deltaZ, deltaR };
                        value = new double[4] { deltaX, deltaY, deltaZ, deltaR };
                        break;
                    case 3:
                        type = AXIS.Y;
                        //deltaX = Pt2Mark1Calib.DblValue1 - Pt2Mark1CalibStd.DblValue1;
                        //deltaY = Pt2Mark1Calib.DblValue2 - Pt2Mark1CalibStd.DblValue2;
                        deltaX = -(Pt2Mark1Calib.DblValue1 - Pt2Mark1CalibStd.DblValue1) / AxisCalibOffset;
                        deltaY = -(Pt2Mark1Calib.DblValue2 - Pt2Mark1CalibStd.DblValue2) / AxisCalibOffset;
                        deltaZ = 0;
                        deltaR = 0;

                        //len = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                        //amp = 1 / len;
                        AxisSerivce.GetInstance().SetA2(0, deltaX);
                        AxisSerivce.GetInstance().SetB2(0, deltaY);

                        value = new double[4] { deltaX, deltaY, deltaZ, deltaR };
                        //value = new double[4] { deltaX / len * amp, deltaY / len * amp, deltaZ, deltaR };
                        break;
                    case 4:
                        type = AXIS.U;
                        double originR = ModuleBase.GetCurAngleByY(Protocols.ConfDisMark, AMP, Pt2Mark1CalibStd, Pt2Mark2CalibStd);
                        double curR = ModuleBase.GetCurAngleByY(Protocols.ConfDisMark, AMP, Pt2Mark1Calib, Pt2Mark2Calib);
                        double r = curR - originR;
                        //如果轴转了0.5，算出来角度偏差0.5，那么补偿的时候就应该加负号
                        //如果算出来是-0.5，那系数是1，直接发给轴做补偿就可以了
                        deltaR = r / Protocols.AxisT_CalibRC > 0 ? -1 : 1;

                        value = new double[4] { 0, 0, 0, deltaR };

                        r = Protocols.AxisT_CalibRC;
                        double dy1 = Pt2Mark2CalibStd.DblValue2 - Pt2Mark1CalibStd.DblValue2;
                        double dy2 = Pt2Mark2Calib.DblValue2 - Pt2Mark1Calib.DblValue2;
                        double a = dy1 / Protocols.ConfDisMark;
                        double b = dy2 / Protocols.ConfDisMark;
                        double m = Math.PI / 180;

                        deltaR = 2 / Math.Sqrt(
                            Math.Pow((a + b) / Math.Cos(r * m / 2), 2)
                            + Math.Pow((a - b) / Math.Sin(r * m / 2), 2));
                        //double r1 = Math.Asin(dy1 * deltaR / Protocols.ConfDisMark) * 180 / Math.PI;
                        //double r2 = Math.Asin(dy2 * deltaR / Protocols.ConfDisMark) * 180 / Math.PI;
                        AxisSerivce.GetInstance().SetAMP(0, deltaR);
                        break;
                }

                //AxisDirectionService.GetInstance().SetCo(type, value);
                //AxisDirectionService.GetInstance().Save();
                LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_AxisCalibOK);
            }
        }

        public void CalcCo(int index)
        {
            AxisCalibAngleStd = ModuleBase.GetCurAngleByY(Protocols.GlassXInPresize, AMP, Pt2Mark1CalibStd, Pt2Mark2CalibStd);
            double r = ModuleBase.GetCurAngleByY(Protocols.GlassXInPresize, AMP, Pt2Mark1Calib, Pt2Mark2Calib);
            double[] delta = new double[4];

            delta[0] = Pt2Mark2Calib.DblValue1 - Pt2Mark2CalibStd.DblValue1;
            delta[1] = Pt2Mark2Calib.DblValue2 - Pt2Mark2CalibStd.DblValue2;
            delta[2] = 0;
            delta[3] = r - AxisCalibAngleStd;

            AxisDirectionService.GetInstance().SetCo((AXIS)Enum.Parse(typeof(AXIS), (index - 1).ToString()), delta);
        }
        #endregion
    }
}
