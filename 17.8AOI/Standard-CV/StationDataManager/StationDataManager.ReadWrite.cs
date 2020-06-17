using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using DealFile;
using Common;
using DealConfigFile;
using System.IO;

namespace StationDataManager
{
    partial class StationDataMngr
    {
        #region 路径
        public static string PathLogDelta
        {
            get
            {
                string path = Log.CreateAllTimeFile(ParPathRoot.PathRoot + "软件运行记录\\Custom\\" + ComConfigPar.C_I.NameModel);
                return path + "LogPosMarkDelta.log";
            }
        }

        public static string PathStationData
        {
            get
            {
                string path = ParPathRoot.PathRoot + "Store\\产品参数\\" + ComConfigPar.C_I.NameModel;
                return path + "\\StationData.ini";
            }
        }

        private static string DirCalibLocalPath = ParPathRoot.PathRoot + "Store\\Calib\\LocalCalib\\" + ComConfigPar.C_I.NameModel + "\\";

        #endregion

        #region 工位放片标定值读写
        public static void ReadIniCalibPos()
        {
            try
            {
                CalibPos_L.Clear();
                for (int i = 0; i < 4; i++)
                {
                    string section = "Pos" + (i + 1).ToString();
                    double xStdCalib = IniFile.I_I.ReadIniDbl(section, "xStdCalib", PathStationData);
                    double yStdCalib = IniFile.I_I.ReadIniDbl(section, "yStdCalib", PathStationData);
                    double zStdCalib = IniFile.I_I.ReadIniDbl(section, "zStdCalib", PathStationData);
                    double rStdCalib = IniFile.I_I.ReadIniDbl(section, "rStdCalib", PathStationData);

                    CalibPos_L.Add(new Point4D(xStdCalib, yStdCalib, zStdCalib, rStdCalib));

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("PosGlass", ex);
            }
        }

        public static void WriteIniCalibPosLocal(int i)
        {
            try
            {
                if (!Directory.Exists(DirCalibLocalPath))
                {
                    Directory.CreateDirectory(DirCalibLocalPath);
                }

                string path = DirCalibLocalPath + "工位" + i + ".ini";

                string section = DateTime.Now.ToString();

                IniFile.I_I.WriteIni(section, "xStdCalib", CalibPos_L[i].DblValue1.ToString(), path);
                IniFile.I_I.WriteIni(section, "yStdCalib", CalibPos_L[i].DblValue2.ToString(), path);
                IniFile.I_I.WriteIni(section, "rStdCalib", CalibPos_L[i].DblValue4.ToString(), path);
            }
            catch(Exception ex)
            {
                Log.L_I.WriteError("PosGlass", ex);
            }
        }


        public static void WriteIniCalibPos(int i)
        {
            try
            {
                string section = "Pos" + (i--).ToString();

                //标定的时间
                IniFile.I_I.WriteIni(section, "Time", DateTime.Now.ToString(), PathStationData);

                IniFile.I_I.WriteIni(section, "xStdCalib", CalibPos_L[i].DblValue1.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "yStdCalib", CalibPos_L[i].DblValue2.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "zStdCalib", CalibPos_L[i].DblValue3.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "rStdCalib", CalibPos_L[i].DblValue4.ToString(), PathStationData);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("PosGlass", ex);
            }
        }
        #endregion

        #region 工位示教值读写
        public static void ReadIniPlacePos()
        {
            try
            {
                PlacePos_L.Clear();
                for (int i = 0; i < 4; i++)
                {
                    string section = "Pos" + (i + 1).ToString();
                    double xStdCalib = IniFile.I_I.ReadIniDbl(section, "xStdAOI", PathStationData);
                    double yStdCalib = IniFile.I_I.ReadIniDbl(section, "yStdAOI", PathStationData);
                    double zStdCalib = IniFile.I_I.ReadIniDbl(section, "zStdAOI", PathStationData);
                    double rStdCalib = IniFile.I_I.ReadIniDbl(section, "rStdAOI", PathStationData);

                    PlacePos_L.Add(new Point4D(xStdCalib, yStdCalib, zStdCalib, rStdCalib));

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("PosGlass", ex);
            }
        }

        public static void WriteIniPlacePos(int i)
        {
            try
            {
                string section = "Pos" + i.ToString();

                //标定的时间
                IniFile.I_I.WriteIni(section, "Time", DateTime.Now.ToString(), PathStationData);

                IniFile.I_I.WriteIni(section, "xStdAOI", PlacePos_L[i - 1].DblValue1.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "yStdAOI", PlacePos_L[i - 1].DblValue2.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "zStdAOI", PlacePos_L[i - 1].DblValue3.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "rStdAOI", PlacePos_L[i - 1].DblValue4.ToString(), PathStationData);

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("PosGlass", ex);
            }
        }
        #endregion

        #region 工位插栏标定值读写
        public static void ReadDeltaInsert()
        {
            try
            {
                InsertPos_L.Clear();
                for (int i = 0; i < 4; i++)
                {
                    string section = "Pos" + (i + 1).ToString();
                    double xDeltaInsert = IniFile.I_I.ReadIniDbl(section, "xDeltaInsert", PathStationData);
                    double yDeltaInsert = IniFile.I_I.ReadIniDbl(section, "yDeltaInsert", PathStationData);
                    double rDeltaInsert = IniFile.I_I.ReadIniDbl(section, "rDeltaInsert", PathStationData);

                    InsertPos_L.Add(new Point3D(xDeltaInsert, yDeltaInsert, rDeltaInsert));

                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("PosGlass", ex);
            }
        }

        public static void WriteDeltaInsert(int index)
        {
            try
            {
                string section = "Pos" + (index--).ToString();

                //标定的时间
                IniFile.I_I.WriteIni(section, "Time", DateTime.Now.ToString(), PathStationData);

                IniFile.I_I.WriteIni(section, "xDeltaInsert", InsertPos_L[index].DblValue1.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "yDeltaInsert", InsertPos_L[index].DblValue2.ToString(), PathStationData);
                IniFile.I_I.WriteIni(section, "rDeltaInsert", InsertPos_L[index].DblValue3.ToString(), PathStationData);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("PosGlass", ex);
            }
        }
        #endregion

        public static void read_station_data()
        {
            ReadDeltaInsert();
            ReadIniCalibPos();
            ReadIniPlacePos();
        }


        public void WriteCalibResult(int index)
        {
            index++;
            WriteIniCalibPos(index);
            //WriteDeltaInsert(index);
            return;
        }
    }
}
