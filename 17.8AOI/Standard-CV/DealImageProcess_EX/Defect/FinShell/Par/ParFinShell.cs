using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using BasicClass;
using DealResult;
using BasicComprehensive;
using DealImageProcess;

namespace DealImageProcess_EX
{
    [Serializable]
    public class ParFinShell : BaseParImageP_PreImageP
    {
        #region 定义
        public string NameCellActualEdge = "C8";//当前片的平滑轮廓
        public int Width_Paral = 10;
        public double OuterStdShift_Paral = 0;
        public double InnerStdShift_Paral = 0;
        //工作区域
        //残留检测-Outer
        //崩缺检测-Inner
        public string WorkingRegion = "Outer";

        public int ClosingCircle = 1;
        public int OpeningCircle = 1;

        public int MinWidth = 1;
        public int MaxWidth = 99999;
        public int MinHeight = 1;
        public int MaxHeight = 99999;
        public int MinArea = 1;
        public int MaxArea = 99999;

        public SmallestSurround_enum SmallestSurround_e = SmallestSurround_enum.Rect1; //最小包络
        //string
        public string TypeOutCoord = "";//输出坐标类型
        #endregion 定义

        #region 初始化
        public ParFinShell()
        {
            NameClass = "ParFinShell";
            Annotation = "边缘异常检查";
        }
        #endregion 初始化

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
                    base.NameFunction = "FunFinShell";
                    resultImage_L.AddRange(base.ResultHObject_L);
                    resultImage_L.AddRange(g_ParPreprocess.ResultHObject_L);//添加预处理的图像资源

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionReduced",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionInner",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionOuter",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionInnerOuter",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });


                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionIntersected",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionFillUp",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });


                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionOpening",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionClosing",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionWidthSelected",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });


                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionHeightSelected",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionAreaSelected",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionUnion1",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionUnion2",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "RegionFinShell",
                        Function = "FunFinShell",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
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
                XmlElement xeRoot = ReadNode(xDoc, "FinShell");

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
                XmlElement xeScaledShape = ReadNode(xeRoot, "FinShell");

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

                if (!g_CellCalibReference.ReadXmlRoot(xePar))
                {

                }

                //真实轮廓单元格
                NameCellActualEdge = ReadNodeStr(xePar, "NameCellActualEdge");

                Width_Paral = ReadNodeInt(xePar, "Width_Paral");
                OuterStdShift_Paral = ReadNodeDbl(xePar, "OuterStdShift_Paral");
                InnerStdShift_Paral = ReadNodeDbl(xePar, "InnerStdShift_Paral");

                WorkingRegion = ReadNodeStr(xePar, "WorkingRegion");

                ClosingCircle = ReadNodeInt(xePar, "ClosingCircle");
                OpeningCircle = ReadNodeInt(xePar, "OpeningCircle");
                MinWidth = ReadNodeInt(xePar, "MinWidth");
                MaxWidth = ReadNodeInt(xePar, "MaxWidth");
                MinHeight = ReadNodeInt(xePar, "MinHeight");
                MaxHeight = ReadNodeInt(xePar, "MaxHeight");
                MinArea = ReadNodeInt(xePar, "MinArea");
                MaxArea = ReadNodeInt(xePar, "MaxArea");

                string str = ReadChildNodeEnum(xePar, "SmallestSurround", "Color");
                SmallestSurround_e = (SmallestSurround_enum)Enum.Parse(typeof(SmallestSurround_enum), str);

                TypeOutCoord = ReadNodeStr(xePar, "TypeOutCoord", "面积中心");

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
                XmlElement xeRoot = xDoc.CreateElement("FinShell");
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
                XmlElement xeScaledShape = CreateNewXe(xeRoot, "FinShell");
                if (!WriteXmlChild(xeScaledShape))
                {
                    numError++;
                }
                xeRoot.AppendChild(xeScaledShape);
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

                //算法参数
                if (!InnerNewText(xePar, "NameCellActualEdge", NameCellActualEdge.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "Width_Paral", Width_Paral.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "OuterStdShift_Paral", OuterStdShift_Paral.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "InnerStdShift_Paral", InnerStdShift_Paral.ToString()))
                {
                    numError++;
                }

                if (!InnerNewText(xePar, "WorkingRegion", WorkingRegion.ToString()))
                {
                    numError++;
                }

                if (!InnerNewText(xePar, "ClosingCircle", ClosingCircle.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "OpeningCircle", OpeningCircle.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "MinWidth", MinWidth.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "MaxWidth", MaxWidth.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "MinHeight", MinHeight.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "MaxHeight", MaxHeight.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "MinArea", MinArea.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "MaxArea", MaxArea.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "SmallestSurround", SmallestSurround_e.ToString()))
                {
                    numError++;
                }

                if (!InnerNewText(xePar, "TypeOutCoord", TypeOutCoord.ToString()))
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
