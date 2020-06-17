using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealComInterface;
using BasicClass;
using Common;
using System.Threading;
using System.Threading.Tasks;
using DealConfigFile;
using System.Windows;
using Main_EX;

namespace Main
{
    public partial class MainWindow
    {
        #region 定义
        bool g_blConnectAckReceived;
        bool g_blNewModelAckReceived;
        bool g_blNewModelResult;
        #endregion 定义

        #region 初始化
        //事件注册
        void LoginEvent_ComInterface()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }

        #endregion  初始化

        #region 通用接口
        //打开通用接口       
        void Init_ComInterface()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }

        #endregion 通用接口

        #region 握手
        void ComInterfaceShake()
        {
            //发送连接成功的确认信号
            try
            {
                
            }
            catch (Exception)
            {
                ShowState("通用端口握手出错！");
            }
        }

        //判断是否收到运控卡的确认信号
        void ReceiveConnectAck()
        {
            g_blConnectAckReceived = true;
        }

        #endregion 握手

        #region 换型处理
        void LogicComInterface_Inst_NewModel_event(string No, string Name)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }

        //上位机换型则触发，
        bool ComInterfaceNewModel()
        {
            try
            {
               
                return true;
            }
            catch
            {
                ShowState("通用控制器换型出错！");
                return false;
            }
        }

        void LogicComInterface_Inst_NewModelResult_event(string State)
        {
        }

        #endregion 换型处理
    }
}
