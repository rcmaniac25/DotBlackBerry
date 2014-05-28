using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Device identifying information.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class DeviceIdentifyingInfo : IDisposable
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int deviceinfo_get_identifying_details(out IntPtr details);

        [DllImport("bps")]
        private static extern void deviceinfo_free_identifying_details(ref IntPtr details);

        [DllImport("bps")]
        private static extern int deviceinfo_identifying_details_get_pin(IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_identifying_details_get_pin_string(IntPtr details);

        [DllImport("bps")]
        private static extern long deviceinfo_identifying_details_get_serial_number(IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_identifying_details_get_serial_number_string(IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_identifying_details_get_imei(IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_identifying_details_get_meid(IntPtr details);

        #endregion

        private IntPtr handle;
        private bool disposed;

        /// <summary>
        /// Create a new instance of DeviceIdentifyingInfo.
        /// </summary>
        [AvailableSince(10, 0)]
        public DeviceIdentifyingInfo()
        {
            Util.GetBPSOrException();
            if (deviceinfo_get_identifying_details(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForErrno();
            }
            disposed = false;
        }

        #region Properties

        /// <summary>
        /// Retrieve the PIN.
        /// </summary>
        [AvailableSince(10, 0)]
        public int PIN
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("DeviceIdentifyingInfo");
                }
                Util.GetBPSOrException();
                return deviceinfo_identifying_details_get_pin(handle);
            }
        }

        /// <summary>
        /// Retrieve the PIN as a string.
        /// </summary>
        [AvailableSince(10, 2)]
        public string PINString
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("DeviceIdentifyingInfo");
                }
                Util.GetBPSOrException();
                return Marshal.PtrToStringAnsi(deviceinfo_identifying_details_get_pin_string(handle));
            }
        }

        /// <summary>
        /// Retrieve the serial number of the device.
        /// </summary>
        [AvailableSince(10, 0)]
        public long SerialNumber
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("DeviceIdentifyingInfo");
                }
                Util.GetBPSOrException();
                return deviceinfo_identifying_details_get_serial_number(handle);
            }
        }

        /// <summary>
        /// Retrieve the serial number of the device as a string.
        /// </summary>
        [AvailableSince(10, 2)]
        public string SerialNumberString
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("DeviceIdentifyingInfo");
                }
                Util.GetBPSOrException();
                return Marshal.PtrToStringAnsi(deviceinfo_identifying_details_get_serial_number_string(handle));
            }
        }

        /// <summary>
        /// Retrieve the IMEI.
        /// </summary>
        [AvailableSince(10, 2)]
        public string IMEI
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("DeviceIdentifyingInfo");
                }
                Util.GetBPSOrException();
                return Marshal.PtrToStringAnsi(deviceinfo_identifying_details_get_imei(handle));
            }
        }

        /// <summary>
        /// Retrieve the MEID.
        /// </summary>
        [AvailableSince(10, 2)]
        public string MEID
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("DeviceIdentifyingInfo");
                }
                Util.GetBPSOrException();
                return Marshal.PtrToStringAnsi(deviceinfo_identifying_details_get_meid(handle));
            }
        }

        #endregion

        /// <summary>
        /// Dispose DeviceIdentifyingInfo.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("DeviceInfo");
            }
            disposed = true;
            deviceinfo_free_identifying_details(ref handle);
        }
    }
}
