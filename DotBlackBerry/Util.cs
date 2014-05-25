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
        /// Serialize an object to a pointer.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The data, or IntPtr.Zero if obj is null or an error occurs.</returns>
        public static IntPtr SerializeToPointer(object obj)
        {
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
            var data = ms.GetBuffer();
            var ptr = Syscall.malloc((ulong)ms.Length + sizeof(int));
            Marshal.WriteInt32(ptr, (int)ms.Length);
            Marshal.Copy(data, 0, ptr, (int)ms.Length);
            return ptr;
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
        }

        /// <summary>
        /// Free the serialized pointer (just a wrapper around Syscall.free)
        /// </summary>
        /// <param name="ptr">The pointer to free.</param>
        public static void FreeSerializePointer(IntPtr ptr)
        {
            Syscall.free(ptr);
        }
    }
}
