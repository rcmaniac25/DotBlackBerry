using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Handle to a BPS allocation.
    /// </summary>
    public sealed class BPSHandle : CriticalHandle
    {
        internal BPSHandle(IntPtr ptr)
            : base(IntPtr.Zero)
        {
            handle = ptr;
        }

        internal IntPtr UnsafeHandle
        {
            get
            {
                return handle;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the handle value is invalid.
        /// </summary>
        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        /// <summary>
        /// Executes the code required to free the handle.
        /// </summary>
        /// <returns>true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false.</returns>
        protected override bool ReleaseHandle()
        {
            BPS.bps_free(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }
}
