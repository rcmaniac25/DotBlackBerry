using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// BlackBerry Platform Services
    /// </summary>
    [AvailableSince(10, 0)]
    public class BPS : IDisposable
    {
        #region PInvoke

        [DllImport("bps")]
        internal static extern int bps_get_version();

        [DllImport("bps")]
        internal static extern int bps_initialize();

        [DllImport("bps")]
        internal static extern void bps_shutdown();

        [DllImport("bps")]
        internal static extern void bps_free(IntPtr ptr);

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

        //TODO

        //XXX should possibly have Equals, HashCode, and ToString. The issue is all BPS instances should be the the same, 
        //but if a BPS instance shouldn't be disposable, then it is not equal and should not be treated as such.
    }
}
