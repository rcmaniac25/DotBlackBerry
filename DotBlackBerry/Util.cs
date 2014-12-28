using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Text;

using Mono.Unix.Native;

namespace BlackBerry
{
    /// <summary>
    /// Utility class for common operations
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
        /// <param name="build">The build version that needs to exist.</param>
        /// <returns>true if it can run, false if otherwise.</returns>
        public static bool IsCapableOfRunning(int major, int minor = 0, int build = 0)
        {
            return IsCapableOfRunning(new Version(major, minor, build));
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
        /// <param name="build">The build version that needs to exist.</param>
        /// <param name="message">Any message to state about the exception.</param>
        public static void ThrowIfUnsupported(int major, int minor = 0, int build = 0, string message = null)
        {
            ThrowIfUnsupported(new Version(major, minor, build), message);
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
        /// Get the exception to throw for errno.
        /// </summary>
        /// <param name="errno">The errno to get an exception for.</param>
        /// <param name="throwForGeneric">Throw exception for general, Mono supported exceptions.</param>
        /// <returns>The exeption to throw, or null if there was no exception.</returns>
        public static Exception GetExceptionForErrno(Errno errno, bool throwForGeneric = false)
        {
            if (NativeConvert.FromErrno(errno) == 0)
            {
                return null;
            }
            switch (errno)
            {
                case Errno.ENOMEM:
                    return new OutOfMemoryException();
                case Errno.EEXIST:
                    return new IOException("File already exists", NativeConvert.FromErrno(errno));
                case Errno.EMFILE:
                    return new IOException("Too many files open", NativeConvert.FromErrno(errno));
                case Errno.ENOTTY:
                    return new IOException("Inappropriate I/O control operation", NativeConvert.FromErrno(errno));
                case Errno.EFBIG:
                    return new IOException("File too large", NativeConvert.FromErrno(errno));
                case Errno.EPIPE:
                    return new PipeException("Broken pipe", NativeConvert.FromErrno(errno));
                case ((Errno)47): //ECANCELED
                    return new OperationCanceledException();
                case ((Errno)48): //ENOTSUP
                    return new NotSupportedException();
            }
            if (throwForGeneric)
            {
                Mono.Unix.UnixMarshal.ThrowExceptionForError(errno);
                return null; //Will never reach here
            }
            else
            {
                // Nasty hack to actually get exception
                try
                {
                    Mono.Unix.UnixMarshal.ThrowExceptionForError(errno);
                    return new InvalidOperationException(Mono.Unix.UnixMarshal.GetErrorDescription(errno)); // Backup if no exception gets thrown.
                }
                catch (Exception e)
                {
                    return e;
                }
            }
        }

        /// <summary>
        /// Get the exception to throw for the last errno.
        /// </summary>
        /// <param name="throwForGeneric">Throw exception for general, Mono supported exceptions.</param>
        /// <returns>The exeption to throw, or null if there was no exception.</returns>
        public static Exception GetExceptionForLastErrno(bool throwForGeneric = false)
        {
            return GetExceptionForErrno(Stdlib.GetLastError(), throwForGeneric);
        }

        /// <summary>
        /// Throw an exception for the errno.
        /// </summary>
        /// <param name="errno">The errno to throw an exception for.</param>
        /// <param name="logNoError">If there is no error, log that this was called (meaning a failure occured) but that no error existed.</param>
        public static void ThrowExceptionForErrno(Errno errno, bool logNoError = true)
        {
            var err = GetExceptionForErrno(errno, true);
            if (err == null)
            {
                if (logNoError)
                {
                    //TODO: should log that no exception occured
                }
                return;
            }
            throw err;
        }

        /// <summary>
        /// Get the last errno and throw an exception for it.
        /// </summary>
        /// <param name="logNoError">If there is no error, log that this was called (meaning a failure occured) but that no error existed.</param>
        public static void ThrowExceptionForLastErrno(bool logNoError = true)
        {
            ThrowExceptionForErrno(Stdlib.GetLastError(), logNoError);
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

        //TODO: replace binary serializer so it will produce raw data... unless GCHandle can do that easily and recursivly (which I don't think it can)

        #region Raw Serialize

        /// <summary>
        /// Serialize an object to a pointer using serialization classes.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="size">The size of the serialized data.</param>
        /// <returns>The data, or IntPtr.Zero if obj is null or an error occurs.</returns>
        public static IntPtr RawSerializeToPointer(object obj, out uint size)
        {
            size = 0;
            if (obj == null)
            {
                return IntPtr.Zero;
            }

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
            size = (uint)ms.Length;
            var data = ms.GetBuffer();
            var ptr = Syscall.malloc((ulong)ms.Length);
            Marshal.Copy(data, 0, ptr, (int)ms.Length);
            return ptr;
        }

        /// <summary>
        /// Deserialize an object from a pointer.
        /// </summary>
        /// <param name="ptr">The pointer to deserialize.</param>
        /// <param name="size">The size of the data to deserialize.</param>
        /// <returns>The object or null if the pointer is IntPtr.Zero or an error has occured.</returns>
        public static object RawDeserializeFromPointer(IntPtr ptr, uint size)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }
            if (size > Int32.MaxValue)
            {
                return null;
            }

            // Get data
            var data = new byte[size];
            Marshal.Copy(ptr, data, 0, (int)size);
            var ms = new MemoryStream(data, false);

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
        }

        /// <summary>
        /// Free the raw serialized pointer.
        /// </summary>
        /// <param name="ptr">The pointer to free.</param>
        /// <returns>true if the pointer was freed, false if otherwise.</returns>
        public static bool FreeRawSerializePointer(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return true;
            }
            Syscall.free(ptr);
            return true;
        }

