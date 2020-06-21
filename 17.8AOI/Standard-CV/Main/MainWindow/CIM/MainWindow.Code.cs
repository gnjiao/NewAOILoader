using BasicClass;
using DealCIM;
using DealPLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Main
{
    partial class MainWindow
    {
        /// <summary>
        /// 二维码读取事件处理程序
        /// </summary>
        /// <param name="strCode"></param>
        void GetCodeEvent(string strCode)
        {
            try
            {
                //string chipid = Regex.Replace(strCode, "[^a-z0-9]", "", RegexOptions.IgnoreCase);
                string chipid = Regex.Match(strCode, @"[A-z0-9]+-?[A-z0-9]+").ToString();
                if (chipid == "")
                {
                    RegeditMain.R_I.CodeArm = "FAILED";
                    ShowState("读码失败,Arm持有ChipID:" + RegeditMain.R_I.CodeArm);
                    ShowAlarm("二维码读取失败");
                    if (Protocols.IfPassCodeNG)
                    {
                        SendCodeResult(OK);
                        ShowState("启用PASS读码失败，不抛料");
                    }
                    else
                    {
                        SendCodeResult(NG);
                    }
                    return;
                }
                else if (chipid.Length < 5)
                {
                    RegeditMain.R_I.CodeArm = chipid;
                    ShowState("二维码长度与设定不符,Arm持有ChipID:" + RegeditMain.R_I.CodeArm);
                    ShowAlarm("二维码长度与设定不符");
                    if (Protocols.IfPassCodeNG)
                    {
                        SendCodeResult(OK);
                        ShowState("启用PASS读码失败，不抛料");
                    }
                    else
                    {
                        SendCodeResult(NG);
                    }
                    return;
                }
                else
                {
                    SendCodeResult(OK);
                    RegeditMain.R_I.CodeArm = chipid;
                    ShowState("读码成功,Arm持有ChipID:" + RegeditMain.R_I.CodeArm);

                    if (CIM.CheckDup(RegeditMain.R_I.CodeArm))
                    {
                        ShowAlarm("读取到重复ChipID：" + RegeditMain.R_I.CodeArm);
                        LogicPLC.L_I.WriteRegData1((int)DataRegister1.CodeResult, NG);
                        return;
                    }
                }


                WriteCodeToRegister((int)DataRegister1.Code, chipid);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
        }

        public void WriteCodeToRegister(int index, string code)
        {
            try
            {
                const int len = 5;
                if (code.Length > len * 4 - 2)
                {
                    ShowAlarm("二维码长度超出指定范围，无法写入寄存器");
                    return;
                }

                double[] array = new double[len];
                int[] ratio = new int[] { 1, (int)Math.Pow(2, 8),
                    (int)Math.Pow(2, 16), (int)Math.Pow(2, 24) };

                array[0] = code.Length;
                for (int i = 0; i < code.Length; ++i)
                {
                    array[(i + 2) / 4] += (int)code[i] * ratio[(i + 2) % 4];
                }

                LogicPLC.L_I.WriteRegData1(index, len, array);
                ShowState("发送二维码到寄存器");
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("Code", ex);
                ShowState("二维码发送到寄存器失败");
            }
        }

        void SendCodeResult(int result)
        {
            try
            {

                LogicPLC.L_I.WriteRegData1((int)DataRegister1.CodeResult, result);

            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
        }

        /// <summary>
        /// 外部触发读码，区分plc和robot
        /// </summary>
        public void TriggerGetCode(bool isPLC)
        {
            try
            {
                ShowState("触发读码");
                if (!Protocols.DefaultQrCodeOK)
                {
                    Thread.Sleep(Protocols.CodeWaitTimeBefore);
                    Code.Write();
                    Thread.Sleep(Protocols.CodeWaitTimeAfter);
                    Code.StartMonitor(PostParams.P_I.iCodeDelay);
                }
                else
                {
                    if (isPLC)
                    {
                        ShowState("屏蔽二维码，默认OK");
                        new Task(() =>//直接吹会拖慢PLC读取线程
                        {
                            //为了在调机阶段准确估算节拍
                            Thread.Sleep(500);
                            LogicPLC.L_I.WriteRegData1((int)DataRegister1.CodeResult, 1);

                        }).Start();

                    }
                    else
                    {
                        SendCodeResult(OK);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);

            }
        }
    }
}
