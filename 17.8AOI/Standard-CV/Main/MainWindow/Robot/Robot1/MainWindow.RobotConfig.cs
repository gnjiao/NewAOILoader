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
        public override void ConfigRobot_Task()
        {
            try
            {
                if (ParSetRobot.P_I.TypeRobot_e == TypeRobot_enum.Null)
                {
                    return;
                }
                if (RegeditRobot.R_I.BlOffLineRobot)
                {
                    return;
                }
                #region 清空旧的参数
                LogicRobot.L_I.ParRobotCom_L.Clear();
                LogicRobot.L_I.ParRobot1_L.Clear();
                LogicRobot.L_I.ParRobot2_L.Clear();
                LogicRobot.L_I.ParRobot3_L.Clear();
                LogicRobot.L_I.ParRobot4_L.Clear();

                LogicRobot.L_I.ParRobotCom_P4L.Clear();
                LogicRobot.L_I.ParRobot1_P4L.Clear();
                LogicRobot.L_I.ParRobot2_P4L.Clear();
                LogicRobot.L_I.ParRobot3_P4L.Clear();
                LogicRobot.L_I.ParRobot4_P4L.Clear();
                #endregion 清空旧的参数

                //StationDataManager.StationDataMngr.read_station_data();
                //for (int i = 0; i < StationDataManager.StationDataMngr.PlacePos_L.Count; ++i)
                //{
                //    StationDataManager.StationDataMngr.PlacePos_L[i].DblValue4 = Protocols.RobotAxisU_PlaceToAOI[0];
                //}

                ShowState("发送取片位置：" + Protocols.BotPickPos);
                LogicRobot.L_I.ParRobotCom_P4L.Add(Protocols.BotPickPos);
                ShowState("发送拍照位置1：" + Protocols.StdBotPrecisePos);
                LogicRobot.L_I.ParRobotCom_P4L.Add(Protocols.StdBotPrecisePos);
                ShowState("发送下游位置：" + Protocols.BotDownPos);
                LogicRobot.L_I.ParRobotCom_P4L.Add(Protocols.BotDownPos);
                //ShowState("发送工位1位置：" + StationDataManager.StationDataMngr.PlacePos_L[0]);
                //LogicRobot.L_I.ParRobotCom_P4L.Add(StationDataManager.StationDataMngr.PlacePos_L[0]);
                //ShowState("发送工位2位置：" + StationDataManager.StationDataMngr.PlacePos_L[1]);
                //LogicRobot.L_I.ParRobotCom_P4L.Add(StationDataManager.StationDataMngr.PlacePos_L[1]);
                //ShowState("发送工位3位置：" + StationDataManager.StationDataMngr.PlacePos_L[2]);
                //LogicRobot.L_I.ParRobotCom_P4L.Add(StationDataManager.StationDataMngr.PlacePos_L[2]);
                //ShowState("发送工位4位置：" + StationDataManager.StationDataMngr.PlacePos_L[3]);
                //LogicRobot.L_I.ParRobotCom_P4L.Add(StationDataManager.StationDataMngr.PlacePos_L[3]);
                var data = Station.StationService.GetInstance().GetData(1);
                Point4D pt = new Point4D(data.StdX, data.StdY, data.StdZ, Protocols.RobotAxisU_PlaceToAOI[0]);
                ShowState("发送工位1位置：" + pt);
                LogicRobot.L_I.ParRobotCom_P4L.Add(pt);
                data = Station.StationService.GetInstance().GetData(2);
                pt = new Point4D(data.StdX, data.StdY, data.StdZ, Protocols.RobotAxisU_PlaceToAOI[1]);                
                ShowState("发送工位2位置：" + pt);
                LogicRobot.L_I.ParRobotCom_P4L.Add(pt);
                data = Station.StationService.GetInstance().GetData(3);
                pt = new Point4D(data.StdX, data.StdY, data.StdZ, Protocols.RobotAxisU_PlaceToAOI[2]);
                ShowState("发送工位3位置：" + pt);
                LogicRobot.L_I.ParRobotCom_P4L.Add(pt);
                data = Station.StationService.GetInstance().GetData(4);
                pt = new Point4D(data.StdX, data.StdY, data.StdZ, Protocols.RobotAxisU_PlaceToAOI[3]);
                ShowState("发送工位4位置：" + pt);
                LogicRobot.L_I.ParRobotCom_P4L.Add(pt);

                //发送参数
                Task task = new Task(LogicRobot.L_I.WriteConfigRobot);
                task.Start();
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }
       
    }
}
