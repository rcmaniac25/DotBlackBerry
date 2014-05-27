using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Reflection;
using System.Linq;

using Microsoft.Win32.SafeHandles;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Extension functions for BPS.
    /// </summary>
    [AvailableSince(10, 0)]
    public static class BPSExtensions
    {
        internal static bool ManuallyFindKey<K, V>(this IDictionary<K, V> dict, V value, out K key)
        {
            foreach (var kv in dict)
            {
                if (kv.Value.Equals(value))
                {
                    key = kv.Key;
                    return true;
                }
            }
            key = default(K);
            return false;
        }

        #region AddFileDescriptor

        /// <summary>
        /// Add a FileStream to the currently active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="fs">The FileStream to start monitoring.</param>
        /// <param name="ioEvents">The I/O conditions to monitor for.</param>
        /// <param name="ioHandler">The I/O callback that is called whenever I/O conditions are met.</param>
        /// <param name="data">User supplied data that will be given to the I/O callback as the third argument.</param>
        /// <returns>true if the FileStream was successfully added to the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool AddFileDescriptor(this BPS bps, FileStream fs, BPSIO ioEvents, Func<FileStream, BPSIO, object, bool> ioHandler, object data = null)
        {
            return bps.AddFileDescriptor(fs.SafeFileHandle, fs, ioEvents, ioHandler, data);
        }

        /// <summary>
        /// Add a Socket to the currently active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="s">The Socket to start monitoring.</param>
        /// <param name="ioEvents">The I/O conditions to monitor for.</param>
        /// <param name="ioHandler">The I/O callback that is called whenever I/O conditions are met.</param>
        /// <param name="data">User supplied data that will be given to the I/O callback as the third argument.</param>
        /// <returns>true if the Socket was successfully added to the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool AddFileDescriptor(this BPS bps, Socket s, BPSIO ioEvents, Func<Socket, BPSIO, object, bool> ioHandler, object data = null)
        {
            return bps.AddFileDescriptor(new SafeFileHandle(s.Handle, false), s, ioEvents, ioHandler, data); // Not really the proper way to do this
        }

        /// <summary>
        /// Add a NetworkStream to the currently active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="ns">The NetworkStream to start monitoring.</param>
        /// <param name="ioEvents">The I/O conditions to monitor for.</param>
        /// <param name="ioHandler">The I/O callback that is called whenever I/O conditions are met.</param>
        /// <param name="data">User supplied data that will be given to the I/O callback as the third argument.</param>
        /// <returns>true if the NetworkStream was successfully added to the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool AddFileDescriptor(this BPS bps, NetworkStream ns, BPSIO ioEvents, Func<NetworkStream, BPSIO, object, bool> ioHandler, object data = null)
        {
            var socketProp = typeof(NetworkStream).GetProperties(BindingFlags.GetProperty | BindingFlags.NonPublic).First(prop => prop.Name == "Socket");
            var socket = socketProp.GetValue(ns) as Socket;
            return bps.AddFileDescriptor(new SafeFileHandle(socket.Handle, false), ns, ioEvents, ioHandler, data); // Not really the proper way to do this
        }

        /// <summary>
        /// Add a PipeStream to the currently active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="p">The PipeStream to start monitoring.</param>
        /// <param name="ioEvents">The I/O conditions to monitor for.</param>
        /// <param name="ioHandler">The I/O callback that is called whenever I/O conditions are met.</param>
        /// <param name="data">User supplied data that will be given to the I/O callback as the third argument.</param>
        /// <returns>true if the PipeStream was successfully added to the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool AddFileDescriptor(this BPS bps, PipeStream p, BPSIO ioEvents, Func<PipeStream, BPSIO, object, bool> ioHandler, object data = null)
        {
            return bps.AddFileDescriptor(p.SafePipeHandle, p, ioEvents, ioHandler, data);
        }

        /// <summary>
        /// Add a MemoryMappedFile to the currently active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="mmf">The MemoryMappedFile to start monitoring.</param>
        /// <param name="ioEvents">The I/O conditions to monitor for.</param>
        /// <param name="ioHandler">The I/O callback that is called whenever I/O conditions are met.</param>
        /// <param name="data">User supplied data that will be given to the I/O callback as the third argument.</param>
        /// <returns>true if the MemoryMappedFile was successfully added to the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool AddFileDescriptor(this BPS bps, MemoryMappedFile mmf, BPSIO ioEvents, Func<MemoryMappedFile, BPSIO, object, bool> ioHandler, object data = null)
        {
            return bps.AddFileDescriptor(mmf.SafeMemoryMappedFileHandle, mmf, ioEvents, ioHandler, data);
        }

        /// <summary>
        /// Add a MemoryMappedViewStream to the currently active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="mmv">The MemoryMappedViewStream to start monitoring.</param>
        /// <param name="ioEvents">The I/O conditions to monitor for.</param>
        /// <param name="ioHandler">The I/O callback that is called whenever I/O conditions are met.</param>
        /// <param name="data">User supplied data that will be given to the I/O callback as the third argument.</param>
        /// <returns>true if the MemoryMappedViewStream was successfully added to the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool AddFileDescriptor(this BPS bps, MemoryMappedViewStream mmv, BPSIO ioEvents, Func<MemoryMappedViewStream, BPSIO, object, bool> ioHandler, object data = null)
        {
            return bps.AddFileDescriptor(mmv.SafeMemoryMappedViewHandle, mmv, ioEvents, ioHandler, data);
        }

        #endregion

        #region RemoveFileDescriptor

        /// <summary>
        /// Remove a FileStream from the active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="fs">The FileStream to remove.</param>
        /// <returns>true if the FileStream was successfully removed from the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RemoveFileDescriptor(this BPS bps, FileStream fs)
        {
            return bps.RemoveFileDescriptor(fs.SafeFileHandle);
        }

        /// <summary>
        /// Remove a Socket from the active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="s">The Socket to remove.</param>
        /// <returns>true if the Socket was successfully removed from the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RemoveFileDescriptor(this BPS bps, Socket s)
        {
            return bps.RemoveFileDescriptor(new SafeFileHandle(s.Handle, false)); // Not really the proper way to do this
        }

        /// <summary>
        /// Remove a Socket from the active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="ns">The Socket to remove.</param>
        /// <returns>true if the Socket was successfully removed from the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RemoveFileDescriptor(this BPS bps, NetworkStream ns)
        {
            var socketProp = typeof(NetworkStream).GetProperties(BindingFlags.GetProperty | BindingFlags.NonPublic).First(prop => prop.Name == "Socket");
            return bps.RemoveFileDescriptor(socketProp.GetValue(ns) as Socket);
        }

        /// <summary>
        /// Remove a PipeStream from the active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="p">The PipeStream to remove.</param>
        /// <returns>true if the PipeStream was successfully removed from the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RemoveFileDescriptor(this BPS bps, PipeStream p)
        {
            return bps.RemoveFileDescriptor(p.SafePipeHandle);
        }

        /// <summary>
        /// Remove a MemoryMappedFile from the active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="mmf">The MemoryMappedFile to remove.</param>
        /// <returns>true if the MemoryMappedFile was successfully removed from the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RemoveFileDescriptor(this BPS bps, MemoryMappedFile mmf)
        {
            return bps.RemoveFileDescriptor(mmf.SafeMemoryMappedFileHandle);
        }

        /// <summary>
        /// Remove a MemoryMappedViewStream from the active channel.
        /// </summary>
        /// <param name="bps">The BPS object to use.</param>
        /// <param name="mmv">The MemoryMappedViewStream to remove.</param>
        /// <returns>true if the MemoryMappedViewStream was successfully removed from the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RemoveFileDescriptor(this BPS bps, MemoryMappedViewStream mmv)
        {
            return bps.RemoveFileDescriptor(mmv.SafeMemoryMappedViewHandle);
        }

        #endregion
    }
}
