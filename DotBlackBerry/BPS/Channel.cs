using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace BlackBerry.BPS
{
    /// <summary>
    /// BPS Channel
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class Channel : IDisposable
    {
        #region Event Token

        [ThreadStatic]
        private static IDictionary<int, CancellationTokenSource> eventValidityToken; // Don't set to any value...

        private static CancellationTokenSource GetTokenSource(int channel, bool initTokens = true)
        {
            if (initTokens)
            {
                Interlocked.CompareExchange(ref eventValidityToken, new ConcurrentDictionary<int, CancellationTokenSource>(), null);
            }
            if (eventValidityToken != null)
            {
                CancellationTokenSource token;
                if (!eventValidityToken.TryGetValue(channel, out token))
                {
                    token = new CancellationTokenSource();
                    eventValidityToken.Add(channel, token);
                }
                return token;
            }
            return null;
        }

        internal static CancellationToken GetNewToken(int channel)
        {
            if (eventValidityToken == null)
            {
                return GetTokenSource(channel).Token;
            }
            GetTokenSource(channel).Cancel();
            var token = new CancellationTokenSource();
            eventValidityToken[channel] = token;
            return token.Token;
        }

        private static void RemoveToken(int channel)
        {
            if (eventValidityToken != null)
            {
                eventValidityToken.Remove(channel);
            }
        }

        #endregion

        private int chid;

        /// <summary>
        /// Create a new instance of a BPS channel.
        /// </summary>
        [AvailableSince(10, 0)]
        public Channel()
            : this(0)
        {
            Util.GetBPSOrException();
            if (BPS.bps_channel_create(out chid, 0) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        internal Channel(int chid)
        {
            this.chid = chid;
            OwnerThread = Thread.CurrentThread;
        }

        /// <summary>
        /// Get or set the active BPS channel.
        /// </summary>
        [AvailableSince(10, 0)]
        public static Channel ActiveChannel
        {
            [AvailableSince(10, 0)]
            get
            {
                return Util.GetBPSOrException().ActiveChannel;
            }
            [AvailableSince(10, 0)]
            set
            {
                Util.GetBPSOrException().ActiveChannel = value;
            }
        }

        internal int Handle
        {
            get
            {
                if (chid == 0)
                {
                    throw new ObjectDisposedException("Channel");
                }
                return chid;
            }
        }

        /// <summary>
        /// Get the thread on which the channel is attached to.
        /// </summary>
        public Thread OwnerThread { get; private set; }

        /// <summary>
        /// Dispose BPS channel.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (chid == 0)
            {
                throw new ObjectDisposedException("Channel");
            }
            if (BPS.bps_channel_destroy(chid) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            var token = GetTokenSource(chid, false);
            if (token != null)
            {
                RemoveToken(chid);
                token.Cancel();
            }
            chid = 0;
        }

        /// <summary>
        /// Push an event to the channel.
        /// </summary>
        /// <param name="ev">The BPS event to push to this channel.</param>
        /// <returns>true if the event was pushed successfully, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool PushEvent(BPSEvent ev)
        {
            if (chid == 0)
            {
                throw new ObjectDisposedException("Channel");
            }
            return BPS.bps_channel_push_event(chid, ev.DangerousGetHandle()) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Register a function for eventual execution.
        /// </summary>
        /// <param name="exec">The function that will be executed.</param>
        /// <param name="args">The user data that will be used as the first argument of the invoke function.</param>
        /// <returns>true if the invoke callback was successfully registered, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool PushDelegate(Action<object> exec, object args = null)
        {
            if (chid == 0)
            {
                throw new ObjectDisposedException("Channel");
            }
            var toSerialize = args == null ? (object)exec : new object[] { exec, args };
            var data = Util.SerializeToPointer(toSerialize);
            if (data == IntPtr.Zero)
            {
                return false;
            }
            return BPS.bps_channel_exec(chid, Util.ActionObjFreeHandlerFromFuncZeroReturn, data) == BPS.BPS_SUCCESS;
        }
    }
}
