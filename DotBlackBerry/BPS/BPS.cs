using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Verbosity levels for BPS logging.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum BPSVerbosity : uint
    {
        /// <summary>
        /// Shutdown and Critical messages are logged.
        /// </summary>
        [AvailableSince(10, 0)]
        Critical = 0,
        /// <summary>
        /// Shutdown, Critical, Error and Warning messages are logged.
        /// </summary>
        [AvailableSince(10, 0)]
        ErrorWarning = 1,
        /// <summary>
        /// Shutdown, Critical, Error, Warning, Notice and Info messages are logged.
        /// </summary>
        [AvailableSince(10, 0)]
        InfoNotice = 2,
        /// <summary>
        /// Shutdown, Critical, Error, Warning, Notice, Info, Debug1 and Debug2 messages are logged.
        /// </summary>
        [AvailableSince(10, 0)]
        Debug = 3
    }

    /// <summary>
    /// BlackBerry Platform Services
    /// </summary>
    [AvailableSince(10, 0)]
    public class BPS : IDisposable
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int bps_get_version();

        [DllImport("bps")]
        internal static extern int bps_initialize();

        [DllImport("bps")]
        internal static extern void bps_shutdown();

        [DllImport("bps")]
        internal static extern void bps_free(IntPtr ptr);

        [DllImport("bps")]
        private static extern int bps_logging_init();

        [DllImport("bps")]
        private static extern void bps_set_verbosity(uint verbosity);

        [DllImport("bps")]
        private static extern int bps_register_domain();

        [DllImport("bps")]
        private static extern int bps_register_shutdown_handler(Action<IntPtr> callback, IntPtr data);

        #endregion

        #region Consts

        internal const int BPS_SUCCESS = 0;
        internal const int BPS_FAILURE = -1;

        #endregion

        private static BPS initBPS = null;
        private static int initBPScount = 0;

        private bool canShutdown;
        private bool disposed;

        /// <summary>
        /// Create an instance of BlackBerry Platform Services.
        /// </summary>
        public BPS()
            : this(true)
        {
        }

        private BPS(bool init)
        {
            if (init)
            {
                // init can be called multiple times
                if (bps_initialize() != BPS_SUCCESS)
                {
                    Util.ThrowExceptionForErrno();
                }
                if ((initBPScount++) == 1)
                {
                    initBPS = this;
                }
            }
            canShutdown = init;
            disposed = false;
        }

        #region Property

        /// <summary>
        /// BPS Version
        /// </summary>
        [AvailableSince(10, 0)]
        public static Version Version
        {
            get
            {
                // The version number is computed as follows:
                // (Major * 1000000) + (Minor * 1000) + Patch
                var encodedVersion = bps_get_version();
                var major = encodedVersion / 1000000;
                var majorEncoded = major * 1000000;
                var minor = (encodedVersion - majorEncoded) / 1000;
                var patch = encodedVersion - (majorEncoded + (minor * 1000));
                return new Version(major, minor, patch);
            }
        }

        /// <summary>
        /// Get the first instance of BPS.
        /// </summary>
        public static BPS AvaliableInstance
        {
            get
            {
                if (initBPS != null)
                {
                    return new BPS(false);
                }
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Shutdown BPS
        /// </summary>
        public void Dispose()
        {
            if (!canShutdown)
            {
                throw new InvalidOperationException("Cannot dispose BPS instance");
            }
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            disposed = true;
            if (initBPScount > 0)
            {
                initBPScount--;
                bps_shutdown();
                if (initBPScount == 0)
                {
                    initBPS = null;
                }
            }
        }

        #region Functions

#if BLACKBERRY_INTERNAL_FUNCTIONS

        /// <summary>
        /// Setup a default slog2 buffer if one is not set.
        /// </summary>
        /// <returns>true if a default slog2 buffer was set up successfully or if one was already setup. false if setting up a default slog2 buffer failed.</returns>
        [AvailableSince(10, 1)]
        public bool InitializeLogging()
        {
            return bps_logging_init() == BPS_SUCCESS;
        }

#endif

        /// <summary>
        /// Set the verbosity of logging for platform services.
        /// </summary>
        /// Default verbosity is BPSVerbosity.InfoNotice.
        /// <param name="verbosity">The desired verbosity</param>
        [AvailableSince(10, 0)]
        public void SetLoggingVerbosity(BPSVerbosity verbosity)
        {
            bps_set_verbosity((uint)verbosity);
        }

        /// <summary>
        /// Reserve a unique domain ID for identifying a service's events.
        /// </summary>
        /// <returns>A domain ID for a service. A value of -1 indicates an error.</returns>
        [AvailableSince(10, 0)]
        public int RegisterDomain()
        {
            return bps_register_domain();
        }

        private static void ShutdownHandler(IntPtr data)
        {
            var parsedData = Util.DeserializeFromPointer(data);
            Util.FreeSerializePointer(data);
            if (parsedData != null)
            {
                var parts = parsedData as object[];
                if (parts != null)
                {
                    var callback = parts[0] as Action<object>;
                    callback(parts[1]);
                }
                else
                {
                    var callback = parsedData as Action<object>;
                    callback(null);
                }
            }
        }

        /// <summary>
        /// Register a callback that will be invoked when the last shutdown function is called.
        /// </summary>
        /// <param name="callback">Callback to invoke on shutdown.</param>
        /// <param name="callbackData">Additional data to pass to callback.</param>
        /// <returns>true is returned if the handler registered successfully with the data, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool RegisterShutdownHandler(Action<object> callback, object callbackData = null)
        {
            var toSerialize = callbackData == null ? (object)callback : new object[] { callback, callbackData };
            var data = Util.SerializeToPointer(toSerialize);
            if (data == IntPtr.Zero)
            {
                return false;
            }
            return bps_register_shutdown_handler(ShutdownHandler, data) == BPS_SUCCESS;
        }

        //TODO: BPSChannel CreateChannel(...) //bps_channel_create

        //TODO: bool GetEvent(..., out BPSEvent ev) //bps_get_event

        //TODO: bool PushEvent(BPSEvent ev) //bps_push_event

        //XXX should possibly have Equals, HashCode, and ToString. The issue is all BPS instances should be the the same, 
        //but if a BPS instance shouldn't be disposable, then it is not equal and should not be treated as such.

        #endregion
    }
}
