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

namespace Main
{
    partial class DealComprehensiveResult9 : BaseDealComprehensiveResult_Main
    {
        #region 静态类实例
        public static DealComprehensiveResult9 D_I = new DealComprehensiveResult9();
        #endregion 静态类实例

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public DealComprehensiveResult9()
        {
            try
            {
                base.NameClass = "DealComprehensiveResult9";
                //图像处理参数
                base.g_BaseParComprehensive = ParComprehensive9.P_I;
                base.g_BaseDealComprehensive = DealComprehensive9.D_I;
                base.g_DealComprehensiveBase = DealComprehensive9.D_I;

                //相机序号
                g_NoCamera = 9;
                NoCamera_e = NoCamera_enum.Camera9;

                //初始化PLC寄存器
                InitPLCReg();

                //判断并设置显示的独立线程
                InitDisplay_Task();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("DealComprehensiveResult9", ex);
            }
        }

        /// <summary>
        /// 初始化PLC寄存器
        /// </summary>
        void InitPLCReg()
        {
            try
            {
                if (ParSetPLC.P_I.TypePLC_e != TypePLC_enum.Null)//三菱PLC
                {
                    //base.g_regClearCamera = ParSetPLC.P_I.regClearCamera9;
                    //base.g_regFinishPhoto = ParSetPLC.P_I.regFinishPhoto_Camera9;
                    //base.g_regData_L.Add(ParSetPLC.P_I.regDataX_Camera9);
                    //base.g_regData_L.Add(ParSetPLC.P_I.regDataY_Camera9);
                    //base.g_regData_L.Add(ParSetPLC.P_I.regDataZ_Camera9);
                    //base.g_regData_L.Add(ParSetPLC.P_I.regDataR_Jamera9);
                    //base.g_regFinishData = ParSetPLC.P_I.regFinsihData_Camera9;
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化
    }
}
