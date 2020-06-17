using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealResult;

namespace DealResult_EX
{
    [Serializable]
    public class ResultLinesDotsPosNegInspect : BaseResultRegion
    {
        //是否有线状NG
        public bool blLNg = false;
        //是否有点状NG
        public bool blPNg = false;
        //点状NG的坐标
        public List<double> XPNg_L = new List<double>();
        public List<double> YPNg_L = new List<double>();
        //线状NG的坐标
        public List<double> XLNg_L = new List<double>();
        public List<double> YLNg_L = new List<double>();
    }
}
