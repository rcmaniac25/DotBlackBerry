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

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_create_alert(out IntPtr dialog);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_alert_message_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_alert_message_has_emoticons(IntPtr dialog, bool has_emoticons);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_alert_checkbox_checked(IntPtr dialog, bool checkd);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_alert_checkbox_label(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string label);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_alert_checkbox_enabled(IntPtr dialog, bool enabled);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_alert_icon(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string path);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern bool dialog_event_get_alert_checkbox_checked(IntPtr ev);

        #endregion

        private string message = null; //XXX
        private bool hasEmoticons = false; //XXX
        private bool checkBox = false; //XXX
        private string checkBoxLabel = null; //XXX
        private bool checkBoxEnabled = true; //XXX
        private string icon = null; //XXX

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
        /// Get or set the message text of an alert dialog.
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
                    if (dialog_set_alert_message_text(handle, value) != BPS.BPS_SUCCESS)
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
                    if (dialog_set_alert_message_has_emoticons(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    hasEmoticons = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the initial state of the check box.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool IsCheckboxChecked
        {
            [AvailableSince(10, 0)]
            get
            {
                return checkBox;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (checkBox != value)
                {
                    if (dialog_set_alert_checkbox_checked(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    checkBox = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Set the label for the check box.
        /// </summary>
        [AvailableSince(10, 0)]
        public string CheckboxLabel
        {
            [AvailableSince(10, 0)]
            get
            {
                return checkBoxLabel;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (checkBoxLabel != value)
                {
                    if (dialog_set_alert_checkbox_label(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    checkBoxLabel = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Set whether the check box is enabled.
        /// </summary>
        [AvailableSince(10, 2)]
        public bool IsCheckboxEnabled
        {
            [AvailableSince(10, 2)]
            get
            {
                return checkBoxEnabled;
            }
            [AvailableSince(10, 2)]
            set
            {
                if (checkBoxEnabled != value)
                {
                    if (dialog_set_alert_checkbox_enabled(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    checkBoxEnabled = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Set the icon of a alert dialog.
        /// </summary>
        [AvailableSince(10, 2)]
        public string IconPath
        {
            [AvailableSince(10, 2)]
            get
            {
                return icon;
            }
            [AvailableSince(10, 2)]
            set
            {
                if (icon != value)
                {
                    if (dialog_set_alert_icon(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    icon = value;
                    UpdateDialog();
                }
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
