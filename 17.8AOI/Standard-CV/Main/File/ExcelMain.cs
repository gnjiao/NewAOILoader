using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using BasicClass;
using System.IO;
using Common;
using System.Threading.Tasks;

namespace Main
{
    public partial class ExcelMain
    {
        #region 静态类实例
        public static ExcelMain E_I = new ExcelMain();
        #endregion 静态类实例

        #region 定义
        //Path
        string PathRoot
        {
            get
            {
                string path = ComValue.c_PathImageLog + ComConfigPar.C_I.NameModel + "\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }
        /// <summary>
        /// 相机1的excel
        /// </summary>
        string PathOKCamera1
        {
            get
            {
                string root = PathRoot ;
                string path = Log.CreateDayFile(root) + "Image1\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path + ComConfigPar.C_I.NameModel + "OK1.xlsx";
            }
        }

        string PathNGCamera1
        {
            get
            {
                string root = PathRoot;
                string path = Log.CreateDayFile(root) + "NGImage1\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path + ComConfigPar.C_I.NameModel + "NG1.xlsx";
            }
        }

        string PathOKCamera2
        {
            get
            {
                string root = PathRoot;
                string path = Log.CreateDayFile(root) + "Image2\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path + ComConfigPar.C_I.NameModel + "OK2.xlsx";
            }
        }

        string PathNGCamera2
        {
            get
            {
                string root = PathRoot;
                string path = Log.CreateDayFile(root) + "NGImage2\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path + ComConfigPar.C_I.NameModel + "NG2.xlsx";
            }
        }

        //double 
        public double g_DblValueCamera1 = 0;
        public double g_DblValueCamera2 = 0;

        Mutex g_MtExcel1 = new Mutex();
        Mutex g_MtCamera2 = new Mutex();

        Mutex g_MtCloseExcelPro = new Mutex();//关闭线程
        #endregion 定义

        
        public void WriteValue(double value, string path)
        {
            try
            {
                //g_MtExcel1.WaitOne();

                //CloseProcess("EXCEL");

                //string strLog = path;   //获取当前可执行文件的路径

                //Excel.Application xlsApp = new Excel.Application();   //创建Application对象
                //if (xlsApp == null)
                //{
                //    return;
                //}
                //xlsApp.Visible = false;                                //使Excel不可视
                //xlsApp.DisplayAlerts = false;                         //不显示提示对话框
                //xlsApp.AlertBeforeOverwriting = false;

                ////判断是否有excel文件
                //if (File.Exists(strLog) == false)
                //{
                //    // 若无文件，创建该文件  
                //    Excel.Workbook xlsBook = xlsApp.Workbooks.Add(Missing.Value);            //新建Excel工作簿

                //    Excel.Worksheet xlsSheet = (Excel.Worksheet)xlsBook.Worksheets[1];

                //    xlsSheet.Cells[1, 1] = "序号";
                //    xlsSheet.Cells[1, 2] = "检测偏移";
                //    xlsSheet.Cells[1, 3] = "时间";

                //    xlsSheet.Cells[2, 1] = "*********";
                //    xlsSheet.Cells[2, 2] = "*********";
                //    xlsSheet.Cells[2, 3] = "*********";

                //    int rowsNum = xlsSheet.UsedRange.Cells.Rows.Count + 1;

                //    ((Excel.Range)xlsSheet.Cells[rowsNum, 1]).NumberFormatLocal = "@";
                //    xlsSheet.Cells[rowsNum, 1] = (rowsNum - 2);
                //    xlsSheet.Cells[rowsNum, 2] = value;
                //    xlsSheet.Cells[rowsNum, 3] = DateTime.Now.ToLongTimeString().ToString();

                //    xlsBook.SaveAs(@strLog,
                //           Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                //          Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value,
                //          Missing.Value, Missing.Value);

                //    xlsSheet = null;
                //    xlsBook.Close(true, Type.Missing, Type.Missing);
                //    xlsBook = null;

                //}
                //else
                //{
                //    //打开excel文件

                //    Excel.Workbook xlsBook;
                //    object missing = Missing.Value;
                //    xlsBook = xlsApp.Workbooks.Open(@strLog,
                //        missing, missing, missing, missing, missing,
                //        missing, missing, missing, missing, missing,
                //        missing, missing, missing, missing);


                //    Excel.Worksheet xlsSheet = (Excel.Worksheet)xlsBook.Worksheets[1];

                //    int rowsNum = xlsSheet.UsedRange.Cells.Rows.Count + 1;

                //    ((Excel.Range)xlsSheet.Cells[rowsNum, 1]).NumberFormatLocal = "@";
                //    xlsSheet.Cells[rowsNum, 1] = (rowsNum - 2);
                //    xlsSheet.Cells[rowsNum, 2] = value;
                //    xlsSheet.Cells[rowsNum, 3] = DateTime.Now.ToLongTimeString().ToString();

                //    xlsBook.SaveAs(@strLog,
                //       Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                //      Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value,
                //      Missing.Value, Missing.Value);

                //    xlsSheet = null;
                //    xlsBook.Close(true, Type.Missing, Type.Missing);
                //    xlsBook = null;
                //}
                //xlsApp.Quit();
                //xlsApp = null;
            }
            catch (Exception ex)
            {
                CloseProcess("EXCEL");
                Log.L_I.WriteError("EXCEL", ex);
            }
            finally
            {
                g_MtExcel1.ReleaseMutex();
            }
        }


        /// <summary>
        /// 关闭所有Excel进程
        /// </summary>
        /// <param name="P_str_Process">"EXCEL"</param>
        public void CloseProcess(string process)
        {
            try
            {
                g_MtCloseExcelPro.WaitOne();
                Process[] excelProcess = Process.GetProcessesByName(process);//实例化进程对象
                foreach (Process p in excelProcess)
                {
                    p.Kill();//关闭进程
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                g_MtCloseExcelPro.ReleaseMutex();
            }
        }
    }
}
