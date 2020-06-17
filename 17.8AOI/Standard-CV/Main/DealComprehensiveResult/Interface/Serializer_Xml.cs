using BasicClass;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Main
{
    static class Serializer_Xml
    {
        public static void Serialize<T>(object data, string path)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            try
            {
                XmlSerializer writer = new XmlSerializer(typeof(T));
                //创建流
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    //序列化写入
                    writer.Serialize(fs, data);
                }
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("Serializer", ex);
            }
        }

        public static T DeSerialize<T>(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    XmlSerializer reader = new XmlSerializer(typeof(T));
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        return (T)reader.Deserialize(fs);
                    }
                }
                catch (Exception ex)
                {
                    Log.L_I.WriteError("Serializer", ex);
                }
            }
            return default;
        }
    }
}
