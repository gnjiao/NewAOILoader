using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using BasicClass;
using DealResult;
using BasicComprehensive;
using DealResult_EX;
using DealImageProcess;

namespace DealImageProcess_EX
{
    [Serializable]
    public partial class ParSmooth : BaseParImageP_PreImageP
    {
        #region 定义
        // 0左下, 1右下，2右上，3左上
        public string Position = "左下";
        //处理方式
        //残留检测-Outer
        //崩缺检测-Inner
        public string DefectType = "崩缺";

      
        //迭代次数（通过拟合多次，逐层逼近边界）
        public int Num = 10;

        //提取边界的阈值和平滑系数
        public int SmoothValue = 101;

        //轮廓面积阈值
        public int SelectAreaLow = 50;

        //轮廓间距，内 外
        public double GapIn = 1;
        public double GapOut = 30;

        //缺陷面积 阈值和膨胀剔除阈值
        public int ErrorAreaValue = 2;
        public double DilationDefectValue = 2;//进行剔除时进行膨胀

        public double NormalTh = 0;//缺陷阈值卡控
        #endregion 定义
       
        #region 处理过程中产生的Hobject
        /// <summary>
        /// 输出数据结果列表
        /// </summary>
        public override List<HObjectReference> ResultHObject_L
        {
            get
            {
                try
                {
                    List<HObjectReference> resultImage_L = new List<HObjectReference>();
                    base.NameFunction = "FunSmooth";
                    //resultImage_L.AddRange(base.ResultHObject_L);
                    //resultImage_L.AddRange(g_ParPreprocess.ResultHObject_L);//添加预处理的图像资源

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ImageBinary",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "输入的二值化图像",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ROI",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "ROI区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_FilterROI",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "台阶面崩缺剔除ROI区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_rectErosion1",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "ROI内缩区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RegionDilation",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "内缩区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_BorderOrigin",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.XLD_Cont,
                        Annotation = "原始边界轮廓",
                    });

                   
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_SmoothedBoaderOriginal",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.XLD_Cont,
                        Annotation = "平滑原始边界轮廓",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ContourConcat",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.XLD_Cont,
                        Annotation = "平行轮廓",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RegionParall",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "平行轮廓区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RegionError",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "细小缺陷",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RegionErosionError",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "剔除细小缺陷",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RegionDilationError",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "膨胀缺陷",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RegionDilationError",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "剔除残留",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_BinImage",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "完整目标图片",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_BorderFull",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.XLD_Cont,
                        Annotation = "完整轮廓",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_SmoothedContoursFull",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.XLD_Cont,
                        Annotation = "完整轮廓平滑s",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_SmoothedContoursFullLongest",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.XLD_Cont,
                        Annotation = "完整轮廓平滑",
                    });

