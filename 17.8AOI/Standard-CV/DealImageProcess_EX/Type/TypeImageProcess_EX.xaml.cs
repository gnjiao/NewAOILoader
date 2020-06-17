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
using BasicClass;
using System.Xml;
using Common;
using System.Reflection;
using System.IO;
using BasicComprehensive;

namespace DealImageProcess_EX
{
    /// <summary>
    /// TypeLocation.xaml 的交互逻辑
    /// </summary>
    public partial class TypeImageProcess_EX : BaseUCTypeTV
    {
        #region 定义
        #region Path
        public override string PathXml
        {
            get
            {
                return "Type.XmlTemplate.XmlImageProcess_EX.xml";
            }
        }

        #endregion Path

        #region 定义事件

        #endregion 定义事件
        #endregion 定义

        #region 初始化
        public TypeImageProcess_EX()
        {
            InitializeComponent();

            NameClass = "TypeImageProcess_EX";

            TV_Type = tvType;

            InitTV();
        }
        #endregion 初始化

        /// <summary>
        /// 读取指定路径的Stream，为系统资源
        /// </summary>
        public override XmlDocument LoadXmlStream(string Path)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string name = assembly.GetName().Name + ".";
                Stream Stream = assembly.GetManifestResourceStream(name + Path);
                //创建文件,从模板中读取
                XmlDocument xDoc = LoadXml(Stream);
                return xDoc;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
