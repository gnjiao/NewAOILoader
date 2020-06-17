using BasicClass;
using DealCalibrate;
using DealFile;
using DealPLC;
using ParComprehensive;
using System;
using System.Collections.Generic;
using System.IO;

namespace Main
{
    public partial class BaseDealComprehensiveResult_Main
    {
        #region algorithm
        public bool CalibRotateCenter(string cellName, Point2D pt2Mark1, Point2D pt2Mark2,
           double rotateAngle, BaseParComprehensive parComprehensive)
        {
            try
            {
                //获取旋转中心cell
                BaseParCalibrate baseParComprehensive = parComprehensive.GetCellParCalibrate(cellName);
                //获取旋转中心算子
                ParCalibRotate parCalibRotate = (ParCalibRotate)baseParComprehensive;
                //根据参数求旋转中心
                Point2D rc = new FunCalibRotate().GetOriginPoint(rotateAngle, pt2Mark1, pt2Mark2);
                //把旋转中心存入算子
                parCalibRotate.XRC = rc.DblValue1;
                parCalibRotate.YRC = rc.DblValue2;
                //讲计算结果写入xml
                parComprehensive.WriteXmlDoc(cellName);
                //将参数保存到结果类
                new FunCalibRotate().SaveParToResult(HtResult_Cam1, parCalibRotate);
                //输出计算结果
                string info = string.Format("相机{0}旋转中心标定完成，X:{1},Y:{2}", g_NoCamera,
                    parCalibRotate.XRC.ToString(), parCalibRotate.YRC.ToString());
                ShowState(info);

                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        public bool SetRotateCenter(string cellName, Point2D rc, BaseParComprehensive parComprehensive)
        {
            //获取旋转中心cell
            BaseParCalibrate baseParComprehensive = parComprehensive.GetCellParCalibrate(cellName);
            //获取旋转中心算子
            ParCalibRotate parCalibRotate = (ParCalibRotate)baseParComprehensive;
            //把旋转中心存入算子
            parCalibRotate.XRC = rc.DblValue1;
            parCalibRotate.YRC = rc.DblValue2;
            //讲计算结果写入xml
            parComprehensive.WriteXmlDoc(cellName);
            //将参数保存到结果类
            new FunCalibRotate().SaveParToResult(HtResult_Cam1, parCalibRotate);
            //输出计算结果
            string info = string.Format("相机{0}旋转中心标定完成，X:{1},Y:{2}", g_NoCamera,
                parCalibRotate.XRC.ToString(), parCalibRotate.YRC.ToString());
            ShowState(info);

            return true;
        }

        #endregion

        #region 记录计算数据
        protected void RecordPreciseData(int id, string data)
        {
            TxtFile t_I = new TxtFile();
            try
            {
                string root = ParPathRoot.PathRoot + "软件运行记录\\RecordData\\Precise\\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                string pathDir = Log.CreateAllTimeFile(root);

                string path = pathDir + "PreciseData" + ".txt";
                string name = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() +
                    ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString();
                t_I.WriteText(path, name + "----->ID: " + id.ToString() + "------->" + data);//写入时间
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        protected void RecordCSTData(string key, List<double> list)
        {
            TxtFile t_I = new TxtFile();
            try
            {
                string root = ParPathRoot.PathRoot + "软件运行记录\\RecordData\\" + key + "\\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                string pathDir = Log.CreateAllTimeFile(root);

                string path = pathDir + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ".txt";
                string name = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() +
                    ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString();

                t_I.WriteText(path, key);//写入
                for (int i = 0; i < list.Count; ++i)
                {
                    t_I.WriteText(path, i.ToString() + "=" + list[i].ToString());//写入
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        protected void RecordCSTData(string key, List<Point2D> list)
        {
            TxtFile t_I = new TxtFile();
            try
            {
                string root = ParPathRoot.PathRoot + "软件运行记录\\RecordData\\" + key + "\\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                string pathDir = Log.CreateAllTimeFile(root);

                string path = pathDir + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ".txt";
                string name = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() +
                    ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString();

                t_I.WriteText(path, key);//写入
                for (int i = 0; i < list.Count; ++i)
                {
                    t_I.WriteText(path, i.ToString() + "=" + list[i].ToString());//写入
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        protected void RecordCSTData(string key, List<List<double>> list)
        {
            TxtFile t_I = new TxtFile();
            try
            {
                string root = ParPathRoot.PathRoot + "软件运行记录\\RecordData\\" + key + "\\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                string pathDir = Log.CreateAllTimeFile(root);

                string path = pathDir + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + ".txt";
                string name = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() +
                    ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString();

                for (int i = 0; i < list.Count; ++i)
                {
                    t_I.WriteText(path, @"-------Col" + i.ToString() + @"------------");
                    for (int j = 0; j < list[i].Count; ++j)
                    {
                        t_I.WriteText(path, j.ToString() + "=" + list[i][j].ToString());//写入
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public bool WriteIni_DblList(string strSection, string baseKey, string strPath, List<double> list)
        {
            try
            {
                IniFile.I_I.WriteIni(strSection, "Num", list.Count.ToString(), strPath);
                for (int i = 0; i < list.Count; i++)
                {
                    IniFile.I_I.WriteIni(strSection, baseKey + i.ToString(), list[i].ToString(), strPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        public bool WriteIni_Pt2List(string strSection, string baseKey, string strPath, List<Point2D> list)
        {
            try
            {
                IniFile.I_I.WriteIni(strSection, "Num", list.Count.ToString(), strPath);
                for (int i = 0; i < list.Count; i++)
                {
                    IniFile.I_I.WriteIni(strSection, baseKey + i.ToString(), list[i].ToString(), strPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 读取INI文件的接口
        /// </summary>
        /// <param name="strSection"></param>
        /// <param name="strKey"></param>
        /// <param name="strPath"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool ReadIni_DblList(string strSection, string strKey, string strPath, out List<double> list)
        {
            list = new List<double>();
            int intNum = IniFile.I_I.ReadIniInt(strSection, "Num", strPath);
            try
            {
                for (int i = 0; i < intNum; i++)
                {
                    double dblDevX = IniFile.I_I.ReadIniDbl(strSection, strKey + i.ToString(), strPath);
                    list.Add(dblDevX);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
                return false;
            }
        }
        #endregion

        #region TransFunction
        /// <summary>
        /// 默认旋转中心为00情况下，计算起始点旋转一定角度之后的位置，此处用于计算偏差转换
        /// </summary>
        /// <param name="oriPt"></param>
        /// <param name="dstAngle"></param>
        /// <param name="startAngle"></param>
        /// <returns></returns>
        public Point2D TransDelta(Point2D oriPt, double dstAngle, double startAngle)
        {
            return MultMatrix(oriPt, dstAngle - startAngle);
        }

        Point2D MultMatrix(Point2D pt, double angle)
        {
            double radius = angle / 180 * Math.PI;
            double x = pt.DblValue1 * Math.Cos(radius) - pt.DblValue2 * Math.Sin(radius);
            double y = pt.DblValue1 * Math.Sin(radius) + pt.DblValue2 * Math.Cos(radius);
            return new Point2D(x, y);
        }
        #endregion

        #region plc
        /// <summary>
        /// 封装writeregdata
        /// </summary>
        /// <param name="regNum"></param>
        /// <param name="addr"></param>
        /// <param name="value"></param>
        public void WritePLC(int regNum, int addr, double value)
        {
            switch (regNum)
            {
                case 1:
                    LogicPLC.L_I.WriteRegData1(addr, value);
                    break;
                case 2:
                    LogicPLC.L_I.WriteRegData2(addr, value);
                    break;
                case 3:
                    LogicPLC.L_I.WriteRegData3(addr, value);
                    break;
                case 4:
                    LogicPLC.L_I.WriteRegData4(addr, value);
                    break;
                case 5:
                    LogicPLC.L_I.WriteRegData5(addr, value);
                    break;
            }
        }

        public void WritePLC(int regNum, int addr, int num, double[] value)
        {
            switch (regNum)
            {
                case 1:
                    LogicPLC.L_I.WriteRegData1(addr, num, value);
                    break;
                case 2:
                    LogicPLC.L_I.WriteRegData2(addr, num, value);
                    break;
                case 3:
                    LogicPLC.L_I.WriteRegData3(addr, num, value);
                    break;
                case 4:
                    LogicPLC.L_I.WriteRegData4(addr, num, value);
                    break;
                case 5:
                    LogicPLC.L_I.WriteRegData5(addr, num, value);
                    break;
            }
        }
        #endregion
    }
}
