using System;
using System.Runtime.InteropServices;

using Mono.Unix.Native;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Defines the supported image formats for screen captures.
    /// </summary>
    [AvailableSince(10, 2)]
    public enum CaptureFormat : int
    {
        /// <summary>
        /// The image format will be determined by the file name extension.
        /// </summary>
        [AvailableSince(10, 2)]
        Filename = 0,
        /// <summary>
        /// The image format is JPEG, regardless of the file name.
        /// </summary>
        [AvailableSince(10, 2)]
        JPG = 1,
        /// <summary>
        /// The image format is PNG, regardless of the file name.
        /// </summary>
        [AvailableSince(10, 2)]
        PNG = 2
    }

    /// <summary>
    /// Functions for taking a snapshot of the display.
    /// </summary>
    [AvailableSince(10, 2), RequiredPermission(Permission.CaptureScreen)]
    public static class ScreenCapture
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screencapture_grab([MarshalAs(UnmanagedType.LPStr)]string filename, CaptureFormat format, out IntPtr result);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screencapture_result_get_error_code(IntPtr result);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr screencapture_result_get_error_message(IntPtr result);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr screencapture_result_get_filename(IntPtr result);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int screencapture_destroy_result(IntPtr result);

        #endregion

        /// <summary>
        /// Take a snapshot of the current display.
        /// </summary>
        /// <param name="filename">If null, the screenshot will be stored in the camera roll and the file name will be automatically generated. Otherwise, <paramref name="filename"/> will be used as the file name.</param>
        /// <param name="format">The format of the image.</param>
        /// <returns>The filename of the saved screenshot.</returns>
        [AvailableSince(10, 2), RequiredPermission(Permission.CaptureScreen)]
        public static string Capture(string filename = null, CaptureFormat format = CaptureFormat.PNG)
        {
            if (format < CaptureFormat.Filename || format > CaptureFormat.PNG)
            {
                throw new ArgumentOutOfRangeException("format", "Not a supported capture format");
            }
            IntPtr result;
            if (screencapture_grab(filename, format, out result) != BPS.BPS_SUCCESS)
            {
                Errno errno = Stdlib.GetLastError();
                if (errno == Errno.ENOMEM)
                {
                    throw new OutOfMemoryException();
                }
                var errCode = BlackBerry.Camera.CameraException.ErrorCodeToCameraError(screencapture_result_get_error_code(result));
                var errMsg = Marshal.PtrToStringAnsi(screencapture_result_get_error_message(result));
                if (screencapture_destroy_result(result) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                throw new BlackBerry.Camera.CameraException(errCode, errMsg, Util.GetExceptionForErrno(errno));
            }
            var file = Marshal.PtrToStringAnsi(screencapture_result_get_filename(result));
            if (screencapture_destroy_result(result) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return file;
        }
    }
}
