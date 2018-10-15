using CryptoViewer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoViewer.Native
{
    /// <summary>
    /// Вспомогательные методы для работы с Microsoft CryptoAPI.
    /// </summary>
    [SecurityCritical]
    public static class CryptoApiHelper
    {
        #region Общие объекты

        private static readonly object ProviderHandleSync = new object();
        private static volatile Dictionary<int, SafeProvHandleImpl> _providerHandles = new Dictionary<int, SafeProvHandleImpl>();

        //public static SafeProvHandleImpl ProviderHandle
        //{
        //    get
        //    {

        //        lock (ProviderHandleSync)
        //        {
        //            if (!_providerHandles.ContainsKey(providerType))
        //            {
        //                var providerParams = new CspParameters(providerType);
        //                var providerHandle = AcquireProvider(providerParams);

        //                Thread.MemoryBarrier();

        //                _providerHandles.Add(providerType, providerHandle);
        //            }
        //        }



        //        return _providerHandles[providerType];
        //    }
        //}


        private static readonly object RandomNumberGeneratorSync = new object();
        private static volatile Dictionary<int, RNGCryptoServiceProvider> _randomNumberGenerators = new Dictionary<int, RNGCryptoServiceProvider>();


        public static SafeProvHandleImpl OpenProvider(CspParameters providerParameters)
        {
            var providerHandle = SafeProvHandleImpl.InvalidHandle;
            var dwFlags = MapCspProviderFlags(providerParameters.Flags);

            if (!CryptoApi.CryptAcquireContext(ref providerHandle, providerParameters.KeyContainerName, providerParameters.ProviderName, (uint)providerParameters.ProviderType, dwFlags))
            {
                throw CreateWin32Error();
            }

            return providerHandle;
        }

        public static SafeProvHandleImpl CreateProvider(CspParameters providerParameters)
        {
            var providerHandle = SafeProvHandleImpl.InvalidHandle;

            if (!CryptoApi.CryptAcquireContext(ref providerHandle, providerParameters.KeyContainerName, providerParameters.ProviderName, (uint)providerParameters.ProviderType, Constants.CRYPT_NEWKEYSET))
            {
                throw CreateWin32Error();
            }

            return providerHandle;
        }

        private static uint MapCspProviderFlags(CspProviderFlags flags)
        {
            uint dwFlags = 0;

            if ((flags & CspProviderFlags.UseMachineKeyStore) != CspProviderFlags.NoFlags)
            {
                dwFlags |= Constants.CRYPT_MACHINE_KEYSET;
            }

            if ((flags & CspProviderFlags.NoPrompt) != CspProviderFlags.NoFlags)
            {
                dwFlags |= Constants.CRYPT_PREGEN;
            }

            return dwFlags;
        }

        public static void SetProviderParameter(SafeProvHandleImpl providerHandle, int keyNumber, uint keyParamId, IntPtr keyParamValue)
        {
            if ((keyParamId == Constants.PP_KEYEXCHANGE_PIN) || (keyParamId == Constants.PP_SIGNATURE_PIN))
            {
                if (keyNumber == Constants.AT_KEYEXCHANGE)
                {
                    keyParamId = Constants.PP_KEYEXCHANGE_PIN;
                }
                else if (keyNumber == Constants.AT_SIGNATURE)
                {
                    keyParamId = Constants.PP_SIGNATURE_PIN;
                }
                else
                {
                    throw ExceptionUtility.NotSupported("KeyAlgorithmNotSupported");
                }
            }

            if (!CryptoApi.CryptSetProvParam(providerHandle, keyParamId, keyParamValue, 0))
            {
                throw CreateWin32Error();
            }
        }

        #endregion


        #region Fo work with cryptoproviders

        public static Dictionary<string, int> GetProviders()
        {
            Dictionary<string, int> installedCSPs = new Dictionary<string, int>();
            uint cbName;
            uint dwType;
            uint dwIndex;
            StringBuilder pszName;
            dwIndex = 0;
            dwType = 1;
            cbName = 0;
            while (CryptoApi.CryptEnumProviders(dwIndex, IntPtr.Zero, 0, ref dwType, null, ref cbName))
            {
                pszName = new StringBuilder((int)cbName);

                if (CryptoApi.CryptEnumProviders(dwIndex++, IntPtr.Zero, 0, ref dwType, pszName, ref cbName))
                {
                    installedCSPs.Add(pszName.ToString(), (int)dwType);
                }
            }
            return installedCSPs;
        }

        #endregion


        #region Для работы с функцией хэширования криптографического провайдера

        public static SafeHashHandleImpl CreateHash_3411_94(SafeProvHandleImpl providerHandle)
        {
            return CreateHash_3411(providerHandle, Constants.CALG_GR3411);
        }

        public static SafeHashHandleImpl CreateHash_3411_2012_256(SafeProvHandleImpl providerHandle)
        {
            return CreateHash_3411(providerHandle, Constants.CALG_GR3411_2012_256);
        }

        public static SafeHashHandleImpl CreateHash_3411_2012_512(SafeProvHandleImpl providerHandle)
        {
            return CreateHash_3411(providerHandle, Constants.CALG_GR3411_2012_512);
        }

        public static SafeHashHandleImpl CreateHash_3411(SafeProvHandleImpl providerHandle, int hashAlgId)
        {
            var hashHandle = SafeHashHandleImpl.InvalidHandle;

            if (!CryptoApi.CryptCreateHash(providerHandle, (uint)hashAlgId, SafeKeyHandleImpl.InvalidHandle, 0, ref hashHandle))
            {
                throw CreateWin32Error();
            }

            return hashHandle;
        }

        public static SafeHashHandleImpl CreateHashImit(SafeProvHandleImpl providerHandle, SafeKeyHandleImpl symKeyHandle)
        {
            var hashImitHandle = SafeHashHandleImpl.InvalidHandle;

            if (!CryptoApi.CryptCreateHash(providerHandle, Constants.CALG_G28147_IMIT, symKeyHandle, 0, ref hashImitHandle))
            {
                throw CreateWin32Error();
            }

            return hashImitHandle;
        }



        public static unsafe void HashData(SafeHashHandleImpl hashHandle, byte[] data, int dataOffset, int dataLength)
        {
            if (data == null)
            {
                throw ExceptionUtility.ArgumentNull("data");
            }

            if (dataOffset < 0)
            {
                throw ExceptionUtility.ArgumentOutOfRange("dataOffset");
            }

            if (dataLength < 0)
            {
                throw ExceptionUtility.ArgumentOutOfRange("dataLength");
            }

            if (data.Length < dataOffset + dataLength)
            {
                throw ExceptionUtility.ArgumentOutOfRange("dataLength");
            }

            if (dataLength > 0)
            {
                fixed (byte* dataRef = data)
                {
                    var dataOffsetRef = dataRef + dataOffset;

                    if (!CryptoApi.CryptHashData(hashHandle, dataOffsetRef, (uint)dataLength, 0))
                    {
                        throw CreateWin32Error();
                    }
                }
            }
        }

        public static byte[] EndHashData(SafeHashHandleImpl hashHandle)
        {
            uint dataLength = 0;

            if (!CryptoApi.CryptGetHashParam(hashHandle, Constants.HP_HASHVAL, null, ref dataLength, 0))
            {
                throw CreateWin32Error();
            }

            var data = new byte[dataLength];

            if (!CryptoApi.CryptGetHashParam(hashHandle, Constants.HP_HASHVAL, data, ref dataLength, 0))
            {
                throw CreateWin32Error();
            }

            return data;
        }

        public static void HashKeyExchange(SafeHashHandleImpl hashHandle, SafeKeyHandleImpl keyExchangeHandle)
        {
            if (!CryptoApi.CryptHashSessionKey(hashHandle, keyExchangeHandle, 0))
            {
                throw CreateWin32Error();
            }
        }

        #endregion





        #region Для работы с ключами криптографического провайдера

        public static SafeKeyHandleImpl GenerateKey(SafeProvHandleImpl providerHandle, int algId, CspProviderFlags flags)
        {
            var keyHandle = SafeKeyHandleImpl.InvalidHandle;
            var dwFlags = MapCspKeyFlags(flags);

            if (!CryptoApi.CryptGenKey(providerHandle, (uint)algId, dwFlags, ref keyHandle))
            {
                throw CreateWin32Error();
            }

            return keyHandle;
        }

        public static SafeKeyHandleImpl GenerateDhEphemeralKey(SafeProvHandleImpl providerHandle, string digestParamSet, string publicKeyParamSet)
        {
            var keyHandle = SafeKeyHandleImpl.InvalidHandle;
            var dwFlags = MapCspKeyFlags(CspProviderFlags.NoFlags) | Constants.CRYPT_PREGEN;

            if (!CryptoApi.CryptGenKey(providerHandle, Constants.CALG_DH_EL_EPHEM, dwFlags, ref keyHandle))
            {
                throw CreateWin32Error();
            }

            SetKeyParameterString(keyHandle, Constants.KP_HASHOID, digestParamSet);
            SetKeyParameterString(keyHandle, Constants.KP_DHOID, publicKeyParamSet);
            SetKeyParameter(keyHandle, Constants.KP_X, null);

            return keyHandle;
        }

        private static uint MapCspKeyFlags(CspProviderFlags flags)
        {
            uint dwFlags = 0;

            if ((flags & CspProviderFlags.UseNonExportableKey) == CspProviderFlags.NoFlags)
            {
                dwFlags |= Constants.CRYPT_EXPORTABLE;
            }

            if ((flags & CspProviderFlags.UseArchivableKey) != CspProviderFlags.NoFlags)
            {
                dwFlags |= Constants.CRYPT_ARCHIVABLE;
            }

            if ((flags & CspProviderFlags.UseUserProtectedKey) != CspProviderFlags.NoFlags)
            {
                dwFlags |= Constants.CRYPT_USER_PROTECTED;
            }

            return dwFlags;
        }

        public static SafeKeyHandleImpl GetUserKey(SafeProvHandleImpl providerHandle, int keyNumber)
        {
            var keyHandle = SafeKeyHandleImpl.InvalidHandle;

            if (!CryptoApi.CryptGetUserKey(providerHandle, (uint)keyNumber, ref keyHandle))
            {
                throw CreateWin32Error();
            }

            return keyHandle;
        }

        public static SafeKeyHandleImpl DeriveSymKey(SafeProvHandleImpl providerHandle, SafeHashHandleImpl hashHandle)
        {
            var symKeyHandle = SafeKeyHandleImpl.InvalidHandle;

            if (!CryptoApi.CryptDeriveKey(providerHandle, Constants.CALG_G28147, hashHandle, Constants.CRYPT_EXPORTABLE, ref symKeyHandle))
            {
                throw CreateWin32Error();
            }

            return symKeyHandle;
        }

        public static SafeKeyHandleImpl DuplicateKey(IntPtr sourceKeyHandle)
        {
            var keyHandle = SafeKeyHandleImpl.InvalidHandle;

            if (!CryptoApi.CryptDuplicateKey(sourceKeyHandle, null, 0, ref keyHandle))
            {
                throw CreateWin32Error();
            }

            return keyHandle;
        }

        public static SafeKeyHandleImpl DuplicateKey(SafeKeyHandleImpl sourceKeyHandle)
        {
            return DuplicateKey(sourceKeyHandle.DangerousGetHandle());
        }

        public static int GetKeyParameterInt32(SafeKeyHandleImpl keyHandle, uint keyParamId)
        {
            const int doubleWordSize = 4;

            uint dwDataLength = doubleWordSize;
            var dwDataBytes = new byte[doubleWordSize];

            if (!CryptoApi.CryptGetKeyParam(keyHandle, keyParamId, dwDataBytes, ref dwDataLength, 0))
            {
                throw CreateWin32Error();
            }

            if (dwDataLength != doubleWordSize)
            {
                throw ExceptionUtility.CryptographicException(Constants.NTE_BAD_DATA);
            }

            return BitConverter.ToInt32(dwDataBytes, 0);
        }

        private static string GetKeyParameterString(SafeKeyHandleImpl keyHandle, uint keyParamId)
        {
            var paramValue = GetKeyParameter(keyHandle, keyParamId);

            return BytesToString(paramValue);
        }

        private static string BytesToString(byte[] value)
        {
            string valueString;

            try
            {
                valueString = Encoding.GetEncoding(0).GetString(value);

                var length = 0;

                while (length < valueString.Length)
                {
                    // Строка заканчивается нулевым символом
                    if (valueString[length] == '\0')
                    {
                        break;
                    }

                    length++;
                }

                if (length == valueString.Length)
                {
                    throw ExceptionUtility.CryptographicException("Invalid string");
                }

                valueString = valueString.Substring(0, length);
            }
            catch (DecoderFallbackException exception)
            {
                throw ExceptionUtility.CryptographicException(exception, "Invalid string");
            }

            return valueString;
        }

        public static byte[] GetKeyParameter(SafeKeyHandleImpl keyHandle, uint keyParamId)
        {
            uint dataLength = 0;

            if (!CryptoApi.CryptGetKeyParam(keyHandle, keyParamId, null, ref dataLength, 0))
            {
                throw CreateWin32Error();
            }

            var dataBytes = new byte[dataLength];

            if (!CryptoApi.CryptGetKeyParam(keyHandle, keyParamId, dataBytes, ref dataLength, 0))
            {
                throw CreateWin32Error();
            }

            return dataBytes;
        }

        public static void SetKeyParameterInt32(SafeKeyHandleImpl keyHandle, int keyParamId, int keyParamValue)
        {
            var dwDataBytes = BitConverter.GetBytes(keyParamValue);

            if (!CryptoApi.CryptSetKeyParam(keyHandle, (uint)keyParamId, dwDataBytes, 0))
            {
                throw CreateWin32Error();
            }
        }

        private static void SetKeyParameterString(SafeKeyHandleImpl keyHandle, int keyParamId, string keyParamValue)
        {
            var stringDataBytes = Encoding.GetEncoding(0).GetBytes(keyParamValue);

            if (!CryptoApi.CryptSetKeyParam(keyHandle, (uint)keyParamId, stringDataBytes, 0))
            {
                throw CreateWin32Error();
            }
        }

        public static void SetKeyParameter(SafeKeyHandleImpl keyHandle, int keyParamId, byte[] keyParamValue)
        {
            if (!CryptoApi.CryptSetKeyParam(keyHandle, (uint)keyParamId, keyParamValue, 0))
            {
                throw CreateWin32Error();
            }
        }

        #endregion


        #region Для экспорта ключей криптографического провайдера

        public static byte[] ExportCspBlob(SafeKeyHandleImpl symKeyHandle, SafeKeyHandleImpl keyExchangeHandle, int blobType)
        {
            uint exportedKeyLength = 0;

            if (!CryptoApi.CryptExportKey(symKeyHandle, keyExchangeHandle, (uint)blobType, 0, null, ref exportedKeyLength))
            {
                throw CreateWin32Error();
            }

            var exportedKeyBytes = new byte[exportedKeyLength];

            if (!CryptoApi.CryptExportKey(symKeyHandle, keyExchangeHandle, (uint)blobType, 0, exportedKeyBytes, ref exportedKeyLength))
            {
                throw CreateWin32Error();
            }

            return exportedKeyBytes;
        }




        #endregion


        #region Для импорта ключей криптографического провайдера

        public static int ImportCspBlob(byte[] importedKeyBytes, SafeProvHandleImpl providerHandle, SafeKeyHandleImpl publicKeyHandle, out SafeKeyHandleImpl keyExchangeHandle)
        {
            var dwFlags = MapCspKeyFlags(CspProviderFlags.NoFlags);
            var keyExchangeRef = SafeKeyHandleImpl.InvalidHandle;

            if (!CryptoApi.CryptImportKey(providerHandle, importedKeyBytes, (uint)importedKeyBytes.Length, publicKeyHandle, dwFlags, ref keyExchangeRef))
            {
                throw CreateWin32Error();
            }

            var keyNumberMask = BitConverter.ToInt32(importedKeyBytes, 4) & 0xE000;
            var keyNumber = (keyNumberMask == 0xA000) ? Constants.AT_KEYEXCHANGE : Constants.AT_SIGNATURE;

            keyExchangeHandle = keyExchangeRef;

            return keyNumber;
        }



        #endregion


        #region Для работы с цифровой подписью

        public static byte[] SignValue(SafeProvHandleImpl hProv, int keyNumber, byte[] hashValue)
        {
            using (var hashHandle = SetupHashAlgorithm(hProv, hashValue))
            {
                uint signatureLength = 0;

                // Вычисление размера подписи
                if (!CryptoApi.CryptSignHash(hashHandle, (uint)keyNumber, null, 0, null, ref signatureLength))
                {
                    throw CreateWin32Error();
                }

                var signatureValue = new byte[signatureLength];

                // Вычисление значения подписи
                if (!CryptoApi.CryptSignHash(hashHandle, (uint)keyNumber, null, 0, signatureValue, ref signatureLength))
                {
                    throw CreateWin32Error();
                }

                return signatureValue;
            }
        }

        public static bool VerifySign(SafeProvHandleImpl providerHandle, SafeKeyHandleImpl keyHandle, byte[] hashValue, byte[] signatureValue)
        {
            using (var hashHandle = SetupHashAlgorithm(providerHandle, hashValue))
            {
                return CryptoApi.CryptVerifySignature(hashHandle, signatureValue, (uint)signatureValue.Length, keyHandle, null, 0);
            }
        }

        private static SafeHashHandleImpl SetupHashAlgorithm(SafeProvHandleImpl providerHandle, byte[] hashValue)
        {
            var hashHandle = CreateHash_3411_94(providerHandle);

            uint hashLength = 0;

            if (!CryptoApi.CryptGetHashParam(hashHandle, Constants.HP_HASHVAL, null, ref hashLength, 0))
            {
                throw CreateWin32Error();
            }

            if (hashValue.Length != hashLength)
            {
                throw ExceptionUtility.CryptographicException(Constants.NTE_BAD_HASH);
            }

            if (!CryptoApi.CryptSetHashParam(hashHandle, Constants.HP_HASHVAL, hashValue, 0))
            {
                throw CreateWin32Error();
            }

            return hashHandle;
        }

        #endregion


        public static T DangerousAddRef<T>(this T handle) where T : SafeHandle
        {
            var success = false;
            handle.DangerousAddRef(ref success);

            return handle;
        }

        public static void TryDispose(this SafeHandle handle)
        {
            if ((handle != null) && !handle.IsClosed)
            {
                handle.Dispose();
            }
        }

        private static CryptographicException CreateWin32Error()
        {
            return ExceptionUtility.CryptographicException(Marshal.GetLastWin32Error());
        }
    }
}