                    //求取缺陷区域
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RegionROIFull",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "缺陷求取ROIFull",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ErrorConnectFull",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "缺陷求取RegionFull",
                    });
                    //
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_FilterErrorFull",
                        Function = "FunSmooth",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "滤除掉台阶面崩缺的Image",
                    });

                    //对列表进行排序
                    SortResultHObjectNo(resultImage_L);
                    return resultImage_L;
                }
                catch (Exception ex)
                {
                    Log.L_I.WriteError(NameClass, ex);
                    return null;
                }
            }
        }
        #endregion 处理过程中产生的Hobject

        #region 读Xml
        /// <summary>
        /// 初始化加载,基础参数为UI设置单元算法时传入
        /// </summary>
        /// <param name="xDoc">Stream文档</param>
        /// <returns></returns>  
        public override bool ReadXmlRoot(XmlDocument xDoc)
        {
            try
            {
                int numError = 0;
                XmlElement xeRoot = ReadNode(xDoc, "Smooth");

                //读取子元素参数
                if (!ReadXmlChildInit(xeRoot))
                {
                    numError++;
                }
                if (numError > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 从本地文档读取参数的时候，读取
        /// </summary>
        /// <param name="xeRoot">根节点</param>
        /// <returns>是否成功</returns>
        public override bool ReadXmlRoot(XmlElement xeRoot)
        {
            try
            {
                int numError = 0;
                XmlElement xeScaledShape = ReadNode(xeRoot, "Smooth");

                //调用基类中的方法，加载基础参数
                if (!ReadXmlChild(xeScaledShape))
                {
                    numError++;
                }
                if (numError > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 读取算法参数
        /// </summary>
        /// <param name="xeRoot">算法参数根节点</param>
        /// <returns>成功</returns>
        public override bool ReadXmlPar(XmlElement xeRoot)
        {
            try
            {
                XmlElement xePar = ReadNode(xeRoot, "Par");
    
                Position = ReadNodeStr(xePar, "Position");
             
                Num = ReadNodeInt(xePar, "Num");
          
                SmoothValue = ReadNodeInt(xePar, "SmoothValue");//平滑
                SelectAreaLow = ReadNodeInt(xePar, "SelectAreaLow");//缺陷疑似区域剔除阈值
                ErrorAreaValue = ReadNodeInt(xePar, "ErrorAreaValue");//缺陷卡控尺寸规格

                GapIn = ReadNodeDbl(xePar, "GapIn");
                GapOut = ReadNodeDbl(xePar, "GapOut");

                DilationDefectValue = ReadNodeDbl(xePar, "DilationDefectValue");
                
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        #endregion 读Xml

        #region 写Xml
        /// <summary>
        /// 从Stream资源读取完整节点后，写入参数
        /// </summary>
        /// <param name="xDoc"></param>
        /// <returns></returns>
        public override XmlElement WriteXmlRoot(XmlDocument xDoc)
        {
            try
            {
                XmlElement xeRoot = xDoc.CreateElement("Smooth");
                if (WriteXmlChild(xeRoot))
                {
                    return xeRoot;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return null;
            }
        }

        /// <summary>
        /// 从Stream资源读取完整节点后，写入参数
        /// </summary>
        /// <param name="xDoc"></param>
        /// <returns></returns>
        public override bool WriteXmlRoot(XmlElement xeRoot)
        {
            try
            {
                int numError = 0;
                XmlElement xeSmooth = CreateNewXe(xeRoot, "Smooth");
                if (!WriteXmlChild(xeSmooth))
                {
                    numError++;
                }
                xeRoot.AppendChild(xeSmooth);
                if (numError > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }

        /// <summary>
        /// 该算法参数
        /// </summary>
        /// <param name="xeRoot"></param>
        /// <returns></returns>
        public override bool WriteXmlPar(XmlElement xeRoot)
        {
            int numError = 0;
            try
            {
                XmlElement xePar = CreateNewXe(xeRoot, "Par");            

                if (!InnerNewText(xePar, "Position", Position.ToString()))
                {
                    numError++;
                }
      
                if (!InnerNewText(xePar, "Num", Num.ToString()))
                {
                    numError++;
                }              

                if (!InnerNewText(xePar, "SmoothValue", SmoothValue.ToString()))
                {
                    numError++;
                }

                if (!InnerNewText(xePar, "SelectAreaLow", SelectAreaLow.ToString()))
                {
                    numError++;
                }

                //缺陷卡控规格
                if (!InnerNewText(xePar, "ErrorAreaValue", ErrorAreaValue.ToString()))
                {
                    numError++;
                }
                //缺陷卡膨胀值，
                if (!InnerNewText(xePar, "DilationDefectValue", DilationDefectValue.ToString()))
                {
                    numError++;
                }
             
                if (!InnerNewText(xePar, "GapIn", GapIn.ToString()))
                {
                    numError++;
                }

                if (!InnerNewText(xePar, "GapOut", GapOut.ToString()))
                {
                    numError++;
                }
                xeRoot.AppendChild(xePar);
                if (numError > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        #endregion 写Xml
    }
}
