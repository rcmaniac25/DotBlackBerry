using System;

namespace BlackBerry.BPS
{
    /// <summary>
    /// BPS-based events.
    /// </summary>
    [AvailableSince(10, 0)]
    public class BPSEvent : IDisposable
    {
        public bool IsDisposable { get; private set; }

        /// <summary>
        /// Get the internal handle of the BPS event.
        /// </summary>
        /// <returns>The internal handle of the BPS event.</returns>
        public IntPtr DangerousGetHandle()
        {
            //TODO
            return IntPtr.Zero;
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
            throw new NotImplementedException();
        }
    }
}
