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
    public class ParLinesDotsPosNegInspect:BaseParImageP_PreImageP
    {
        #region 定义

        #region Path
        public override string PathTemplate
        {
            get
            {
                return "Defect.LinesDotsPosNegInspect.XmlTemplate.XmlFunLinesDotsPosNegInspect.xml";
            }
        }
        #endregion Path

        //基本参数
        // 点状缺陷灰度阈值
        public int DotsTh = 50;
        // 线状缺陷灰度阈值
        public int LinesTh = 35;
        //高级参数
        //填充细小凹陷
        public int ClosingRadius = 1;
        //剔除细小凸起
        public int OpeningRadius = 1;
        //尺寸筛选阈值
        public int SizeTh = 5;
        //点-线状缺陷分离阈值
        public double DotsLinesSeperateTh = 3;
        //正反判定搜索区域上扩
        public double UpExtend = 4;
        //正反判定搜索区域左扩
        public double LeftExtend = 0.5;
        //正反判定搜索区域下扩
        public double DownExtend = 5;
        //正反判定搜索区域右扩
        public double RightExtend = 5;

        //多相机引用
        int noCameraMult = 0;
        public int NoCameraMult//相机序号
        {
            get
            {
                if (noCameraMult==0)
                {
                    noCameraMult = 1;
                }
                return noCameraMult;
            }
            set
            {
                noCameraMult = value;
            }
        }
        public CellReference CellRefImage_Mult = new CellReference();//算法单元

        #endregion 定义

        #region 初始化
        public ParLinesDotsPosNegInspect()
        {
            NameClass = "ParLinesDotsPosNegInspect";
            Annotation = "表面缺陷检测(PN)";
        }

        /// <summary>
        /// 插入算法单元的时候，输入相关的插入参数
        /// </summary>      
        public override void Init(string typeParent, string typeChild, string nameCell, int pos, int noCamera)
        {
            try
            {
                //基类初始化
                base.Init(typeParent, typeChild, nameCell, pos, noCamera, TypeROI_enum.Rectangle1);
                g_ParROI.Add(TypeROI_enum.Rectangle1, new double[] { 50, 50, 150, 150 });
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
            }
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
                    base.NameFunction = "FunLinesDotsPosNegInspect";
                    resultImage_L.AddRange(base.ResultHObject_L);
                    resultImage_L.AddRange(g_ParPreprocess.ResultHObject_L);//添加预处理的图像资源

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ReduceImage",
                        Function = "ReduceImage",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "蒙版图像",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ImageDesReduce",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "目的蒙版图像",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_PNgRegions",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "点状缺陷区域",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_LNgRegions",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "线状缺陷区域",
                    });


                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CameraZWorkR",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "目的图片R通道",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CameraZWorkG",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "目的图片G通道",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CameraZWorkB",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "目的图片B通道",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CameraCTWorkR",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "图片R通道",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CameraCTWorkG",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "图片G通道",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CameraCTWorkB",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "图片B通道",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_OutputRegionsZSizeSelP",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "目的点状阈值区域",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_OutputRegionsCSizeSelP",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "点状阈值区域",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_OutputRegionsZSizeSelL",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "目的线状阈值区域",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_OutputRegionsCSizeSelL",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "线状阈值区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ZPSelectedRegions",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "目的点状区域",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CTPSelectedRegions",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "点状区域",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ZLSelectedRegions",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "目的线状区域",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_CTLSelectedRegions",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "线状区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RectangleZPSearch",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "点状异物搜索区域",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RectangleZLSearch",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "线状异物搜索区域",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RegionDilation",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "ROI扩展区域",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ImageReducedZWorkD",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "正视扩展裁剪区域",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_ImageReducedCTWorkD",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Image,
                        Annotation = "侧视扩展裁剪区域",
                    });


                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_EdgesCTD",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.XLD_L,
                        Annotation = "侧视边缘xld",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_EdgesZD",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.XLD_L,
                        Annotation = "正视边缘xld",
                    });

                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RegionCTD",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "侧视边缘区域",
                    });
                    resultImage_L.Add(new HObjectReference()
                    {
                        NameImage = "ho_RegionZD",
                        Function = "ImageDesReduce",
                        TypeHObject_e = TypeHObject_enum.Region,
                        Annotation = "正视边缘区域",
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

        #region 读xml
        /// <summary>
        /// 初始化加载,基础参数为UI设置单元算法时传入
        /// </summary>
        /// <param name="xDoc">Stream文档</param>
        public override bool ReadXmlRoot(XmlDocument xDoc)
        {
            try
            {
                int numError = 0;
                XmlElement xeRoot = ReadNode(xDoc, "LinesDotsPosNegInspect");

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
                XmlElement xeLinesDotsPosNegInspect = ReadNode(xeRoot, "LinesDotsPosNegInspect");

                //调用基类中的方法，加载基础参数
                if (!ReadXmlChild(xeLinesDotsPosNegInspect))
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

                DotsTh = ReadNodeInt(xePar, "DotsTh");
                LinesTh = ReadNodeInt(xePar, "LinesTh");

                ClosingRadius = ReadNodeInt(xePar, "ClosingRadius");
                OpeningRadius = ReadNodeInt(xePar, "OpeningRadius");

                SizeTh = ReadNodeInt(xePar, "SizeTh");
                DotsLinesSeperateTh = ReadNodeDbl(xePar, "DotsLinesSeperateTh");

                UpExtend = ReadNodeDbl(xePar, "UpExtend");
                LeftExtend = ReadNodeDbl(xePar, "LeftExtend");
                DownExtend = ReadNodeDbl(xePar, "DownExtend");
                RightExtend = ReadNodeDbl(xePar, "RightExtend");

                //多相机
                NoCameraMult = ReadNodeInt(xePar, "NoCameraMult");
                CellRefImage_Mult.ReadXmlRoot(xePar);
                return true;
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError(NameClass, ex);
                return false;
            }
        }
        #endregion 读xml

        #region 写xml
        /// <summary>
        /// 从Stream资源读取完整节点后，写入参数
        /// </summary>
        public override XmlElement WriteXmlRoot(XmlDocument xDoc)
        {
            try
            {
                XmlElement xeRoot = xDoc.CreateElement("LinesDotsPosNegInspect");
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
                XmlElement xeLinesDotsPosNegInspect = CreateNewXe(xeRoot, "LinesDotsPosNegInspect");
                if (!WriteXmlChild(xeLinesDotsPosNegInspect))
                {
                    numError++;
                }
                xeRoot.AppendChild(xeLinesDotsPosNegInspect);
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
                if (!InnerNewText(xePar, "DotsTh", DotsTh.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "LinesTh", LinesTh.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "ClosingRadius", ClosingRadius.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "OpeningRadius", OpeningRadius.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "SizeTh", SizeTh.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "DotsLinesSeperateTh", DotsLinesSeperateTh.ToString()))
                {
                    numError++;
                }

                if (!InnerNewText(xePar, "UpExtend", UpExtend.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "LeftExtend", LeftExtend.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "DownExtend", DownExtend.ToString()))
                {
                    numError++;
                }
                if (!InnerNewText(xePar, "RightExtend", RightExtend.ToString()))
                {
                    numError++;
                }

                //多相机关联
                if (!CellRefImage_Mult.WriteXmlRoot(xePar))
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
        #endregion 写xml
    }
}
