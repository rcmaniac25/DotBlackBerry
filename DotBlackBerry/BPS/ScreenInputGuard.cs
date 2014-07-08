using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Screen Input Guard turns off the screen and disables the touchscreen when something (assumed to be a face) is detected near the screen.
    /// </summary>
    [AvailableSince(10, 2)]
    public static class ScreenInputGuard
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screen_input_guard_request_events(int flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screen_input_guard_stop_events(int flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screen_input_guard_get_domain();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screen_input_guard_enable();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screen_input_guard_disable();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screen_input_guard_event_get_status(IntPtr ev);

        #endregion

        private const int SCREEN_INPUT_GUARD_EVENT_STATUS = 1;

        private const int SCREEN_INPUT_GUARD_STATUS_DEACTIVATED = 0;
        private const int SCREEN_INPUT_GUARD_STATUS_ACTIVATED = 1;

        #region BPS

        /// <summary>
        /// Get the unique domain ID for the Screen Input Guard service.
        /// </summary>
        [AvailableSince(10, 2)]
        public static int Domain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return screen_input_guard_get_domain();
            }
        }

        /// <summary>
        /// Start receiving Screen Input Guard events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 2)]
        public static bool RequestEvents()
        {
            Util.GetBPSOrException();
            return screen_input_guard_request_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving Screen Input Guard events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 2)]
        public static bool StopEvents()
        {
            Util.GetBPSOrException();
            return screen_input_guard_stop_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get if the Screen Input Guard has been activated or deactivated.
        /// </summary>
        /// <param name="ev">A BPSEvent to get the status of.</param>
        /// <returns>true if the Screen Input Guard has been activated, false if it has been deactivated.</returns>
        [AvailableSince(10, 2)]
        public static bool IsActivated(BPSEvent ev)
        {
            if (ev.Domain != ScreenInputGuard.Domain)
            {
                throw new ArgumentException("BPSEvent is not a Screen Input Guard event");
            }
            if (ev.Code != SCREEN_INPUT_GUARD_EVENT_STATUS)
            {
                throw new ArgumentException("BPSEvent is an unknown Screen Input Guard event");
            }
            var result = screen_input_guard_event_get_status(ev.DangerousGetHandle());
            if (result == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return result == SCREEN_INPUT_GUARD_STATUS_ACTIVATED;
        }

        #endregion

        /// <summary>
        /// Enable Screen Input Guard.
        /// </summary>
        /// <returns>true if the Screen Input Guard has been enabled, false if otherwise.</returns>
        [AvailableSince(10, 2)]
        public static bool Enable()
        {
            return screen_input_guard_enable() == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Disable Screen Input Guard.
        /// </summary>
        /// <returns>true if the Screen Input Guard has been disabled, false if otherwise.</returns>
        [AvailableSince(10, 2)]
        public static bool Disable()
        {
            return screen_input_guard_disable() == BPS.BPS_SUCCESS;
        }
    }
}
