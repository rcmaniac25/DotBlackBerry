using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// The physical buttons.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum Button : int
    {
        /// <summary>
        /// The power button.
        /// </summary>
        [AvailableSince(10, 0)]
        Power = 0,
        /// <summary>
        /// The play-pause button.
        /// </summary>
        [AvailableSince(10, 0)]
        PlayPause = 1,
        /// <summary>
        /// The plus button.
        /// </summary>
        [AvailableSince(10, 0)]
        Plus = 2,
        /// <summary>
        /// The minus button.
        /// </summary>
        [AvailableSince(10, 0)]
        Minus = 3
    }

    /// <summary>
    /// Button event.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class ButtonEvent : BPSEvent
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int button_request_events(int flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int button_stop_events(int flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int button_get_domain();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int button_event_get_button(IntPtr ev);

        #endregion

        internal ButtonEvent(IntPtr ev, bool pressed)
            : base(ev, false)
        {
            IsPressed = pressed;
            var result = button_event_get_button(ev);
            if (result == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            Button = (Button)result;
        }

        private const int BUTTON_UP = 0;
        private const int BUTTON_DOWN = 1;

        /// <summary>
        /// Get if the button is pressed or released.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool IsPressed { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get what button has been pressed or released.
        /// </summary>
        [AvailableSince(10, 0)]
        public Button Button { [AvailableSince(10, 0)]get; private set; }

        #region BPS

        /// <summary>
        /// Retrieve the unique domain ID for the button service.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int BPSDomain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return button_get_domain();
            }
        }

        /// <summary>
        /// Start receiving button status change events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestEvents()
        {
            Util.GetBPSOrException();
            return button_request_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving button status change events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopEvents()
        {
            Util.GetBPSOrException();
            return button_stop_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get a button event from a BPS Event.
        /// </summary>
        /// <param name="ev">A BPSEvent to convert to a ButtonEvent.</param>
        /// <returns>The ButtonEvent for the event.</returns>
        [AvailableSince(10, 0)]
        public static ButtonEvent GetButtonEvent(BPSEvent ev)
        {
            if (ev.Domain != ButtonEvent.BPSDomain)
            {
                throw new ArgumentException("BPSEvent is not a button event");
            }
            var code = ev.Code;
            if (code != BUTTON_UP && code != BUTTON_DOWN)
            {
                throw new ArgumentException("BPSEvent is an unknown button event");
            }
            return new ButtonEvent(ev.DangerousGetHandle(), code == BUTTON_DOWN);
        }

        #endregion
    }
}
