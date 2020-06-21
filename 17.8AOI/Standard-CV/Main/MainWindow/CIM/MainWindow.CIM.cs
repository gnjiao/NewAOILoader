using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicClass;
using System.Threading;
using DealFile;
using DealConfigFile;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DealPLC;
using Common;
using System.Text.RegularExpressions;
using DealCIM;

namespace Main
{
    partial class MainWindow
    {
        public QRCodeBase Code = null;
        const int OK = 1;
        const int NG = 2;
        /// <summary>
        /// 新建一个Task来初始化CIM端口
        /// </summary>
        public override void Init_CIM()
        {

            PostParams.P_I.InitParams();
            new Task(new Action(() =>
            {
                try
                {
                    ConnectCode();
                    //ConnectCIM();
                }
                catch (Exception ex)
                {
                    Log.L_I.WriteError(NameClass, ex);
                }
            })).Start();
        }

        /// <summary>
        /// 注册
        /// </summary>
        public override void LoginEvent_CIM()
        {
            try
            {
                QRCodeBase.GetData_event += new StrAction(GetCodeEvent);

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        void ConnectCode()
        {
            try
            {
                if (!Protocols.DefaultQrCodeOK)
                {
                    Code = CodeFactory.Instance.GetCodeType(PostParams.P_I.ETypeCode);
                    if (Code.Init(PostParams.P_I.StrCom, PostParams.P_I.iBaudrate))
                        ShowState("二维码初始化成功");
                    else
                        ShowAlarm("二维码初始化失败");
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        /// <summary>
        /// 关闭CIM端口
        /// </summary>
        public override void CloseCIM()
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
