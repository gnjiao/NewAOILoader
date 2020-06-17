using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealComInterface;
using BasicClass;

namespace DealRobot
{
    public class ParInterfaceEthernetRobot : ParInterfaceEthernet
    {
        private int RobotIndex = 1;
        public ParInterfaceEthernetRobot()
        {
            RobotIndex = 1;
        }
        public ParInterfaceEthernetRobot(int index)
        {
            RobotIndex = index;
        }

        #region 定义
        #region Path
        public override string PathSave
        {
            get
            {
                switch (RobotIndex)
                {
                    case 1:
                        return ParPathRoot.PathRoot + "Store\\Robot\\TypeRobot.ini";
                    case 2:
                        return ParPathRoot.PathRoot + "Store\\Robot\\TypeRobot2.ini";
                    default:
                        return ParPathRoot.PathRoot + "Store\\Robot\\TypeRobot.ini";
                }
            }
        }
        #endregion Path
        #endregion 定义
    }
}
