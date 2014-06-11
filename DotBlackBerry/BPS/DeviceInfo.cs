using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// The presence of a physical keyboard on the device.
    /// </summary>
    [AvailableSince(10, 1)]
    public enum PhysicalKeyboardExistence : int
    {
        /// <summary>
        /// It is unknown if the device has a physical keyboard or not.
        /// </summary>
        [AvailableSince(10, 1)]
        Unknown = -1,
        /// <summary>
        /// A physical keyboard is not present on the device.
        /// </summary>
        [AvailableSince(10, 1)]
        NotPresent = 0,
        /// <summary>
        /// A physical keyboard is present on the device.
        /// </summary>
        [AvailableSince(10, 1)]
        Present = 1
    }

    /// <summary>
    /// The type of HDMI connector on the device.
    /// </summary>
    [AvailableSince(10, 2)]
    public enum HDMIConnector : int
    {
        /// <summary>
        /// Is is unknown what HDMI adapter exists, if any.
        /// </summary>
        [AvailableSince(10, 2)]
        Unknown = -1,
        /// <summary>
        /// An HDMI connector is not present on the device.
        /// </summary>
        [AvailableSince(10, 2)]
        None = 0,
        /// <summary>
        /// A Micro HDMI connector is present on the device.
        /// </summary>
        [AvailableSince(10, 2)]
        MicroHDMI = 1
    }

    /// <summary>
    /// Device information.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class DeviceInfo : IDisposable
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int deviceinfo_get_details(out IntPtr details);

        [DllImport("bps")]
        private static extern void deviceinfo_free_details(ref IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_details_get_model_name(IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_details_get_model_number(IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_details_get_device_os(IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_details_get_device_os_version(IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_details_get_hardware_id(IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_details_get_device_name(IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_details_get_processor_name(IntPtr details);

        [DllImport("bps")]
        private static extern int deviceinfo_details_get_processor_core_count(IntPtr details);

        [DllImport("bps")]
        private static extern IntPtr deviceinfo_details_get_processor_core_name(IntPtr details, int index);

        [DllImport("bps")]
        private static extern int deviceinfo_details_get_processor_core_speed(IntPtr details, int index);

        [DllImport("bps")]
        private static extern bool deviceinfo_details_is_simulator(IntPtr details);

        [DllImport("bps")]
        private static extern int deviceinfo_details_get_keyboard(IntPtr details);

        [DllImport("bps")]
        private static extern int deviceinfo_details_get_hdmi_connector(IntPtr details);

        #endregion

        private static OperatingSystem osVer = null;

        private IntPtr handle;

        /// <summary>
        /// Create a new instance of DeviceInfo.
        /// </summary>
        [AvailableSince(10, 0)]
        public DeviceInfo()
        {
            Util.GetBPSOrException();
            handle = IntPtr.Zero;
            if (deviceinfo_get_details(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        /// <summary>
        /// Finalize DeviceInfo instance.
        /// </summary>
        ~DeviceInfo()
        {
            Dispose(false);
        }

        #region Properties

        // Public due to how often this gets used by .BlackBerry and potentially by programs determining what they can use.

        /// <summary>
        /// Current OS version.
        /// </summary>
        [AvailableSince(10, 0)]
        public static OperatingSystem OSVersion
        {
            [AvailableSince(10, 0)]
            get
            {
                if (osVer == null)
                {
                    using (var info = new DeviceInfo())
                    {
                        osVer = new OperatingSystem(PlatformID.Unix, Version.Parse(Marshal.PtrToStringAnsi(deviceinfo_details_get_device_os_version(info.handle))));
                    }
                }
                return osVer;
            }
        }

        /// <summary>
        /// Retrieve the hardware ID.
        /// </summary>
        [AvailableSince(10, 0)]
        public string HardwareID
        {
            [AvailableSince(10, 0)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("DeviceInfo");
                }
                Util.GetBPSOrException();
                return Marshal.PtrToStringAnsi(deviceinfo_details_get_hardware_id(handle));
            }
        }

        /// <summary>
        /// Retrieve the device name.
        /// </summary>
        [AvailableSince(10, 2)]
        public string DeviceName
        {
            [AvailableSince(10, 2)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("DeviceInfo");
                }
                Util.GetBPSOrException();
                return Marshal.PtrToStringAnsi(deviceinfo_details_get_device_name(handle));
            }
        }

        /// <summary>
        /// Retrieve the model name.
        /// </summary>
        [AvailableSince(10, 2)]
        public string ModelName
        {
            [AvailableSince(10, 2)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("DeviceInfo");
                }
                Util.GetBPSOrException();
                return Marshal.PtrToStringAnsi(deviceinfo_details_get_model_name(handle));
            }
        }

        /// <summary>
        /// Retrieve the model number.
        /// </summary>
        [AvailableSince(10, 2)]
        public string ModelNumber
        {
            [AvailableSince(10, 2)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("DeviceInfo");
                }
                Util.GetBPSOrException();
                return Marshal.PtrToStringAnsi(deviceinfo_details_get_model_number(handle));
            }
        }

        /// <summary>
        /// Retrieve the device OS.
        /// </summary>
        [AvailableSince(10, 0)]
        public string DeviceOS
        {
            [AvailableSince(10, 0)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("DeviceInfo");
                }
                Util.GetBPSOrException();
                return Marshal.PtrToStringAnsi(deviceinfo_details_get_device_os(handle));
            }
        }

        /// <summary>
        /// Retrieve the processor name.
        /// </summary>
        [AvailableSince(10, 2)]
        public string ProcessorName
        {
            [AvailableSince(10, 2)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("DeviceInfo");
                }
                Util.GetBPSOrException();
                return Marshal.PtrToStringAnsi(deviceinfo_details_get_processor_name(handle));
            }
        }

        /// <summary>
        /// Retrieve the number of processor cores.
        /// </summary>
        [AvailableSince(10, 2)]
        public int ProcessorCoreCount
        {
            [AvailableSince(10, 2)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("DeviceInfo");
                }
                Util.GetBPSOrException();
                return deviceinfo_details_get_processor_core_count(handle);
            }
        }

        /// <summary>
        /// Indicate whether the device is a simulator.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool IsSimulator
        {
            [AvailableSince(10, 0)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("DeviceInfo");
                }
                Util.GetBPSOrException();
                return deviceinfo_details_is_simulator(handle);
            }
        }

        /// <summary>
        /// Indicate whether the device has a physical keyboard.
        /// </summary>
        [AvailableSince(10, 1)]
        public PhysicalKeyboardExistence PhysicalKeyboard
        {
            [AvailableSince(10, 1)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("DeviceInfo");
                }
                Util.GetBPSOrException();
                return (PhysicalKeyboardExistence)deviceinfo_details_get_keyboard(handle);
            }
        }

        /// <summary>
        /// Retrieve the type of HDMI connector on the device.
        /// </summary>
        [AvailableSince(10, 2)]
        public HDMIConnector HDMIConnector
        {
            [AvailableSince(10, 2)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("DeviceInfo");
                }
                Util.GetBPSOrException();
                return (HDMIConnector)deviceinfo_details_get_hdmi_connector(handle);
            }
        }

        #endregion

        /// <summary>
        /// Dispose DeviceInfo.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("DeviceInfo");
            }
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (handle != IntPtr.Zero)
            {
                deviceinfo_free_details(ref handle);
                handle = IntPtr.Zero;
            }
        }

        #region Functions

        /// <summary>
        /// Retrieve the name of a processor core.
        /// </summary>
        /// <param name="index">The index of the processor core to get the name of.</param>
        /// <returns>The processor core name.</returns>
        [AvailableSince(10, 2)]
        public string GetProcessorCoreName(int index)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("DeviceInfo");
            }
            Util.GetBPSOrException();
            if (index < 0 || index > deviceinfo_details_get_processor_core_count(handle))
            {
                throw new ArgumentOutOfRangeException("index", "0 <= index < ProcessorCoreCount");
            }
            return Marshal.PtrToStringAnsi(deviceinfo_details_get_processor_core_name(handle, index));
        }

        /// <summary>
        /// Retrieve the speed of a processor core.
        /// </summary>
        /// <param name="index">The index of the processor core to get the speed of.</param>
        /// <returns>The processor core speed in MHz.</returns>
        [AvailableSince(10, 2)]
        public int GetProcessorCoreSpeed(int index)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("DeviceInfo");
            }
            Util.GetBPSOrException();
            if (index < 0 || index > deviceinfo_details_get_processor_core_count(handle))
            {
                throw new ArgumentOutOfRangeException("index", "0 <= index < ProcessorCoreCount");
            }
            var res = deviceinfo_details_get_processor_core_speed(handle, index);
            if (res == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Processor core speed could not be retrieved.");
            }
            return res;
        }

        #endregion
    }
}
