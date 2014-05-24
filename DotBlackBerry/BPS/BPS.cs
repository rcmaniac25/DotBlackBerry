using System;
using System.Runtime.InteropServices;

namespace blackberry.BPS
{
    /// <summary>
    /// BlackBerry Platform Services
    /// </summary>
    public class BPS
    {
        #region PInvoke

        [DllImport("bps")]
        static extern int bps_get_version();

        #endregion

        /// <summary>
        /// BPS Version
        /// </summary>
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

        //TODO
    }
}
