using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealPLC;
using System.Threading;
using System.Threading.Tasks;
using DealComprehensive;
using Common;
using BasicClass;
using System.Collections;
using DealDisplay;
using BasicDisplay;
using Camera;
using Main_EX;

namespace Main
{
    /// <summary>
    /// 处理外部触发拍照的命令,并调用DealComprehensiveResult类中的处理方法，进行处理并输出结果
    /// </summary>
    public partial class MainWindow
    {
        #region 定义
        Hashtable g_HtUCDisplayImage = new Hashtable();

        #endregion 定义

        #region 初始化
        /// <summary>
        /// 初始化控件和参数
        /// </summary>
        public override void Init_TrrigerDealResult()
        {
            try
            {
                BaseDealComprehensiveResult[] baseDealComprehensiveResults = new BaseDealComprehensiveResult[12] { 
                    DealComprehensiveResult1.D_I,
                    DealComprehensiveResult2.D_I,
                    DealComprehensiveResult3.D_I,
                    DealComprehensiveResult4.D_I,
                    DealComprehensiveResult5.D_I,
                    DealComprehensiveResult6.D_I,
                    DealComprehensiveResult7.D_I,
                    DealComprehensiveResult8.D_I,

                    DealComprehensiveResult9.D_I,
                    DealComprehensiveResult10.D_I,
                    DealComprehensiveResult11.D_I,
                    DealComprehensiveResult12.D_I
                };

                //初始化
                base.Init_TrrigerDealResult(baseDealComprehensiveResults);                
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 初始化

        ///相机响应的函数，可以重载
        #region 相机1
        /// <summary>
        /// 响应触发，调用对应相机的处理方法
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        //protected override void DealComprehensive_Camera1_event(TriggerSource_enum trigerSource_e, int i)
        //{
        //    g_MtCamera1.WaitOne();
        //    try
        //    {
        //        Pos_Cam1 = i;//拍照位置
        //        TriggerSource_e_Cam1 = trigerSource_e;            
        //        BlTriggerCam1 = true;

        //        Pos_Cam2 = i;//拍照位置
        //        TriggerSource_e_Cam2 = trigerSource_e;
        //        BlTriggerCam2 = true;

        //        Pos_Cam3 = i;//拍照位置
        //        TriggerSource_e_Cam3 = trigerSource_e;
        //        BlTriggerCam3 = true;

        //        Pos_Cam4 = i;//拍照位置
        //        TriggerSource_e_Cam4 = trigerSource_e;
        //        BlTriggerCam4 = true;

        //        Pos_Cam5 = i;//拍照位置
        //        TriggerSource_e_Cam5 = trigerSource_e;
        //        BlTriggerCam5 = true;

        //        ShowState_Hidden("相机1单线程循环触发");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.L_I.WriteError(NameClass, ex);
        //    }
        //    finally
        //    {
        //        g_MtCamera1.ReleaseMutex();
        //    }
        //}
        #endregion 相机1

        #region 相机2

        #endregion 相机2

        #region 相机3

        #endregion 相机3

        #region 相机4

        #endregion 相机4

        #region 相机5

        #endregion 相机5

        #region 相机6

        #endregion 相机6

        #region 相机7

        #endregion 相机7

        #region 相机8

        #endregion 相机8
    }
}
