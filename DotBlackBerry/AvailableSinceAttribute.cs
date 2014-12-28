using System;

using BlackBerry.BPS;

namespace BlackBerry
{
    /// <summary>
    /// The state of a function's availability.
    /// </summary>
    public enum FunctionAvailability
    {
        /// <summary>
        /// Member's availability is unknown. It may or may not be supported.
        /// </summary>
        Unknown,

        /// <summary>
        /// Member is available for use.
        /// </summary>
        Avaliable,
        /// <summary>
        /// Member is not available for use.
        /// </summary>
        NotSupported
    }

    /// <summary>
    /// Attribute to define when a function has been available.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class AvailableSinceAttribute : Attribute
    {
        #region Static version tests

#if BLACKBERRY_STATIC_VERSION_TESTS

        /// <summary>
        /// BlackBerry 10 OS is at least 10.0
        /// </summary>
        public static bool IsAtLeastOS_10_0 { get; private set; }
        /// <summary>
        /// BlackBerry 10 OS is at least 10.1
        /// </summary>
        public static bool IsAtLeastOS_10_1 { get; private set; }
        /// <summary>
        /// BlackBerry 10 OS is at least 10.2
        /// </summary>
        public static bool IsAtLeastOS_10_2 { get; private set; }
        /// <summary>
        /// BlackBerry 10 OS is at least 10.2.1
        /// </summary>
        public static bool IsAtLeastOS_10_2_1 { get; private set; }
        /// <summary>
        /// BlackBerry 10 OS is at least 10.3
        /// </summary>
        public static bool IsAtLeastOS_10_3 { get; private set; }
        /// <summary>
        /// BlackBerry 10 OS is at least 10.3.1
        /// </summary>
        public static bool IsAtLeastOS_10_3_1 { get; private set; }

        static AvailableSinceAttribute()
        {
            var version = BlackBerry.BPS.DeviceInfo.OSVersion.Version;
            if (version.Major > 10)
            {
                IsAtLeastOS_10_0 = true;
                IsAtLeastOS_10_1 = true;
                IsAtLeastOS_10_2 = true;
                IsAtLeastOS_10_2_1 = true;
                IsAtLeastOS_10_3 = true;
                IsAtLeastOS_10_3_1 = true;
            }
            else
            {
                // Eek, this is ugly...
                if (version.Minor >= 3)
                {
                    if (version.Minor > 3 || version.Build >= 1)
                    {
                        IsAtLeastOS_10_3_1 = true;
                    }
                    IsAtLeastOS_10_3 = true;
                }
                if (version.Minor >= 2)
                {
                    if (version.Minor > 2 || version.Build >= 1)
                    {
                        IsAtLeastOS_10_2_1 = true;
                    }
                    IsAtLeastOS_10_2 = true;
                }
                if (version.Minor >= 1)
                {
                    IsAtLeastOS_10_1 = true;
                }
                IsAtLeastOS_10_0 = true;
            }
        }

#endif

        #endregion

        /// <summary>
        /// Get when a member has been supported since.
        /// </summary>
        public Version AvailableSince { get; private set; }

        /// <summary>
        /// Create a new AvailableSinceAttribute for a member.
        /// </summary>
        /// <param name="major">The major version the member has been available.</param>
        /// <param name="minor">The minor version the member has been available.</param>
        /// <param name="build">The build version the member has been available.</param>
        public AvailableSinceAttribute(int major, int minor, int build = 0)
        {
            AvailableSince = new Version(major, minor, build);
        }

        /// <summary>
        /// Test is a member is available.
        /// </summary>
        /// <param name="member">The member to test for availability.</param>
        /// <returns>The availability of the member.</returns>
        public static FunctionAvailability GetAvailability(System.Reflection.MemberInfo member)
        {
            foreach (var att in member.GetCustomAttributes(false))
            {
                if (att is AvailableSinceAttribute)
                {
                    return Util.IsCapableOfRunning((att as AvailableSinceAttribute).AvailableSince) ? FunctionAvailability.Avaliable : FunctionAvailability.NotSupported;
                }
            }
            return FunctionAvailability.Unknown;
        }

        internal static Version GetRequiredVersion(System.Reflection.MemberInfo member)
        {
            foreach (var att in member.GetCustomAttributes(false))
            {
                if (att is AvailableSinceAttribute)
                {
                    return (att as AvailableSinceAttribute).AvailableSince;
                }
            }
            return new Version(); // 0.0
        }
    }
}
