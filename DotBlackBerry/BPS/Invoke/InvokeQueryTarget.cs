using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Invoke
{
    /// <summary>
    /// Invocation query result target.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class InvokeQueryTarget
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_result_target_get_key(IntPtr target);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_result_target_get_icon(IntPtr target);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_result_target_get_label(IntPtr target);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern InvokeTargetType navigator_invoke_query_result_target_get_type(IntPtr target);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern InvokePerimeterType navigator_invoke_query_result_target_get_perimeter(IntPtr target);

        #endregion

        private IntPtr handle;
        private BPSEvent ev;

        internal InvokeQueryTarget(IntPtr handle, BPSEvent ev)
        {
            this.handle = handle;
            this.ev = ev;
        }

        #region Properties

        /// <summary>
        /// Get if the target is still valid and usable.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return handle != IntPtr.Zero && ev.IsValid;
            }
        }

        /// <summary>
        /// Get the key of the target for the query result.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Key
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_query_result_target_get_key(handle));
            }
        }

        /// <summary>
        /// Get the icon of the target for the query result.
        /// </summary>
        [AvailableSince(10, 0)]
        public Uri Icon
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return new Uri(Marshal.PtrToStringAnsi(navigator_invoke_query_result_target_get_icon(handle)));
            }
        }

        /// <summary>
        /// Get the label of the target for the query result.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Label
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Util.PtrToStringUTF8(navigator_invoke_query_result_target_get_label(handle));
            }
        }

        /// <summary>
        /// Get the type of target for the query result.
        /// </summary>
        [AvailableSince(10, 0)]
        public InvokeTargetType Type
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return navigator_invoke_query_result_target_get_type(handle);
            }
        }

        /// <summary>
        /// Get the perimeter of the target for the query result.
        /// </summary>
        [AvailableSince(10, 0)]
        public InvokePerimeterType Perimeter
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return navigator_invoke_query_result_target_get_perimeter(handle);
            }
        }

        #endregion

        private void Verify()
        {
            if (!ev.IsValid)
            {
                handle = IntPtr.Zero;
            }
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("InvokeQueryTarget");
            }
        }
    }
}
