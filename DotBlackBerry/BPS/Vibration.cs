using System;
using System.Threading;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Vibration intensities.
    /// </summary>
    [AvailableSince(10, 1)]
    public enum VibrationIntensity : int
    {
        /// <summary>
        /// Specifies a low intensity vibration.
        /// </summary>
        [AvailableSince(10, 1)]
        Low = 1,
        /// <summary>
        /// Specifies a regular vibration.
        /// </summary>
        [AvailableSince(10, 1)]
        Medium = 10,
        /// <summary>
        /// Specifies a high intensity vibration.
        /// </summary>
        [AvailableSince(10, 1)]
        High = 100
    }

    /// <summary>
    /// Vibration status
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class VibrationStatus
    {
        internal VibrationStatus(IntPtr ev)
        {
            Util.GetBPSOrException();
            int value;
            if (Vibration.vibration_event_get_duration(ev, out value) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            Duration = value < 0 ? Timeout.InfiniteTimeSpan : TimeSpan.FromMilliseconds(value);
            if (Vibration.vibration_event_get_time_left(ev, out value) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            TimeRemaining = value < 0 ? Timeout.InfiniteTimeSpan : TimeSpan.FromMilliseconds(value);
            if (Vibration.vibration_event_get_intensity(ev, out value) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            Intensity = value;
        }

        /// <summary>
        /// Retrieve the duration of a vibration.
        /// </summary>
        [AvailableSince(10, 0)]
        public TimeSpan Duration { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Retrieve the remaining vibration time
        /// </summary>
        [AvailableSince(10, 0)]
        public TimeSpan TimeRemaining { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Retrieve the intensity setting of a vibration.
        /// </summary>
        [AvailableSince(10, 0)]
        public int Intensity { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Retrieve the intensity setting of a vibration.
        /// </summary>
        [AvailableSince(10, 1)]
        public VibrationIntensity IntensityLevel
        {
            [AvailableSince(10, 1)]
            get
            {
                return (VibrationIntensity)Intensity;
            }
        }
    }

    /// <summary>
    /// Functions to control the vibration capabilities on the device.
    /// </summary>
    [AvailableSince(10, 0)]
    public static class Vibration
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern bool vibration_is_supported();

        [DllImport("bps")]
        private static extern int vibration_request_events(int flags);

        [DllImport("bps")]
        private static extern int vibration_stop_events(int flags);

        [DllImport("bps")]
        private static extern int vibration_get_domain();

        [DllImport("bps")]
        internal static extern int vibration_event_get_duration(IntPtr ev, out int duration);

        [DllImport("bps")]
        internal static extern int vibration_event_get_time_left(IntPtr ev, out int time_left);

        [DllImport("bps")]
        internal static extern int vibration_event_get_intensity(IntPtr ev, out int intensity);

        [DllImport("bps")]
        private static extern int vibration_request(int intensity, int duration);

        #endregion

        private const int VIBRATION_INFO = 1;

        /// <summary>
        /// Determine whether the device supports vibration.
        /// </summary>
        [AvailableSince(10, 0)]
        public static bool IsSupported
        {
            [AvailableSince(10, 0)]
            get
            {
                return vibration_is_supported();
            }
        }

        #region BPS

        /// <summary>
        /// Retrieve the unique domain ID for the vibration service.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int Domain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return vibration_get_domain();
            }
        }

        /// <summary>
        /// Start receiving vibration status change events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestEvents()
        {
            Util.GetBPSOrException();
            return vibration_request_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving vibration status change events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopEvents()
        {
            Util.GetBPSOrException();
            return vibration_stop_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get the status of the LEDs.
        /// </summary>
        /// <param name="ev">A BPSEvent to convert to LEDColor.</param>
        /// <returns>The LEDColor for the event.</returns>
        [AvailableSince(10, 0)]
        public static VibrationStatus GetVibrationStatus(BPSEvent ev)
        {
            if (ev.Domain != Vibration.Domain)
            {
                throw new ArgumentException("BPSEvent is not a Vibration event");
            }
            if (ev.Code != VIBRATION_INFO)
            {
                throw new ArgumentException("BPSEvent is an unknown Vibration event");
            }
            return new VibrationStatus(ev.DangerousGetHandle());
        }

        #endregion

        /// <summary>
        /// Cancel any vibration in progress.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool Cancel()
        {
            return Request(0, TimeSpan.Zero);
        }

        /// <summary>
        /// Request that the device vibrates.
        /// </summary>
        /// <param name="intensity">The intensity at which the device should vibrate, 0 to 100 inclusive.</param>
        /// <param name="duration">The length of time the device should vibrate for, 1 ms to 5000 ms inclusive. A TimeSpan of zero will stop any vibration.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool Request(int intensity, TimeSpan duration)
        {
            Util.GetBPSOrException();
            if (intensity < 0 || intensity > 100)
            {
                throw new ArgumentOutOfRangeException("intensity", "0 <= intensity <= 100");
            }
            if (duration.TotalMilliseconds < 0 || duration.TotalMilliseconds > 5000)
            {
                throw new ArgumentOutOfRangeException("duration", "0 ms <= duration <= 5000 ms");
            }
            return vibration_request(intensity, (int)duration.TotalMilliseconds) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Request that the device vibrates.
        /// </summary>
        /// <param name="intensity">The intensity at which the device should vibrate.</param>
        /// <param name="duration">The length of time the device should vibrate for, 1 ms to 5000 ms inclusive. A TimeSpan of zero will stop any vibration.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 1)]
        public static bool Request(VibrationIntensity intensity, TimeSpan duration)
        {
            return Request((int)intensity, duration);
        }
    }
}
