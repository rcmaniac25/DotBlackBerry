using System;
using System.Runtime.InteropServices;

using Mono.Unix.Native;

namespace BlackBerry.Internal
{
    /// <summary>
    /// Representation of a sigevent.
    /// </summary>
    /// <remarks>
    /// This is not intended for direct sigevent usage, and is instead for advanced usage within .BlackBerry APIs 
    /// which will not provide any way to manipulate the contents of the sigevent. Only to provide a safe reference to one.
    /// </remarks>
    public sealed class SafeSigeventHandle : SafeHandle
    {
        private static readonly uint SIGEVENT_SIZE =
            // sigev_notify
            sizeof(int) +
            // __sigev_un1 //TODO
            (uint)IntPtr.Size +
            // sigev_value
            (uint)IntPtr.Size +
            // __sigev_un2 //TODO
            (uint)IntPtr.Size;

        /// <summary>
        /// Create a new sigevent.
        /// </summary>
        public SafeSigeventHandle()
            : base(IntPtr.Zero, true)
        {
            SetHandle(Syscall.malloc(SIGEVENT_SIZE));
        }

        /// <summary>
        /// Get if the sigevent handle is invalid.
        /// </summary>
        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero;
            }
        }

        /// <summary>
        /// Free the sigevent.
        /// </summary>
        /// <returns>true if the sigevent was freed.</returns>
        protected override bool ReleaseHandle()
        {
            if (!IsInvalid)
            {
                Syscall.free(handle);
                SetHandleAsInvalid();
            }
            return true;
        }
    }
}
