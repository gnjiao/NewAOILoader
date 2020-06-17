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
    public class ParRaisedEdge : BaseParImageP_PreImageP
    {
        #region 定义
        // 0 polygon 折线,1 arc 弧线,
        public int OutlineType = 0;
        public string Position = "左下";

        //工作区域
        //残留检测-Outer
        //崩缺检测-Inner
        public string DefectType = "Fin";
        public string NameCellPolygon1 = "";
        public string NameCellPolygon2 = "";


        //public int ErosionMatch = 0;
        //public int AreaErrorMatch = 0;

        #endregion 定义

        #region 初始化
        public ParRaisedEdge()
        {
            NameClass = "ParRaisedEdge";
            Annotation = "边缘凸起";
            //g_BaseResult = new ResultRaisedEdge();//结果类
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
                    base.NameFunction = "FunRaisedEdge";
                    //resultImage_L.AddRange(base.ResultHObject_L);
                    //resultImage_L.AddRange(g_ParPreprocess.ResultHObject_L);//添加预处理的图像资源

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CcutRegionReduceXld",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.XLD_L,
                        Annotation = "C角区域缩减的xld",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CcutRegionReduceRegion",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "C角区域缩减的Region",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CcutRegionReduceImage",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "C角区域缩减后的图像",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_MLineQuadRegion",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "M直线的四边形区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_UpQuadPaintImage",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "M直线四边形填充后的图像",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_binaryArea",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "上下二值化合并区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_binaryImage",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_binaryAreaUp",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "上二值化区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_binaryImageUp",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_binaryAreaDown",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "上二值化区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_binaryImageDown",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "",
                    });

                    //通过匹配轮廓求取缺陷
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RegionMatch",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ErrorMatch",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ErrorMatchConnectioin",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ErrorMatchSelect",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CircleEdges",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.XLD_L,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_LongestContour",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.XLD_L,
                        Annotation = "",
                    });


                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ContourA",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.XLD_L,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CircleRegion",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_TriCirIntersectRegion",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "缺陷求取选区",
                    });




                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_InterestROI1",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "内切区域1的ROI",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_TriCirRegionConnect1",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_TriCirRegionIntersection1",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_TriCirBiggested1",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "与内切区域1相交的最大残留提取区域",
                    });


                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_InterestROI2",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "内切区域2的ROI",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_TriCirRegionConnect2",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_TriCirRegionIntersection2",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_TriCirBiggested2",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "与内切区域2相交的最大残留提取区域",
                    });


                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_TriCirReduced",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_TriCirReducedThRegion",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_TriCirReducedContours",
                        Function = "FunRaisedEdge",
                        TypeHObject_e = TypeHObject_enum.XLD_L,
                        Annotation = "缺陷选区",
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
        public override bool ReadXmlRoot(XmlDocument xDoc)
        {
            try
            {
                int numError = 0;
                XmlElement xeRoot = ReadNode(xDoc, "RaisedEdge");

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
            int numError = 0;
            try
            {
                XmlElement xeRaisedEdge = ReadNode(xeRoot, "RaisedEdge");

                //调用基类中的方法，加载基础参数
                if (!ReadXmlChild(xeRaisedEdge))
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

                NameCellPolygon1 = ReadNodeStr(xePar, "NameCellPolygon1");
                NameCellPolygon2 = ReadNodeStr(xePar, "NameCellPolygon2");

                OutlineType = ReadNodeInt(xePar, "OutlineType");
                DefectType = ReadNodeStr(xePar, "DefectType");

                //检测位置
                Position = ReadNodeStr(xePar, "Position");
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
        public override XmlElement WriteXmlRoot(XmlDocument xDoc)
        {
            try
            {
                XmlElement xeRoot = xDoc.CreateElement("RaisedEdge");
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
                XmlElement xeRaisedEdge = CreateNewXe(xeRoot, "RaisedEdge");
                if (!WriteXmlChild(xeRaisedEdge))
                {
                    numError++;
                }
                xeRoot.AppendChild(xeRaisedEdge);
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
                if (!InnerNewText(xePar, "OutlineType", OutlineType.ToString()))
                {
                    numError++;
                }
                //检测位置
                if (!InnerNewText(xePar, "Position", Position.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "DefectType", DefectType.ToString()))
                {
                    numError++;
                }

                if (!InnerNewText(xePar, "NameCellPolygon1", NameCellPolygon1.ToString()))
                {
                    numError++;
                }

                if (!InnerNewText(xePar, "NameCellPolygon2", NameCellPolygon2.ToString()))
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
