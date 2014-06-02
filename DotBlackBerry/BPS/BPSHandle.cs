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

    #region Implementations

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

    /// <summary>
    /// An array of elements allocated by BPS.
    /// </summary>
    /// <typeparam name="T">The type of element that makes up the array.</typeparam>
    [AvailableSince(10, 0)]
    public abstract class BPSArray<T> : BPSHandle
    {
        internal BPSArray(IntPtr ptr, int count)
            : base(ptr)
        {
            var data = new T[count];
            if (count > 0)
            {
                CreateArray(ref data, ptr);
            }
        }

        internal abstract void CreateArray(ref T[] data, IntPtr ptr);

        /// <summary>
        /// Get the value of the BPS array.
        /// </summary>
        [AvailableSince(10, 0)]
        public T[] Value { get; private set; }

        /// <summary>
        /// Convert a BPSArray into an array.
        /// </summary>
        /// <param name="bpsArray">The BPSArray to convert.</param>
        /// <returns>The array contents.</returns>
        public static implicit operator T[](BPSArray<T> bpsArray)
        {
            return bpsArray.Value;
        }
    }

    /// <summary>
    /// An array of ints allocated by BPS.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class BPSIntArray : BPSArray<int>
    {
        internal BPSIntArray(IntPtr ptr, int count)
            : base(ptr, count)
        {
        }

        internal override void CreateArray(ref int[] data, IntPtr ptr)
        {
            var intSize = sizeof(int);
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = Marshal.ReadInt32(ptr, intSize * i);
            }
        }
    }

#endregion
}
