using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Dialog
{
    /// <summary>
    /// The state of progress in progress dialogs and toasts.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum ProgressState : int
    {
        /// <summary>
        /// Specify that progress is on-going.
        /// </summary>
        [AvailableSince(10, 0)]
        Play = 0,
        /// <summary>
        /// Specify that progress has been paused.
        /// </summary>
        [AvailableSince(10, 0)]
        Pause = 1,
        /// <summary>
        /// Specify that an error has occured.
        /// </summary>
        [AvailableSince(10, 0)]
        Error = 2
    }

    /// <summary>
    /// Progress dialog.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class ProgressDialog : Dialog
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_create_progress(out IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_set_progress_message_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_progress_message_has_emoticons(IntPtr dialog, bool has_emoticons);

        [DllImport("bps")]
        private static extern int dialog_set_progress_level(IntPtr dialog, int progress);

        [DllImport("bps")]
        private static extern int dialog_set_progress_icon(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string path);

        [DllImport("bps")]
        private static extern int dialog_set_progress_state(IntPtr dialog, int state);

        [DllImport("bps")]
        private static extern int dialog_set_progress_left_details(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_progress_right_details(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        #endregion

        /// <summary>
        /// Create a new progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public ProgressDialog()
            : base()
        {
        }

        internal override void CreateDialog()
        {
            if (dialog_create_progress(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        #region Properties

        /// <summary>
        /// Set the message text of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Message
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_progress_message_text(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set whether the message text has emoticons.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool HasEmoticons
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_progress_message_has_emoticons(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the progress level of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public int Progress
        {
            [AvailableSince(10, 0)]
            set
            {
                if (value < -1 || value > 100)
                {
                    throw new ArgumentOutOfRangeException("Progress", "0 <= Progress <= 100 OR -1 for indefinite progress");
                }
                if (dialog_set_progress_level(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the icon of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string IconPath
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_progress_icon(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the progress state of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public ProgressState State
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_progress_state(handle, (int)value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the left side details text of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string LeftDetails
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_progress_left_details(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the right side details text of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string RightDetails
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_progress_right_details(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        #endregion
    }
}
