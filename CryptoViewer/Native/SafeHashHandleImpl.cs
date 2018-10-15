using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CryptoViewer.Native
{
    /// <summary>
    /// Дескриптор функции хэширования криптографического провайдера.
    /// </summary>
    [SecurityCritical]
    public class SafeHashHandleImpl : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeHashHandleImpl()
            : base(true)
        {
        }

        public SafeHashHandleImpl(IntPtr handle)
            : base(true)
        {
            SetHandle(handle);
        }

        public static SafeHashHandleImpl InvalidHandle
        {
            get { return new SafeHashHandleImpl(IntPtr.Zero); }
        }

        [SecurityCritical]
        protected override bool ReleaseHandle()
        {
            CryptoApi.CryptDestroyHash(handle);
            return true;
        }
    }
}
