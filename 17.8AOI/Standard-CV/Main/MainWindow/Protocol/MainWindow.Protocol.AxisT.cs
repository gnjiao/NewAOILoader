using DealConfigFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    partial class Protocols
    {

        #region robot
        /// <summary>
        /// 机器人从上游平台取片的u轴角度
        /// </summary>
        private static double RobotAxisU_PickFromUp => -90;
        /// <summary>
        /// 机器人双目定位的u轴角度
        /// </summary>
        private static double RobotAxisU_Precise => RobotAxisU_PickFromUp;
        /// <summary>
        /// 机器人放aoi的u轴角度，由于空间原因为了避干涉可能导致不同工位角度不同
        /// </summary>
        public static double[] RobotAxisU_PlaceToAOI => new double[4]
        {
            0,0,0,-45
        };
        /// <summary>
        /// 机器人从aoi去片的u轴角度，同一工位，认为取放角度相同
        /// </summary>
        public static double[] RobotAxisU_PickFromAOI => RobotAxisU_PlaceToAOI;
        /// <summary>
        /// 机器人去下游平台放片的u轴角度
        /// </summary>
        public static double RobotAxisU_PlaceToDown => 90;
        #endregion

        #region aixs t
        /// <summary>
        /// 从平台取片的t轴角度
        /// </summary>
        public static double AxisT_PickFromPlat => 0;
        /// <summary>
        /// 双目定位的t轴角度
        /// </summary>
        public static double AxisT_Precise => AxisT_PickFromPlat;
        /// <summary>
        /// 放aoi的t轴角度，4工位分开
        /// </summary>
        public static double[] AxisT_PlaceToAOI => new double[4]
        {
            //前两个数字计算，放片和取片时，机器人u轴角度差，这是t轴要旋转的第一部分
            //第二个数字，是aoi角度和来料角度差
            RobotAxisU_PlaceToAOI[0] - RobotAxisU_PickFromUp + Cell_AOI - Cell_Origin,
            RobotAxisU_PlaceToAOI[1] - RobotAxisU_PickFromUp + Cell_AOI - Cell_Origin,
            RobotAxisU_PlaceToAOI[2] - RobotAxisU_PickFromUp + Cell_AOI - Cell_Origin,
            RobotAxisU_PlaceToAOI[3] - RobotAxisU_PickFromUp + Cell_AOI - Cell_Origin,
        };
        /// <summary>
        /// 取aoi的t轴角度，4工位分开
        /// <para>此处要保证取出的产品，如果回到robotu=0,t=0，玻璃应该和来料一个方向</para>
        /// </summary>
        //public static double[] AxisT_PickFromAOI => new double[4]
        //{
        //    RobotAxisU_PickFromAOI[0] + RobotAxisU_PlaceToDown + Cell_AOI,
        //    RobotAxisU_PickFromAOI[1] + RobotAxisU_PlaceToDown + Cell_AOI,
        //    RobotAxisU_PickFromAOI[2] + RobotAxisU_PlaceToDown + Cell_AOI,
        //    RobotAxisU_PickFromAOI[3] + RobotAxisU_PlaceToDown + Cell_AOI,
        //};
        public static double[] AxisT_PickFromAOI => AxisT_PlaceToAOI;
        /// <summary>
        /// 放下游的t轴角度
        /// <para>因为plc确保上料平台和aoi方向一致，同时要求放下游也与前两个位置一致</para>
        /// <para>所以从aoi取出的片是0°，且要确保下游角度是0°</para>
        /// </summary>
        public static double AxisT_PlaceToDown => 
            RobotAxisU_PlaceToDown - RobotAxisU_PickFromUp + Cell_Down - Cell_Origin;
        /// <summary>
        /// 旋转中心标定时的t轴角度
        /// </summary>
        public static double AxisT_CalibRC => 0.5;
        #endregion

        #region cell
        public static double Cell_Origin
        {
            get
            {
                int dir = (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DIR_ORIGIN].DblValue;
                int angle = 0;
                while (dir > 1)
                {
                    dir >>= 1;
                    angle -= 90;
                }
                return GetAngle(angle);
            }
        }
        /// <summary>
        /// 产品放aoi的角度，plc在recipe中写1248对应0/90/180/270（顺时针）
        /// <para>本机中这个角度应该常为0</para>
        /// </summary>
        public static double Cell_AOI
        {
            get
            {
                int dir = (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DIR_PLACETOAOI].DblValue;
                int angle = 0;
                while (dir > 1)
                {
                    dir >>= 1;
                    angle -= 90;
                }
                return GetAngle(angle);
            }
        }

        /// <summary>
        /// 产品放下游平台的角度，plc在recipe中写1248对应0/90/180/270
        /// <para>本机中这个角度应该常为0</para>
        /// </summary>
        public static double Cell_Down => Cell_Origin;
        #endregion

        #region common
        static double GetAngle(double r)
        {
            return (r + 360) % 360;
        }
        #endregion
    }
}