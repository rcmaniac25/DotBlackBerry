using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Dialog
{
    /// <summary>
    /// A prompt dialog is a simple dialog with a title, a message, an input field, and buttons.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class PromptDialog : Dialog
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_create_prompt(out IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_set_prompt_message_text(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_prompt_message_has_emoticons(IntPtr dialog, bool has_emoticons);

        [DllImport("bps")]
        private static extern int dialog_set_prompt_input_field(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_prompt_input_placeholder(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string input_placeholder);

        [DllImport("bps")]
        private static extern int dialog_set_prompt_maximum_characters(IntPtr dialog, int max_chars);

        [DllImport("bps")]
        private static extern int dialog_set_prompt_display_as_password(IntPtr dialog, bool display_as_password);

        [DllImport("bps")]
        private static extern int dialog_set_prompt_input_keyboard_layout(IntPtr dialog, int layout);

        [DllImport("bps")]
        internal static extern IntPtr dialog_event_get_prompt_input_field(IntPtr ev);

        #endregion

        private string message = null; //XXX
        private bool hasEmoticons = false; //XXX
        private string input = null; //XXX
        private string inputPlaceholder = null; //XXX
        private int maxChars = 1024; //XXX
        private bool displayAsPassword = false;
        private VirtualKeyboardLayout keyboard = VirtualKeyboardLayout.Default;

        /// <summary>
        /// Create a new prompt dialog.
        /// </summary>
        /// <param name="initialText">The initial input field text.</param>
        [AvailableSince(10, 0)]
        public PromptDialog(string initialText = null)
            : base()
        {
            if (!string.IsNullOrWhiteSpace(initialText) && dialog_set_prompt_input_field(handle, initialText) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        internal override void CreateDialog()
        {
            if (dialog_create_prompt(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        internal override DialogEvent GetEventForDialog(IntPtr ev)
        {
            return new PromptDialogEvent(ev, this);
        }

        #region Properties

        /// <summary>
        /// Get or set a prompt dialog's message text.
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
                    if (dialog_set_prompt_message_text(handle, value) != BPS.BPS_SUCCESS)
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
                    if (dialog_set_prompt_message_has_emoticons(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    hasEmoticons = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the initial text of the input field of a prompt dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string InputField
        {
            [AvailableSince(10, 0)]
            get
            {
                return input;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (input != value)
                {
                    if (dialog_set_prompt_input_field(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    input = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the placeholder text of the input field of a prompt dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string InputPlaceholder
        {
            [AvailableSince(10, 0)]
            get
            {
                return inputPlaceholder;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (inputPlaceholder != value)
                {
                    if (dialog_set_prompt_input_placeholder(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    inputPlaceholder = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Get or set the maximum number of characters of the input field of a prompt dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public int MaxInputCharacters
        {
            [AvailableSince(10, 0)]
            get
            {
                return maxChars;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (maxChars != value)
                {
                    if (dialog_set_prompt_maximum_characters(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    maxChars = value;
                }
            }
        }

        /// <summary>
        /// Get or set whether to display the field as a password input.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool ShouldDisplayAsPassword
        {
            [AvailableSince(10, 0)]
            get
            {
                return displayAsPassword;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (displayAsPassword != value)
                {
                    if (dialog_set_prompt_display_as_password(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    displayAsPassword = value;
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Specify the layout to use on the virtual keyboard.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardLayout KeyboardLayout
        {
            [AvailableSince(10, 0)]
            get
            {
                return keyboard;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (keyboard != value)
                {
                    if (dialog_set_prompt_input_keyboard_layout(handle, VirtualKeyboard.LayoutToInt(value)) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    keyboard = value;
                    ResetInputFlags();
                    UpdateDialog();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Event associated with a prompt dialog.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class PromptDialogEvent : DialogEvent
    {
        internal PromptDialogEvent(IntPtr ev, PromptDialog dia)
            : base(ev, dia)
        {
            Util.GetBPSOrException();
            InputField = Marshal.PtrToStringAnsi(PromptDialog.dialog_event_get_prompt_input_field(DangerousGetHandle()));
        }

        /// <summary>
        /// Get the contents of the prompt dialog input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public string InputField { [AvailableSince(10, 0)]get; private set; }
    }
}
