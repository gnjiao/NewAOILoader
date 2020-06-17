using BasicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    partial class BaseDealComprehensiveResult_Main
    {
        const string ReservDigits = @"f3";

        protected const string Camera1Match1 = @"C2";
        protected const string Camera2Match1 = @"C2";
        protected const string Camera2RC = @"C6";

        protected const string StrMonoMatch1 = @"C4";
        protected const string StrMonoMatch2 = @"C12";
        protected const string StrMonoRC = @"C6";

        public Point2D[] pt2MarkArray = new Point2D[10] { new Point2D(), new Point2D(),
            new Point2D(), new Point2D(), new Point2D(), new Point2D(), new Point2D(), new Point2D(),new Point2D(),new Point2D() };

        public static double MonoAngleCom { get; set; }
    }

    public class CameraResult
    {
        public const int OK = 1;
        public const int NG = 2;
    }

    public enum PtType_Mono
    {
        AutoMark1,//自动流程Mark1
        AutoMark2,//自动流程Mark2
        AutoMark3,
    }
}
