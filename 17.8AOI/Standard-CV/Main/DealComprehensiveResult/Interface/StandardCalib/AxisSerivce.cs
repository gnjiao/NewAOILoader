using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class AxisSerivce
    {
        #region singleton
        static readonly object _locker = new object();
        static AxisSerivce _instance = null;
        public static AxisSerivce GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                        _instance = new AxisSerivce();
                }
            }
            return _instance;
        }

        private AxisSerivce()
        {

        }
        #endregion

        #region properties
        const string Path = @"D:\Store\Custom\Axis\axis.xml";
        #endregion

        ObservableCollection<AxisModel> _axisParam = null;

        public void SetA1(int axisId, double a1)
        {
            var axis = _axisParam
                .Where(p => p.UniqueId == axisId).ToArray().FirstOrDefault();
            if (axis == null) _axisParam.Add(new AxisModel { UniqueId = axisId });

            axis.A1 = a1;
            Save();
        }

        public void SetB1(int axisId, double b1)
        {
            var axis = _axisParam
                .Where(p => p.UniqueId == axisId).ToArray().FirstOrDefault();
            if (axis == null) _axisParam.Add(new AxisModel { UniqueId = axisId });

            axis.B1 = b1;
            Save();
        }


        public void SetA2(int axisId, double a2)
        {
            var axis = _axisParam
                .Where(p => p.UniqueId == axisId).ToArray().FirstOrDefault();
            if (axis == null) _axisParam.Add(new AxisModel { UniqueId = axisId });

            axis.A2 = a2;
            Save();
        }

        public void SetB2(int axisId, double b2)
        {
            var axis = _axisParam
                .Where(p => p.UniqueId == axisId).ToArray().FirstOrDefault();
            if (axis == null) _axisParam.Add(new AxisModel { UniqueId = axisId });

            axis.B2 = b2;
            Save();
        }

        public void SetAMP(int axisId, double amp)
        {
            var axis = _axisParam
                .Where(p => p.UniqueId == axisId).ToArray().FirstOrDefault();
            if (axis == null) _axisParam.Add(new AxisModel { UniqueId = axisId });

            axis.AMP = amp;
            Save();
        }

        public double[] GetXYValues(int axisId, double[] offset)
        {
            if (offset.Length != 2) return new double[2];
            var axis = _axisParam
                .Where(p => p.UniqueId == axisId).ToArray().FirstOrDefault();
            if (axis == null) return new double[2];
            double[] result = new double[2];

            result[0] = (offset[0] * axis.B2 - offset[1] * axis.A2)
                / (axis.B2 * axis.A1 - axis.B1 * axis.A2);
            result[1] = (offset[0] * axis.B1 - offset[1] * axis.A1)
                / -(axis.B2 * axis.A1 - axis.B1 * axis.A2);
            return result;
        }

        public double GetAMP(int axisId)
        {
            var axis = _axisParam
                .Where(p => p.UniqueId == axisId).ToArray().FirstOrDefault();
            if (axis == null) return 0;
            return axis.AMP;
        }

        public void Save()
        {
            CommonTool.Serialize<ObservableCollection<AxisModel>>(_axisParam, Path);
        }

        public void Load()
        {
            _axisParam = CommonTool
                .DeSerialize<ObservableCollection<AxisModel>>(Path);
            if (_axisParam == null)
                _axisParam = new ObservableCollection<AxisModel>();
        }
    }
}
