using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    /// <summary>
    /// 针对二维定位做的一个数据类
    /// <para>二维定位，主要涉及三个轴，xyr</para>
    /// <para>xy的系数解属于二元一次方程，所以参数有a1b1a2b2</para>
    /// <para>r的系数解，目前就通过amp来调节</para>
    /// </summary>
    [Serializable]    
    public class AxisModel
    {
        private int _uniqueId = 0;
        public int UniqueId
        {
            get => _uniqueId;
            set
            {
                _uniqueId = value;
            }
        }

        private double _a1 = 0;
        public double A1
        {
            get => _a1;
            set
            {
                _a1 = value;
            }
        }

        private double _a2 = 0;
        public double A2
        {
            get => _a2;
            set
            {
                _a2 = value;
            }
        }

        private double _b1 = 0;
        public double B1
        {
            get => _b1;
            set
            {
                _b1 = value;
            }
        }

        private double _b2 = 0;
        public double B2
        {
            get => _b2;
            set
            {
                _b2 = value;
            }
        }

        private double _amp = 0;
        /// <summary>
        /// 系数
        /// </summary>
        public double AMP
        {
            get => _amp;
            set
            {
                _amp = value;
            }
        }
    }
}
