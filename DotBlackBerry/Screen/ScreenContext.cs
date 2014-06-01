using System;
using System.Runtime.InteropServices;

namespace BlackBerry.Screen
{
    /// <summary>
    /// A screen context used to identify the scope of the relationship with the underlying windowing system.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class ScreenContext : IDisposable
    {
        internal ScreenContext(IntPtr ptr)
        {
            //TODO
        }

        //TODO

        internal IntPtr Handle
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Dispose screen context.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
