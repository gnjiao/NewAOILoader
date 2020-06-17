using BasicClass;
using DealFile;
using ModulePackage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Main
{
    public partial class BaseDealComprehensiveResult_Main
    {
        #region 加载本地插栏数据
        /// <summary>
        /// 加载本地的卡塞数据，只读取插栏基准位，和所有位置的偏差
        /// </summary>
        public static void LoadCstData()
        {
            try
            {
                //清空数据
                CSTLocation.ClearCstData();
                //加载插栏基准位
                ReadIni_DblList("StdInsert", "Col", CSTLocation.Path_StdInsert_INI, out CSTLocation.StdInsert_L);
                //加载插栏偏差
                int intNum = IniFile.I_I.ReadIniInt("CST", "Num", CSTLocation.Path_InsertDev_INI);
                for (int i = 0; i < intNum; i++)
                {
                    //加1是为了从第一列开始记录，方便人员理解和查看
                    ReadIni_DblList("Col" + (i + 1).ToString(), "Row", CSTLocation.Path_InsertDev_INI, out List<double> p_L);
                    CSTLocation.InsertDev_L.Add(p_L);
                }

                if (CSTLocation.StdInsert_L.Count != Protocols.ConfCSTCol ||
                    CSTLocation.InsertDev_L.Count != Protocols.ConfCSTCol)
                {
                    ShowAlarm("插栏数据加载失败，请重新上卡塞");
                }
                else
                {
                    ShowState("插栏数据加载成功！");
                }
            }
            catch (Exception ex)
            {
                ShowAlarm("插栏数据加载失败，请重新上卡塞");
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        #endregion

        #region 记录卡塞数据
        /// <summary>
        /// 记录插栏偏差
        /// </summary>
        /// <returns></returns>
        public bool WriteIni_InsertDev()
        {
            try
            {
                if (File.Exists(CSTLocation.Path_InsertDev_INI))
                {
                    File.Delete(CSTLocation.Path_InsertDev_INI);
                }

                IniFile.I_I.WriteIni("CST", "Num", CSTLocation.InsertDev_L.Count.ToString(), CSTLocation.Path_InsertDev_INI);
                for (int i = 0; i < CSTLocation.InsertDev_L.Count; ++i)
                    WriteIni_DblList(@"Col" + (i + 1).ToString(), "Row", CSTLocation.Path_InsertDev_INI, CSTLocation.InsertDev_L[i]);
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        /// <summary>
        /// 记录龙骨间距和配方的偏差
        /// </summary>
        /// <returns></returns>
        public bool WriteIni_KeelSpacingDev()
        {
            try
            {
                if (File.Exists(CSTLocation.Path_KeelSpacingDev_INI))
                {
                    File.Delete(CSTLocation.Path_KeelSpacingDev_INI);
                }
                WriteIni_Pt2List("KeelHeightDev", "Col", CSTLocation.Path_KeelSpacingDev_INI, CSTLocation.KeelSpacingDev_L);
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        /// <summary>
        /// 记录高度补偿
        /// </summary>
        /// <returns></returns>
        public bool WriteIni_HeightDev()
        {
            try
            {
                if (File.Exists(CSTLocation.Path_HeightDev_INI))
                {
                    File.Delete(CSTLocation.Path_HeightDev_INI);
                }
                WriteIni_DblList("HeightDev", "Col", CSTLocation.Path_HeightDev_INI, CSTLocation.HeightDev_L);
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        /// <summary>
        /// 记录左右两列龙骨的高度差
        /// </summary>
        /// <returns></returns>
        public bool WriteIni_KeelHeightDev()
        {
            try
            {
                if (File.Exists(CSTLocation.Path_KeelHeightDev_INI))
                {
                    File.Delete(CSTLocation.Path_KeelHeightDev_INI);
                }
                WriteIni_Pt2List("KeelHeightDev", "Col", CSTLocation.Path_KeelHeightDev_INI, CSTLocation.KeelHeightDev_L);
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        /// <summary>
        /// 记录卡塞每列补偿
        /// </summary>
        /// <returns></returns>
        public bool WriteIni_ColDev()
        {
            try
            {
                if (File.Exists(CSTLocation.Path_ColDev_INI))
                {
                    File.Delete(CSTLocation.Path_ColDev_INI);
                }
                WriteIni_DblList("ColDev", "Col", CSTLocation.Path_ColDev_INI, CSTLocation.ColDev_L);
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        /// <summary>
        /// 记录龙骨每列补偿
        /// </summary>
        /// <returns></returns>
        public bool WriteIni_KeelDev()
        {
            try
            {
                if (File.Exists(CSTLocation.Path_KeelDev_INI))
                {
                    File.Delete(CSTLocation.Path_KeelDev_INI);
                }
                for (int i = 0; i < CSTLocation.KeelDev_L.Count; ++i)
                    WriteIni_DblList("Col" + i, "Row", CSTLocation.Path_KeelDev_INI, CSTLocation.KeelDev_L[i]);
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        /// <summary>
        /// 记录龙骨层间距
        /// </summary>
        /// <returns></returns>
        public bool WriteIni_LayerSpacing()
        {
            try
            {
                if (File.Exists(CSTLocation.Path_LayerSpacing_INI))
                {
                    File.Delete(CSTLocation.Path_LayerSpacing_INI);
                }
                WriteIni_DblList("LayerSpacing", "Col", CSTLocation.Path_LayerSpacing_INI, CSTLocation.LayerSpacing_L);
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        /// <summary>
        /// 记录插栏基准位
        /// </summary>
        public void WriteIni_StdInsert()
        {
            try
            {
                if (File.Exists(CSTLocation.Path_StdInsert_INI))
                {
                    File.Delete(CSTLocation.Path_StdInsert_INI);
                }
                WriteIni_DblList("StdInsert", "Col", CSTLocation.Path_StdInsert_INI, CSTLocation.StdInsert_L);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion
    }
}
