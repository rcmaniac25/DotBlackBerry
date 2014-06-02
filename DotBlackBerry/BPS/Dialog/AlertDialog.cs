using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Dialog
{
    /// <summary>
    /// An alert dialog is a simple dialog that displays a title, message, icon, checkbox and buttons.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class AlertDialog : Dialog
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_create_alert(out IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_set_alert_message_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_alert_message_has_emoticons(IntPtr dialog, bool has_emoticons);

        [DllImport("bps")]
        private static extern int dialog_set_alert_checkbox_checked(IntPtr dialog, bool checkd);

        [DllImport("bps")]
        private static extern int dialog_set_alert_checkbox_label(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string label);

        [DllImport("bps")]
        private static extern int dialog_set_alert_checkbox_enabled(IntPtr dialog, bool enabled);

        [DllImport("bps")]
        private static extern int dialog_set_alert_icon(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string path);

        [DllImport("bps")]
        internal static extern bool dialog_event_get_alert_checkbox_checked(IntPtr ev);

        #endregion

        /// <summary>
        /// Create a new alert dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public AlertDialog()
            : base()
        {
        }

        internal override void CreateDialog()
        {
            if (dialog_create_alert(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        internal override DialogEvent GetEventForDialog(IntPtr ev)
        {
            return new AlertDialogEvent(ev, this);
        }

        #region Properties

        /// <summary>
        /// Set the message text of an alert dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Message
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_alert_message_text(handle, value) != BPS.BPS_SUCCESS)
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
                if (dialog_set_alert_message_has_emoticons(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the initial state of the check box.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool IsCheckboxChecked
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_alert_checkbox_checked(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the label for the check box.
        /// </summary>
        [AvailableSince(10, 0)]
        public string CheckboxLabel
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_alert_checkbox_label(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set whether the check box is enabled.
        /// </summary>
        [AvailableSince(10, 2)]
        public bool IsCheckboxEnabled
        {
            [AvailableSince(10, 2)]
            set
            {
                if (dialog_set_alert_checkbox_enabled(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the icon of a alert dialog.
        /// </summary>
        [AvailableSince(10, 2)]
        public string IconPath
        {
            [AvailableSince(10, 2)]
            set
            {
                if (dialog_set_alert_icon(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        #endregion
    }

    /// <summary>
    /// Event associated with an alert.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class AlertDialogEvent : DialogEvent
    {
        internal AlertDialogEvent(IntPtr ev, AlertDialog dia)
            : base(ev, dia)
        {
            Util.GetBPSOrException();
            CheckboxChecked = AlertDialog.dialog_event_get_alert_checkbox_checked(DangerousGetHandle());
        }

        /// <summary>
        /// Get the state of the alert dialog's check box.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool CheckboxChecked { [AvailableSince(10, 0)]get; private set; }
    }
}
