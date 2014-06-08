using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Dialog
{
    /// <summary>
    /// Password change dialog.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class PasswordChangeDialog : Dialog
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_create_password_change(out IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_error_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_username(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_username_placeholder(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string placeholder);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_username_keyboard_layout(IntPtr dialog, int layout);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_old_password(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_old_password_placeholder(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string placeholder);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_old_password_keyboard_layout(IntPtr dialog, int layout);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_new_password(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_new_password_placeholder(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string placeholder);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_new_password_keyboard_layout(IntPtr dialog, int layout);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_confirm_password(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_confirm_password_placeholder(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string placeholder);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_confirm_password_keyboard_layout(IntPtr dialog, int layout);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_remember_me(IntPtr dialog, bool remember_me);

        [DllImport("bps")]
        private static extern int dialog_set_password_change_remember_me_label(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string label);

        [DllImport("bps")]
        internal static extern IntPtr dialog_event_get_password_change_username(IntPtr ev);

        [DllImport("bps")]
        internal static extern IntPtr dialog_event_get_password_change_old_password(IntPtr ev);

        [DllImport("bps")]
        internal static extern IntPtr dialog_event_get_password_change_new_password(IntPtr ev);

        [DllImport("bps")]
        internal static extern IntPtr dialog_event_get_password_change_confirm_password(IntPtr ev);

        [DllImport("bps")]
        internal static extern bool dialog_event_get_password_change_remember_me(IntPtr ev);

        #endregion

        private string error = null; //XXX
        private string user = null; //XXX
        private string userPlaceholder = null; //XXX
        private VirtualKeyboardLayout userKeyboard = VirtualKeyboardLayout.Default;
        private string oldPass = null; //XXX
        private string oldPassPlaceholder = null; //XXX
        private VirtualKeyboardLayout oldPassKeyboard = VirtualKeyboardLayout.Default;
        private string newPass = null; //XXX
        private string newPassPlaceholder = null; //XXX
        private VirtualKeyboardLayout newPassKeyboard = VirtualKeyboardLayout.Default;
        private string confPass = null; //XXX
        private string confPassPlaceholder = null; //XXX
        private VirtualKeyboardLayout confPassKeyboard = VirtualKeyboardLayout.Default;
        private bool rememberMe = false; //XXX
        private string rememberMeLabel = null; //XXX

        /// <summary>
        /// Create a new password change dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public PasswordChangeDialog()
            : base()
        {
        }

        internal override void CreateDialog()
        {
            if (dialog_create_password_change(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        internal override DialogEvent GetEventForDialog(IntPtr ev)
        {
            return new PasswordChangeDialogEvent(ev, this);
        }

        #region Properties

        /// <summary>
        /// Get or set the error text of a password change dialog.
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
                    if (dialog_set_password_change_error_text(handle, value) != BPS.BPS_SUCCESS)
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
                    if (dialog_set_password_change_username(handle, value) != BPS.BPS_SUCCESS)
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
                    if (dialog_set_password_change_username_placeholder(handle, value) != BPS.BPS_SUCCESS)
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
                    if (dialog_set_password_change_username_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
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
        /// Get or set the initial text of the old password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string OldPasswordInitial
        {
            [AvailableSince(10, 0)]
            get
            {
                return oldPass;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (oldPass != value)
                {
                    if (dialog_set_password_change_old_password(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    oldPass = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the placeholder text of the old password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string OldPasswordPlaceholder
        {
            [AvailableSince(10, 0)]
            get
            {
                return oldPassPlaceholder;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (oldPassPlaceholder != value)
                {
                    if (dialog_set_password_change_old_password_placeholder(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    oldPassPlaceholder = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Specify the layout to use on the virtual keyboard for the old password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardLayout OldPasswordKeyboard
        {
            [AvailableSince(10, 0)]
            get
            {
                return oldPassKeyboard;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (oldPassKeyboard != value)
                {
                    if (dialog_set_password_change_old_password_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    oldPassKeyboard = value;
                    ResetInputFlags();
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the initial text of the new password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string NewPasswordInitial
        {
            [AvailableSince(10, 0)]
            get
            {
                return newPass;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (newPass != value)
                {
                    if (dialog_set_password_change_new_password(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    newPass = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the placeholder text of the new password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string NewPasswordPlaceholder
        {
            [AvailableSince(10, 0)]
            get
            {
                return newPassPlaceholder;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (newPassPlaceholder != value)
                {
                    if (dialog_set_password_change_new_password_placeholder(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    newPassPlaceholder = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Specify the layout to use on the virtual keyboard for the new password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardLayout NewPasswordKeyboard
        {
            [AvailableSince(10, 0)]
            get
            {
                return newPassKeyboard;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (newPassKeyboard != value)
                {
                    if (dialog_set_password_change_new_password_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    newPassKeyboard = value;
                    ResetInputFlags();
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the initial text of the confirm password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string ConfirmPasswordInitial
        {
            [AvailableSince(10, 0)]
            get
            {
                return confPass;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (confPass != value)
                {
                    if (dialog_set_password_change_confirm_password(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    confPass = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the placeholder text of the confirm password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string ConfirmPasswordPlaceholder
        {
            [AvailableSince(10, 0)]
            get
            {
                return confPassPlaceholder;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (confPassPlaceholder != value)
                {
                    if (dialog_set_password_change_confirm_password_placeholder(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    confPassPlaceholder = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Specify the layout to use on the virtual keyboard for the confirm password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardLayout ConfirmPasswordKeyboard
        {
            [AvailableSince(10, 0)]
            get
            {
                return confPassKeyboard;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (confPassKeyboard != value)
                {
                    if (dialog_set_password_change_confirm_password_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    confPassKeyboard = value;
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
                    if (dialog_set_password_change_remember_me(handle, value) != BPS.BPS_SUCCESS)
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
                    if (dialog_set_password_change_remember_me_label(handle, value) != BPS.BPS_SUCCESS)
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
    /// Event associated with a password change.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class PasswordChangeDialogEvent : DialogEvent
    {
        internal PasswordChangeDialogEvent(IntPtr ev, PasswordChangeDialog dia)
            : base(ev, dia)
        {
            Util.GetBPSOrException();
            var ptr = DangerousGetHandle();
            Username = Marshal.PtrToStringAnsi(PasswordChangeDialog.dialog_event_get_password_change_username(ptr));
            OldPassword = Marshal.PtrToStringAnsi(PasswordChangeDialog.dialog_event_get_password_change_old_password(ptr));
            NewPassword = Marshal.PtrToStringAnsi(PasswordChangeDialog.dialog_event_get_password_change_new_password(ptr));
            ConfirmPassword = Marshal.PtrToStringAnsi(PasswordChangeDialog.dialog_event_get_password_change_confirm_password(ptr));
            RememberMe = PasswordChangeDialog.dialog_event_get_password_change_remember_me(ptr);
        }

        /// <summary>
        /// Get the contents of the password change dialog's username field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Username { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get the contents of the password change dialog's old password field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string OldPassword { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get the contents of the password change dialog's new password field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string NewPassword { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get the contents of the password change dialog's confirm password field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string ConfirmPassword { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get the state of the password change dialog's "Remember me" check box.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool RememberMe { [AvailableSince(10, 0)]get; private set; }
    }
}
