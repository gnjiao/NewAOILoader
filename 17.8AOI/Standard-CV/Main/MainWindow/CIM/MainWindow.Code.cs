using BasicClass;
using DealPLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    partial class MainWindow
    {
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
    }
}
