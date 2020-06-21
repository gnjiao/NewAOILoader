using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Station
{
    public interface IFormatter
    {
        object Deserialize(Stream stream);
        void Serialize(Stream stream, object data);
    }
}