        #endregion

        #region Normal Serialize

#if !BLACKBERRY_USE_SERIALIZATION
        private static IDictionary<IntPtr, IntPtr> dataPtrToPointer = new ConcurrentDictionary<IntPtr, IntPtr>();
#endif

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
            Marshal.Copy(data, sizeof(int), ptr, (int)ms.Length);
            return ptr;
#else
            var handle = GCHandle.Alloc(obj, type);
            var ptr = GCHandle.ToIntPtr(handle);
            if (type == GCHandleType.Pinned)
            {
                var pinnedPtr = handle.AddrOfPinnedObject();
                dataPtrToPointer.Add(pinnedPtr, ptr);
                return pinnedPtr;
            }
            return ptr;
#endif
        }

        private static IntPtr GetPointerForPotentialPinnedData(IntPtr ptr)
        {
#if BLACKBERRY_USE_SERIALIZATION
            return ptr;
#else
            if (ptr == IntPtr.Zero)
            {
                return ptr;
            }
            // If the data pointer exists, then use it. Otherwise return ptr, as serialized data might not be pinned (and thus wouldn't be in the collection)
            IntPtr pointer;
            if (dataPtrToPointer.TryGetValue(ptr, out pointer))
            {
                return pointer;
            }
            else
            {
                return ptr;
            }
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
            var pointer = GetPointerForPotentialPinnedData(ptr);
            if (pointer == IntPtr.Zero)
            {
                return null;
            }
            return GCHandle.FromIntPtr(pointer).Target;
#endif
        }

        /// <summary>
        /// Free the serialized pointer.
        /// </summary>
        /// <param name="ptr">The pointer to free.</param>
        /// <returns>true if the pointer was freed, false if otherwise.</returns>
        public static bool FreeSerializePointer(IntPtr ptr)
        {
#if BLACKBERRY_USE_SERIALIZATION
            return FreeRawSerializePointer(ptr);
#else
            if (ptr == IntPtr.Zero)
            {
                return true;
            }
            var pointer = GetPointerForPotentialPinnedData(ptr);
            if (pointer == IntPtr.Zero)
            {
                return false;
            }
            GCHandle.FromIntPtr(pointer).Free();
            dataPtrToPointer.Remove(ptr);
            return true;
#endif
        }

        #endregion

        #endregion

        #region General

        /// <summary>
        /// Read a int array.
        /// </summary>
        /// <param name="handle">The handle to the array.</param>
        /// <param name="count">The number of elements in the array.</param>
        /// <returns>The parsed array.</returns>
        public static int[] ParseInt32Array(IntPtr handle, int count)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }
            if (count == 0)
            {
                return new int[0];
            }
            var arr = new int[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = Marshal.ReadInt32(handle, i * sizeof(int));
            }
            return arr;
        }

        /// <summary>
        /// Copies all characters up to the first null character from an unmanaged UTF8 string to a managed String.
        /// </summary>
        /// <param name="handle">The address of the first character of the unmanaged string.</param>
        /// <returns>A managed string that holds a copy of the unmanaged UTF8 string. If ptr is null, the method returns a null string.</returns>
        public static string PtrToStringUTF8(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }
            var len = Mono.Unix.Native.Syscall.strlen(handle);
            if (len == 0)
            {
                return string.Empty;
            }
            var data = new byte[len];
            Marshal.Copy(handle, data, 0, data.Length);
            return Encoding.UTF8.GetString(data);
        }

        #endregion
    }
}
