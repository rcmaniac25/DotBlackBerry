using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Dialog
{
    /// <summary>
    /// Base dialog window.
    /// </summary>
    [AvailableSince(10, 0)]
    public abstract class Dialog : IDisposable
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_request_events(int flags);

        [DllImport("bps")]
        private static extern int dialog_stop_events(int flags);

        [DllImport("bps")]
        private static extern int dialog_get_domain();

        [DllImport("bps")]
        private static extern int dialog_show(IntPtr dialog);

        [DllImport("bps")]
        internal static extern int dialog_update(IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_cancel(IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_destroy(IntPtr dialog);

        #endregion

        internal Dialog()
        {
            handle = IntPtr.Zero;
            Util.GetBPSOrException();
            CreateDialog();
        }

        internal IntPtr handle;

        internal abstract void CreateDialog();

        /// <summary>
        /// Dispose of the Dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("Dialog");
            }
            if(dialog_destroy(handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForErrno();
            }
            handle = IntPtr.Zero;
        }

        /// <summary>
        /// Get the unique domain ID for the dialog service.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int Domain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return dialog_get_domain();
            }
        }

        #region Functions

        /// <summary>
        /// Start receiving dialog events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestEvents()
        {
            Util.GetBPSOrException();
            return dialog_request_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving dialog events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopEvents()
        {
            Util.GetBPSOrException();
            return dialog_stop_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Display a dialog.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool Show()
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("Dialog");
            }
            return dialog_show(handle) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Cancel a dialog.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool Cancel()
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("Dialog");
            }
            return dialog_cancel(handle) == BPS.BPS_SUCCESS;
        }

        #endregion
    }
}
