using System;
//using System.Runtime.InteropServices;
using Mono.Unix.Native;

namespace BlackBerry
{
    /// <summary>
    /// Simple utility class for common operations
    /// </summary>
    internal class Util
    {
        /// <summary>
        /// Get the current errno and throw an exception for it
        /// </summary>
        public static void ThrowExceptionForErrno()
        {
            var errno = Stdlib.GetLastError();
            if (NativeConvert.FromErrno(errno) == 0)
            {
                return;
            }
            //TODO
            throw new Exception(string.Format("An exception has occured with the error code: {0:X}", errno));
        }
    }
}
