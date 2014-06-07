using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Dialog
{
    #region Enums

    /// <summary>
    /// The available toast dialog positions.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum ToastPosition : int
    {
        /// <summary>
        /// Specify that the toast will appear centered near the top of the screen.
        /// </summary>
        [AvailableSince(10, 0)]
        TopCenter = 0,
        /// <summary>
        /// Specify that the toast will appear centered in the middle of the screen.
        /// </summary>
        [AvailableSince(10, 0)]
        MiddleCenter = 1,
        /// <summary>
        /// Specify that the toast will appear centered near the bottom of the screen.
        /// </summary>
        [AvailableSince(10, 0)]
        BottomCenter = 2
    }

    /// <summary>
    /// The available volume directions in volume toast dialogs.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum VolumeDirection : int
    {
        /// <summary>
        /// Specify that the volume has been decreased.
        /// </summary>
        [AvailableSince(10, 0)]
        Down = -1,
        /// <summary>
        /// Specify that the volume has not been changed.
        /// </summary>
        [AvailableSince(10, 0)]
        NoChange = 0,
        /// <summary>
        /// Specify that the volume has been increased.
        /// </summary>
        [AvailableSince(10, 0)]
        Up = 1
    }

    /// <summary>
    /// The available volume controls in volume toast dialogs.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum VolumeControlType : int
    {
        /// <summary>
        /// Specify that volume control should not be allowed.
        /// </summary>
        [AvailableSince(10, 0)]
        Unsupported = 0,
        /// <summary>
        /// Specify that simple volume control be allowed.
        /// </summary>
        [AvailableSince(10, 0)]
        Simple = 1,
        /// <summary>
        /// Specify that percentage volume control be allowed.
        /// </summary>
        [AvailableSince(10, 0)]
        Percentage = 2
    }

    #endregion

    #region TextToast

    /// <summary>
    /// Basic text toast dialog.
    /// </summary>
    [AvailableSince(10, 0)]
    public class TextToast : Dialog
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_create_toast(out IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_set_toast_message_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_toast_position(IntPtr dialog, int position);

        #endregion

        /// <summary>
        /// Create a new text toast.
        /// </summary>
        [AvailableSince(10, 0)]
        public TextToast()
            : base()
        {
        }

        internal override void CreateDialog()
        {
            if (dialog_create_toast(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        #region Properties

        /// <summary>
        /// Set the message text of a toast dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Message
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_toast_message_text(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the position of a toast dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public ToastPosition Position
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_toast_position(handle, (int)value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        #endregion
    }

    #endregion

    #region IconToast

    /// <summary>
    /// Toast dialog with an icon.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class IconToast : TextToast
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_create_icon_toast(out IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_set_toast_icon(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string path);

        #endregion

        /// <summary>
        /// Create a new icon toast.
        /// </summary>
        [AvailableSince(10, 0)]
        public IconToast()
            : base()
        {
        }

        internal override void CreateDialog()
        {
            if (dialog_create_icon_toast(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        /// <summary>
        /// Set the icon of an icon toast dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string IconPath
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_toast_icon(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }
    }

    #endregion

    #region ProgressToast

    /// <summary>
    /// Toast dialog with progress.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class ProgressToast : TextToast
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_create_progress_toast(out IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_set_progress_toast_state(IntPtr dialog, int state);

        [DllImport("bps")]
        private static extern int dialog_set_progress_toast_level(IntPtr dialog, int progress);

        [DllImport("bps")]
        private static extern int dialog_set_progress_toast_details(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        #endregion

        /// <summary>
        /// Create a new progress toast.
        /// </summary>
        [AvailableSince(10, 0)]
        public ProgressToast()
            : base()
        {
        }

        internal override void CreateDialog()
        {
            if (dialog_create_progress_toast(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        #region Properties

        /// <summary>
        /// Set the progress state of a progress toast dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public ProgressState State
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_progress_toast_state(handle, (int)value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the progress level of a progress toast dialog.
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
                if (dialog_set_progress_toast_level(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the details text of a progress toast dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Details
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_progress_toast_details(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        #endregion
    }

    #endregion

    #region VolumeToast

    /// <summary>
    /// Toast dialog with volume indicator.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class VolumeToast : TextToast
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_create_volume_toast(out IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_set_volume_toast_device_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_volume_toast_muted(IntPtr dialog, bool muted);

        [DllImport("bps")]
        private static extern int dialog_set_volume_toast_level(IntPtr dialog, int level);

        [DllImport("bps")]
        private static extern int dialog_set_volume_toast_direction(IntPtr dialog, int direction);

        [DllImport("bps")]
        private static extern int dialog_set_volume_toast_control(IntPtr dialog, int control);

        #endregion

        /// <summary>
        /// Create a new volume toast.
        /// </summary>
        [AvailableSince(10, 0)]
        public VolumeToast()
            : base()
        {
        }

        internal override void CreateDialog()
        {
            if (dialog_create_volume_toast(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        #region Properties

        /// <summary>
        /// Set the device text of a volume toast dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Device
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_volume_toast_device_text(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set whether to indicate that volume is muted on a volume toast dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool Muted
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_volume_toast_muted(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the volume level on a volume toast dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public int Level
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_volume_toast_level(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the volume direction on a volume toast dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public VolumeDirection Direction
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_volume_toast_direction(handle, (int)value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the volume control on a volume toast dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public VolumeControlType Type
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_volume_toast_control(handle, (int)value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        #endregion
    }

    #endregion
}
