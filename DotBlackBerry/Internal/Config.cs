using System;

namespace BlackBerry.Internal
{
    /// <summary>
    /// Configuration values for .BlackBerry
    /// </summary>
    [Flags]
    public enum ConfigSettings
    {
        /// <summary>
        /// Default build settings. Based on common BlackBerry development practices.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Some internal functions are implemented, but are hidden from the docs. This enables those functions.
        /// </summary>
        /// Note: if determined that those functions are used, it is possible for your app to be denied release.
        InternalsPublic = 1 << 0,
        /// <summary>
        /// By default, when pointers are produced for .Net objects, they are not readable by native code. This makes them all readable, but disables GC for the most part. This must be disposed when done with.
        /// </summary>
        ObjectPointersPinnedByDefault = 1 << 1,
        /// <summary>
        /// By default, a simple reference to the .Net object is created. This allocates memory and serializes the object into it. This must be disposed when done with.
        /// </summary>
        ObjectPointersAreSerialized = 1 << 2,
        /// <summary>
        /// BPS GetEvent returns a BPSEvent on success, and null on timeout. This changes it so it returns a BPSEvent on success, and throws a TimeoutException on timeout.
        /// </summary>
        BPSThrowsTimeout = 1 << 3
    }

    /// <summary>
    /// Configuration information for .BlackBerry
    /// </summary>
    public static class Config
    {
        private static ConfigSettings settings =
            ConfigSettings.Default
#if BLACKBERRY_INTERNAL_FUNCTIONS
            | ConfigSettings.InternalsPublic
#endif
#if BLACKBERRY_USE_SERIALIZATION
            | ConfigSettings.ObjectPointersAreSerialized
#elif BLACKBERRY_PIN_OBJ_POINTERS
            | ConfigSettings.ObjectPointersPinnedByDefault
#endif
#if BLACKBERRY_BPS_EVENT_THROW_TIMEOUT
            | ConfigSettings.BPSThrowsTimeout
#endif
            ;

        /// <summary>
        /// Get the build settings for .BlackBerry
        /// </summary>
        public static ConfigSettings BuildSettings
        {
            get
            {
                return settings;
            }
        }
    }
}
