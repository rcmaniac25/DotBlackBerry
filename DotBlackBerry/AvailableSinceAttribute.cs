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
        /// <param name="revision">The revision version the member has been available.</param>
        public AvailableSinceAttribute(int major, int minor, int build = 0, int revision = 0)
        {
            AvailableSince = new Version(major, minor, build, revision);
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
                    using (var deviceInfo = new DeviceInfo())
                    {
                        var since = (att as AvailableSinceAttribute).AvailableSince;
                        return deviceInfo.OSVersion.Version >= since ? FunctionAvailability.Avaliable : FunctionAvailability.NotSupported;
                    }
                }
            }
            return FunctionAvailability.Unknown;
        }
    }
}
