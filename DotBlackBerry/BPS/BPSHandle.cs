using System;
using System.Threading;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Handle to a BPS allocation.
    /// </summary>
    [AvailableSince(10, 0)]
    public abstract class BPSHandle : CriticalHandleZeroOrMinusOneIsInvalid
    {
        internal BPSHandle(IntPtr ptr)
        {
            SetHandle(ptr);
        }

        internal IntPtr Handle
        {
            get
            {
                return handle;
            }
        }

        /// <summary>
        /// Executes the code required to free the handle.
        /// </summary>
        /// <returns>true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false.</returns>
        [AvailableSince(10, 0)]
        protected override bool ReleaseHandle()
        {
            IntPtr hwnd;
            if ((hwnd = Interlocked.Exchange(ref handle, IntPtr.Zero)) != IntPtr.Zero)
            {
                BPS.bps_free(hwnd);
            }
            return true;
        }
    }

    /// <summary>
    /// A string allocated by BPS.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class BPSString : BPSHandle
    {
        internal BPSString(IntPtr ptr)
            : base(ptr)
        {
            Value = Marshal.PtrToStringAnsi(ptr);
        }

        /// <summary>
        /// Get the value of the BPS string.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Value { get; private set; }

        /// <summary>
        /// Convert a BPSString into a string.
        /// </summary>
        /// <param name="bpsString">The BPSString to convert.</param>
        /// <returns>The string contents.</returns>
        public static implicit operator string(BPSString bpsString)
        {
            return bpsString.Value;
        }
    }
}
