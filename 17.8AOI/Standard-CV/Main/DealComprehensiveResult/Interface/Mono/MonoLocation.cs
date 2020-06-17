using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using DealAlgorithm;
using DealCalibrate;
using ParComprehensive;

namespace Main
{
    /// <summary>
    /// 单目定位
    /// </summary>
    public class MonocularLation : ModuleBase
    {
        static readonly string ClassName = "MonocularLation";

        #region define
        public static Point2D[] ptArray = new Point2D[10];
        #endregion
        /// <summary>
        /// 偏差计算，取deltaX进行角度计算
        /// </summary>
        /// <param name="src">旋转起始点</param>
        /// <param name="dst">旋转终止点</param>
        /// <param name="ratio">相机系数</param>
        /// <param name="disMark">mark间距</param>
        /// <param name="cellName">算子序号</param>
        /// <param name="index">对应的基准值索引</param>
        /// <param name="baseParComprehensive">相机综合设置算子序列</param>
        /// <param name="pt3Result">计算结果</param>
        /// <returns></returns>
        public static bool CalcDeviationX(Point2D src, Point2D dst, double ratio,
            double disMark, string cellName, int index,
            BaseParComprehensive baseParComprehensive, out double[] pt3Result)
        {
            pt3Result = new double[3];
            try
            {
                //用于计算结果的工具类
                FunCalibRotate funCalibRotate = new FunCalibRotate();
                //计算当前位置与标定位的角度
                double deltar = GetCurAngleByX(disMark, ratio, src, dst);
                //算上基准值的角度，本机只用到pos0/1
                //Point4D calibPos = CalibDataMngr.instance.CalibPos_L[index];
                Point4D calibPos = new Point4D(640, 480, 0, 0);
                deltar -= calibPos.DblValue4;
                //计算旋转之后的mark位置，此处用第二点计算
                if (!GetPtAfterRotate(dst, -deltar, cellName, baseParComprehensive, out Point2D result))
                    return false;

                pt3Result = new double[] { Math.Round((calibPos.DblValue1 - result.DblValue1) * ratio, 4),
                    Math.Round((calibPos.DblValue2 - result.DblValue2) * ratio, 4),
                    Math.Round(deltar, 4) };
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 偏差计算，取deltaX进行角度计算
        /// </summary>
        /// <param name="mark1">旋转起始点</param>
        /// <param name="mark2">旋转终止点</param>
        /// <param name="ratio">相机系数</param>
        /// <param name="disMark">mark间距</param>
        /// <param name="cellName">算子序号</param>
        /// <param name="index">对应的基准值索引</param>
        /// <param name="baseParComprehensive">相机综合设置算子序列</param>
        /// <param name="pt3Result">计算结果</param>
        /// <returns></returns>
        public static bool CalcDeviationY(Point2D mark1, Point2D mark2, double ratio,
            double disMark, string cellName, int index,
            BaseParComprehensive baseParComprehensive, out double[] pt3Result)
        {
            pt3Result = new double[3];
            try
            {
                //用于计算结果的工具类
                FunCalibRotate funCalibRotate = new FunCalibRotate();
                //计算当前位置与标定位的角度
                double deltar = GetCurAngleByY(disMark, ratio, mark1, mark2);
                //算上基准值的角度，本机只用到pos0/1
                //Point4D calibPos = CalibDataMngr.instance.CalibPos_L[index];
                Point4D calibPos = new Point4D(640, 480, 0, 0);
                deltar -= calibPos.DblValue4;
                //计算旋转之后的mark位置，此处用第二点计算
                if (!GetPtAfterRotate(mark2, -deltar, cellName, baseParComprehensive, out Point2D result))
                    return false;

                pt3Result = new double[] { Math.Round((calibPos.DblValue1 - result.DblValue1) * ratio, 4),
                    Math.Round((calibPos.DblValue2 - result.DblValue2) * ratio, 4),
                    Math.Round(deltar, 4) };
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
            return true;
        }

        public static bool CalcDeviationY(Point2D mark, double r, double ratio,
            string cellName, int index, BaseParComprehensive baseParComprehensive, out double[] pt3Result)
        {
            pt3Result = new double[3];
            try
            {
                //用于计算结果的工具类
                FunCalibRotate funCalibRotate = new FunCalibRotate();
                //计算当前位置与标定位的角度
                double deltar = r;
                //算上基准值的角度，本机只用到pos0/1
                //Point4D calibPos = CalibDataMngr.instance.CalibPos_L[index];
                Point4D calibPos = new Point4D(640, 480, 0, 0);
                deltar -= calibPos.DblValue4;
                //计算旋转之后的mark位置，此处用第二点计算
                if (!GetPtAfterRotate(mark, -deltar, cellName, baseParComprehensive, out Point2D result))
                    return false;

                pt3Result = new double[] { Math.Round((calibPos.DblValue1 - result.DblValue1) * ratio, 4),
                    Math.Round((calibPos.DblValue2 - result.DblValue2) * ratio, 4),
                    Math.Round(deltar, 4) };
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
            return true;
        }

        public static double[] CalcDeltaWithAngle(Point2D pt1, Point2D pt2, double disMark, double amp, Point2D src)
        {
            double[] result = new double[3];

            double r = GetCurAngleByX(disMark, amp, pt1, pt2);
            Point2D dst = TransDelta(src, r, 0);
            result[0] = dst.DblValue1 - src.DblValue1;
            result[1] = dst.DblValue2 - src.DblValue2;
            result[2] = r;

            return result;
        }
    }
}
