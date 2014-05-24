using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Device information.
    /// </summary>
    [AvailableSince(10, 0)]
    public class DeviceInfo : IDisposable
    {
        #region PInvoke

        [DllImport("bps")]
        static extern int deviceinfo_get_details(out IntPtr details);

        [DllImport("bps")]
        static extern void deviceinfo_free_details(ref IntPtr details);

        [DllImport("bps")]
        static extern IntPtr deviceinfo_details_get_device_os_version(IntPtr details);

        #endregion

        private IntPtr handle;
        private bool disposed;

        /// <summary>
        /// Create a new instance of DeviceInfo.
        /// </summary>
        public DeviceInfo()
        {
            if (deviceinfo_get_details(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForErrno();
            }
            disposed = false;
        }

        /// <summary>
        /// Current OS version.
        /// </summary>
        [AvailableSince(10, 0)]
        public OperatingSystem OSVersion
        {
            get
            {
                return new OperatingSystem(PlatformID.Unix, Version.Parse(Marshal.PtrToStringAnsi(deviceinfo_details_get_device_os_version(handle))));
            }
        }

        /// <summary>
        /// Dispose DeviceInfo.
        /// </summary>
        public void Dispose()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("DeviceInfo");
            }
            disposed = true;
            deviceinfo_free_details(ref handle);
        }

        //TODO
    }
}
