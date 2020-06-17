using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalconDotNet;
using DealFile;
using System.Xml;
using Common;
using BasicClass;
using DealImageProcess;

namespace DealImageProcess_EX
{
    [Serializable]
    public class BaseParDefect : BaseParImageP_PreImageP
    {
        #region 定义
        //灰度值
        public double MinGray { get; set; }
        public double MaxGray { get; set; }
        public double MinArea { set; get; }//面筋筛选
        public double MaxArea { set; get; }
        public double OpenRadius { set; get; }//开运算
        public double CloseRadius { set; get; }//闭运算

        public double DblMinCircularity { set; get; }//圆度
        public double DblMaxCircularity { set; get; }//圆度

        public double DblMinRectangularity { set; get; }//矩形度
        public double DblMaxRectangularity { set; get; }//矩形度

        public double DblMinWidth { get; set; }
        public double DblMaxWidth { get; set; }

        public double DblMinHeight { get; set; }
        public double DblMaxHeight { get; set; }

        public double DblMinX { get; set; }
        public double DblMaxX{ get; set; }
        
        public double DblMinY { get; set; }
        public double DblMaxY { get; set; }
      
        #endregion 定义

        #region 读Xml        
      
        #endregion 读Xml

        #region 写Xml
       
        #endregion 写Xml
    }
}
