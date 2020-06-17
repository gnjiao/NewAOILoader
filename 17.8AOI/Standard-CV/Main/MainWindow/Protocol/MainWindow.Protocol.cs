using DealConfigFile;
using DealLog;
using DealRobot;
using ModulePackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public partial class Protocols
    {
        /// <summary>
        /// 插栏相机朝向
        /// </summary>
        public static DirCstCamera_Enum DirPhoto => DirCstCamera_Enum.Backward;
        /// <summary>
        /// 插栏z轴补偿轴
        /// </summary>
        public static TypeModuleZ_Enum DirZ => TypeModuleZ_Enum.ModuleUp;
        /// <summary>
        /// 插栏方向
        /// </summary>
        public static DirInsert_Enum DirInsert => DirInsert_Enum.NToP;
        /// <summary>
        /// 插栏相机画面显示是否镜像
        /// </summary>
        public static bool CstIsMirrorX => true;
        /// <summary>
        /// 根据机器人型号，确定厚度补偿方向
        /// </summary>
        static double ThicknessOffset
        {
            get
            {
                double coef = 1;
                switch (ParSetRobot.P_I.TypeRobot_e)
                {
                    case TypeRobot_enum.Epsion_Ethernet:
                        coef = 1;
                        break;
                    case TypeRobot_enum.Epsion_Serial:
                        coef = 1;
                        break;
                    case TypeRobot_enum.YAMAH_Ethernet:
                        coef = -1;
                        break;
                    case TypeRobot_enum.YAMAH_Serial:
                        coef = -1;
                        break;
                }
                return coef * ConfGlassThicknes;
            }
        }

        /// <summary>
        /// 配方-玻璃X
        /// </summary>
        public static double ConfGlassX
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.GlassX].DblValue;
            }
        }
        /// <summary>
        /// 配方-玻璃Y
        /// </summary>
        public static double ConfGlassY
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.GlassY].DblValue;
            }
        }
        /// <summary>
        /// 配方-玻璃厚度
        /// </summary>
        public static double ConfGlassThicknes
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.Thickness].DblValue;
            }
        }


        public static double ConfCodeX => ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.CodeX].DblValue;

        public static double ConfCodeY => ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.CodeY].DblValue;

        public static double ConfDisMark
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DisMark].DblValue;                
            }
        }

        public static int ConfDirPick =>
            (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DIR_PICK].DblValue;

        public static int ConfDirPlaceToAOI =>
            (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DIR_PLACETOAOI].DblValue;

        
        /// <summary>
        /// 配方-龙骨列数
        /// </summary>
        public static int KeelCol
        {
            get
            {
                return ConfCSTCol + 1;
            }
        }

        public static double ConfLayerSpacing
        {
            get=> ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.LayerSpacing].DblValue;
        }

        /// <summary>
        /// 配方-插栏列数
        /// </summary>
        public static int ConfCSTCol
        {
            get
            {
                return (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.CSTCols].DblValue;
            }
        }
        /// <summary>
        /// 配方-插栏行数
        /// </summary>
        public static int ConfCSTRow
        {
            get
            {
                return (int)ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.CSTRows].DblValue;
            }
        }
        /// <summary>
        /// 配方-龙骨间距
        /// </summary>
        public static double ConfKeelInterval
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.KeelInterval].DblValue;
            }
        }
        /// <summary>
        /// 配方-第一列龙骨位置
        /// </summary>
        public static double ConfCol1Interval
        {
            get
            {
                return ParConfigPar.P_I.ParProduct_L[(int)RecipeRegister.DisCol1].DblValue;
            }
        }
    }
}
