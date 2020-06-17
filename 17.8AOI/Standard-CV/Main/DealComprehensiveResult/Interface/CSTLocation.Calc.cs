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
        const string ReserveDigits = "f3";
        const string CSTMatch1 = @"C2";
        const string CSTMatch2 = @"C2";
        const string CSTMatch3 = @"C2";
        public double AMP => ParCalibWorld.V_I[g_NoCamera];
        #region 卡塞计算
        /// <summary>
        /// 计算卡塞的的龙骨位置
        /// </summary>
        private bool CalcCSTData()
        {
            try
            {
                ShowState("卡塞数据计算并校验中...");
                Directory.CreateDirectory(CSTLocation.root);
                GetStdInsertPos();//写入所有的点的插篮位置
                WriteIni_StdInsert();

                if (CalcDevHeight() && CalcDevX())
                {
                    VerifyCSTData();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        /// <summary>
        /// 获取每列插栏基准位
        /// </summary>
        private void GetStdInsertPos()
        {
            try
            {
                double stdPos = LogicPLC.L_I.ReadRegData2((int)DataRegister2.StdCSTPos1 + CSTLocation.CurrentCstNo - 1);
                CSTLocation.StdInsert_L = CSTLocation.CreateKeelStdPos(
                    stdPos, Protocols.ConfCSTCol, Protocols.ConfKeelInterval,
                    Protocols.ConfCol1Interval, Protocols.DirInsert);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 卡塞高度补偿计算
        /// </summary>
        /// <returns></returns>
        private bool CalcDevHeight()
        {
            try
            {
                CSTLocation.HeightDev_L = CSTLocation.CalcHeightDeviation(
                    Protocols.ConfCSTCol, CSTLocation.TopDev_L, Protocols.DirZ);
                WriteIni_HeightDev();
                CSTLocation.LayerSpacing_L = CSTLocation.CalcLayerSpacing(
                    Protocols.ConfCSTCol, Protocols.ConfCSTRow, Protocols.ConfLayerSpacing,
                    CSTLocation.TopDev_L, CSTLocation.BottomDev_L);
                WriteIni_LayerSpacing();
                CSTLocation.KeelHeightDev_L = CSTLocation.CalcKeelHeightDeviation(
                    Protocols.ConfCSTCol, CSTLocation.TopDev_L, CSTLocation.BottomDev_L);
                WriteIni_KeelHeightDev();

                LogicPLC.L_I.WriteRegData3((int)DataRegister3.InsertComZ1 + (CSTLocation.CurrentCstNo - 1) * 20,
                    CSTLocation.HeightDev_L.Count, CSTLocation.HeightDev_L.ToArray());
                LogicPLC.L_I.WriteRegData3((int)DataRegister3.KeelSpacing1 + (CSTLocation.CurrentCstNo - 1) * 20,
                    CSTLocation.LayerSpacing_L.Count, CSTLocation.LayerSpacing_L.ToArray());

                //if (Protocols.IfRecordData)
                //{
                RecordCSTData("HeightDev", CSTLocation.HeightDev_L);
                RecordCSTData("LayerSpacing", CSTLocation.LayerSpacing_L);
                RecordCSTData("KeelHeight", CSTLocation.KeelHeightDev_L);
                //}

                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        /// <summary>
        /// 计算最终插栏的X偏差
        /// </summary>
        /// <returns></returns>
        private bool CalcDevX()
        {
            try
            {
                //计算龙骨单列偏差        
                CSTLocation.KeelDev_L = CSTLocation.CalcKeelDeviation(
                    Protocols.KeelCol, Protocols.ConfCSTRow, CSTLocation.TopDev_L, CSTLocation.BottomDev_L);
                WriteIni_KeelDev();
                //计算卡塞单列偏差
                CSTLocation.ColDev_L = CSTLocation.CalcColDeviation(
                    Protocols.ConfCSTCol, CSTLocation.TopDev_L);
                WriteIni_ColDev();
                //偏差相加
                CSTLocation.InsertDev_L = CSTLocation.MixDeviation(
                    Protocols.ConfCSTCol, Protocols.ConfCSTRow,
                    CSTLocation.KeelDev_L, CSTLocation.ColDev_L, Protocols.DirPhoto, Protocols.CstIsMirrorX);
                WriteIni_InsertDev();
                CSTLocation.KeelSpacingDev_L = CSTLocation.CalcKeelSpacingDeviation(
                    Protocols.ConfCSTCol, CSTLocation.TopDev_L, CSTLocation.BottomDev_L);
                WriteIni_KeelSpacingDev();

                //if (ModelParams.IfRecordData)
                //{
                    RecordCSTData("KeelDev", CSTLocation.KeelDev_L);
                    RecordCSTData("ColDev", CSTLocation.ColDev_L);
                    RecordCSTData("InsertDev", CSTLocation.InsertDev_L);
                    RecordCSTData("KeelSpacing", CSTLocation.KeelSpacingDev_L);
                //}

                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        #endregion

        #region 清空卡塞拍照数据数据
        //在切换工位时已经调用
        #endregion 清空卡塞拍照数据数据

        #region 校验插栏数据
        /// <summary>
        /// 校验插栏数据是否在限定阈值内
        /// </summary>
        public void VerifyCSTData()
        {
            try
            {
                bool blError = false;

                blError |= VerifyInsertDev();
                blError |= VerifyKeelSpacing();
                blError |= VerifyHeightDev();
                blError |= VerifyKeelHeightDev();
                blError |= VerifyLayerSpacing();

                if (blError)
                {
                    ShowState("通知PLC卡塞不符合规格");
                    LogicPLC.L_I.WriteRegData1((int)DataRegister1.PCAlarm, (int)PCAlarm_Enum.卡塞计算失败);
                }
                else
                {
                    ShowState("卡塞各项参数符合规格要求，开始插栏");
                }
            }
            catch (Exception ex)
            {
                ShowAlarm("卡塞数据校验失败");
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 校验插栏偏差，分上下龙骨
        /// </summary>
        /// <returns></returns>
        bool VerifyInsertDev()
        {
            bool blError = false;
            try
            {
                //校验插栏偏差
                for (int i = 0; i < CSTLocation.InsertDev_L.Count; ++i)
                {
                    List<double> item = CSTLocation.InsertDev_L[i];
                    //数据显示
                    ShowState(string.Format("卡塞第{0}列上端偏差{1}mm,阈值{2}",
                        i + 1, item[0].ToString(ReserveDigits), Protocols.CSTThread_DevX));
                    ShowState(string.Format("卡塞第{0}列下端偏差{1}mm,阈值{2}",
                        i + 1, item[item.Count - 1].ToString(ReserveDigits), Protocols.CSTThread_DevX));
                    //校验部分
                    if (Math.Abs(item[0]) > Protocols.CSTThread_DevX)
                    {
                        blError = true;
                        ShowAlarm(string.Format("卡塞第{0}列上端偏差{1}超阈值{2}",
                            i + 1, item[0].ToString(ReserveDigits), Protocols.CSTThread_DevX));
                    }
                    if (Math.Abs(item[item.Count - 1]) > Protocols.CSTThread_DevX)
                    {
                        blError = true;
                        ShowAlarm(string.Format("卡塞第{0}列下端偏差{1}超阈值{2}",
                            i + 1, item[0].ToString(ReserveDigits), Protocols.CSTThread_DevX));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                ShowAlarm("龙骨偏差校验异常,详细信息见异常信息日志");
                blError = true;
            }
            return blError;
        }
        /// <summary>
        /// 校验龙骨间距，分上下龙骨
        /// </summary>
        /// <returns></returns>
        bool VerifyKeelSpacing()
        {
            bool blError = false;
            try
            {
                //校验龙骨间距
                for (int i = 0; i < CSTLocation.KeelSpacingDev_L.Count; ++i)
                {
                    Point2D item = CSTLocation.KeelSpacingDev_L[i];
                    //数据显示
                    ShowState(string.Format("卡塞第{0}列上龙骨间距与配方相差{1}mm,阈值{2}",
                        i + 1, item.DblValue1.ToString(ReserveDigits), Protocols.CSTThread_KeelSpacing));
                    ShowState(string.Format("卡塞第{0}列下龙骨间距与配方相差{1}mm,阈值{2}",
                        i + 1, item.DblValue2.ToString(ReserveDigits), Protocols.CSTThread_KeelSpacing));
                    //校验部分
                    if (item.DblValue1 > Protocols.CSTThread_KeelSpacing)
                    {
                        blError = true;
                        ShowAlarm(string.Format("卡塞第{0}列上龙骨间距较配方小{1}mm,超阈值{2}",
                            i + 1, item.DblValue1.ToString(ReserveDigits), Protocols.CSTThread_KeelSpacing));
                    }
                    else if (item.DblValue1 < -Protocols.CSTThread_KeelSpacing)
                    {
                        blError = true;
                        ShowAlarm(string.Format("卡塞第{0}列上龙骨间距较配方大{1}mm,超阈值{2}",
                            i + 1, item.DblValue1.ToString(ReserveDigits), Protocols.CSTThread_KeelSpacing));
                    }

                    if (item.DblValue2 > Protocols.CSTThread_KeelSpacing)
                    {
                        blError = true;
                        ShowAlarm(string.Format("卡塞第{0}列下龙骨间距较配方小{1}mm,超阈值{2}",
                            i + 1, item.DblValue2.ToString(ReserveDigits), Protocols.CSTThread_KeelSpacing));
                    }
                    else if (item.DblValue2 < -Protocols.CSTThread_KeelSpacing)
                    {
                        blError = true;
                        ShowAlarm(string.Format("卡塞第{0}列下龙骨间距较配方大{1}mm,超阈值{2}",
                            i + 1, item.DblValue2.ToString(ReserveDigits), Protocols.CSTThread_KeelSpacing));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                ShowAlarm("龙骨间距校验异常,详细信息见异常信息日志");
                blError = true;
            }

            return blError;
        }
        /// <summary>
        /// 校验插栏高度补偿
        /// </summary>
        /// <returns></returns>
        bool VerifyHeightDev()
        {
            bool blError = false;
            try
            {
                //校验高度补偿
                for (int i = 0; i < CSTLocation.HeightDev_L.Count; ++i)
                {
                    //数据显示
                    ShowState(string.Format("卡塞第{0}列高度偏差{1},阈值{2}",
                        i + 1, CSTLocation.HeightDev_L[i].ToString(ReserveDigits), Protocols.CSTThread_HeightDev));
                    //校验部分
                    if (Math.Abs(CSTLocation.HeightDev_L[i]) > Protocols.CSTThread_HeightDev)
                    {
                        blError = true;
                        ShowAlarm(string.Format("卡塞第{0}列高度偏差{1},超阈值{2}",
                            i + 1, CSTLocation.HeightDev_L[i].ToString(ReserveDigits), Protocols.CSTThread_HeightDev));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                ShowAlarm("龙骨高度补偿校验异常,详细信息见异常信息日志");
                blError = true;
            }
            return blError;
        }
        /// <summary>
        /// 校验左右龙骨高度差
        /// </summary>
        /// <returns></returns>
        bool VerifyKeelHeightDev()
        {
            bool blError = false;
            try
            {
                //校验左右龙骨高度差
                for (int i = 0; i < CSTLocation.KeelHeightDev_L.Count; ++i)
                {
                    //数据显示
                    ShowState(string.Format("卡塞第{0}列上龙骨左右龙骨高度相差{1}mm,阈值{2}",
                        i + 1, CSTLocation.KeelHeightDev_L[i].DblValue1.ToString(ReserveDigits),
                        Protocols.CSTThread_KeelHeight));
                    //校验部分
                    if (Math.Abs(CSTLocation.KeelHeightDev_L[i].DblValue1) > Protocols.CSTThread_KeelHeight)
                    {
                        blError = true;
                        ShowAlarm(string.Format("卡塞第{0}列上龙骨左右龙骨高度相差{1}mm，超过阈值{2}",
                            i + 1, CSTLocation.KeelHeightDev_L[i].DblValue1, Protocols.CSTThread_KeelHeight));
                    }
                    if (Math.Abs(CSTLocation.KeelHeightDev_L[i].DblValue2) > Protocols.CSTThread_KeelHeight)
                    {
                        blError = true;
                        ShowAlarm(string.Format("卡塞第{0}列下龙骨左右龙骨高度相差{1}mm，超过阈值{2}",
                            i + 1, CSTLocation.KeelHeightDev_L[i].DblValue2, Protocols.CSTThread_KeelHeight));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                ShowAlarm("左右龙骨高度差校验异常,详细信息见异常信息日志");
                blError = true;
            }
            return blError;
        }
        /// <summary>
        /// 校验龙骨层间距
        /// </summary>
        /// <returns></returns>
        bool VerifyLayerSpacing()
        {
            bool blError = false;
            try
            {
                //校验层间距
                for (int i = 0; i < CSTLocation.LayerSpacing_L.Count; ++i)
                {
                    ShowState(string.Format("卡塞第{0}列层间距{1},阈值{2}",
                            i + 1, CSTLocation.LayerSpacing_L[i].ToString(ReserveDigits), Protocols.CSTThread_LayerSpacing));
                    if (Math.Abs(CSTLocation.LayerSpacing_L[i] - Protocols.ConfLayerSpacing) >
                        Protocols.CSTThread_LayerSpacing)
                    {
                        blError = true;
                        ShowAlarm(string.Format("卡塞第{0}列层间距{1}超阈值{2}",
                            i + 1, CSTLocation.LayerSpacing_L[i].ToString(ReserveDigits), Protocols.CSTThread_HeightDev));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                ShowAlarm("龙骨层间距校验异常,详细信息见异常信息日志");
                blError = true;
            }
            return blError;
        }
        #endregion
    }
}
