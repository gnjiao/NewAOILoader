﻿using BasicClass;
using Camera;
using Common;
using DealCalibrate;
using DealConfigFile;
using DealPLC;
using DealRobot;
using ParComprehensive;
using StationDataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Main
{
    partial class BaseDealComprehensiveResult_Main
    {
        protected static Point2D Pt2Mark1 = new Point2D();
        protected static Point2D Pt2Mark2 = new Point2D();
        protected static Point2D Pt2MarkRC = new Point2D();

        //标定用的点位，一组基准点，一组cur点，cur点每次计算会更新
        protected static Point2D Pt2Mark1CalibStd = new Point2D();
        protected static Point2D Pt2Mark2CalibStd = new Point2D();
        protected static Point2D Pt2Mark1Calib = new Point2D();
        protected static Point2D Pt2Mark2Calib = new Point2D();
        //轴标定用的基准角度
        protected static double AxisCalibAngleStd = 0;

        public static bool Camera1Done = false;
        public static bool Camera2Done = false;        

        public void DualLocation(int index)
        {
            if (Camera1Done & Camera2Done)
            {
                BaseParCalibrate baseParComprehensive = ParComprehensive2.P_I.GetCellParCalibrate(Camera2RC);
                ParCalibRotate parCalibRotate = (ParCalibRotate)baseParComprehensive;
                int num = Protocols.BotAdjIndex + index - 1;

                double angle = Math.Asin(
                    (Pt2Mark2.DblValue2
                    - Pt2Mark1.DblValue2)
                    * AMP
                    / Protocols.ConfDisMark) * 180 / Math.PI
                    - StationDataMngr.CalibPos_L[index - 1].DblValue4;
                ShowState("工位" + index + "角度偏差: " + angle);
                LogicPLC.L_I.WriteRegData2((int)DataRegister2.AxisT_PlaceToAOI1 + index - 1,
                    Protocols.AxisT_PlaceToAOI[index - 1] - angle + ParAdjust.Value3("adj" + num));

                FunCalibRotate fcr = new FunCalibRotate();
                Point2D MarkAfterRotate = fcr.GetPoint_AfterRotation(
                    -angle / 180 * Math.PI, parCalibRotate.PointRC, Pt2Mark1);

                double deltaX = MarkAfterRotate.DblValue1 - StationDataMngr.CalibPos_L[index - 1].DblValue1;
                double deltaY = MarkAfterRotate.DblValue2 - StationDataMngr.CalibPos_L[index - 1].DblValue2;

                #region axis calc
                double[] arr = new double[4] { deltaX, deltaY, 0, 0 };
                double[] value = AxisDirectionService.GetInstance().GetValues(arr);

                #endregion

                deltaX *= -AMP;
                deltaY *= AMP;
                ShowState("计算得偏差X:" + deltaX.ToString("f3") + ",Y:" + deltaY.ToString("f3"));
                //Point2D delta = TransDelta(new Point2D(deltaX, deltaY),
                //    Protocols.ConfPlaceAngle, Protocols.ConfPreciseAngle);
                Point2D delta = TransDelta(new Point2D(value[0], value[1]),
                    Protocols.ConfPlaceAngle, Protocols.ConfPreciseAngle);
                ShowState("当前使用轴计算得到的偏差");
                ShowState(string.Format("工位{0}X方向补偿{1},Y方向补偿{2}", index, 
                    delta.DblValue1.ToString(ReserveDigits), 
                    delta.DblValue2.ToString(ReserveDigits)));


                Point4D pos = new Point4D
                {
                    DblValue1 = delta.DblValue1 + StationDataMngr.PlacePos_L[index - 1].DblValue1 
                    + ParAdjust.Value1("adj" + num),
                    DblValue2 = delta.DblValue2 + StationDataMngr.PlacePos_L[index - 1].DblValue2 
                    + ParAdjust.Value2("adj" + num),
                    DblValue3 = StationDataMngr.PlacePos_L[index - 1].DblValue3,
                    DblValue4 = Protocols.RobotAxisU_PlaceToAOI[index - 1]
                };

                LogicRobot.L_I.WriteRobotCMD(pos, Protocols.BotCmd_StationPos);
            }
        }

        public void CalibDual(int index)
        {
            if (Camera1Done & Camera2Done)
            {
                double angle = Math.Asin((Pt2Mark2.DblValue2 - Pt2Mark1.DblValue2) * AMP
                    / Protocols.ConfDisMark) * 180 / Math.PI;
                ShowState("精定位验证工位" + index + "逆时针角度偏差: " + angle);

                StationDataMngr.CalibPos_L[index - 1].DblValue1 = Pt2Mark1.DblValue1;
                StationDataMngr.CalibPos_L[index - 1].DblValue2 = Pt2Mark1.DblValue2;
                StationDataMngr.CalibPos_L[index - 1].DblValue4 = angle;
                StationDataMngr.WriteIniCalibPos(index);

                StationDataMngr.WriteIniCalibPosLocal(index);

                LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_CalibOK);

                #region new station
                double[] value = new double[3] { Pt2Mark1.DblValue1, Pt2Mark1.DblValue2, angle };
                Station.StationService.GetInstance().SetCalib(0, value, Protocols.StationDataPath);
                #endregion
            }
        }

        public void CalibRC()
        {
            BaseParCalibrate baseParComprehensive = ParComprehensive2.P_I.GetCellParCalibrate(Camera2RC);
            ParCalibRotate parCalibRotate = (ParCalibRotate)baseParComprehensive;

            Point2D orgPoint = new FunCalibRotate().GetOriginPoint(Protocols.BotRCCalibAngle, Pt2Mark1, Pt2MarkRC);

            parCalibRotate.XRC = orgPoint.DblValue1;
            parCalibRotate.YRC = orgPoint.DblValue2;

            ParComprehensive2.P_I.WriteXmlDoc(Camera2RC);
            //将参数保存到结果类
            new FunCalibRotate().SaveParToResult(HtResult_Cam2, parCalibRotate);

            ShowState(string.Format("旋转中心标定完成,X_{0},Y_{1}",
                parCalibRotate.XRC.ToString(), parCalibRotate.YRC.ToString()));

            LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_CalibOK);
        }
    }
}