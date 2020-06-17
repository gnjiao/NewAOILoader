using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealRobot;
using System.Threading;
using System.Threading.Tasks;
using BasicClass;
using DealLog;
using DealPLC;

namespace Main
{
    partial class MainWindow
    {
        /// <summary>
        /// 给机器人发送配置参数
        /// </summary>
        public void ConfigRobot2_Task()
        {
            try
            {
                if (ParSetRobot2.P_I.TypeRobot_e == TypeRobot_enum.Null)
                {
                    return;
                }
                if (RegeditRobot.R_I.BlOffLineRobot)
                {
                    return;
                }
                #region 清空旧的参数
                LogicRobot2.L_I.ParRobotCom_L.Clear();
                LogicRobot2.L_I.ParRobot1_L.Clear();
                LogicRobot2.L_I.ParRobot2_L.Clear();
                LogicRobot2.L_I.ParRobot3_L.Clear();
                LogicRobot2.L_I.ParRobot4_L.Clear();

                LogicRobot2.L_I.ParRobotCom_P4L.Clear();
                LogicRobot2.L_I.ParRobot1_P4L.Clear();
                LogicRobot2.L_I.ParRobot2_P4L.Clear();
                LogicRobot2.L_I.ParRobot3_P4L.Clear();
                LogicRobot2.L_I.ParRobot4_P4L.Clear();
                #endregion 清空旧的参数

              
                //发送参数
                Task task = new Task(LogicRobot2.L_I.WriteConfigRobot);
                task.Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }       

    }
}
