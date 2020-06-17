using System;
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
    public delegate void ParFinShell_del(ParFinShell parFinShell);
    /// <summary>
    /// WinFinShell.xaml 的交互逻辑
    /// </summary>
    public partial class WinFinShell : BaseWinImageProcess
    {
        #region 窗体单实例
        private static WinFinShell g_WinFinShell = null;
        public static WinFinShell GetWinInst(out bool blNew)
        {
            blNew = false;
            try
            {
                if (g_WinFinShell == null)
                {
                    blNew = true;
                    g_WinFinShell = new WinFinShell();
                }
                return g_WinFinShell;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinFinShell", ex);
                return null;
            }
        }

        public static WinFinShell GetWinInst()
        {
            try
            {
                return g_WinFinShell;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("WinFinShell", ex);
                return null;
            }
        }
        #endregion 窗体单实例

        #region 定义
        //Class
        ParFinShell g_ParFinShell = null;
        ParFinShell g_ParFinShell_Old = null;
      
        //定义事件
        public event ParFinShell_del ParFinShell_event;
        public event FdBlStr2Action SavePar_event;//保存参数       
        #endregion 定义

        #region 初始化
        public WinFinShell()
        {
            InitializeComponent();

            //初始化控件位置
            LocationRight();
            //赋值基类控件
            g_GdLayout = this.gdLayout;
            g_UCPreProcess = uCPreProcess;//预处理
            g_UCSetROI = uCSetROI;//设置ROI
            g_UCTestRun = uCTestRun;//测试运行

            //事件注册
            LoginEvent();
        }


        /// <summary>
        /// 事件注册
        /// </summary>
        void LoginEvent()
        {
            uCTestRun.CellComprehensive_event += new CellComprehensive_del(uCTestRun_CellComprehensive_event);
        }    

        /// <summary>
        /// 初始化参数
        /// </summary>      
        public void Init(ParFinShell par, BaseUCDisplayCamera baseUCCamera, List<CellReference> cellExe_L, List<CellHObjectReference> cellHObject_L, List<CellReference> cellData_L, Hashtable htResult)
        {
            try
            {
                NameClass = "WinFinShell" + par.NameCell;

                //参数赋值
                g_ParAlgorithm = par;
                g_ParFinShell = par;
                g_ParFinShell_Old = (ParFinShell)par.Clone();
                //权限设置
                g_AuthorityCtr_L = par.g_AuthorityCtr_L;

                g_HtResult = htResult;//结果
                g_CellData_L = cellData_L;//数据单元
                g_CellExecute_L = cellExe_L;//可执行单元

                //参数初始化
                uCFinShell.Init(par, cellExe_L, cellHObject_L);

                //相机控制类  
                g_BaseUCDisplayCamera = baseUCCamera;

                //图像预处理
                uCPreProcess.Init(baseUCCamera, par.g_ParPreprocess, cellExe_L, par.IndexCell);

                //设定ROI
                uCSetROI.Init(baseUCCamera, par, cellHObject_L, cellData_L, htResult);

                //设置输入输出
                uCSetInput.Init(par.g_ParInput, cellHObject_L);
                uCOutputImageProcess.Init(par.g_ParOutput, null, cellExe_L, cellData_L, htResult);

                //基准值和结果
                baseUCStandard.Init((ParStdStraightLine)par.g_ParStd, ResultToStd, ShowStdImage);

                //设置其他
                uCSetOthers.Init(par, cellHObject_L);

                //测试运行                
                uCTestRun.Init(baseUCCamera, par, cellExe_L, cellHObject_L, htResult, uCResultStraightLine);               

                //显示参数
                ShowPar_Invoke();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
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
                if (g_ParFinShell != null)
                {
                    //触发保存此单元格参数到本地
                    if (SavePar_event(g_ParFinShell.NameCell, g_ParFinShell.TypeParent + ":" + g_ParFinShell.TypeParent))
                    {
                        btnSaveOnly.RefreshDefaultColor("保存成功", true);
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
                "相机综合设置" + g_ParFinShell.NoCamera.ToString() + g_ParFinShell.NameCell + ":M直线参数设置," + info);
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string info = "保存成功";
            try
            {
                //参数设置错误则退出
                if (!uCSetOthers.Close())
                {
                    info = "保存失败";
                    return;
                }

                if (g_ParFinShell != null)
                {
                    //触发保存此单元格参数到本地
                    if (SavePar_event(g_ParFinShell.NameCell, g_ParFinShell.TypeParent + ":" + g_ParFinShell.TypeParent))
                    {
                        btnSave.RefreshDefaultColor("保存成功", true);
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
                "相机综合设置" + g_ParFinShell.NoCamera.ToString() + g_ParFinShell.NameCell + ":M直线参数设置," + info);
            }
        }
        #endregion 保存

        #region 显示
        public override void ShowPar()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 显示

        #region 退出
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                e.Handled = true;

                if (!uCTestRun.Close())
                {
                    btnClose.RefreshDefaultColor("退出连续运行", true);
                    return;
                }

                //参数设置错误则退出
                if (!uCSetOthers.Close())
                {
                    return;
                }
                CloseWin();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        public void CloseWin()
        {
            try
            {
                uCTestRun.Close();
                ParFinShell_event(g_ParFinShell_Old);
                this.Close();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        private void BaseMetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                LogOutEvent();//注销事件

                RecoverHalWin();//显示Halcon窗体
                g_WinFinShell = null;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        //注销事件
        void LogOutEvent()
        {
            try
            {
                uCTestRun.CellComprehensive_event -= new CellComprehensive_del(uCTestRun_CellComprehensive_event);
                uCSetROI.LogoutEvent();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion 退出      
    }
}
