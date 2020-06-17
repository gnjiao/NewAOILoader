using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using DealCalibrate;

namespace StationDataManager
{
    public partial class StationDataMngr
    {
        //public static StationDataMngr instance = new StationDataMngr();

        public static List<Point4D> CalibPos_L = new List<Point4D>();
        public static List<Point4D> PlacePos_L = new List<Point4D>();
        public static List<Point3D> InsertPos_L = new List<Point3D>();

        /// <summary>
        /// mark1坐标
        /// </summary>
        //public Point2D pt2Mark1
        //{
        //    get;
        //    set;
        //}
        ///// <summary>
        ///// mark2坐标
        ///// </summary>
        //public Point2D pt2Mark2
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 提取string转换为point2D
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        Point2D ConvertToPoint2D(string strValue)
        {
            try
            {
                string[] strArr = strValue.Split(',');
                return new Point2D(double.Parse(strArr[0]), double.Parse(strArr[1]));
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("RegeditMain.ConvertToPoint2D", ex);
                return new Point2D();
            }
        }

       


    }
}
