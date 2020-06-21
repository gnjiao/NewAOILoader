using BasicDisplay;
using DealCIM;
using DealConfigFile;
using DealLog;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Main
{
    public class MainViewModel
    {
        public ICommand RobotStdCommand { get; set; }
        public ICommand RobotAdjCommand { get; set; }
        public ICommand RobotGuideCommand { get; set; }
        public ICommand CodeSettingCommand { get; set; }
        public ICommand CodeModeSettingCommand { get; set; }

        public MainViewModel()
        {
            RobotStdCommand = new RelayCommand(OpenRobotStdView);
            RobotAdjCommand = new RelayCommand(OpenRobotAdjView);
            RobotGuideCommand = new RelayCommand(() => WndStationCalib.GetInstance().Show());
            CodeSettingCommand = new RelayCommand(() => new CIMWnd().Show());
            CodeModeSettingCommand = new RelayCommand(() => WndCimMode.GetInstance().Show());
        }

        void OpenRobotStdView()
        {
            ////权限返回
            //if (!EngineerReturn())
            //{
            //    return;
            //}

            //if (ParSetDisplay.P_I.TypeScreen_e != TypeScreen_enum.S800)
            //{
            //    WinSetRobotPoints winSetAdjustWork = WinSetRobotPoints.GetWinInst();
            //    winSetAdjustWork.Show();
            //}
            //else
            //{
            //    WinSetRobotPoints winSetAdjustWork = WinSetRobotPoints.GetWinInst();
            //    winSetAdjustWork.Show();
            //}

            ////按钮日志
            //FunLogButton.P_I.AddInfo("cimAdjust" + g_CimAdjust.Header.ToString(), "Main窗体");
        }

        void OpenRobotAdjView()
        {

        }
    }
}
