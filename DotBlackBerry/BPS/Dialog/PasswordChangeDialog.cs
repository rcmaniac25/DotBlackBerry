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
        /// Set the error text of a password change dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string ErrorText
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_password_change_error_text(handle, value) != BPS.BPS_SUCCESS)
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
                if (dialog_set_password_change_username(handle, value) != BPS.BPS_SUCCESS)
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
                if (dialog_set_password_change_username_placeholder(handle, value) != BPS.BPS_SUCCESS)
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
                if (dialog_set_password_change_username_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the initial text of the old password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string OldPasswordInitial
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_password_change_old_password(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the placeholder text of the old password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string OldPasswordPlaceholder
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_password_change_old_password_placeholder(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Specify the layout to use on the virtual keyboard for the old password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardLayout OldPasswordKeyboard
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_password_change_old_password_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the initial text of the new password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string NewPasswordInitial
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_password_change_new_password(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the placeholder text of the new password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string NewPasswordPlaceholder
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_password_change_new_password_placeholder(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Specify the layout to use on the virtual keyboard for the new password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardLayout NewPasswordKeyboard
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_password_change_new_password_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the initial text of the confirm password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string ConfirmPasswordInitial
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_password_change_confirm_password(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Set the placeholder text of the confirm password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string ConfirmPasswordPlaceholder
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_password_change_confirm_password_placeholder(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
            }
        }

        /// <summary>
        /// Specify the layout to use on the virtual keyboard for the confirm password input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardLayout ConfirmPasswordKeyboard
        {
            [AvailableSince(10, 0)]
            set
            {
                if (dialog_set_password_change_confirm_password_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
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
                if (dialog_set_password_change_remember_me(handle, value) != BPS.BPS_SUCCESS)
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
                if (dialog_set_password_change_remember_me_label(handle, value) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                UpdateDialog();
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
