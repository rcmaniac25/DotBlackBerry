using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Verbosity levels for BPS logging.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum BPSVerbosity : uint
    {
        /// <summary>
        /// Shutdown and Critical messages are logged.
        /// </summary>
        [AvailableSince(10, 0)]
        Critical = 0,
        /// <summary>
        /// Shutdown, Critical, Error and Warning messages are logged.
        /// </summary>
        [AvailableSince(10, 0)]
        ErrorWarning = 1,
        /// <summary>
        /// Shutdown, Critical, Error, Warning, Notice and Info messages are logged.
        /// </summary>
        [AvailableSince(10, 0)]
        InfoNotice = 2,
        /// <summary>
        /// Shutdown, Critical, Error, Warning, Notice, Info, Debug1 and Debug2 messages are logged.
        /// </summary>
        [AvailableSince(10, 0)]
        Debug = 3
    }

    /// <summary>
    /// The bit to use when adding a file descriptor.
    /// </summary>
    [Flags, AvailableSince(10, 0)]
    public enum BPSIO : int
    {
        /// <summary>
        /// Indicate that there is data available for reading.
        /// </summary>
        [AvailableSince(10, 0)]
        Input = 1 << 0,
        /// <summary>
        /// Indicate that there is room in the output buffer for more data.
        /// </summary>
        [AvailableSince(10, 0)]
        Output = 1 << 1,
        /// <summary>
        /// An error occured, the device disconnected, or the file descriptor was invalid.
        /// </summary>
        [AvailableSince(10, 0)]
        Except = 1 << 2
    }

    /// <summary>
    /// BlackBerry Platform Services
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class BPS : IDisposable
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int bps_get_version();

        [DllImport("bps")]
        internal static extern int bps_initialize();

        [DllImport("bps")]
        internal static extern void bps_shutdown();

        [DllImport("bps")]
        internal static extern void bps_free(IntPtr ptr);

        [DllImport("bps")]
        private static extern int bps_logging_init();

        [DllImport("bps")]
        private static extern void bps_set_verbosity(uint verbosity);

        [DllImport("bps")]
        private static extern int bps_register_domain();

        [DllImport("bps")]
        private static extern int bps_push_event(IntPtr ev);

        [DllImport("bps")]
        private static extern int bps_register_shutdown_handler(Action<IntPtr> shutdown_handler, IntPtr data);

        [DllImport("bps")]
        private static extern int bps_register_channel_destroy_handler(Action<IntPtr> destroy_handler, IntPtr data);

        [DllImport("bps")]
        internal static extern int bps_channel_create(out int chid, int flags);

        [DllImport("bps")]
        private static extern int bps_channel_get_active();

        [DllImport("bps")]
        private static extern int bps_channel_set_active(int chid);

        [DllImport("bps")]
        internal static extern int bps_channel_destroy(int chid);

        [DllImport("bps")]
        internal static extern int bps_channel_push_event(int chid, IntPtr ev);

        [DllImport("bps")]
        internal static extern int bps_channel_exec(int chid, Func<IntPtr, int> exec, IntPtr data);

        [DllImport("bps")]
        private static extern IntPtr bps_get_domain_data(int domain_id);

        [DllImport("bps")]
        private static extern int bps_set_domain_data(int domain_id, IntPtr new_data, out IntPtr old_data);

        [DllImport("bps")]
        private static extern int bps_add_fd(int fd, int io_events, Func<int, int, IntPtr, int> io_handler, IntPtr data);

        [DllImport("bps")]
        private static extern int bps_remove_fd(int fd);

        [DllImport("bps")]
        private static extern int bps_get_event(out IntPtr ev, int timeout_ms);

        #endregion

        #region Consts

        internal const int BPS_SUCCESS = 0;
        internal const int BPS_FAILURE = -1;

        #endregion

        private static BPS initBPS = null;
        private static int initBPScount = 0;
        private static ConcurrentSet<IntPtr> allocatedPointers = new ConcurrentSet<IntPtr>();
        private static IDictionary<IntPtr, IntPtr> fdToPointer = new ConcurrentDictionary<IntPtr, IntPtr>();

        private bool canShutdown;
        private bool disposed;

        /// <summary>
        /// Create an instance of BlackBerry Platform Services.
        /// </summary>
        [AvailableSince(10, 0)]
        public BPS()
            : this(true)
        {
        }

        private BPS(bool init)
        {
            if (init)
            {
                // init can be called multiple times
                if (bps_initialize() != BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                if ((initBPScount++) == 1)
                {
                    bps_register_channel_destroy_handler(CleanupPointers, IntPtr.Zero);
                    initBPS = this;
                }
            }
            canShutdown = init;
            disposed = false;
        }

        #region Properties

        /// <summary>
        /// BPS Version
        /// </summary>
        [AvailableSince(10, 0)]
        public static Version Version
        {
            [AvailableSince(10, 0)]
            get
            {
                // The version number is computed as follows:
                // (Major * 1000000) + (Minor * 1000) + Patch
                var encodedVersion = bps_get_version();
                var major = encodedVersion / 1000000;
                var majorEncoded = major * 1000000;
                var minor = (encodedVersion - majorEncoded) / 1000;
                var patch = encodedVersion - (majorEncoded + (minor * 1000));
                return new Version(major, minor, patch);
            }
        }

        /// <summary>
        /// Get or set the active BPS channel.
        /// </summary>
        [AvailableSince(10, 0)]
        public Channel ActiveChannel
        {
            [AvailableSince(10, 0)]
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("BPS");
                }
                return new Channel(bps_channel_get_active());
            }
            [AvailableSince(10, 0)]
            set
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("BPS");
                }
                if (value.OwnerThread != Thread.CurrentThread)
                {
                    throw new InvalidOperationException("Channel is from a different thread");
                }
                if (bps_channel_set_active(value.Handle) == BPS_FAILURE)
                {
                    Util.ThrowExceptionForLastErrno();
                }
            }
        }

        /// <summary>
        /// Get the first instance of BPS.
        /// </summary>
        public static BPS AvaliableInstance
        {
            get
            {
                if (initBPS != null)
                {
                    return new BPS(false);
                }
                return null;
            }
        }

        #endregion

        /// <summary>
        /// Shutdown BPS
        /// </summary>
        /// If Dispose is called more times then BPS was created, then it will exit the program abnormally.
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (!canShutdown)
            {
                throw new InvalidOperationException("Cannot dispose BPS instance");
            }
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            if (initBPScount == 0)
            {
                //TODO: should log why the program just exited abnormally
                Environment.Exit(1);
            }
            disposed = true;
            if (initBPScount > 0)
            {
                initBPScount--;
                if (initBPScount == 0)
                {
                    initBPS = null;
                }
                bps_shutdown();
            }
            // Don't call GC.SuppressFinalize as this works off a reference count-style methodology
        }

        private static void CleanupPointers(IntPtr ignore)
        {
            var oldData = allocatedPointers.ToArray();
            allocatedPointers.Clear();
            foreach (var ptr in oldData)
            {
                try
                {
                    Util.FreeSerializePointer(ptr);
                }
                catch
                {
                    //TODO big error...
                }
            }

            fdToPointer.Clear();
        }

        #region Functions

