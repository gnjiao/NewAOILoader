using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DealRobot;
using DealPLC;
using Common;
using SetPar;
using DealFile;
using BasicClass;
using DealConfigFile;

namespace Main
{
    /// <summary>
    /// 机器人触发保留响应
    /// </summary>
    partial class MainWindow
    {
        #region 实现方法选择
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="index"></param>
        protected override void R_Inst_Others_event(int index)
        {
            switch (index)
            {
                case 1:
                    RobotOthers1();
                    break;

                case 2:
                    RobotOthers2();
                    break;

                case 3:
                    RobotOthers3();
                    break;

                case 4:
                    RobotOthers4();
                    break;

                case 5:
                    RobotOthers5();
                    break;

                case 6:
                    RobotOthers6();
                    break;
            }

            //再次判断选择
            if (index > 4)
            {

            }
        }
        #endregion 实现方法选择

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RobotOthers1()
        {
            try
            {
                ShowState("双目清空标志位");
                BaseDealComprehensiveResult_Main.Camera1Done = false;
                BaseDealComprehensiveResult_Main.Camera2Done = false;
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RobotOthers2()
        {
            try
            {
                
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RobotOthers3()
        {
            try
            {
                
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RobotOthers4()
        {
            try
            {
               
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        void SendPreciseGlassDataAndCode(int index)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RobotOthers5()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RobotOthers6()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        
    }
}
