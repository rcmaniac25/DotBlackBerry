using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// BPS-based events.
    /// </summary>
    [AvailableSince(10, 0)]
    public class BPSEvent : IDisposable
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern void bps_event_destroy(IntPtr ev);

        [DllImport("bps")]
        private static extern int bps_event_get_domain(IntPtr ev);

        [DllImport("bps")]
        private static extern uint bps_event_get_code(IntPtr ev);

        #endregion

        private IntPtr handle;

        internal BPSEvent(IntPtr hwnd, bool disposable = false)
        {
            handle = hwnd;
            IsDisposable = disposable;
        }

        /// <summary>
        /// Get if the event is disposable.
        /// </summary>
        public bool IsDisposable { get; private set; }

        /// <summary>
        /// Get the code of an event.
        /// </summary>
        [AvailableSince(10, 0)]
        public uint Code
        {
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("BPSEvent");
                }
                return bps_event_get_code(handle);
            }
        }

        /// <summary>
        /// Get the domain of an event.
        /// </summary>
        [AvailableSince(10, 0)]
        public int Domain
        {
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("BPSEvent");
                }
                return bps_event_get_domain(handle);
            }
        }

        /// <summary>
        /// Get the internal handle of the BPS event.
        /// </summary>
        /// <returns>The internal handle of the BPS event.</returns>
        public IntPtr DangerousGetHandle()
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("BPSEvent");
            }
            return handle;
        }

        /// <summary>
        /// Dispose of the BPS event.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (!IsDisposable)
            {
                throw new InvalidOperationException("BPSEvent cannot be disposed directly");
            }
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("BPSEvent");
            }
            bps_event_destroy(handle);
            handle = IntPtr.Zero;
        }
    }
}
