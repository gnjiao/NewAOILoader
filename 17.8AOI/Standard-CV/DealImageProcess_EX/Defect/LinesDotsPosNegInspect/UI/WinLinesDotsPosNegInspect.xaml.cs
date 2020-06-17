﻿using System;
using System.Collections.Generic;
using System.Windows;
using BasicClass;
using Camera;
using BasicComprehensive;
using System.Collections;
using DealLog;
using DealAlgorithm;
using DealResult;
using DealImageProcess;

namespace DealImageProcess_EX
{
    //委托
    public delegate void ParLinesDotsPosNegInspect_del(ParLinesDotsPosNegInspect parRaisedEdge);

    /// <summary>
    /// WinLinesDotsPosNegInspect.xaml 的交互逻辑
    /// </summary>
    public partial class WinLinesDotsPosNegInspect : BaseWinImageProcess
    {
        #region 窗体单实例
        private static WinLinesDotsPosNegInspect g_WinLinesDotsPosNegInspect = null;
        public static WinLinesDotsPosNegInspect GetWinInst(ref bool blNew)
        {
            blNew = false;
            try
            {
                if (g_WinLinesDotsPosNegInspect == null)
                {
                    blNew = true;
                    g_WinLinesDotsPosNegInspect = new WinLinesDotsPosNegInspect();
                }
                return g_WinLinesDotsPosNegInspect;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinLinesDotsPosNegInspect", ex);
                return null;
            }
        }
        #endregion 窗体单实例

        #region 定义
        //Class
        ParLinesDotsPosNegInspect g_ParLinesDotsPosNegInspect = null;
        //定义事件
        public event FdBlStr2Action SavePar_event;//保存参数       
        #endregion 定义

        #region 初始化
        public WinLinesDotsPosNegInspect()
        {
            InitializeComponent();

            //初始化控件位置
            LocationRight();
            //赋值基类控件
            g_GdLayout = this.gdLayout;
            g_UCPreProcess = uCPreProcess;//预处理
            g_UCSetROI = uCSetROI;//设置ROI
            g_UCTestRun = uCTestRun;//测试运行
            //输入输出控件赋值
            g_UCSetInput = uCSetInput;
            g_UCOutputImageProcess = uCOutputImageProcess;
            g_UCTestRun = uCTestRun;
            g_BaseUCResult = uCResult;

            //事件注册
            LoginEvent();
            TcAlgorithm = tcAlgorithm;//选项卡
        }


        /// <summary>
        /// 事件注册
        /// </summary>
        void LoginEvent()
        {
            uCTestRun.CellComprehensive_event += new CellComprehensive_del(uCTestRun_CellComprehensive_event);

            //注册结果列表选择事件
            LoginEvent_UCResult(uCResult);
        }

