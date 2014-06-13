using System;

using Mono.Unix.Native;

namespace BlackBerry.Camera
{
    /// <summary>
    /// Error codes for the Camera library.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum CameraError : int
    {
        /// <summary>
        /// The function call to the camera completed successfully.
        /// </summary>
        [AvailableSince(10, 0)]
        EOK = 0,
        /// <summary>
        /// The function call failed because the specified camera was not available. Try to call the function again.
        /// </summary>
        [AvailableSince(10, 0)]
        EAGAIN = (int)Errno.EAGAIN,
        /// <summary>
        /// The function call failed because of an invalid argument.
        /// </summary>
        [AvailableSince(10, 0)]
        EINVAL = (int)Errno.EINVAL,
        /// <summary>
        /// The function call failed because the specified camera was not found.
        /// </summary>
        [AvailableSince(10, 0)]
        ENODEV = (int)Errno.ENODEV,
        /// <summary>
        /// The function call failed because of a file table overflow.
        /// </summary>
        [AvailableSince(10, 0)]
        EMFILE = (int)Errno.EMFILE,
        /// <summary>
        /// The function call failed because an invalid handle to a camera handle value was used.
        /// </summary>
        [AvailableSince(10, 0)]
        EBADF = (int)Errno.EBADF,
        /// <summary>
        /// The function call failed because the necessary permissions to access the camera are not available.
        /// </summary>
        [AvailableSince(10, 0)]
        EACCES = (int)Errno.EACCES,
        /// <summary>
        /// The function call failed because an invalid file descriptor was used.
        /// </summary>
        [AvailableSince(10, 0)]
        EBADR = (int)Errno.EBADR,
        /// <summary>
        /// The function call failed because the requested data does not exist.
        /// </summary>
        [AvailableSince(10, 0, 9)]
        ENODATA = (int)Errno.ENODATA,
        /// <summary>
        /// The function call failed because the specified file or directory does not exist.
        /// </summary>
        [AvailableSince(10, 0)]
        ENOENT = (int)Errno.ENOENT,
        /// <summary>
        /// The function call failed because memory allocation failed.
        /// </summary>
        [AvailableSince(10, 0)]
        ENOMEM = (int)Errno.ENOMEM,
        /// <summary>
        /// The function call failed because the requested operation is not supported.
        /// </summary>
        [AvailableSince(10, 0)]
        EOPNOTSUPP = (int)Errno.EOPNOTSUPP,
        /// <summary>
        /// The function call failed due to communication problem or time-out with the camera.
        /// </summary>
        [AvailableSince(10, 0)]
        ETIMEDOUT = (int)Errno.ETIMEDOUT,
        /// <summary>
        /// The function call failed because an operation on the camera is already in progress. In addition, this error can indicate 
        /// that a call could not be completed because it was invalid or completed already.
        /// </summary>
        [AvailableSince(10, 0)]
        EALREADY = CameraException.EALREADY_NEW,
        /// <summary>
        /// The function call failed because the camera is busy. Typically you receive this error when you try to open a camera while 
        /// the camera or its required resources are in use.
        /// </summary>
        [AvailableSince(10, 0, 9)]
        EBUSY = (int)Errno.EBUSY,
        /// <summary>
        /// The function call failed because the disk is full. This typically happens when you are trying to start a video recording and 
        /// less than the system-reserved amount of disk space remains.
        /// </summary>
        [AvailableSince(10, 0, 9)]
        ENOSPC = (int)Errno.ENOSPC,
        /// <summary>
        /// The function call failed because the Camera library has not been initialized.
        /// </summary>
        [AvailableSince(10, 0)]
        EUNINIT = 0x1000,
        /// <summary>
        /// The function call failed because the registration of a callback failed.
        /// </summary>
        [AvailableSince(10, 0)]
        EREGFAULT,
        /// <summary>
        /// The function call failed because the microphone is already in use.
        /// </summary>
        [AvailableSince(10, 0)]
        EMICINUSE,
        /// <summary>
        /// The function call failed because the operation cannot be completed while the desktop camera is in use.
        /// </summary>
        [AvailableSince(10, 0, 9)]
        EDESKTOPCAMERAINUSE,
        /// <summary>
        /// The function call failed because the camera is in the power down state.
        /// </summary>
        [AvailableSince(10, 1)]
        EPOWERDOWN,
        /// <summary>
        /// The function call failed because the 3A have been locked.
        /// </summary>
        [AvailableSince(10, 2)]
        THREE_ALOCKED,
        /// <summary>
        /// The function call failed because the freeze flag was set on the device.
        /// </summary>
        [AvailableSince(10, 3)]
        EVIEWFINDERFROZEN,
        /// <summary>
        /// The function call failed due to an internal overflow.
        /// </summary>
        [AvailableSince(10, 3)]
        EOVERFLOW,
        /// <summary>
        /// The function call failed because the camera is in power down state to prevent damage due to excessive heat.
        /// </summary>
        [AvailableSince(10, 3)]
        ETHERMALSHUTDOWN
    }

