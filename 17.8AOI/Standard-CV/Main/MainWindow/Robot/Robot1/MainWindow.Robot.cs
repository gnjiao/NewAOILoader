using System;
using System.Threading.Tasks;
using DealPLC;
using DealRobot;
using System.Threading;
using BasicClass;
using DealComprehensive;
using Main_EX;

namespace Main
{
    partial class MainWindow
    {
        #region 定义
        bool BlRobotToSafe = false;//通知机器人去安全位置
        #endregion 定义

        #region 超时
        /// <summary>
        /// 机器人接收PLC指令超时
        /// </summary>
        /// <param name="i"></param>
        protected override void L_I_Delay_event(int i)
        {

        }
        #endregion 超时

        #region 机器人HomeThrow
        /// <summary>
        /// 机器人复位完成
        /// </summary>
        /// <param name="i"></param>
        protected override void L_I_RobotReset_event(int i)
        {
            try
            {
                ShowState("机器人复位完成");
                MainCom.M_I.ResetRobot = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        /// <summary>
        /// 机器人回到Home点
        /// </summary>
        /// <param name="i"></param>
        protected override void L_I_RobotHome_event(int i)
        {
            try
            {
                ShowState("机器人回到Home点");
                MainCom.M_I.HomeRobot = true;               
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 抛料
        /// </summary>
        /// <param name="i"></param>
        protected override void L_I_RobotThrow_event(int i)
        {
            try
            {
                ShowState("机器人进行抛料");
                MainCom.M_I.HomeRobot = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 机器人HomeThrow

        #region 通知机器人去安全位置
        /// <summary>
        /// 通知机器人去安全位置
        /// </summary>
        void SendRobotSafe()
        {
            try
            {
                if (BlRobotToSafe)
                {
                    BlRobotToSafe = false;
                    LogicRobot.L_I.WriteRobotCMD("10003");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 通知机器人去安全位置

    }
}
