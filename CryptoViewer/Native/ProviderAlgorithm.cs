using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CryptoViewer.Native
{
    public class ProviderAlgorithm
    {
        

        public ProviderAlgorithm(PROVENUMALGS alg)
            :this(alg.aiAlgid,alg.dwBitLen,alg.szName)
        {
            
        }

        public ProviderAlgorithm(uint algorithmId,uint bitLength,string name)
        {
            AlgorithmId = algorithmId;
            BitLength = bitLength;
            Name = name;
        }

        #region Properties

        public uint AlgorithmId { get; private set; }

        /// <summary>
        /// Длина ключа в битах
        /// </summary>
        public uint BitLength { get; private set; }

        /// <summary>
        /// Имя алгоритма
        /// </summary>
        public string Name { get; private set; }

        #endregion
    }


    [StructLayout( LayoutKind.Sequential)]
    public struct PROVENUMALGS {
        [MarshalAs(UnmanagedType.U4)]
        public uint aiAlgid;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwBitLen;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwNameLen;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string szName;
    }
}