    /// <summary>
    /// Camera generated exception.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class CameraException : Exception
    {
        internal const int EALREADY_NEW = 237;

        /// <summary>
        /// Create a new camera exception.
        /// </summary>
        /// <param name="error">The error code that occured.</param>
        [AvailableSince(10, 0)]
        public CameraException(CameraError error)
            : this(error, (Exception)null)
        {
        }

        /// <summary>
        /// Create a new camera exception.
        /// </summary>
        /// <param name="error">The error code that occured.</param>
        /// <param name="message">Error message.</param>
        [AvailableSince(10, 0)]
        public CameraException(CameraError error, string message)
            : this(error, message, null)
        {
        }

        /// <summary>
        /// Create a new camera exception.
        /// </summary>
        /// <param name="error">The error code that occured.</param>
        /// <param name="innerException">The inner exception that caused the problem.</param>
        [AvailableSince(10, 0)]
        public CameraException(CameraError error, Exception innerException)
            : this(error, ErrorCodeString(error), innerException)
        {
        }

        /// <summary>
        /// Create a new camera exception.
        /// </summary>
        /// <param name="error">The error code that occured.</param>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">The inner exception that caused the problem.</param>
        [AvailableSince(10, 0)]
        public CameraException(CameraError error, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = error;
        }

        /// <summary>
        /// Get the camera error code for this exception.
        /// </summary>
        [AvailableSince(10, 0)]
        public CameraError ErrorCode { [AvailableSince(10, 0)]get; private set; }

        #region CameraError Helpers

        /// <summary>
        /// Convert from a platform specific error code to CameraError.
        /// </summary>
        /// <param name="errorCode">Platform specific error code.</param>
        /// <returns>CameraError</returns>
        [AvailableSince(10, 0)]
        public static CameraError ErrorCodeToCameraError(int errorCode)
        {
            if (Util.IsCapableOfRunning(10, 1))
            {
                if (errorCode == NativeConvert.FromErrno(Errno.EALREADY))
                {
                    errorCode = EALREADY_NEW;
                }
            }
            return (CameraError)errorCode;
        }

        /// <summary>
        /// Convert from CameraError to platform specific error code.
        /// </summary>
        /// <param name="error">CameraError</param>
        /// <returns>Platform specific error code.</returns>
        [AvailableSince(10, 0)]
        public static int CameraErrorToErrorCode(CameraError error)
        {
            if (!Util.IsCapableOfRunning(10, 1) && error == CameraError.EALREADY)
            {
                return NativeConvert.FromErrno(Errno.EALREADY);
            }
            return (int)error;
        }

        #region CameraError string

        private static string ErrorCodeString(CameraError error)
        {
            switch (error)
            {
                case CameraError.EOK:
                    return "The function call to the camera completed successfully.";
                case CameraError.EAGAIN:
                    return "The function call failed because the specified camera was not available. Try to call the function again.";
                case CameraError.EINVAL:
                    return "The function call failed because of an invalid argument.";
                case CameraError.ENODEV:
                    return "The function call failed because the specified camera was not found.";
                case CameraError.EMFILE:
                    return "The function call failed because of a file table overflow.";
                case CameraError.EBADF:
                    return "The function call failed because an invalid handle to a camera handle value was used.";
                case CameraError.EACCES:
                    return "The function call failed because the necessary permissions to access the camera are not available.";
                case CameraError.EBADR:
                    return "The function call failed because an invalid file descriptor was used.";
                case CameraError.ENODATA:
                    return "The function call failed because the requested data does not exist.";
                case CameraError.ENOENT:
                    return "The function call failed because the specified file or directory does not exist.";
                case CameraError.ENOMEM:
                    return "The function call failed because memory allocation failed.";
                case CameraError.EOPNOTSUPP:
                    return "The function call failed because the requested operation is not supported.";
                case CameraError.ETIMEDOUT:
                    return "The function call failed due to communication problem or time-out with the camera.";
                case CameraError.EALREADY:
                    return "The function call failed because an operation on the camera is already in progress. In addition, this error can indicate that a call could not be completed because it was invalid or completed already.";
                case CameraError.EBUSY:
                    return "The function call failed because the camera is busy. Typically you receive this error when you try to open a camera while the camera or its required resources are in use.";
                case CameraError.ENOSPC:
                    return "The function call failed because the disk is full. This typically happens when you are trying to start a video recording and less than the system-reserved amount of disk space remains.";
                case CameraError.EUNINIT:
                    return "The function call failed because the Camera library has not been initialized.";
                case CameraError.EREGFAULT:
                    return "The function call failed because the registration of a callback failed.";
                case CameraError.EMICINUSE:
                    return "The function call failed because the microphone is already in use.";
                case CameraError.EDESKTOPCAMERAINUSE:
                    return "The function call failed because the operation cannot be completed while the desktop camera is in use.";
                case CameraError.EPOWERDOWN:
                    return "The function call failed because the camera is in the power down state.";
                case CameraError.THREE_ALOCKED:
                    return "The function call failed because the 3A have been locked.";
                case CameraError.EVIEWFINDERFROZEN:
                    return "The function call failed because the freeze flag was set on the device.";
                case CameraError.EOVERFLOW:
                    return "The function call failed due to an internal overflow.";
                case CameraError.ETHERMALSHUTDOWN:
                    return "The function call failed because the camera is in power down state to prevent damage due to excessive heat.";
            }
            return string.Format("Unknown error: {0}", error);
        }

        #endregion

        #endregion
    }
}