#if BLACKBERRY_INTERNAL_FUNCTIONS

        /// <summary>
        /// Setup a default slog2 buffer if one is not set.
        /// </summary>
        /// <returns>true if a default slog2 buffer was set up successfully or if one was already setup. false if setting up a default slog2 buffer failed.</returns>
        [AvailableSince(10, 1)]
        public bool InitializeLogging()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            return bps_logging_init() == BPS_SUCCESS;
        }

#endif

        internal bool RegisterSerializedPointer(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                return allocatedPointers.Add(ptr);
            }
            return true;
        }

        internal bool UnregisterSerializedPointer(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                return allocatedPointers.Remove(ptr);
            }
            return true;
        }

        /// <summary>
        /// Set the verbosity of logging for platform services.
        /// </summary>
        /// Default verbosity is BPSVerbosity.InfoNotice.
        /// <param name="verbosity">The desired verbosity</param>
        [AvailableSince(10, 0)]
        public void SetLoggingVerbosity(BPSVerbosity verbosity)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            bps_set_verbosity((uint)verbosity);
        }

        /// <summary>
        /// Reserve a unique domain ID for identifying a service's events.
        /// </summary>
        /// <returns>A domain ID for a service. A value of -1 indicates an error.</returns>
        [AvailableSince(10, 0)]
        public int RegisterDomain()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            return bps_register_domain();
        }

        /// <summary>
        /// Register a callback that will be invoked when the last shutdown function is called.
        /// </summary>
        /// <param name="callback">Callback to invoke on shutdown.</param>
        /// <param name="callbackData">Additional data to pass to callback.</param>
        /// <returns>true is returned if the handler registered successfully with the data, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool RegisterShutdownHandler(Action<object> callback, object callbackData = null)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            var toSerialize = callbackData == null ? (object)callback : new object[] { callback, callbackData };
            var data = Util.SerializeToPointer(toSerialize);
            if (data == IntPtr.Zero)
            {
                return false;
            }
            return bps_register_shutdown_handler(Util.ActionObjFreeHandlerFromAction, data) == BPS_SUCCESS;
        }

        #region Add/Remove File Descriptor

        private int HandleFileDescriptor(int fd, int ioe, IntPtr ptr)
        {
            var parsedData = Util.DeserializeFromPointer(ptr) as object[];
            IntPtr key;
            if (parsedData != null)
            {
                var sender = parsedData[0];
                var handler = parsedData[1] as Func<object, BPSIO, object, bool>;
                var data = parsedData.Length > 2 ? parsedData[2] : null;
                var continueExec = true;
                try
                {
                    continueExec = handler(sender, (BPSIO)ioe, data);
                }
                catch
                {
                    //TODO: log error
                    continueExec = false;
                }
                if (continueExec)
                {
                    return BPS_SUCCESS;
                }
            }
            if (fdToPointer.ManuallyFindKey(ptr, out key))
            {
                fdToPointer.Remove(key);
            }
            allocatedPointers.Remove(ptr);
            Util.FreeSerializePointer(ptr);
            return BPS_FAILURE;
        }

        internal bool AddFileDescriptor<T>(SafeHandle handle, T sender, BPSIO ioEvents, Func<T, BPSIO, object, bool> ioHandler, object callbackData)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            // Could probably just pass sender directly into the ioHandler instead of routing it through the HandleFileDescriptor function
            var genericCallback = new Func<object, BPSIO, object, bool>((s, e, d) => ioHandler((T)s, e, d));
            var toSerialize = callbackData == null ? new object[] { sender, genericCallback } : new object[] { sender, genericCallback, callbackData };
            var data = Util.SerializeToPointer(toSerialize);
            if (data == IntPtr.Zero)
            {
                return false;
            }
            var fd = handle.DangerousGetHandle();
            var success = bps_add_fd(fd.ToInt32(), (int)ioEvents, HandleFileDescriptor, data) == BPS_SUCCESS;
            if (success)
            {
                fdToPointer.Add(fd, data);
                allocatedPointers.Add(data);
            }
            else
            {
                Util.FreeSerializePointer(data);
            }
            return success;
        }

        /// <summary>
        /// Add a file descriptor to the currently active channel.
        /// </summary>
        /// <param name="handle">The file descriptor to start monitoring.</param>
        /// <param name="ioEvents">The I/O conditions to monitor for.</param>
        /// <param name="ioHandler">The I/O callback that is called whenever I/O conditions are met.</param>
        /// <param name="data">User supplied data that will be given to the I/O callback as the third argument.</param>
        /// <returns>true if the file descriptor was successfully added to the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool AddFileDescriptor(SafeHandle handle, BPSIO ioEvents, Func<SafeHandle, BPSIO, object, bool> ioHandler, object data = null)
        {
            return AddFileDescriptor(handle, handle, ioEvents, ioHandler, data);
        }

        /// <summary>
        /// Remove a file descriptor from the active channel.
        /// </summary>
        /// <param name="handle">The file descriptor to remove.</param>
        /// <returns>true if the file descriptor was successfully removed from the channel, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool RemoveFileDescriptor(SafeHandle handle)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            if (handle.IsInvalid)
            {
                throw new ArgumentException("Handle is invalid");
            }
            var pointer = handle.DangerousGetHandle();
            var success = bps_remove_fd(pointer.ToInt32()) == BPS_SUCCESS;
            if (fdToPointer.ContainsKey(pointer) && success)
            {
                var dataPtr = fdToPointer[pointer];
                fdToPointer.Remove(pointer);
                allocatedPointers.Remove(dataPtr);
                Util.FreeSerializePointer(dataPtr);
            }
            return success;
        }

        #endregion

        #region Add/Remove sigevent

        //TODO bps_add_sigevent_handler
        //TODO bps_remove_sigevent_handler

        #endregion

        #region Get/Set Domain Data

        /// <summary>
        /// Retrieve domain-specific data from the active channel.
        /// </summary>
        /// <param name="domain">Domain ID</param>
        /// <returns>The user data that was associated with the active channel and the <paramref name="domain"/>. If no data is found, a <c>null</c> value is returned.</returns>
        [AvailableSince(10, 0)]
        public object GetDomainData(int domain)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            return Util.DeserializeFromPointer(bps_get_domain_data(domain));
        }

        /// <summary>
        /// Set domain specific data for the active channel.
        /// </summary>
        /// <param name="domain">Domain ID</param>
        /// <param name="data">The service user data that should be stored in the active channel.</param>
        /// <returns>true if the data was set, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool SetDomainData(int domain, object data)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            var o = IntPtr.Zero;
            var n = Util.SerializeToPointer(data);
            if (n != IntPtr.Zero)
            {
                allocatedPointers.Add(n);
            }
            var success = bps_set_domain_data(domain, n, out o) == BPS_SUCCESS;
            if (success && o != IntPtr.Zero)
            {
                if (allocatedPointers.Remove(o))
                {
                    Util.FreeSerializePointer(o);
                }
            }
            if (!success && n != IntPtr.Zero)
            {
                allocatedPointers.Remove(n);
                Util.FreeSerializePointer(n);
            }
            return success;
        }

        #endregion

        /// <summary>
        /// Register a callback that will be invoked when the active channel is being destroyed.
        /// </summary>
        /// <param name="callback">The callback that is invoked on channel destruction.</param>
        /// <param name="callbackData">The user data that is passed to the <paramref name="callback"/>.</param>
        /// <returns>true when the handler has successfully been registered along with the data, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool RegisterChannelDestroyHandler(Action<object> callback, object callbackData = null)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            var toSerialize = callbackData == null ? (object)callback : new object[] { callback, callbackData };
            var data = Util.SerializeToPointer(toSerialize);
            if (data == IntPtr.Zero)
            {
                return false;
            }
            return bps_register_channel_destroy_handler(Util.ActionObjFreeHandlerFromAction, data) == BPS_SUCCESS;
        }

        #region GetEvent

        /// <summary>
        /// Retrieve the next event from the application's active channel.
        /// </summary>
        /// <returns>A BPSEvent.</returns>
        [AvailableSince(10, 0)]
        public BPSEvent GetEvent()
        {
            return GetEvent(Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// Retrieve the next event from the application's active channel.
        /// </summary>
        /// <param name="timeout">The timeout for getting the event. If timeout occurs, a TimeoutException is thrown.</param>
        /// <returns>A BPSEvent.</returns>
        [AvailableSince(10, 0)]
        public BPSEvent GetEvent(TimeSpan timeout)
        {
            return GetEventAsync(timeout).Result;
        }

        /// <summary>
        /// Retrieve the next event from the application's active channel.
        /// </summary>
        /// <returns>An async BPSEvent task.</returns>
        [AvailableSince(10, 0)]
        public async Task<BPSEvent> GetEventAsync()
        {
            return await GetEventAsync(Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// Retrieve the next event from the application's active channel.
        /// </summary>
        /// <param name="timeout">The timeout for getting the event. If timeout occurs, a TimeoutException is thrown.</param>
        /// <returns>An async BPSEvent task.</returns>
        [AvailableSince(10, 0)]
        public async Task<BPSEvent> GetEventAsync(TimeSpan timeout)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            return await Task.Factory.StartNew(t => GetEventBlocking((TimeSpan)t), timeout);
        }

        private BPSEvent GetEventBlocking(TimeSpan timeout)
        {
            IntPtr ev;
            if (bps_get_event(out ev, timeout == Timeout.InfiniteTimeSpan ? -1 : (int)timeout.TotalMilliseconds) != BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            if (ev == IntPtr.Zero)
            {
                if (timeout != Timeout.InfiniteTimeSpan)
                {
#if BLACKBERRY_BPS_EVENT_THROW_TIMEOUT
                    throw new TimeoutException();
#else
                    return null;
#endif
                }
                throw new OperationCanceledException("GetEvent exited without returning an event or an error");
            }
            return new BPSEvent(ev, false);
        }

        #endregion

        /// <summary>
        /// Post an event to the active channel.
        /// </summary>
        /// <param name="ev">The event to post to the active channel.</param>
        /// <returns>true when the event has been posted, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool PushEvent(BPSEvent ev)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("BPS");
            }
            return bps_push_event(ev.DangerousGetHandle()) == BPS_SUCCESS;
        }

        //XXX should possibly have Equals, HashCode, and ToString. The issue is all BPS instances should be the the same, 
        //but if a BPS instance shouldn't be disposable, then it is not equal and should not be treated as such.

        #endregion
    }
}
