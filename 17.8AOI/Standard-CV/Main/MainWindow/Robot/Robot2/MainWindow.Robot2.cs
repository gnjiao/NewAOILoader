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
       
        bool BlRobot2ToSafe = false;//通知机器人去安全位置

        #endregion 定义    

        #region 超时
        protected override void L_I_Delay2_event(int i)
        {

        }
        #endregion 超时      

        #region 机器人HomeThrow
        protected override void L_I_Robot2Reset_event(int i)
        {
            try
            {
                ShowState("机器人复位完成");
                MainCom.M_I.ResetRobot2 = true;
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
        protected override void L_I_Robot2Home_event(int i)
        {
            try
            {
                ShowState("机器人回到Home点");
                MainCom.M_I.HomeRobot2 = true;

                //通知机器人去安全位置
                if (BlRobot2ToSafe)
                {
                    SendRobot2Safe();
                    return;
                }

                int enable=(int)LogicPLC.L_I.ReadRegData1(37);
                //机器人复位后重新拍照
                if (enable==1)
                {
                    ShowState("机器人触发精确定位拍照");
                    Robot2Photo();
                }
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
        protected override void L_I_Robot2Throw_event(int i)
        {
            try
            {
                ShowState("机器人进行抛料");
                MainCom.M_I.HomeRobot2 = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 机器人HomeThrow

        #region 通知机器人去安全位置
        void SendRobot2Safe()
        {
            try
            {
                if (BlRobot2ToSafe)
                {
                    BlRobot2ToSafe = false;
                    LogicRobot2.L_I.WriteRobotCMD("10003");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }
        #endregion 通知机器人去安全位置

        #region 通知机器人去安全位置
        void Robot2Photo()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
        #endregion 通知机器人去安全位置
       
        
    }
}
