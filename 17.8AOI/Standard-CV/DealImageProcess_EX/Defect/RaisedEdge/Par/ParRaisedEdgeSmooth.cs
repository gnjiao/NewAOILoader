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
    public class ParRaisedEdgeSmooth : BaseParImageP_PreImageP
    {
        #region 定义
        #region Path
        public override string PathTemplate
        {
            get
            {
                return "Defect.RaisedEdge.XmlTemplate.XmlRaisedEdge.xml";
            }
        }
        #endregion Path

        public ParRaisedEdge g_ParRaisedEdge = new ParRaisedEdge();
        public ParSmooth g_ParSmooth = new ParSmooth();

        #endregion 定义

        #region 初始化
        public ParRaisedEdgeSmooth()
        {
            NameClass = "ParRaisedEdge";
            Annotation = "边缘检测";

            //g_BaseResult = new ResultRaisedEdge();//结果类
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
                g_ParROI.Add(TypeROI_enum.Rectangle1, new double[] { 50, 50,  150, 150 });
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
                    resultImage_L.AddRange(base.ResultHObject_L);
                    resultImage_L.AddRange(g_ParPreprocess.ResultHObject_L);//添加预处理的图像资源
                    resultImage_L.AddRange(g_ParRaisedEdge.ResultHObject_L);
                    resultImage_L.AddRange(g_ParSmooth.ResultHObject_L);
        
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
            int numError = 0;
            try
            {               
                XmlElement xeRoot = ReadNode(xDoc, "RaisedEdgeSmooth");

                ////创建预处理
                //g_ParPreprocess.CreatePreprocess("二值化", 0);
                //读取子元素参数
                if (!ReadXmlChildInit(xeRoot))
                {
                    numError++;
                }
                if (!g_ParPreprocess.ReadXmlRoot(xeRoot))
                {
                    numError++;
                }
                g_ParPreprocess.g_BaseParImageProcess_L[0].Init(this.TypeParent, "二值化", this.NameCell, this.Pos, this.NoCamera);
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
                XmlElement xeRaisedEdgeSmooth = ReadNode(xeRoot, "RaisedEdgeSmooth");
                //调用基类中的方法，加载基础参数
                if (!ReadXmlChild(xeRaisedEdgeSmooth))
                {
                    numError++;
                }
                //校准
                if (!g_CellCalibReference.ReadXmlRoot(xeRaisedEdgeSmooth))
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
        public override bool ReadXmlPar(XmlElement xeRoot)
        {
            int numError = 0;
            try
            {

                XmlElement xeRaisedEdge = ReadNode(xeRoot, "RaisedEdge");
                XmlElement xeSmooth = ReadNode(xeRoot, "Smooth");
                //调用基类中的方法，加载基础参数
                if (!g_ParRaisedEdge.ReadXmlPar(xeRaisedEdge))
                {
                    numError++;
                }
                if (!g_ParSmooth.ReadXmlPar(xeSmooth))
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
        #endregion 读Xml

        #region 写Xml
        /// <summary>
        /// 从Stream资源读取完整节点后，写入参数
        /// </summary>
        public override XmlElement WriteXmlRoot(XmlDocument xDoc)
        {
            try
            {
                XmlElement xeRoot = xDoc.CreateElement("RaisedEdgeSmooth");
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
                XmlElement xeRaisedEdgeSmooth = CreateNewXe(xeRoot, "RaisedEdgeSmooth");

                if (!WriteXmlChild(xeRaisedEdgeSmooth))
                {
                    numError++;
                }

                if (!g_CellCalibReference.WriteXmlRoot(xeRaisedEdgeSmooth))
                {
                    numError++;
                }

                xeRoot.AppendChild(xeRaisedEdgeSmooth);
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
                XmlElement xeRaisedEdge = CreateNewXe(xeRoot, "RaisedEdge");
                XmlElement xeSmooth = CreateNewXe(xeRoot, "Smooth");
                xeRoot.AppendChild(xeRaisedEdge);
                xeRoot.AppendChild(xeSmooth);
                if (!g_ParRaisedEdge.WriteXmlPar(xeRaisedEdge))
                {
                    numError++;
                }

                if (!g_ParSmooth.WriteXmlPar(xeSmooth))
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
        #endregion 写Xml
    }
}
