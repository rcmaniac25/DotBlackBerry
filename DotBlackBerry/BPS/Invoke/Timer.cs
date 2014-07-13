using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BlackBerry.BPS.Invoke
{
    /// <summary>
    /// Timer trigger control.
    /// </summary>
    [AvailableSince(10, 3)]
    public enum TimerAction : int
    {
        /// <summary>
        /// Indicates that timer registration action was not set.
        /// </summary>
        [AvailableSince(10, 3)]
        NotSet = 0,
        /// <summary>
        /// Action to install a new timer trigger or update an existing trigger.
        /// </summary>
        [AvailableSince(10, 3)]
        Register = 1,
        /// <summary>
        /// Action to deregister an existing timer trigger.
        /// </summary>
        [AvailableSince(10, 3)]
        Unregister = 2
    }

    /// <summary>
    /// Timer trigger types.
    /// </summary>
    [AvailableSince(10, 3)]
    public enum TriggerType : int
    {
        /// <summary>
        /// The timer trigger is an unknown type of trigger.
        /// </summary>
        [AvailableSince(10, 3)]
        Unknown = 0,
        /// <summary>
        /// The timer trigger is recurrent.
        /// </summary>
        [AvailableSince(10, 3)]
        Recurrent = 1,
        /// <summary>
        /// The timer trigger is for a specific local time.
        /// </summary>
        [AvailableSince(10, 3)]
        SpecificLocalTime = 2,
        /// <summary>
        /// The timer trigger is for a specific global time.
        /// </summary>
        [AvailableSince(10, 3)]
        SpecificGlobalTime = 3
    }

    /// <summary>
    /// Timer registration request.
    /// </summary>
    [AvailableSince(10, 3)]
    public sealed class Timer : IDisposable
    {
        #region PInvoke

        #region Specific Time

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_create(out IntPtr specific_time);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_invoke_specific_time_destroy(IntPtr specific_time);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_set_year(IntPtr specific_time, int year);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_set_month(IntPtr specific_time, int month);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_set_day(IntPtr specific_time, int day);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_set_hour(IntPtr specific_time, int hour);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_set_minute(IntPtr specific_time, int minute);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_set_time_zone(IntPtr specific_time, [MarshalAs(UnmanagedType.LPStr)]string time_zone);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_get_year(IntPtr specific_time);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_get_month(IntPtr specific_time);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_get_day(IntPtr specific_time);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_get_hour(IntPtr specific_time);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_specific_time_get_minute(IntPtr specific_time);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_specific_time_get_time_zone(IntPtr specific_time);

        #endregion

        #region Timer

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_timer_registration_create(out IntPtr reg, TriggerType type);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_timer_registration_destroy(IntPtr reg);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_timer_registration_set_target(IntPtr reg, [MarshalAs(UnmanagedType.LPStr)]string target);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_timer_registration_get_target(IntPtr reg);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_timer_registration_set_id(IntPtr reg, [MarshalAs(UnmanagedType.LPStr)]string id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_timer_registration_get_id(IntPtr reg);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_timer_registration_set_timer_id(IntPtr reg, [MarshalAs(UnmanagedType.LPStr)]string id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_timer_registration_get_timer_id(IntPtr reg);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern TriggerType navigator_invoke_timer_registration_get_type(IntPtr reg);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_timer_registration_set_action(IntPtr reg, TimerAction action);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_timer_registration_set_specific_time(IntPtr reg, IntPtr specific_time);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_timer_registration_set_recurrence_rule(IntPtr reg, IntPtr rule);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern TimerAction navigator_invoke_timer_registration_get_action(IntPtr reg);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_timer_registration_get_specific_time(IntPtr reg);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_timer_registration_get_recurrence_rule(IntPtr reg);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_timer_registration_send(IntPtr reg);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern TimerAction navigator_invoke_timer_registration_event_get_action(IntPtr ev);

        #endregion

        #endregion

        #region Specific Time

        private static string GenerateTimeZone(DateTimeOffset dt)
        {
            return "GMT" + dt.ToString("zzz");
        }

        private static TimeSpan ParseTimeZone(string tz)
        {
            //XXX Doesn't handle "programmatic ID" based timezones. See http://developer.blackberry.com/native/reference/core/com.qnx.doc.bps.lib_ref/topic/navigator_invoke_specific_time_set_time_zone.html
            if (tz != null && tz.StartsWith("GMT"))
            {
                // GMT[+|-]hh[[:]mm]
                TimeSpan offset;
                if (tz.IndexOf(':') > 0)
                {
                    offset = new TimeSpan(int.Parse(tz.Substring(4, 2)), int.Parse(tz.Substring(8, 2)), 0);
                }
                else
                {
                    offset = new TimeSpan(int.Parse(tz.Substring(4, 2)), 0, 0);
                }
                return tz[3] == '-' ? -offset : offset;
            }
            return TimeSpan.Zero;
        }

        internal static IntPtr CreateSpecificTime(DateTimeOffset dt)
        {
            IntPtr ptr;
            if (navigator_invoke_specific_time_create(out ptr) != BPS.BPS_SUCCESS)
            {
                return IntPtr.Zero;
            }
            navigator_invoke_specific_time_set_time_zone(ptr, GenerateTimeZone(dt));
            navigator_invoke_specific_time_set_year(ptr, dt.Year);
            navigator_invoke_specific_time_set_month(ptr, dt.Month);
            navigator_invoke_specific_time_set_day(ptr, dt.Day);
            navigator_invoke_specific_time_set_hour(ptr, dt.Hour);
            navigator_invoke_specific_time_set_minute(ptr, dt.Minute);
            return ptr;
        }

        internal static DateTimeOffset GetSpecificTime(IntPtr ptr)
        {
            var timezone = Marshal.PtrToStringAnsi(navigator_invoke_specific_time_get_time_zone(ptr));
            var now = DateTime.Now;
            Func<Func<IntPtr, int>, int, int> safeGetValue = (func, replace) =>
            {
                int value = func(ptr);
                return value == BPS.BPS_FAILURE ? replace : value;
            };
            var year = safeGetValue(navigator_invoke_specific_time_get_year, now.Year);
            var month = safeGetValue(navigator_invoke_specific_time_get_month, now.Month);
            var day = safeGetValue(navigator_invoke_specific_time_get_day, now.Day);
            var hour = safeGetValue(navigator_invoke_specific_time_get_hour, now.Hour);
            var minute = safeGetValue(navigator_invoke_specific_time_get_minute, now.Minute);
            return new DateTimeOffset(year, month, day, hour, minute, 0, ParseTimeZone(timezone));
        }

        #endregion

        private IntPtr handle;

        /// <summary>
        /// Create an invoke timer registration request.
        /// </summary>
        /// <param name="type">The type of trigger to register.</param>
        [AvailableSince(10, 3)]
        public Timer(TriggerType type)
        {
            if (!type.IsValidValue() || type == TriggerType.Unknown)
            {
                throw new ArgumentException("Not a valid trigger type", "type");
            }
            Util.GetBPSOrException();
            if (navigator_invoke_timer_registration_create(out handle, type) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        #region Properties

        /// <summary>
        /// Get if the timer is still valid and usable.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return handle != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Get or set target key for the timer registration request.
        /// </summary>
        [AvailableSince(10, 3)]
        public string Target
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_timer_registration_get_target(handle));
            }
            [AvailableSince(10, 3)]
            set
            {
                Verify();
                navigator_invoke_timer_registration_set_target(handle, value);
            }
        }

        /// <summary>
        /// Get or set request message ID for the timer registration request.
        /// </summary>
        [AvailableSince(10, 3)]
        public string ID
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_timer_registration_get_id(handle));
            }
            [AvailableSince(10, 3)]
            set
            {
                Verify();
                navigator_invoke_timer_registration_set_id(handle, value);
            }
        }

        /// <summary>
        /// Get or set the timer ID for the timer registration request.
        /// </summary>
        [AvailableSince(10, 3)]
        public string TimerID
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_timer_registration_get_timer_id(handle));
            }
            [AvailableSince(10, 3)]
            set
            {
                Verify();
                navigator_invoke_timer_registration_set_timer_id(handle, value);
            }
        }

        /// <summary>
        /// Retrieve the trigger type from a timer.
        /// </summary>
        [AvailableSince(10, 3)]
        public TriggerType Type
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                return navigator_invoke_timer_registration_get_type(handle);
            }
        }

        /// <summary>
        /// Get or set the request action for the timer registration request.
        /// </summary>
        [AvailableSince(10, 3)]
        public TimerAction Action
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                return navigator_invoke_timer_registration_get_action(handle);
            }
            [AvailableSince(10, 3)]
            set
            {
                Verify();
                navigator_invoke_timer_registration_set_action(handle, value);
            }
        }

        /// <summary>
        /// Get or set the specific time for the timer registration request.
        /// </summary>
        [AvailableSince(10, 3)]
        public DateTime SpecificTime
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                var res = navigator_invoke_timer_registration_get_specific_time(handle);
                if (res == IntPtr.Zero)
                {
                    Util.ThrowExceptionForLastErrno(false);
                    return DateTime.Now;
                }
                return Timer.GetSpecificTime(res).DateTime;
            }
            [AvailableSince(10, 3)]
            set
            {
                Verify();
                var ptr = Timer.CreateSpecificTime(value);
                navigator_invoke_timer_registration_set_specific_time(handle, ptr);
                Timer.navigator_invoke_specific_time_destroy(ptr);
            }
        }

        /// <summary>
        /// Get or set a recurrence rule for a timer registration request.
        /// </summary>
        [AvailableSince(10, 3)]
        public RecurrenceRule RecurrenceRule
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                var res = navigator_invoke_timer_registration_get_recurrence_rule(handle);
                if (res == IntPtr.Zero)
                {
                    Util.ThrowExceptionForLastErrno(false);
                    return null;
                }
                return new RecurrenceRule(res);
            }
            [AvailableSince(10, 3)]
            set
            {
                Verify();
                if (value == null)
                {
                    throw new ArgumentNullException("RecurrenceRule");
                }
                navigator_invoke_timer_registration_set_recurrence_rule(handle, value.DangerousGetHandle());
            }
        }

        #endregion

        private void Verify()
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException("Timer");
            }
        }

        /// <summary>
        /// Dispose of timer registration.
        /// </summary>
        [AvailableSince(10, 3)]
        public void Dispose()
        {
            if (IsValid)
            {
                if (navigator_invoke_timer_registration_destroy(handle) == BPS.BPS_SUCCESS)
                {
                    handle = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Send the timer registration to service.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 3)]
        public bool Send()
        {
            Verify();
            return navigator_invoke_timer_registration_send(handle) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Retrieve request action from a BPS event.
        /// </summary>
        /// <param name="ev">The <see cref="NavigatorEvents.InvokeTimerRegistration">InvokeTimerRegistration</see> event to query.</param>
        /// <returns>The action retrieved from the BPS event.</returns>
        [AvailableSince(10, 3)]
        public static TimerAction GetTimerRegistrationAction(BPSEvent ev)
        {
            if (ev.Domain != Navigator.Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.InvokeTimerRegistration)
            {
                throw new ArgumentException("BPSEvent is not a invoke timer registration event");
            }
            Util.GetBPSOrException();
            return navigator_invoke_timer_registration_event_get_action(ev.DangerousGetHandle());
        }
    }
}
