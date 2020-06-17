using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Station
{
    [Serializable]
    public class StationModel:ICloneable
    {
        private bool _isCalibed = false;
        /// <summary>
        /// 当前工位是否标定过
        /// </summary>
        public bool IsCalibed
        {
            get => _isCalibed;
            set
            {
                _isCalibed = value;
            }
        }

        private bool _isTeached = false;
        /// <summary>
        /// 当前工位是否示教过
        /// </summary>
        public bool IsTeached
        {
            get => _isTeached;
            set
            {
                _isTeached = value;
            }
        }

        private int _index = 0;
        /// <summary>
        /// 数据索引，一般为工位号
        /// </summary>
        public int Index
        {
            get => _index;
            set
            {
                _index = value;
            }
        }

        private double _stdX = 0;
        /// <summary>
        /// 基准值x
        /// </summary>
        public double StdX
        {
            get => _stdX;
            set
            {
                _stdX = value;
            }
        }

        private double _stdY = 0;
        /// <summary>
        /// 基准值y
        /// </summary>
        public double StdY
        {
            get => _stdY;
            set
            {
                _stdY = value;
            }
        }

        private double _stdZ = 0;
        /// <summary>
        /// 基准值z
        /// </summary>
        public double StdZ
        {
            get => _stdZ;
            set
            {
                _stdZ = value;
            }
        }

        private double _stdR = 0;
        /// <summary>
        /// 基准值r
        /// </summary>
        public double StdR
        {
            get => _stdR;
            set
            {
                _stdR = value;
            }
        }

        private double _calibX = 0;
        /// <summary>
        /// 标定值x
        /// </summary>
        public double CalibX
        {
            get=> _calibX;
            set
            {
                _calibX = value;
            }
        }

        private double _calibY = 0;
        /// <summary>
        /// 标定值y
        /// </summary>
        public double CalibY
        {
            get => _calibY;
            set
            {
                _calibY = value;
            }
        }

        private double _calibR = 0;
        /// <summary>
        /// 标定值r
        /// </summary>
        public double CalibR
        {
            get => _calibR;
            set
            {
                _calibR = value;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
