using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Station
{
    /// <summary>
    /// 工位数据的服务类
    /// <para>使用此类必须在程序启动时，先手动加载本地的工位数据</para>
    /// </summary>
    public class StationService
    {
        #region singleton
        static StationService _instance = null;
        static readonly object _locker = new object();

        public static StationService GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                        _instance = new StationService();
                }
            }
            return _instance;
        }

        private StationService() { }
        #endregion

        /// <summary>
        /// 当前型号的工位数据集合
        /// </summary>
        private ObservableCollection<StationModel> _datas { get; set; } = null;

        /// <summary>
        /// 获得工位数据集
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<StationModel> GetDatas() => _datas;

        /// <summary>
        /// 获取指定工位的数据
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public StationModel GetData(int stationId) => _datas
            .Where(p => p.Index == stationId).ToArray().FirstOrDefault();

        /// <summary>
        /// 设置基准值，即示教的位置
        /// </summary>
        /// <param name="stationId">工位号,不是数组，推荐从1开始</param>
        /// <param name="value">基准值，数组长度必须是4，包含xyzr</param>
        /// <param name="path">保存文件的路径</param>
        /// <param name="ifCreate">如果原先并没有输入工位号对应的数据，是否直接创建新的数据</param>
        public void SetStd(int stationId, double[] value, string path, bool ifCreate = true)
        {
            //输入数组长度不对，直接返回
            if (value.Length < 4) return;
            //在数据集合中搜索对应的工位数据
            var data = _datas.Where(p => p.Index == stationId).ToArray().FirstOrDefault();
            //如果集合中不存在指定工位号的数据
            if (data == null)
            {
                //如果不创建新数据，则直接返回
                if (!ifCreate)
                    return;
                //创建一组新的数据，并以输入的工位号标识
                data = new StationModel { Index = stationId };
            }

            //赋值
            data.StdX = value[0];
            data.StdY = value[1];
            data.StdZ = value[2];
            data.StdR = value[3];
            data.IsTeached = true;
            //增加到集合当中
            _datas.Add(data);
            //保存更新后的数据
            Save(path);
        }

        /// <summary>
        /// 设置标定结果，即相机中标定的像素xy以及r
        /// <para>在外部调用时，应当确保该工位已经示教过，程序内部判断未示教过会直接返回</para>
        /// </summary>
        /// <param name="stationId">工位号</param>
        /// <param name="value">标定时获得的值，数组长度必须是3，包含xyr</param>
        /// <param name="path">保存文件的路径</param>
        public void SetCalib(int stationId, double[] value, string path)
        {
            //如果输入数组长度小于3则返回
            if (value.Length < 3) return;
            //在数据集合中搜索对应的工位数据
            var data = _datas.Where(p => p.Index == stationId).ToArray().FirstOrDefault();
            //如果指定工位号的数据不存在或者该工位尚未示教，直接返回
            if (data == null || !data.IsTeached) return;
            //赋值
            data.CalibX = value[0];
            data.CalibY = value[1];
            data.CalibR = value[2];
            data.IsCalibed = true;
            //保存更新后的数据
            Save(path);
        }

        /// <summary>
        /// 修正示教数据，在标定时根据标定mark与视野中心的距离进行反补
        /// </summary>
        /// <param name="stationId">工位号，推荐从1开始</param>
        /// <param name="value">反补偏差</param>
        /// <param name="path">保存数据的路径</param>
        public void ModifyStd(int stationId, double[] value, string path)
        {
            //如果输入数组长度小于3则返回
            if (value.Length < 2) return;
            //在数据集合中搜索对应的工位数据
            var data = _datas.Where(p => p.Index == stationId).ToArray().FirstOrDefault();
            //如果指定工位号的数据不存在或者该工位尚未示教，直接返回
            if (data == null || !data.IsTeached) return;
            //赋值
            data.StdX += value[0];
            data.StdY += value[1];
            data.StdZ += value[2];
            //保存更新后的数据
            Save(path);
        }

        /// <summary>
        /// 重新加载指定路径下的工位数据，用于切换配方时调用
        /// <para>一旦调用这个函数，新路径下的数据会被刷新成无效数据，即没有示教标定过</para>
        /// </summary>
        /// <param name="path">新的数据路径</param>
        public void Reload(string path)
        {
            //加载指定路径的数据
            Load(path);
            //新加载的数据视为没有示教和标定过
            foreach (var item in _datas)
            {
                item.IsTeached = false;
                item.IsCalibed = false;
            }
            Save(path);
        }

        /// <summary>
        /// 将所有工位数据序列化保存到本地
        /// </summary>
        /// <param name="path"></param>
        private void Save(string path)
        {
            Serialize<ObservableCollection<StationModel>>(_datas, path);
        }

        /// <summary>
        /// 反序列化加载本地的所有工位数据，如果本地无数据，会做初始化，保证数据集不为空
        /// </summary>
        /// <param name="path"></param>
        public void Load(string path)
        {
            _datas = DeSerialize<ObservableCollection<StationModel>>(path);
            if (_datas == null) _datas = new ObservableCollection<StationModel>();
        }

        /// <summary>
        /// 序列化接口函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="path"></param>
        public void Serialize<T>(object data, string path)
        {
            //检查目录是否存在
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            try
            {
                //XmlSerializer _formatter = new XmlSerializer(typeof(T));
                BinaryFormatter _formatter = new BinaryFormatter();
                //创建流
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    //序列化写入
                    _formatter.Serialize(fs, data);
                }
            }
            catch { }
        }

        /// <summary>
        /// 反序列化接口函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T DeSerialize<T>(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    //XmlSerializer _formatter = new XmlSerializer(typeof(T));
                    BinaryFormatter _formatter = new BinaryFormatter();
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        return (T)_formatter.Deserialize(fs);
                    }
                }
                catch { }
            }
            return default;
        }
    }
}
