using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// The possible LED colors.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum LEDColor : int
    {
        /// <summary>
        /// LED is off.
        /// </summary>
        [AvailableSince(10, 0)]
        Black = 0x00000000,

        /// <summary>
        /// LED is blue.
        /// </summary>
        [AvailableSince(10, 0)]
        Blue = 0x000000FF,
        /// <summary>
        /// LED is green.
        /// </summary>
        [AvailableSince(10, 0)]
        Green = 0x0000FF00,
        /// <summary>
        /// LED is cyan.
        /// </summary>
        [AvailableSince(10, 0)]
        Cyan = 0x0000FFFF,
        /// <summary>
        /// LED is red.
        /// </summary>
        [AvailableSince(10, 0)]
        Red = 0x00FF0000,
        /// <summary>
        /// LED is magenta.
        /// </summary>
        [AvailableSince(10, 0)]
        Magenta = 0x00FF00FF,
        /// <summary>
        /// LED is yellow.
        /// </summary>
        [AvailableSince(10, 0)]
        Yellow = 0x00FFFF00,
        /// <summary>
        /// LED is white.
        /// </summary>
        [AvailableSince(10, 0)]
        White = 0x00FFFFFF
    }

    /// <summary>
    /// A running instance of the LED.
    /// </summary>
    [AvailableSince(10, 0), RequiredPermission(Permission.LED)]
    public sealed class LEDInstance
    {
        private string id;

        internal LEDInstance(Guid id)
        {
            this.id = id.ToString();
        }

        /// <summary>
        /// The color of the LED.
        /// </summary>
        [AvailableSince(10, 0)]
        public LEDColor Color { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get if the LED is blinking continuously.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool ContinuousBlinking { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get the number of times the LED will blink.
        /// </summary>
        [AvailableSince(10, 0)]
        public int BlinkCount { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Cancel a request to flash the LEDs.
        /// </summary>
        /// <returns>true if the LED flash request was canceled, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool Cancel()
        {
            return LED.led_cancel(id) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Update an instance to flash the LEDs with a named color.
        /// </summary>
        /// <param name="color">The color to flash the LEDs.</param>
        /// <param name="blinkCount">The number of times to blink.  Use a value of 0 to continue blinking until canceled or until the application exits.</param>
        /// <returns>true if the update worked, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool Update(LEDColor color, int blinkCount = 0)
        {
            if (blinkCount < 0)
            {
                throw new ArgumentOutOfRangeException("blinkCount", blinkCount, "0 <= blinkCount");
            }
            var success = LED.led_request_color(id, color, blinkCount) == BPS.BPS_SUCCESS;
            if (success)
            {
                Color = color;
                ContinuousBlinking = blinkCount == 0;
                BlinkCount = blinkCount;
            }
            return success;
        }
    }

    /// <summary>
    /// LED controls.
    /// </summary>
    [AvailableSince(10, 0)]
    public static class LED
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int led_request_events(int flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int led_stop_events(int flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int led_get_domain();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int led_event_get_rgb(IntPtr ev, out bool red, out bool green, out bool blue);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int led_request_color([MarshalAs(UnmanagedType.LPStr)]string id, LEDColor color, int blink_count);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int led_cancel([MarshalAs(UnmanagedType.LPStr)]string id);

        #endregion

        private const int LED_INFO = 1;

        #region BPS

        /// <summary>
        /// Get the unique domain ID for the LED service.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int Domain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return led_get_domain();
            }
        }

        /// <summary>
        /// Start receiving LED status change events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestEvents()
        {
            Util.GetBPSOrException();
            return led_request_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving LED status change events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopEvents()
        {
            Util.GetBPSOrException();
            return led_stop_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get the status of the LEDs.
        /// </summary>
        /// <param name="ev">A BPSEvent to convert to LEDColor.</param>
        /// <returns>The LEDColor for the event.</returns>
        [AvailableSince(10, 0)]
        public static LEDColor GetLEDColor(BPSEvent ev)
        {
            if (ev.Domain != LED.Domain)
            {
                throw new ArgumentException("BPSEvent is not a LED event");
            }
            if (ev.Code != LED_INFO)
            {
                throw new ArgumentException("BPSEvent is an unknown LED event");
            }
            bool red, green, blue;
            if (led_event_get_rgb(ev.DangerousGetHandle(), out red, out green, out blue) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return (LEDColor)(((red ? 0xFF : 0x00) << 16) | ((green ? 0xFF : 0x00) << 8) | (blue ? 0xFF : 0x00));
        }

        #endregion

        /// <summary>
        /// Request that the LEDs flash a named color.
        /// </summary>
        /// <param name="color">The color to flash the LEDs.</param>
        /// <param name="blinkCount">The number of times to blink.  Use a value of 0 to continue blinking until canceled or until the application exits.</param>
        /// <returns>An LED instance.</returns>
        [AvailableSince(10, 0)]
        public static LEDInstance Flash(LEDColor color, int blinkCount = 0)
        {
            var instance = new LEDInstance(Guid.NewGuid());
            if (!instance.Update(color, blinkCount))
            {
                Util.ThrowExceptionForLastErrno();
            }
            return instance;
        }
    }
}
