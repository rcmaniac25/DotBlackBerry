using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Dialog
{
    /// <summary>
    /// Login dialog.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class LoginDialog : Dialog
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_create_login(out IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_set_login_message_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_login_message_has_emoticons(IntPtr dialog, bool has_emoticons);

        [DllImport("bps")]
        private static extern int dialog_set_login_error_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_login_username(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_login_username_placeholder(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string placeholder);

        [DllImport("bps")]
        private static extern int dialog_set_login_username_keyboard_layout(IntPtr dialog, int layout);

        [DllImport("bps")]
        private static extern int dialog_set_login_password(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_login_password_placeholder(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string placeholder);

        [DllImport("bps")]
        private static extern int dialog_set_login_password_keyboard_layout(IntPtr dialog, int layout);

        [DllImport("bps")]
        private static extern int dialog_set_login_remember_me(IntPtr dialog, bool remember_me);

        [DllImport("bps")]
        private static extern int dialog_set_login_remember_me_label(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string label);

        [DllImport("bps")]
        internal static extern IntPtr dialog_event_get_login_username(IntPtr ev);

        [DllImport("bps")]
        internal static extern IntPtr dialog_event_get_login_password(IntPtr ev);

        [DllImport("bps")]
        internal static extern bool dialog_event_get_login_remember_me(IntPtr ev);

        #endregion

        private string message = null; //XXX
        private bool hasEmoticons = false; //XXX

        /// <summary>
        /// Create a new login dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public LoginDialog()
            : base()
        {
        }

        internal override void CreateDialog()
        {
            if (dialog_create_login(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        internal override DialogEvent GetEventForDialog(IntPtr ev)
        {
            return new LoginDialogEvent(ev, this);
        }

        #region Properties

        /// <summary>
        /// Get or set the message text of a login dialog.
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
                    if (dialog_set_login_message_text(handle, value) != BPS.BPS_SUCCESS)
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
                    if (dialog_set_login_message_has_emoticons(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    hasEmoticons = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Set the error text of a login dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string ErrorText
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_login_error_text(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the initial text of the username input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string UsernameInitial
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_login_username(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the placeholder text of the username input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string UsernamePlaceholder
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_login_username_placeholder(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Specify the layout to use on the virtual keyboard for the username input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardLayout UsernameKeyboard
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_login_username_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the initial text of the password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string PasswordInitial
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_login_password(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the placeholder text of the password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string PasswordPlaceholder
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_login_password_placeholder(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Specify the layout to use on the virtual keyboard for the password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardLayout PasswordKeyboard
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_login_password_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the initial state of the "Remember me" check box.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool RememberMeChecked
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_login_remember_me(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the label for the "Remember me" check box.
        /// </summary>
        [AvailableSince(10, 0)]
        public string RememberMeLabel
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_login_remember_me_label(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        #endregion
    }

    /// <summary>
    /// Event associated with a login.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class LoginDialogEvent : DialogEvent
    {
        internal LoginDialogEvent(IntPtr ev, LoginDialog dia)
            : base(ev, dia)
        {
            Util.GetBPSOrException();
            var ptr = DangerousGetHandle();
            Username = Marshal.PtrToStringAnsi(LoginDialog.dialog_event_get_login_username(ptr));
            Password = Marshal.PtrToStringAnsi(LoginDialog.dialog_event_get_login_password(ptr));
            RememberMe = LoginDialog.dialog_event_get_login_remember_me(ptr);
        }

        /// <summary>
        /// Get the contents of the login dialog's username field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Username { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get the contents of the login dialog's password field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Password { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get the state of the login dialog's "Remember me" check box.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool RememberMe { [AvailableSince(10, 0)]get; private set; }
    }
}