        /// <summary>
        /// 初始化参数
        /// </summary>      
        public override string Init(BaseParImageProcess par, BaseUCDisplayCamera baseUCCamera, List<CellReference> cellExe_L, List<CellHObjectReference> cellHObject_L, List<CellReference> cellData_L, Hashtable htResult, Hashtable htCellRef_Mult, Hashtable htResult_MultC)
        {
            string nameWin = "";
            try
            {
                #region 窗体名称
                if (g_ParAlgorithm != null)
                {
                    nameWin = g_ParAlgorithm.NoCamera.ToString() + g_ParAlgorithm.NameCell;
                }
                #endregion 窗体名称

                #region 赋值
                NameClass = "WinLinesDotsPosNegInspect" + par.NameCell;

                //参数赋值
                g_ParAlgorithm = par;
                g_ParLinesDotsPosNegInspect = (ParLinesDotsPosNegInspect)par;
                g_ParAlgorithm_Old = (ParLinesDotsPosNegInspect)par.Clone();
                //权限设置
                g_AuthorityCtr_L = par.g_AuthorityCtr_L;

                g_HtResult = htResult;//结果
                g_CellData_L = cellData_L;//数据单元
                g_CellExecute_L = cellExe_L;//可执行单元
                g_CellHObject_L = cellHObject_L;//可执行图像单元
                #endregion 赋值

                //参数初始化
                uCLinesDotsPosNegInspect.Init((ParLinesDotsPosNegInspect)par, cellExe_L, cellHObject_L, htCellRef_Mult, htResult_MultC);

                //相机控制类  
                g_BaseUCDisplayCamera = baseUCCamera;

                //图像预处理
                uCPreProcess.Init(baseUCCamera, ((ParLinesDotsPosNegInspect)par).g_ParPreprocess, cellExe_L, par.IndexCell);

                //设定ROI
                uCSetROI.Init(baseUCCamera, (ParLinesDotsPosNegInspect)par, cellHObject_L, cellData_L, htResult);

                //设置输入输出
                uCSetInput.Init(par.g_ParInput, cellHObject_L);
                //uCOutputImageProcess.Init(par.g_ParOutput, par.g_BaseResult, cellExe_L, cellData_L, htResult);

                //基准值和结果
                baseUCStandard.Init((ParStdStraightLine)((ParLinesDotsPosNegInspect)par).g_ParStd, ResultToStd, ShowStdImage);

                //设置其他
                //uCSetOthers.Init(par, cellHObject_L);

                //测试运行                
                uCTestRun.Init(baseUCCamera, par, cellExe_L, cellHObject_L, htResult, uCResult);

                //显示参数
                ShowPar_Invoke();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
            return nameWin;
        }
        #endregion 初始化

        #region 保存
        /// <summary>
        /// 仅仅保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveOnly_Click(object sender, RoutedEventArgs e)
        {
            string info = "保存成功";
            try
            {
                if (g_ParLinesDotsPosNegInspect != null)
                {
                    //触发保存此单元格参数到本地
                    if (SavePar_event(g_ParLinesDotsPosNegInspect.NameCell, g_ParLinesDotsPosNegInspect.TypeParent + ":" + g_ParLinesDotsPosNegInspect.TypeParent))
                    {
                        btnSaveOnly.RefreshDefaultColor("保存成功", true);
                        g_ParAlgorithm_Old = (ParLinesDotsPosNegInspect)g_ParLinesDotsPosNegInspect.Clone();
                    }
                    else
                    {
                        btnSaveOnly.RefreshDefaultColor("保存失败", false);
                        info = "保存失败";
                    }
                }
                else
                {
                    btnSaveOnly.RefreshDefaultColor("保存失败", false);
                    info = "保存失败";
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                btnSaveOnly.RefreshDefaultColor("保存失败", false);
            }
            finally
            {
                //按钮日志
                FunLogButton.P_I.AddInfo("btnSave保存",
                "相机综合设置" + g_ParLinesDotsPosNegInspect.NoCamera.ToString() + g_ParLinesDotsPosNegInspect.NameCell + ":点线异物检测设置," + info);
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string info = "保存成功";
            try
            {
                //参数设置错误则退出
                if (!uCTestRun.Close())
                {
                    btnClose.RefreshDefaultColor("退出连续运行", true);
                    return;
                }

                if (g_ParLinesDotsPosNegInspect != null)
                {
                    //触发保存此单元格参数到本地
                    if (SavePar_event(g_ParLinesDotsPosNegInspect.NameCell, g_ParLinesDotsPosNegInspect.TypeParent + ":" + g_ParLinesDotsPosNegInspect.TypeParent))
                    {
                        btnSave.RefreshDefaultColor("保存成功", true);
                        g_ParAlgorithm_Old = (ParLinesDotsPosNegInspect)g_ParLinesDotsPosNegInspect.Clone();
                        Close700_Task(); //延迟退出
                    }
                    else
                    {
                        btnSave.RefreshDefaultColor("保存失败", false);
                        info = "保存失败";
                    }
                }
                else
                {
                    btnSave.RefreshDefaultColor("保存失败", false);
                    info = "保存失败";
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                btnSave.RefreshDefaultColor("保存失败", false);
            }
            finally
            {
                //按钮日志
                FunLogButton.P_I.AddInfo("btnSave保存&退出",
                "相机综合设置" + g_ParLinesDotsPosNegInspect.NoCamera.ToString() + g_ParLinesDotsPosNegInspect.NameCell + ":点线异物检测设置," + info);
            }
        }
        #endregion 保存

        #region 显示
        public override void ShowPar()
        {
            try
            {
                ShowTitle("点线异物检测");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示

        #region 退出
       
        #endregion 退出


    }
}
