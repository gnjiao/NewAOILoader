using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DealRobot
{
    public partial class ParLogicRobot2
    {
        #region 静态类实例
        public static ParLogicRobot2 P_I = new ParLogicRobot2();
        #endregion 静态类实例

        #region 定义
        
        //bool       
        public bool isReadOK = false;//表示机器人读取数据完成     
        public bool blStartRead = false;//开始读取数据

        //string       
        public string strTrrigerRobot="";        
    
        //enum
        public StatePortRobot_enum StatePortRobot_e = StatePortRobot_enum.Wait;//服务器默认为等待状态
        
        #endregion 定义
    }

   
}
