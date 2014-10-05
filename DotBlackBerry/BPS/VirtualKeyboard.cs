using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Virtual keyboard layouts.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum VirtualKeyboardLayout : int
    {
        /// <summary>
        /// The default layout.
        /// </summary>
        [AvailableSince(10, 0)]
        Default = 0,
        /// <summary>
        /// Layout for entering URLs.
        /// </summary>
        [AvailableSince(10, 0)]
        Url = 1,
        /// <summary>
        /// Layout for entering email addresses.
        /// </summary>
        [AvailableSince(10, 0)]
        Email = 2,
        /// <summary>
        /// Layout for use with the Web.
        /// </summary>
        [AvailableSince(10, 0)]
        Web = 3,
        /// <summary>
        /// Layout showing numbers and punctuation.
        /// </summary>
        [AvailableSince(10, 0)]
        NumbersAndPunctuation = 4,
        /// <summary>
        /// Layout showing symbols.
        /// </summary>
        [AvailableSince(10, 0)]
        Symbol = 5,
        /// <summary>
        /// Layout for entering phone numbers.
        /// </summary>
        [AvailableSince(10, 0)]
        Phone = 6,
        /// <summary>
        /// Layout for entering PINs.
        /// </summary>
        [AvailableSince(10, 0)]
        Pin = 7,
        /// <summary>
        /// Layout for entering passwords.
        /// </summary>
        [AvailableSince(10, 0)]
        Password = 8,
        /// <summary>
        /// Layout for entering the PIN of a SIM card.
        /// </summary>
        [AvailableSince(10, 0)]
        SimPin = 9,
        /// <summary>
        /// Layout for entering numbers.
        /// </summary>
        [AvailableSince(10, 2)]
        Number = 10,
        /// <summary>
        /// Layout for entering alphabetic characters and numbers.
        /// </summary>
        [AvailableSince(10, 3)]
        Alphanumeric = 11
    }

    /// <summary>
    /// Text for the Enter key on the virtual keyboard.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum VirtualKeyboardEnter : int
    {
        /// <summary>
        /// The default Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Default = 0,
        /// <summary>
        /// Display "Go" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Go = 1,
        /// <summary>
        /// Display "Join" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Join = 2,
        /// <summary>
        /// Display "Next" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Next = 3,
        /// <summary>
        /// Display "Search" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Search = 4,
        /// <summary>
        /// Display "Send" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Send = 5,
        /// <summary>
        /// Display "Submit" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Submit = 6,
        /// <summary>
        /// Display "Done" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Done = 7,
        /// <summary>
        /// Display "Connect" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Connect = 8,
        /// <summary>
        /// Display "Replace" on the Enter key.
        /// </summary>
        [AvailableSince(10, 2)]
        Replace = 9
    }

    /// <summary>
    /// Virtual keyboard.
    /// </summary>
    [AvailableSince(10, 0)]
    public static class VirtualKeyboard
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern void virtualkeyboard_change_options(int layout, int enter);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern void virtualkeyboard_change_options_v2(int layout, int enter, bool no_help);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int virtualkeyboard_get_height(out int pixels);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int virtualkeyboard_request_events(int flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int virtualkeyboard_stop_events(int flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int virtualkeyboard_get_domain();

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int virtualkeyboard_event_get_height(IntPtr ev);

        #endregion

        private const int VIRTUALKEYBOARD_EVENT_VISIBLE = 1;
        private const int VIRTUALKEYBOARD_EVENT_HIDDEN = 2;
        private const int VIRTUALKEYBOARD_EVENT_INFO = 3;

        /// <summary>
        /// Get the height of the virtual keyboard.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int Height
        {
            [AvailableSince(10, 0)]
            get
            {
                int height;
                if (virtualkeyboard_get_height(out height) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                return height;
            }
        }

        #region BPS

        /// <summary>
        /// Get the unique domain ID for the virtual keyboard service.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int Domain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return virtualkeyboard_get_domain();
            }
        }

        /// <summary>
        /// Start receiving virtual keyboard events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestEvents()
        {
            Util.GetBPSOrException();
            return virtualkeyboard_request_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving virtual keyboard events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopEvents()
        {
            Util.GetBPSOrException();
            return virtualkeyboard_stop_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get a virtual keyboard event from a BPS Event.
        /// </summary>
        /// <param name="ev">A BPSEvent to convert to a VirtualKeyboardEvent.</param>
        /// <returns>The VirtualKeyboardEvent for the event.</returns>
        [AvailableSince(10, 0)]
        public static VirtualKeyboardEvent GetVirtualKeyboardEvent(BPSEvent ev)
        {
            if (ev.Domain != ScreenEvent.BPSDomain)
            {
                throw new ArgumentException("BPSEvent is not a virtual keyboard event");
            }
            var code = ev.Code;
            if (code != VIRTUALKEYBOARD_EVENT_VISIBLE && 
                code != VIRTUALKEYBOARD_EVENT_HIDDEN &&
                code != VIRTUALKEYBOARD_EVENT_INFO)
            {
                throw new ArgumentException("BPSEvent is an unknown virtual keyboard event");
            }
            return new VirtualKeyboardEvent(ev.DangerousGetHandle(), code == VIRTUALKEYBOARD_EVENT_INFO, code == VIRTUALKEYBOARD_EVENT_VISIBLE);
        }

        #endregion

        #region Functions

        internal static int LayoutToInt(VirtualKeyboardLayout layout)
        {
            var members = typeof(VirtualKeyboardLayout).GetMember(layout.ToString());
            if (members.Length == 0)
            {
                throw new NotSupportedException(string.Format("Unknown keyboard layout: {0}", layout));
            }
            Util.ThrowIfUnsupported(members[0], string.Format("The keyboard layout, {0}, is not supported.", layout));
            return (int)layout;
        }

        internal static int EnterKeyToInt(VirtualKeyboardEnter enter)
        {
            if (enter == VirtualKeyboardEnter.Replace && !Util.IsCapableOfRunning(10, 2))
            {
                throw new NotSupportedException("The keyboard enter key, Replace, is not supported on this OS. Requires OS 10.2 or higher");
            }
            return (int)enter;
        }

        /// <summary>
        /// Display the virtual keyboard.
        /// </summary>
        [DllImport(BPS.BPS_LIBRARY, EntryPoint = "virtualkeyboard_show"), AvailableSince(10, 0)]
        public static extern void Show();

        /// <summary>
        /// Hide the virtual keyboard.
        /// </summary>
        [DllImport(BPS.BPS_LIBRARY, EntryPoint = "virtualkeyboard_hide"), AvailableSince(10, 0)]
        public static extern void Hide();

        /// <summary>
        /// Change the virtual keyboard layout, and Enter key options.
        /// </summary>
        /// <param name="layout">The virtual keyboard layout to set.</param>
        /// <param name="enter">The Enter key text to set.</param>
        /// <param name="noHelp">Turn off the hints if true. Otherwise, turn on the hints. This only works on 10.3 and higher.</param>
        [AvailableSince(10, 0)]
        public static void SetOptions(VirtualKeyboardLayout layout, VirtualKeyboardEnter enter, bool noHelp = true)
        {
            if (Util.IsCapableOfRunning(10, 3))
            {
                virtualkeyboard_change_options_v2(LayoutToInt(layout), EnterKeyToInt(enter), noHelp);
            }
            else
            {
                virtualkeyboard_change_options(LayoutToInt(layout), EnterKeyToInt(enter));
            }
        }

        #endregion
    }

    /// <summary>
    /// Event details related to the virtual keyboard.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class VirtualKeyboardEvent : BPSEvent
    {
        internal VirtualKeyboardEvent(IntPtr ev, bool isInfo, bool isVisible)
            : base(ev, false)
        {
            Util.GetBPSOrException();
            if (isInfo)
            {
                Height = VirtualKeyboard.virtualkeyboard_event_get_height(ev);
                IsVisible = Height > 0;
            }
            else
            {
                Height = VirtualKeyboard.Height;
                IsVisible = isVisible;
            }
        }

        /// <summary>
        /// Get the height of the virtual keyboard.
        /// </summary>
        [AvailableSince(10, 0)]
        public int Height { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get if the virtual keyboard is visible.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool IsVisible { [AvailableSince(10, 0)]get; private set; }
    }
}
