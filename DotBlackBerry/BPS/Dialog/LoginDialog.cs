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

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_create_login(out IntPtr dialog);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_login_message_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_login_message_has_emoticons(IntPtr dialog, bool has_emoticons);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_login_error_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_login_username(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_login_username_placeholder(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string placeholder);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_login_username_keyboard_layout(IntPtr dialog, int layout);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_login_password(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_login_password_placeholder(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string placeholder);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_login_password_keyboard_layout(IntPtr dialog, int layout);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_login_remember_me(IntPtr dialog, bool remember_me);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_login_remember_me_label(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string label);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr dialog_event_get_login_username(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr dialog_event_get_login_password(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern bool dialog_event_get_login_remember_me(IntPtr ev);

        #endregion

        private string message = null; //XXX
        private bool hasEmoticons = false; //XXX
        private string error = null; //XXX
        private string user = null; //XXX
        private string userPlaceholder = null; //XXX
        private VirtualKeyboardLayout userKeyboard = VirtualKeyboardLayout.Default;
        private string pass = null; //XXX
        private string passPlaceholder = null; //XXX
        private VirtualKeyboardLayout passKeyboard = VirtualKeyboardLayout.Default;
        private bool rememberMe = false; //XXX
        private string rememberMeLabel = null; //XXX

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
        /// Get or set the error text of a login dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string ErrorText
        {
            [AvailableSince(10, 0)]
            get
            {
                return error;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (error != value)
                {
                    if (dialog_set_login_error_text(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    error = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the initial text of the username input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string UsernameInitial
        {
            [AvailableSince(10, 0)]
            get
            {
                return user;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (user != value)
                {
                    if (dialog_set_login_username(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    user = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the placeholder text of the username input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string UsernamePlaceholder
        {
            [AvailableSince(10, 0)]
            get
            {
                return userPlaceholder;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (userPlaceholder != value)
                {
                    if (dialog_set_login_username_placeholder(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    userPlaceholder = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Specify the layout to use on the virtual keyboard for the username input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardLayout UsernameKeyboard
        {
            [AvailableSince(10, 0)]
            get
            {
                return userKeyboard;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (userKeyboard != value)
                {
                    if (dialog_set_login_username_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    userKeyboard = value;
                    ResetInputFlags();
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the initial text of the password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string PasswordInitial
        {
            [AvailableSince(10, 0)]
            get
            {
                return pass;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (pass != value)
                {
                    if (dialog_set_login_password(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    pass = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the placeholder text of the password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string PasswordPlaceholder
        {
            [AvailableSince(10, 0)]
            get
            {
                return passPlaceholder;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (passPlaceholder != value)
                {
                    if (dialog_set_login_password_placeholder(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    passPlaceholder = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Specify the layout to use on the virtual keyboard for the password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardLayout PasswordKeyboard
        {
            [AvailableSince(10, 0)]
            get
            {
                return passKeyboard;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (passKeyboard != value)
                {
                    if (dialog_set_login_password_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    passKeyboard = value;
                    ResetInputFlags();
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the initial state of the "Remember me" check box.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool RememberMeChecked
        {
            [AvailableSince(10, 0)]
            get
            {
                return rememberMe;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (rememberMe != value)
                {
                    if (dialog_set_login_remember_me(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    rememberMe = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the label for the "Remember me" check box.
        /// </summary>
        [AvailableSince(10, 0)]
        public string RememberMeLabel
        {
            [AvailableSince(10, 0)]
            get
            {
                return rememberMeLabel;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (rememberMeLabel != value)
                {
                    if (dialog_set_login_remember_me_label(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    rememberMeLabel = value;
                    UpdateDialog();
                }
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
