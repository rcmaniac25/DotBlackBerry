using System;
using System.IO;
#if BLACKBERRY_USE_SERIALIZATION
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
#else
using System.Runtime.InteropServices;
#endif

using Mono.Unix.Native;

namespace BlackBerry
{
    /// <summary>
    /// Simple utility class for common operations
    /// </summary>
    internal class Util
    {
        #region Versioning

        //While it would be nice to have something like "IsOS10_2", "IsOS_10_2_1", etc., it would result in needing to set them up statically and init/shutdown of BPS and other processes. Just do it at runtime and cache it.

        /// <summary>
        /// Determine if something is able to run with the specified version.
        /// </summary>
        /// <param name="major">The major version that needs to exist.</param>
        /// <param name="minor">The minor version that needs to exist.</param>
        /// <returns>true if it can run, false if otherwise.</returns>
        public static bool IsCapableOfRunning(int major, int minor = 0)
        {
            return IsCapableOfRunning(new Version(major, minor));
        }

        /// <summary>
        /// Determine if something is able to run with the specified version.
        /// </summary>
        /// <param name="requiredVersion">The version that is required.</param>
        /// <returns>true if it can run, false if otherwise.</returns>
        public static bool IsCapableOfRunning(Version requiredVersion)
        {
            return BlackBerry.BPS.DeviceInfo.OSVersion.Version >= requiredVersion;
        }

        /// <summary>
        /// Throw an exception if the OS does not meet the required version for the member to execute.
        /// </summary>
        /// <param name="info">The member that wants to be used.</param>
        /// <param name="message">Any message to state about the exception.</param>
        public static void ThrowIfUnsupported(System.Reflection.MemberInfo info, string message = null)
        {
            ThrowIfUnsupported(AvailableSinceAttribute.GetRequiredVersion(info), message);
        }

        /// <summary>
        /// Throw an exception if the OS does not meet the required version for the member to execute.
        /// </summary>
        /// <param name="major">The major version that needs to exist.</param>
        /// <param name="minor">The minor version that needs to exist.</param>
        /// <param name="message">Any message to state about the exception.</param>
        public static void ThrowIfUnsupported(int major, int minor = 0, string message = null)
        {
            ThrowIfUnsupported(new Version(major, minor), message);
        }

        /// <summary>
        /// Throw an exception if the OS does not meet the required version for execution.
        /// </summary>
        /// <param name="requiredVersion">Required version for execution.</param>
        /// <param name="message">Any message to state about the exception.</param>
        public static void ThrowIfUnsupported(Version requiredVersion, string message = null)
        {
            if (!IsCapableOfRunning(requiredVersion))
            {
                ThrowUnsupported(requiredVersion, message);
            }
        }

        private static void ThrowUnsupported(Version requiredVersion, string message = null)
        {
            var format = string.IsNullOrWhiteSpace(message) ? "Requires OS version {1}" : "{0}, Requires OS version {1}";
            throw new NotSupportedException(string.Format(format, message, requiredVersion));
        }

        #endregion

        #region Errno

        /// <summary>
        /// Get the last errno and throw an exception for it.
        /// </summary>
        public static void ThrowExceptionForLastErrno()
        {
            ThrowExceptionForErrno(Stdlib.GetLastError());
        }

        /// <summary>
        /// Throw an exception for the errno.
        /// </summary>
        /// <param name="errno">The errno to throw an exception for.</param>
        /// <param name="logNoError">If there is no error, log that this was called (meaning a failure occured) but that no error existed.</param>
        public static void ThrowExceptionForErrno(Errno errno, bool logNoError = true)
        {
            if (NativeConvert.FromErrno(errno) == 0)
            {
                if (logNoError)
                {
                    //TODO: should log that no exception occured
                }
                return;
            }
            switch (errno)
            {
                case Errno.ENOMEM:
                    throw new OutOfMemoryException();
                case Errno.EEXIST:
                    throw new IOException("File already exists", NativeConvert.FromErrno(errno));
                case Errno.EMFILE:
                    throw new IOException("Too many files open", NativeConvert.FromErrno(errno));
                case Errno.ENOTTY:
                    throw new IOException("Inappropriate I/O control operation", NativeConvert.FromErrno(errno));
                case Errno.EFBIG:
                    throw new IOException("File too large", NativeConvert.FromErrno(errno));
                case Errno.EPIPE:
                    throw new PipeException("Broken pipe", NativeConvert.FromErrno(errno));
                case ((Errno)47): //ECANCELED
                    throw new OperationCanceledException();
                case ((Errno)48): //ENOTSUP
                    throw new NotSupportedException();
            }
            Mono.Unix.UnixMarshal.ThrowExceptionForError(errno);
        }

