using BasicClass;
using DealCalibrate;
using DealComprehensive;
using DealFile;
using DealPLC;
using DealResult;
using Main_EX;
using ModulePackage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Main
{
    public partial class BaseDealComprehensiveResult_Main
    {
        #region 卡塞拍照
        /// <summary>
        /// 龙骨拍照计算
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="index"></param>
        /// <param name="pos"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public StateComprehensive_enum CSTProcessing(
            TriggerSource_enum trigerSource_e, int index, int pos, out Hashtable htResult)
        {
            htResult = null;
            #region 空跑
            if (ParStateSoft.StateMachine_e == StateMachine_enum.NullRun)
            {
                return DealResult(1, string.Format("相机{0}空跑默认OK", g_NoCamera));
            }
            #endregion

            StateComprehensive_enum stateComprehensive_e =
                    g_BaseDealComprehensive.DealComprehensivePosNoDisplay(
                        g_UCDisplayCamera, g_HtUCDisplay, 1, out htResult);

            if (index == 1)
                return RecordTopKeel(htResult);
            else if (index == 2)
                return RecordBottomKeel(htResult);
            else if (index == 3)
                return CalcKeel(pos, htResult);
            else
            {
                htResult = null;
                return StateComprehensive_enum.False;
            }
        }
        /// <summary>
        /// 记录左右横拍的数据
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="pos"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public StateComprehensive_enum CalcKeel(int pos, Hashtable htResult)
        {
            try
            {
                BaseResult result = htResult[CSTMatch3] as BaseResult;

                if (pos == 1)
                {
                    //CSTLocation.SetLeftDevEnable(result.DeltaX * AMP);
                    CSTLocation.SetLeftDevEnable((result.X_L[0] - Protocols.CstStdValue.DblValue1) * AMP);
                }
                else if (pos == 2)
                {
                    //CSTLocation.SetRightDevEnable(result.DeltaX * AMP);
                    CSTLocation.SetRightDevEnable((result.X_L[0] - Protocols.CstStdValue.DblValue1) * AMP);
                }

                return DealResult(1, @"龙骨识别成功");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("DealComprehensiveResult", ex);
                return DealResult(2, @"龙骨计算异常");
            }
        }
        /// <summary>
        /// 记录上龙骨的拍照结果
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="pos"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public StateComprehensive_enum RecordTopKeel(Hashtable htResult)
        {
            try
            {
                BaseResult result = (BaseResult)htResult[CSTMatch1];
                //把算子基准值记录到基准值
                if (Protocols.CstStdValue.DblValue1 == 0)
                    Protocols.CstStdValue = new Point2D(result.StdX, result.StdY);

                if (result.Num != 0)
                {
                    CSTLocation.TopPhotoCount++;
                    if (CSTLocation.TopPhotoCount > Protocols.KeelCol)
                    {
                        return DealResult(2, "上龙骨拍照次数大于龙骨数量，本次拍照无效");
                    }
                    result.Y_L.Sort();
                    //double deltaY = (result.Y_L[0] - result.StdY) * ParCalibWorld.V_I[g_NoCamera];
                    double deltaY = (result.Y_L[0] - Protocols.CstStdValue.DblValue2) * AMP;
                    if (deltaY > 0)
                    {
                        int i = 0;
                        while (Math.Abs(deltaY) > Protocols.ConfLayerSpacing / 2)
                        {
                            deltaY -= Protocols.ConfLayerSpacing;
                            ++i;
                        }
                        if (i > 0)
                            ShowAlarm(string.Format(
                                 "第{0}列上龙骨漏匹配，进行自动校正{1}层", CSTLocation.TopPhotoCount, i));
                    }

                    //CSTLocation.TopDev_L.Add(new Point2D(
                    //    result.DeltaX * ParCalibWorld.V_I[g_NoCamera], deltaY));
                    CSTLocation.TopDev_L.Add(new Point2D(
                        (result.X_L[0] - Protocols.CstStdValue.DblValue1) * AMP, deltaY));

                    return DealResult(1, string.Format("第{0}列上龙骨识别成功", CSTLocation.TopPhotoCount));
                }
                else
                {
                    return DealResult(2, string.Format("第{0}列上龙骨识别失败", CSTLocation.TopPhotoCount));
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("DealComprehensiveResult", ex);
                return DealResult(2, string.Format("第{0}列上龙骨计算异常", CSTLocation.TopPhotoCount));
            }
        }
        /// <summary>
        /// 记录下龙骨的拍照结果
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="pos"></param>
        /// <param name="htResult"></param>
        /// <returns></returns>
        public StateComprehensive_enum RecordBottomKeel(Hashtable htResult)
        {
            try
            {
                BaseResult result = (BaseResult)htResult[CSTMatch2];

                if (result.Num != 0)
                {
                    CSTLocation.BottomPhotoCount++;
                    if (CSTLocation.BottomPhotoCount > Protocols.KeelCol)
                    {
                        return DealResult(2, "下龙骨拍照次数大于龙骨数量，本次拍照无效");
                    }
                    result.Y_L.Sort((x, y) => -x.CompareTo(y));
                    //double deltaY = (result.Y_L[0] - result.StdY) * ParCalibWorld.V_I[g_NoCamera];
                    double deltaY = (result.Y_L[0] - Protocols.CstStdValue.DblValue2) * AMP;
                    if (deltaY < 0)
                    {
                        int i = 0;
                        while (Math.Abs(deltaY) > Protocols.ConfLayerSpacing / 2)
                        {
                            deltaY += Protocols.ConfLayerSpacing;
                            ++i;
                        }
                        if (i > 0)
                            ShowAlarm(string.Format(
                                "第{0}列下龙骨漏匹配，进行自动校正{1}层", CSTLocation.BottomPhotoCount, i));
                    }
                    //CSTLocation.BottomDev_L.Add(new Point2D(
                    //    result.DeltaX * ParCalibWorld.V_I[g_NoCamera], deltaY));
                    CSTLocation.BottomDev_L.Add(new Point2D(
                        (result.X_L[0] - Protocols.CstStdValue.DblValue1) * AMP, deltaY));

                    DealResult(1, string.Format("第{0}列下龙骨识别成功", CSTLocation.BottomPhotoCount));
                    if (CSTLocation.BottomPhotoCount == Protocols.KeelCol)
                    {
                        ShowState("龙骨拍照完成");
                        CalcCSTData();
                    }
                    return StateComprehensive_enum.True;
                }
                else
                {
                    return DealResult(2, string.Format("第{0}列下龙骨识别失败", CSTLocation.BottomPhotoCount));
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("DealComprehensiveResult", ex);
                return DealResult(2, string.Format("第{0}列下龙骨计算异常", CSTLocation.BottomPhotoCount));
            }
        }
        #endregion        

        public StateComprehensive_enum DealResult(int result, string msg)
        {
            LogicPLC.L_I.FinishPhoto(g_regClearCamera + g_regFinishPhoto, result);
            if (result == 1)
            {
                ShowState(msg);
                return StateComprehensive_enum.True;
            }
            else
            {
                ShowAlarm(msg);
                return StateComprehensive_enum.False;
            }
        }

    }
}
