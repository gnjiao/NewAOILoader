using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Main
{
    public static class CommonTool
    {
        public static void Serialize<T>(object data, string path)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            try
            {
                XmlSerializer _formatter = new XmlSerializer(typeof(T));
                //创建流
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    //序列化写入
                    _formatter.Serialize(fs, data);
                }
            }
            catch { }
        }

        public static T DeSerialize<T>(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    XmlSerializer _formatter = new XmlSerializer(typeof(T));
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
