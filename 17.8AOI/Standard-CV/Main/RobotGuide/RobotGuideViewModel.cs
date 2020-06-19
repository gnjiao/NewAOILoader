using BasicClass;
using DealRobot;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StationDataManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Main
{
    class RobotGuideViewModel : ValidateModelBase
    {
        public RobotGuideViewModel()
        {
            MoveCommand = new RelayCommand<string>(Move, IsValidate);
            CalibCommand = new RelayCommand(Calib);
            CalibRCComand = new RelayCommand(CalibRC);
            TeachCommand = new RelayCommand(Teach);
            SaveCurrentPosCommand = new RelayCommand(Save);
            SetStepCommand = new RelayCommand<string>(SetStep);
            AskBuildVacuumCommand = new RelayCommand(AskBuildVacuum);
            AskBreakVacuumCommand = new RelayCommand(AskBreakVacuum);

            Messenger.Default.Register<string[]>(this, "CurrentPos", UpdateCurrentPos);
        }

        #region properties
        double _currentX = 0d;
        public double CurrentX
        {
            get => _currentX;
            set => Set(ref _currentX, value);
        }

        double _currentY = 0d;
        public double CurrentY
        {
            get => _currentY;
            set => Set(ref _currentY, value);
        }

        double _currentZ = 0d;
        public double CurrentZ
        {
            get => _currentZ;
            set => Set(ref _currentZ, value);
        }

        double _incrementX = 0d;
        [Required]
        public double IncrementX
        {
            get => _incrementX;
            set => Set(ref _incrementX, value);
        }

        double _incrementY = 0d;
        [Required]
        public double IncrementY
        {
            get => _incrementY;
            set => Set(ref _incrementY, value);
        }

        double _incrementZ = 0d;
        [Required]
        public double IncrementZ
        {
            get => _incrementZ;
            set => Set(ref _incrementZ, value);
        }

        private bool _ifCalibRC = false;
        public bool IfCalibRC
        {
            get => _ifCalibRC;
            set => Set(ref _ifCalibRC, value);
        }

        int _stationNum = 1;
        public int StationNum
        {
            get => _stationNum;
            set => Set(ref _stationNum, value);
        }

        bool _moveButtonsEnabled = true;
        public bool MoveButtonsEnabled
        {
            get => _moveButtonsEnabled;
            set => Set(ref _moveButtonsEnabled, value);
        }
        #endregion

        #region command
        public ICommand MoveCommand { get; set; }
        public ICommand CalibCommand { get; set; }
        public ICommand CalibRCComand { get; set; }
        public ICommand TeachCommand { get; set; }
        public ICommand SaveCurrentPosCommand { get; set; }

        public ICommand SetStepCommand { get; set; }

        public ICommand AskBuildVacuumCommand { get; set; }
        public ICommand AskBreakVacuumCommand { get; set; }
        #endregion

        #region func
        void Move(string i)
        {
            if(!IsValidate)
            {
                MessageBox.Show("步进增量无效");
                return;
            }    

            Point4D pt = new Point4D();
            switch (Convert.ToInt32(i))
            {
                case 1:
                    pt.DblValue1 = IncrementX;
                    break;
                case 2:
                    pt.DblValue1 = -IncrementX;
                    break;
                case 3:
                    pt.DblValue2 = IncrementY;
                    break;
                case 4:
                    pt.DblValue2 = -IncrementY;
                    break;
                case 5:
                    pt.DblValue3 = IncrementZ;
                    break;
                case 6:
                    pt.DblValue3 = -IncrementZ;
                    break;
            }
            LogicRobot.L_I.WriteRobotCMD(pt, Protocols.BotCmd_Move);
            MoveButtonsEnabled = false;
        }

        void Calib()
        {
            if (MessageBox.Show("是否确定开始标定工位：" + StationNum,
                "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;

            if (MessageBox.Show("请确认机台内无人，并且标定针已拆下",
                "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;

            if (IfCalibRC)
                LogicRobot.L_I.WriteRobotCMD(new Point4D(StationNum, 0, 0, 0), Protocols.BotCmd_CalibRC);
            else
                LogicRobot.L_I.WriteRobotCMD(new Point4D(Convert.ToInt32(StationNum), 0, 0, 0), Protocols.BotCmd_CalibStation);
        }

        void CalibRC()
        {
            if (MessageBox.Show("是否确定开始去工位：" + StationNum + "标定旋转中心",
                "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;

            if (MessageBox.Show("请确认机台内无人，并且标定针已拆下",
                "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;

            LogicRobot.L_I.WriteRobotCMD(new Point4D(StationNum, 0, 0, 0), Protocols.BotCmd_CalibRC);
        }

        void Teach()
        {
            if (MessageBox.Show("准备开始示教工位：" + StationNum,
                "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;

            if (MessageBox.Show("请确认是否安装示教针",
                "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;

            if (MessageBox.Show("请确认机台内部没有人员",
                "确认信息", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;

            MoveButtonsEnabled = false;
            //将需要示教的工位号发送给机器人
            LogicRobot.L_I.WriteRobotCMD(new Point4D(StationNum, 0, 0, 0), Protocols.BotCmd_Teach);

        }

        void Save()
        {
            if (StationDataMngr.PlacePos_L.Count < StationNum)
                StationDataMngr.PlacePos_L.Add(new Point4D());
            StationDataMngr.PlacePos_L[StationNum - 1].DblValue1 = CurrentX;
            StationDataMngr.PlacePos_L[StationNum - 1].DblValue2 = CurrentY;
            StationDataMngr.PlacePos_L[StationNum - 1].DblValue3 = CurrentZ;
            StationDataMngr.WriteIniPlacePos(StationNum);

            #region new station
            double[] value = new double[4] { CurrentX, CurrentY, CurrentZ, 0 };
            Station.StationService.GetInstance()
                .SetStd(StationNum, value, Protocols.StationDataPath);
            #endregion

            LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_TeachOver);
            MessageBox.Show("数据保存成功");
        }

        void UpdateCurrentPos(string[] strArray)
        {
            try
            {
                CurrentX = Convert.ToDouble(strArray[2]);
                CurrentY = Convert.ToDouble(strArray[3]);
                CurrentZ = Convert.ToDouble(strArray[4]);
                MoveButtonsEnabled = true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("RobotGuide", ex);
            }
        }

        void SetStep(string step)
        {
            IncrementX = IncrementY = IncrementZ = Convert.ToDouble(step);
        }

        void AskBuildVacuum()
        {
            LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_AskBuildVacuum);
        }

        void AskBreakVacuum()
        {
            LogicRobot.L_I.WriteRobotCMD(Protocols.BotCmd_AskBreakVacuum);
        }
        #endregion
    }
}
