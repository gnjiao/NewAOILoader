using BasicClass;
using DealAlgorithm;
using DealCalibrate;
using DealFile;
using Microsoft.Win32;
using ParComprehensive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class ModuleBase
    {
        #region define
        static string ClassName => "ModuleBase";

        static bool blValid1_BinCam1 = false;
        static bool blValid2_BinCam1 = false;
        static bool blValid1_BinCam2 = false;
        static bool blValid2_BinCam2 = false;

        const string SubKey1 = "SOFTWARE";
        const string SubKey2 = "Module";

        public const string ReservDigits = "f3";
        #endregion

        #region msg
        public static void ShowState(string msg)
        {
            Main_EX.BaseDealComprehensiveResult.ShowState(msg);
        }

        public static void ShowAlarm(string msg)
        {
            Main_EX.BaseDealComprehensiveResult.ShowAlarm(msg);
        }
        #endregion

        #region calib


        public static bool CalibRotateCenter(string cellName, Point2D pt2Mark1, Point2D pt2Mark2,
            double rotateAngle, BaseParComprehensive baseParComprehensive)
        {
            try
            {
                //获取旋转中心cell
                BaseParCalibrate baseParCalibrate = baseParComprehensive.GetCellParCalibrate(cellName);
                //获取旋转中心算子
                ParCalibRotate parCalibRotate = (ParCalibRotate)baseParCalibrate;
                //根据参数求旋转中心
                Point2D rc = new FunCalibRotate().GetOriginPoint(rotateAngle, pt2Mark1, pt2Mark2);
                //把旋转中心存入算子
                parCalibRotate.XRC = rc.DblValue1;
                parCalibRotate.YRC = rc.DblValue2;
                //讲计算结果写入xml
                baseParComprehensive.WriteXmlDoc(cellName);
                //将参数保存到结果类
                //new FunCalibRotate().SaveParToResult(HtResult_Cam1, parCalibRotate);
                //输出计算结果
                ShowState(string.Format("旋转中心标定完成，X:{0},Y:{1}", parCalibRotate.XRC, parCalibRotate.YRC));

                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
        }

        public static bool VerifyRotateCenter(Point2D pt2SrcMark1, Point2D pt2SrcMark2,
            Point2D pt2DstMark1, Point2D pt2DstMark2, double r, double disMark, double ratio, double thread)
        {
            double srcR = GetCurAngleByX(disMark, ratio, pt2SrcMark1, pt2SrcMark2);
            double dstR = GetCurAngleByX(disMark, ratio, pt2DstMark1, pt2DstMark2);
            double verifyR = srcR - dstR;
            double deviation = Math.Abs(verifyR - r);
            if (deviation > thread)
            {
                ShowState(string.Format("旋转中心校验失败，使用角度:{0},校验角度：{1}", r, verifyR));
                return false;
            }
            else
                ShowState(string.Format("旋转中心校验成功，使用角度:{0},校验角度：{1}", r, verifyR));
            return true;
        }

        public static double GetCurAngleByX(double disMark, double ratio, Point2D pt1, Point2D pt2)
        {
            return Math.Asin((pt2.DblValue1 - pt1.DblValue1) * ratio / disMark) / Math.PI * 180;
        }

        public static double GetCurAngleByY(double disMark, double ratio, Point2D pt1, Point2D pt2)
        {
            return Math.Asin((pt2.DblValue2 - pt1.DblValue2) / disMark * ratio) / Math.PI * 180;
        }

        public static bool RecordStdValue(BaseParStd par, Point2D mark1, Point2D mark2)
        {
            try
            {
                //保存两次坐标的平均值到算子中
                par.X_BS.Value = (mark1.DblValue1 + mark2.DblValue1) / 2;
                par.Y_BS.Value = (mark1.DblValue2 + mark2.DblValue2) / 2;
                ShowState(string.Format("基准值保存成功:X{0}/Y:{1}", par.X_BS.Value, par.Y_BS.Value));

                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                ShowAlarm("基准值保存失败");
                return false;
            }
        }
        #endregion

        #region calc
        public static Point2D TransDelta(Point2D oriPt, double dstAngle, double startAngle)
        {
            return MultMatrix(oriPt, dstAngle - startAngle);
        }

        static Point2D MultMatrix(Point2D pt, double angle)
        {
            double radius = angle / 180 * Math.PI;
            double x = pt.DblValue1 * Math.Cos(radius) - pt.DblValue2 * Math.Sin(radius);
            double y = pt.DblValue1 * Math.Sin(radius) + pt.DblValue2 * Math.Cos(radius);
            return new Point2D(x, y);
        }
        #endregion

        #region 旋转中心相关
        public static bool GetPtAfterRotate(Point2D pt, double r, string cellName, BaseParComprehensive parComprehensive, out Point2D result)
        {
            result = new Point2D();
            try
            {
                if (!GetRcFromPar(cellName, parComprehensive, out Point2D rc))
                    return false;
                result = new FunCalibRotate().GetPoint_AfterRotation(-r / 180 * Math.PI, rc, pt);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
            return true;
        }
        public static bool GetRcFromPar(string cellName, BaseParComprehensive parComprehensive, out Point2D rc)
        {
            rc = new Point2D();
            try
            {
                //获取单元格
                BaseParCalibrate baseParComprehensive = parComprehensive.GetCellParCalibrate(cellName);
                //获取旋转中心算子
                ParCalibRotate parCalibRotate = (ParCalibRotate)baseParComprehensive;
                //计算旋转之后的mark位置
                rc = parCalibRotate.PointRC;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
                return false;
            }
            return true;
        }
        #endregion

        #region 数据记录
        protected static void RecordPreciseData(int id, string data)
        {
            Record(id, data, "Precise");
        }

        protected static void RecordSharpnessData(int id, string data)
        {
            Record(id, data, "Sharpness");
        }

        static void Record(int id, string data, string folerName)
        {
            TxtFile t_I = new TxtFile();
            try
            {
                string root = ParPathRoot.PathRoot + "软件运行记录\\RecordData\\" + folerName + "\\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                string pathDir = Log.CreateAllTimeFile(root);

                string path = pathDir + folerName + ".txt";
                string name = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() +
                    ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString();
                t_I.WriteText(path, name + "----->ID: " + id.ToString() + "------->" + data);//写入时间
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(ClassName, ex);
            }
        }
        #endregion

        #region 注册表
        public static string ReadRegedit(string name)
        {
            try
            {
                RegistryKey hkml = Registry.CurrentUser;
                RegistryKey software = hkml.OpenSubKey(SubKey1, true);
                RegistryKey aimdir = software.OpenSubKey(SubKey2, true);
                return aimdir.GetValue(name).ToString();
            }
            catch
            {
                return string.Empty;
            }

        }

        public static void WriteRegedit(string name, string value)
        {
            try
            {
                RegistryKey hklm = Registry.CurrentUser;
                RegistryKey software = hklm.OpenSubKey(SubKey1, true);
                RegistryKey aimdir = software.CreateSubKey(SubKey2);
                aimdir.SetValue(name, value);
            }
            catch { }
        }
        #endregion
    }
}
