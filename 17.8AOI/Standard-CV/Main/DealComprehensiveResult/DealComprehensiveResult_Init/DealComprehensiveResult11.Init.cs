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
    partial class DealComprehensiveResult11: BaseDealComprehensiveResult_Main
    {
        #region 静态类实例
        public static DealComprehensiveResult11 D_I = new DealComprehensiveResult11();
        #endregion 静态类实例

        #region 初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        public DealComprehensiveResult11()
        {
            try
            {
                base.NameClass = "DealComprehensiveResult11";
                //图像处理参数
                base.g_BaseParComprehensive = ParComprehensive11.P_I;
                base.g_BaseDealComprehensive = DealComprehensive11.D_I;
                base.g_DealComprehensiveBase = DealComprehensive11.D_I;

                //相机序号
                g_NoCamera = 11;
                NoCamera_e = NoCamera_enum.Camera11;

                //初始化PLC寄存器
                InitPLCReg();

                //判断并设置显示的独立线程
                InitDisplay_Task();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("DealComprehensiveResult11", ex);
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
                    //base.g_regClearCamera = ParSetPLC.P_I.regClearCamera11;
                    //base.g_regFinishPhoto = ParSetPLC.P_I.regFinishPhoto_Camera11;
                    //base.g_regData_L.Add(ParSetPLC.P_I.regDataX_Camera11);
                    //base.g_regData_L.Add(ParSetPLC.P_I.regDataY_Camera11);
                    //base.g_regData_L.Add(ParSetPLC.P_I.regDataZ_Camera11);
                    //base.g_regData_L.Add(ParSetPLC.P_I.regDataR_Jamera11);
                    //base.g_regFinishData = ParSetPLC.P_I.regFinsihData_Camera11;
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
