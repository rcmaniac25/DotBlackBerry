using System;
using System.Threading;

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
}
