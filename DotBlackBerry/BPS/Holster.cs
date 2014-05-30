using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Holster functionality.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class Holster
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int holster_request_events(int flags);

        [DllImport("bps")]
        private static extern int holster_stop_events(int flags);

        [DllImport("bps")]
        private static extern int holster_get_domain();

        [DllImport("bps")]
        private static extern int holster_event_get_holster_status(IntPtr ev);

        #endregion

        private Holster()
        {
        }

        private const int HOLSTER_INFO = 1;

        private const int HOLSTER_IN = 0;
        private const int HOLSTER_OUT = 1;

        #region BPS

        /// <summary>
        /// Get the unique domain ID for the holster service.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int Domain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return holster_get_domain();
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
            return holster_request_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving button status change events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopEvents()
        {
            Util.GetBPSOrException();
            return holster_stop_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get if the device is holstered.
        /// </summary>
        /// <param name="ev">A BPSEvent to check if holstered.</param>
        /// <returns>true if the device is holstered, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool GetIfHolstered(BPSEvent ev)
        {
            if (ev.Domain != Holster.Domain)
            {
                throw new ArgumentException("BPSEvent is not a holster event");
            }
            if (ev.Code != HOLSTER_INFO)
            {
                throw new ArgumentException("BPSEvent is an unknown holster event");
            }
            var result = holster_event_get_holster_status(ev.DangerousGetHandle());
            if (result == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForErrno();
            }
            return result == HOLSTER_IN;
        }

        #endregion
    }
}
