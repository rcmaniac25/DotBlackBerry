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

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_create_progress(out IntPtr dialog);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_progress_message_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_progress_message_has_emoticons(IntPtr dialog, bool has_emoticons);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_progress_level(IntPtr dialog, int progress);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_progress_icon(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string path);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_progress_state(IntPtr dialog, ProgressState state);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_progress_left_details(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_progress_right_details(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        #endregion

        private string message = null; //XXX
        private bool hasEmoticons = false; //XXX
        private int level = 0;
        private string icon = null; //XXX
        private ProgressState state = ProgressState.Play;
        private string leftDetails = null; //XXX
        private string rightDetails = null; //XXX

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
        /// Get or set the message text of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Message
        {
            [AvailableSince(10, 0)]
            get
            {
                return message;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (message != value)
                {
                    if (dialog_set_progress_message_text(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    message = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set whether the message text has emoticons.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool HasEmoticons
        {
            [AvailableSince(10, 0)]
            get
            {
                return hasEmoticons;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (hasEmoticons != value)
                {
                    if (dialog_set_progress_message_has_emoticons(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    hasEmoticons = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the progress level of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public int Progress
        {
            [AvailableSince(10, 0)]
            get
            {
                return level;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (level != value)
                {
                    if (value < -1 || value > 100)
                    {
                        throw new ArgumentOutOfRangeException("Progress", value, "0 <= Progress <= 100 OR -1 for indefinite progress");
                    }
                    if (dialog_set_progress_level(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    level = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the icon of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string IconPath
        {
            [AvailableSince(10, 0)]
            get
            {
                return icon;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (icon != value)
                {
                    if (dialog_set_progress_icon(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    icon = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the progress state of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public ProgressState State
        {
            [AvailableSince(10, 0)]
            get
            {
                return state;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (state != value)
                {
                    if (dialog_set_progress_state(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    state = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the left side details text of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string LeftDetails
        {
            [AvailableSince(10, 0)]
            get
            {
                return leftDetails;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (leftDetails != value)
                {
                    if (dialog_set_progress_left_details(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    leftDetails = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the right side details text of a progress dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string RightDetails
        {
            [AvailableSince(10, 0)]
            get
            {
                return rightDetails;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (rightDetails != value)
                {
                    if (dialog_set_progress_right_details(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    rightDetails = value;
                    UpdateDialog();
                }
            }
        }

        #endregion
    }
}
