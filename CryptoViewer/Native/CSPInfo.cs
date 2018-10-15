using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoViewer.Native
{
    public class CSPInfo
    {
        public CSPInfo(string name,string version)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; }
        public string Version { get; }
    }
}
