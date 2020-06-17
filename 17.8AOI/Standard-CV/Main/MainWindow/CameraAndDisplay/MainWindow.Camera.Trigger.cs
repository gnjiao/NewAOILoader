
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ControlLib;
using Common;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Camera;
using HalconDotNet;
using DealFile;
using BasicClass;
using DealConfigFile;
using SetPar;
using DealPLC;
using DealDisplay;

namespace Main
{
    partial class MainWindow
    {
        ///相机触发模式产生的事件
        ///
        /// 
        /// <summary>
        /// 相机1
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected override void DealComprehensive_Camera1Trigger_event(TriggerSource_enum trigerSource_e, int i)
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
        /// 相机2
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected override void DealComprehensive_Camera2Trigger_event(TriggerSource_enum trigerSource_e, int i)
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
        /// 相机3
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected override void DealComprehensive_Camera3Trigger_event(TriggerSource_enum trigerSource_e, int i)
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
        /// 相机4
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected override void DealComprehensive_Camera4Trigger_event(TriggerSource_enum trigerSource_e, int i)
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
        /// 相机5
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected override void DealComprehensive_Camera5Trigger_event(TriggerSource_enum trigerSource_e, int i)
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
        /// 相机6
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected override void DealComprehensive_Camera6Trigger_event(TriggerSource_enum trigerSource_e, int i)
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
        /// 相机7
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected override void DealComprehensive_Camera7Trigger_event(TriggerSource_enum trigerSource_e, int i)
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
        /// 相机8
        /// </summary>
        /// <param name="trigerSource_e"></param>
        /// <param name="i"></param>
        protected override void DealComprehensive_Camera8Trigger_event(TriggerSource_enum trigerSource_e, int i)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

    }
}
