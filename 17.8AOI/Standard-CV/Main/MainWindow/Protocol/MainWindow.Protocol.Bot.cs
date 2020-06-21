using BasicClass;
using DealConfigFile;
using DealImageProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    partial class Protocols
    {
        #region cmd
        //public static string BotCmd_Station1 = "11";
        public static string BotCmd_StationPos => "11";
        //public static string BotCmd_Station2 = "12";
        public static string BotCmd_PreciseOK => "13";
        public static string BotCmd_PreciseNG => "19";

        public static string BotCmd_CalibStation => "21";
        public static string BotCmd_CalibOK => "22";
        public static string BotCmd_CalibRC => "23";
        public static string BotCmd_Move => "24";
        public static string BotCmd_Teach => "25";
        public static string BotCmd_TeachOver => "26";
        public static string BotCmd_AxisCalibOK => "27";

        public static string BotCmd_AskBuildVacuum => "90";
        public static string BotCmd_AskBreakVacuum => "91";
        #endregion

        #region adj
        public static int BotAdjIndex => 7;
        #endregion


        public static Point4D BotPickPos
        {
            get
            {
                return StdBotPickPos + AdjBotPickPos;
            }
        }

        public static Point4D StdBotPickPos
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.PickPos].Add(3, RobotAxisU_PickFromUp);
            }
        }

        public static Point4D AdjBotPickPos
        { 
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.PickPos];
            }
        }

        public static Point4D StdBotPrecisePos
        {
            get
            {                
                return ParBotStd.P_I[
                    (int)BotStd.PrecisePos]
                    .Add(1, -GlassYInPresize / 2)
                    .Add(3, RobotAxisU_Precise);
            }
        }

        public static Point4D BotDownPos
        {
            get
            {
                return StdBotDownPos + AdjBotDownPos;
            }
        }

        public static Point4D StdBotDownPos
        {
            get
            {
                return ParBotStd.P_I[(int)BotStd.DownPos].Add(3, RobotAxisU_PlaceToDown);
            }
        }

        public static Point4D AdjBotDownPos
        {
            get
            {
                return ParBotAdj.P_I[(int)BotAdj.DownPos];
            }
        }

        public static int ConfPreciseAngle
        {
            get => (int)Cell_Origin;
            //{
            //    int dir = 0;// (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DIR_PRECISE].DblValue;
            //    int angle = 0;
            //    while (dir > 1)
            //    {
            //        dir >>= 1;
            //        angle -= 90;
            //    }
            //    return (angle + 360) % 360;
            //}
        }

        public static int ConfPlaceAngle
        {
            get => (int)Cell_AOI;
            //{
            //    int dir = (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DIR_PLACETOAOI].DblValue;
            //    int angle = 0;
            //    while (dir > 1)
            //    {
            //        dir >>= 1;
            //        angle -= 90;
            //    }
            //    return (angle + 360) % 360;
            //}
        }

        public static double GlassXInPresize
        {
            get
            {
                return ConfPreciseAngle % 180 == 0 ? ConfGlassX : ConfGlassY;
            }
        }

        public static double GlassYInPresize
        {
            get
            {
                return ConfPreciseAngle % 180 == 0 ? ConfGlassY : ConfGlassX;
            }
        }

        public static double BotRCCalibAngle = 0.5;
    }
}
