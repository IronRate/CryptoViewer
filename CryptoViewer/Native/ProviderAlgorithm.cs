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
        private uint _algorithmClass;
        private uint _algorithmId;

        public ProviderAlgorithm(PROVENUMALGS alg)
            : this(alg.aiAlgid, alg.dwBitLen, alg.szName)
        {

        }

        public ProviderAlgorithm(PROVENUMALGS_EX alg)
        {
            AlgorithmId = alg.aiAlgid;
            BitLength = alg.dwDefaultLen;
            Name = alg.szName;
            LongName = alg.szLongName;
            MinLength = alg.dwMinLen;
            MaxLength = alg.dwMaxLen;
            Protocols = alg.dwProtocols;
        }

        public ProviderAlgorithm(uint algorithmId, uint bitLength, string name)
        {
            AlgorithmId = algorithmId;
            BitLength = bitLength;
            Name = name;
        }

        #region Properties

        public uint AlgorithmId
        {
            get => _algorithmId;
            private set
            {
                _algorithmId = value;
                _algorithmClass = (_algorithmId & (7 << 13));
            }
        }

        /// <summary>
        /// Длина ключа в битах
        /// </summary>
        public uint BitLength { get; private set; }

        /// <summary>
        /// Имя алгоритма
        /// </summary>
        public string Name { get; private set; }


        public uint MinLength { get; private set; }

        public uint MaxLength { get; private set; }

        public uint Protocols { get; private set; }

        public string LongName { get; private set; }

        public uint AlgorithmClass { get => _algorithmClass; }

        public string AlgorithmClassName
        {
            get
            {
                string s = null;
                switch (this.AlgorithmClass)
                {
                    case Constants.ALG_CLASS_DATA_ENCRYPT:
                        s = "Encrypt";
                        break;
                    case Constants.ALG_CLASS_HASH:
                        s = "Hash";
                        break;
                    case Constants.ALG_CLASS_KEY_EXCHANGE:
                        s = "Exchange";
                        break;
                    case Constants.ALG_CLASS_SIGNATURE:
                        s = "Signature";
                        break;
                    default:
                        s = "Unknown";
                        break;
                }
                return s;
            }
        }

        #endregion
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct PROVENUMALGS
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint aiAlgid;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwBitLen;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwNameLen;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string szName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROVENUMALGS_EX
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint aiAlgid;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwDefaultLen;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwMinLen;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwMaxLen;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwProtocols;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwNameLen;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string szName;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwLongNameLen;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string szLongName;




    }
}
