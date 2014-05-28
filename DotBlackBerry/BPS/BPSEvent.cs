using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

using Mono.Unix.Native;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Structure that represents the payload of an event.
    /// </summary>
    [AvailableSince(10, 0)]
    public struct BPSEventPayload
    {
        /// <summary>
        /// Create a new instance of a event payload.
        /// </summary>
        /// <param name="d1">Payload data.</param>
        /// <param name="d2">Payload data.</param>
        /// <param name="d3">Payload data.</param>
        [AvailableSince(10, 0)]
        public BPSEventPayload(object d1 = null, object d2 = null, object d3 = null)
            : this()
        {
            Data1 = d1;
            Data2 = d2;
            Data3 = d3;
        }

        /// <summary>
        /// Get payload data.
        /// </summary>
        [AvailableSince(10, 0)]
        public object Data1 { get; private set; }

        /// <summary>
        /// Get payload data.
        /// </summary>
        [AvailableSince(10, 0)]
        public object Data2 { get; private set; }

        /// <summary>
        /// Get payload data.
        /// </summary>
        [AvailableSince(10, 0)]
        public object Data3 { get; private set; }

        #region Native Conversion

        private struct Payload
        {
            public IntPtr data1;
            public IntPtr data2;
            public IntPtr data3;
        }

        internal BPSEventPayload(IntPtr ptr)
            : this()
        {
            if (ptr != IntPtr.Zero)
            {
                var payload = new Payload();
                Marshal.PtrToStructure(ptr, payload);
                Data1 = Util.DeserializeFromPointer(payload.data1);
                Data2 = Util.DeserializeFromPointer(payload.data2);
                Data3 = Util.DeserializeFromPointer(payload.data3);
            }
        }

        internal IntPtr GetDataPointer()
        {
            if (Data1 == null && Data2 == null && Data3 == null)
            {
                return IntPtr.Zero;
            }

            var payload = new Payload();
            payload.data1 = Util.SerializeToPointer(Data1);
            payload.data2 = Util.SerializeToPointer(Data2);
            payload.data3 = Util.SerializeToPointer(Data3);

            var result = Stdlib.malloc((ulong)Marshal.SizeOf(payload));
            if (result == IntPtr.Zero)
            {
                Util.FreeSerializePointer(payload.data1);
                Util.FreeSerializePointer(payload.data2);
                Util.FreeSerializePointer(payload.data3);
                return IntPtr.Zero;
            }
            Marshal.StructureToPtr(payload, result, false);
            BPS.AvaliableInstance.RegisterSerializedPointer(payload.data1);
            BPS.AvaliableInstance.RegisterSerializedPointer(payload.data2);
            BPS.AvaliableInstance.RegisterSerializedPointer(payload.data3);
            return result;
        }

        internal static void FreeDataPointer(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return;
            }
            var payload = new Payload();
            Marshal.PtrToStructure(ptr, payload);
            BPS.AvaliableInstance.UnregisterSerializedPointer(payload.data1);
            BPS.AvaliableInstance.UnregisterSerializedPointer(payload.data2);
            BPS.AvaliableInstance.UnregisterSerializedPointer(payload.data3);
            Util.FreeSerializePointer(payload.data1);
            Util.FreeSerializePointer(payload.data2);
            Util.FreeSerializePointer(payload.data3);
        }

        #endregion
    }

    /// <summary>
    /// BPS-based events.
    /// </summary>
    [AvailableSince(10, 0)]
    public class BPSEvent : IDisposable
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern void bps_event_destroy(IntPtr ev);

        [DllImport("bps")]
        private static extern int bps_event_get_domain(IntPtr ev);

        [DllImport("bps")]
        private static extern uint bps_event_get_code(IntPtr ev);

        [DllImport("bps")]
        private static extern IntPtr bps_event_get_payload(IntPtr ev);

        [DllImport("bps")]
        private static extern int bps_event_create(out IntPtr ev, uint domain, uint code, IntPtr payload_ptr, Action<IntPtr> completion_function);

        #endregion

        /// <summary>
        /// The maximum allowable domain of an event that you create using.
        /// </summary>
        [AvailableSince(10, 0)]
        public const int BPS_EVENT_DOMAIN_MAX = 0x00000FFF;

        private static IDictionary<IntPtr, Action<BPSEvent>> handleToCallback = new ConcurrentDictionary<IntPtr, Action<BPSEvent>>();

        private IntPtr handle;

        internal BPSEvent(IntPtr hwnd, bool disposable = false)
        {
            handle = hwnd;
            IsDisposable = disposable;
        }

        /// <summary>
        /// Create an event.
        /// </summary>
        /// <param name="domain">The domain of the event.</param>
        /// <param name="code">The code of the event.</param>
        [AvailableSince(10, 0)]
        public BPSEvent(int domain, uint code)
            : this(domain, code, new BPSEventPayload(), null, false)
        {
        }

        /// <summary>
        /// Create an event.
        /// </summary>
        /// <param name="domain">The domain of the event.</param>
        /// <param name="code">The code of the event.</param>
        /// <param name="payload">The event's payload.</param>
        [AvailableSince(10, 0)]
        public BPSEvent(int domain, uint code, BPSEventPayload payload)
            : this(domain, code, payload, null, false)
        {
        }

        /// <summary>
        /// Create an event.
        /// </summary>
        /// <param name="domain">The domain of the event.</param>
        /// <param name="code">The code of the event.</param>
        /// <param name="payload">The event's payload.</param>
        /// <param name="completionFunction">An optional completion function that will be invoked when the system is done with the event.</param>
        [AvailableSince(10, 0)]
        public BPSEvent(int domain, uint code, BPSEventPayload payload, Action<BPSEvent> completionFunction)
            : this(domain, code, payload, completionFunction, true)
        {
        }

        private BPSEvent(int domain, uint code, BPSEventPayload payload, Action<BPSEvent> completionFunction, bool recordCompletion)
        {
            if (domain < 0 || domain > BPS_EVENT_DOMAIN_MAX)
            {
                throw new ArgumentOutOfRangeException("domain", "0 <= domain < BPS_EVENT_DOMAIN_MAX");
            }
            if (code > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException("code", "0 <= code < UInt16.MaxValue");
            }
            Util.GetBPSOrException();
            var payloadPtr = payload.GetDataPointer();
            var success = bps_event_create(out handle, (uint)domain, code, payloadPtr, EventCompletion) != BPS.BPS_SUCCESS;
            if (payloadPtr != IntPtr.Zero)
            {
                if (!success)
                {
                    BPSEventPayload.FreeDataPointer(payloadPtr);
                }
                Stdlib.free(payloadPtr);
            }
            if (success)
            {
                if (recordCompletion)
                {
                    handleToCallback.Add(handle, completionFunction);
                }
            }
            else
            {
                Util.ThrowExceptionForErrno();
            }
            IsDisposable = true;
        }

        private static void EventCompletion(IntPtr ptr)
        {
            if (handleToCallback.ContainsKey(ptr))
            {
                var ev = new BPSEvent(ptr, false);
                var callback = handleToCallback[ptr];
                handleToCallback.Remove(ptr);
                try
                {
                    callback(ev);
                }
                catch
                {
                }
            }
            BPSEventPayload.FreeDataPointer(bps_event_get_payload(ptr));
        }

        /// <summary>
        /// Get if the event is disposable.
        /// </summary>
        public bool IsDisposable { get; private set; }

        /// <summary>
        /// Get the code of an event.
        /// </summary>
        [AvailableSince(10, 0)]
        public uint Code
        {
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("BPSEvent");
                }
                return bps_event_get_code(handle);
            }
        }

        /// <summary>
        /// Get the domain of an event.
        /// </summary>
        [AvailableSince(10, 0)]
        public int Domain
        {
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("BPSEvent");
                }
                return bps_event_get_domain(handle);
            }
        }

        /// <summary>
        /// Get the payload of an event.
        /// </summary>
        [AvailableSince(10, 0)]
        public BPSEventPayload Payload
        {
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("BPSEvent");
                }
                return new BPSEventPayload(bps_event_get_payload(handle));
            }
        }

        /// <summary>
        /// Get the internal handle of the BPS event.
        /// </summary>
        /// <returns>The internal handle of the BPS event.</returns>
#if BLACKBERRY_INTERNAL_FUNCTIONS
        public IntPtr DangerousGetHandle()
#else
        internal IntPtr DangerousGetHandle()
#endif
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("BPSEvent");
            }
            return handle;
        }

        /// <summary>
        /// Dispose of the BPS event.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (!IsDisposable)
            {
                throw new InvalidOperationException("BPSEvent cannot be disposed directly");
            }
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("BPSEvent");
            }
            bps_event_destroy(handle);
            handle = IntPtr.Zero;
        }
    }
}
