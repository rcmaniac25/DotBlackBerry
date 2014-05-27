using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

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
        public static IntPtr SerializeToPointer(object obj, GCHandleType type = GCHandleType.Normal)
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
