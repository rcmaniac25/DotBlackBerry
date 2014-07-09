using System;
using System.Runtime.InteropServices;

using Mono.Unix.Native;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Allow Screen namespace to interact with BPS.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class ScreenEvent : BPSEvent
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screen_request_events(IntPtr context);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screen_stop_events(IntPtr context);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screen_get_domain();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr screen_event_get_context(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr screen_event_get_event(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern Errno screen_event_get_errno(IntPtr ev);

        #endregion

        internal ScreenEvent(IntPtr ev, bool isFailure)
            : base(ev, false)
        {
            if (isFailure)
            {
                ScreenError = screen_event_get_errno(ev);
            }
            Event = new BlackBerry.Screen.ScreenEvent(screen_event_get_event(ev));
            Context = null;
            if (Util.IsCapableOfRunning(10, 2))
            {
                Context = new BlackBerry.Screen.ScreenContext(screen_event_get_context(ev));
            }
        }

        private const int BPS_SCREEN_EVENT = 1;
        private const int BPS_SCREEN_FAILURE = 2;

        /// <summary>
        /// Get the errno from screen failure.
        /// </summary>
        [AvailableSince(10, 2)]
        public Nullable<Errno> ScreenError { [AvailableSince(10, 2)]get; private set; }

        /// <summary>
        /// Get the screen and window event that has occured.
        /// </summary>
        [AvailableSince(10, 0)]
        public BlackBerry.Screen.ScreenEvent Event { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get the screen context that generated this event.
        /// </summary>
        [AvailableSince(10, 2)]
        public BlackBerry.Screen.ScreenContext Context { [AvailableSince(10, 2)]get; private set; }

        /// <summary>
        /// If a screen error exists, throw an exception for it.
        /// </summary>
        [AvailableSince(10, 2)]
        public void ThrowScreenError()
        {
            if (ScreenError.HasValue)
            {
                Util.ThrowExceptionForErrno(ScreenError.Value, false);
            }
        }

        #region BPS

        /// <summary>
        /// Get the unique domain ID for the screen.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int BPSDomain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return screen_get_domain();
            }
        }

        /// <summary>
        /// Start receiving libscreen events.
        /// </summary>
        /// <param name="context">The Screen context to use for event retrieval.</param>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestEvents(BlackBerry.Screen.ScreenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            Util.GetBPSOrException();
            return screen_request_events(context.Handle) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving libscreen events.
        /// </summary>
        /// <param name="context">The Screen context passed into <see cref="RequestEvents"/>.</param>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopEvents(BlackBerry.Screen.ScreenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            Util.GetBPSOrException();
            return screen_stop_events(context.Handle) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get a screen event from a BPS Event.
        /// </summary>
        /// <param name="ev">A BPSEvent to convert to a ScreenEvent.</param>
        /// <returns>The ScreenEvent for the event.</returns>
        [AvailableSince(10, 0)]
        public static ScreenEvent GetScreenEvent(BPSEvent ev)
        {
            if (ev.Domain != ScreenEvent.BPSDomain)
            {
                throw new ArgumentException("BPSEvent is not a screen event");
            }
            var code = ev.Code;
            if (code != BPS_SCREEN_EVENT && code != BPS_SCREEN_FAILURE)
            {
                throw new ArgumentException("BPSEvent is an unknown screen event");
            }
            return new ScreenEvent(ev.DangerousGetHandle(), code == BPS_SCREEN_FAILURE);
        }

        #endregion
    }
}