        #endregion

        /// <summary>
        /// Get the current instance of BPS, or throw an exception if it isn't avaliable.
        /// </summary>
        /// <returns>Current instance of BPS.</returns>
        public static BlackBerry.BPS.BPS GetBPSOrException()
        {
            var bps = BlackBerry.BPS.BPS.AvaliableInstance;
            if (bps == null)
            {
                throw new InvalidOperationException("BPS has not been started");
            }
            return bps;
        }

        #region Callbacks

        private static void ActionObjHandler(IntPtr ptr, bool free)
        {
            var parsedData = DeserializeFromPointer(ptr);
            if (free)
            {
                FreeSerializePointer(ptr);
            }
            if (parsedData != null)
            {
                var parts = parsedData as object[];
                try
                {
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
                catch
                {
                    //TODO: log error
                }
            }
        }

        /// <summary>
        /// Parse argument data to Action&lt;object&gt; and then free the pointer.
        /// </summary>
        /// <param name="ptr">The data pointer to parse.</param>
        public static void ActionObjFreeHandlerFromAction(IntPtr ptr)
        {
            ActionObjHandler(ptr, true);
        }

        /// <summary>
        /// Parse argument data to Action&lt;object&gt; and then free the pointer.
        /// </summary>
        /// <param name="ptr">The data pointer to parse.</param>
        /// <returns>zero</returns>
        public static int ActionObjFreeHandlerFromFuncZeroReturn(IntPtr ptr)
        {
            ActionObjHandler(ptr, true);
            return 0;
        }

        #endregion

        #region Serialize .Net Objects

        /// <summary>
        /// Serialize an object to a pointer.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="type">What type of handle handle should be created.</param>
        /// <returns>The data, or IntPtr.Zero if obj is null or an error occurs.</returns>
        public static IntPtr SerializeToPointer(object obj, GCHandleType type = 
#if BLACKBERRY_PIN_OBJ_POINTERS
            GCHandleType.Pinned)
#else
            GCHandleType.Normal)
#endif
        {
            if (obj == null)
            {
                return IntPtr.Zero;
            }

#if BLACKBERRY_USE_SERIALIZATION
            // Serialize
            var formatter = new BinaryFormatter();
            var ms = new MemoryStream();
            try
            {
                formatter.Serialize(ms, obj);
            }
            catch
            {
                return IntPtr.Zero;
            }

            // Check data length
            if (ms.Length > Int32.MaxValue)
            {
                return IntPtr.Zero;
            }

            // Copy to pointer
            var data = ms.GetBuffer();
            var ptr = Syscall.malloc((ulong)ms.Length + sizeof(int));
            Marshal.WriteInt32(ptr, (int)ms.Length);
            Marshal.Copy(data, 0, ptr, (int)ms.Length);
            return ptr;
#else
            return GCHandle.ToIntPtr(GCHandle.Alloc(obj, type));
#endif
        }

        /// <summary>
        /// Deserialize an object from a pointer.
        /// </summary>
        /// <param name="ptr">The pointer to deserialize.</param>
        /// <returns>The object or null if the pointer is IntPtr.Zero or an error has occured.</returns>
        public static object DeserializeFromPointer(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }

#if BLACKBERRY_USE_SERIALIZATION
            // Get data
            var dataLength = Marshal.ReadInt32(ptr);
            var data = new byte[dataLength + sizeof(int)];
            Marshal.Copy(ptr, data, 0, dataLength + sizeof(int));
            var ms = new MemoryStream(data, sizeof(int), dataLength, false);

            // Deserialize
            var formatter = new BinaryFormatter();
            try
            {
                return formatter.Deserialize(ms);
            }
            catch
            {
                return null;
            }
#else
            return GCHandle.FromIntPtr(ptr).Target;
#endif
        }

        /// <summary>
        /// Free the serialized pointer (just a wrapper around Syscall.free)
        /// </summary>
        /// <param name="ptr">The pointer to free.</param>
        public static void FreeSerializePointer(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return;
            }
#if BLACKBERRY_USE_SERIALIZATION
            Syscall.free(ptr);
#else
            GCHandle.FromIntPtr(ptr).Free();
#endif
        }

        #endregion
    }
}
