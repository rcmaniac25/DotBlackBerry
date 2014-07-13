using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Invoke
{
    /// <summary>
    /// The frequency of the recurrence.
    /// </summary>
    [AvailableSince(10, 3)]
    public enum Frequency : int
    {
        /// <summary>
        /// Unknown frequency. Readonly.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Yearly
        /// </summary>
        /// <remarks>The yearly frequency corresponds to a calendar year. eg. A rule with a yearly frequency starting on January 1, 2014 will have its first recurrence on January 1, 2015.</remarks>
        [AvailableSince(10, 3)]
        Yearly = 1,
        /// <summary>
        /// Monthly
        /// </summary>
        /// <remarks>The monthly frequency corresponds to a calendar month. eg. A rule with a monthly frequency starting on March 20, 2015 will have its first recurrence on April 20, 2015.</remarks>
        [AvailableSince(10, 3)]
        Monthly = 2,
        /// <summary>
        /// Weekly
        /// </summary>
        [AvailableSince(10, 3)]
        Weekly = 3,
        /// <summary>
        /// Daily
        /// </summary>
        [AvailableSince(10, 3)]
        Daily = 4,
        /// <summary>
        /// Hourly
        /// </summary>
        [AvailableSince(10, 3)]
        Hourly = 5,
        /// <summary>
        /// Minutely
        /// </summary>
        [AvailableSince(10, 3)]
        Minutely = 6
    }

    /// <summary>
    /// Recurrence rule for invocations.
    /// </summary>
    [AvailableSince(10, 3)]
    public sealed class RecurrenceRule : IDisposable
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_create(out IntPtr recurrence_rule, Frequency frequency);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_destroy(IntPtr recurrence_rule);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_set_count_limit(IntPtr recurrence_rule, int count_limit);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_set_date_limit(IntPtr recurrence_rule, IntPtr date_limit);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_set_interval(IntPtr recurrence_rule, int interval);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_set_start_date(IntPtr recurrence_rule, IntPtr start_date);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_set_minutes_of_hour(IntPtr recurrence_rule, int[] minutes_of_hour, uint count);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_set_hours_of_day(IntPtr recurrence_rule, int[] hours_of_day, uint count);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_set_days_of_week(IntPtr recurrence_rule, int[] days_of_week, uint count);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_set_days_of_month(IntPtr recurrence_rule, int[] days_of_month, uint count);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_set_months_of_year(IntPtr recurrence_rule, int[] months_of_year, uint count);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_get_count_limit(IntPtr recurrence_rule);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_recurrence_rule_get_date_limit(IntPtr recurrence_rule);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_recurrence_rule_get_interval(IntPtr recurrence_rule);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_recurrence_rule_get_start_date(IntPtr recurrence_rule);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_recurrence_rule_get_minutes_of_hour(IntPtr recurrence_rule, out int count);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_recurrence_rule_get_hours_of_day(IntPtr recurrence_rule, out int count);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_recurrence_rule_get_days_of_week(IntPtr recurrence_rule, out int count);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_recurrence_rule_get_days_of_month(IntPtr recurrence_rule, out int count);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_recurrence_rule_get_months_of_year(IntPtr recurrence_rule, out int count);

        #endregion

        private IntPtr handle;

        /// <summary>
        /// Create a new instance of a recurrence rule.
        /// </summary>
        /// <param name="frequency">The frequency at which the recurrence will occur.</param>
        [AvailableSince(10, 3)]
        public RecurrenceRule(Frequency frequency)
        {
            if (!frequency.IsValidValue() || frequency == Frequency.Unknown)
            {
                throw new ArgumentException("Not a valid frequency", "frequency");
            }
            Util.GetBPSOrException();
            if (navigator_invoke_recurrence_rule_create(out handle, frequency) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            Frequency = frequency;
        }

        internal RecurrenceRule(IntPtr ptr)
        {
            handle = ptr;
            Frequency = Frequency.Unknown;
        }

        #region Properties

        /// <summary>
        /// Get the frequency of the recurrence rule.
        /// </summary>
        public Frequency Frequency { get; private set; }

        /// <summary>
        /// Get if the recurrence rule is still valid and usable.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return handle != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Get or set the count limit.
        /// </summary>
        /// <remarks>A recurrence rule can have either a count limit or a date limit. Setting a count limit deletes any previously set date limit.</remarks>
        [AvailableSince(10, 3)]
        public int? CountLimit
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                var res = navigator_invoke_recurrence_rule_get_count_limit(handle);
                if (res == BPS.BPS_FAILURE)
                {
                    //Util.ThrowExceptionForLastErrno(false);
                    return null;
                }
                return res;
            }
            [AvailableSince(10, 3)]
            set
            {
                Verify();
                if (value.HasValue)
                {
                    if (value.Value < 0)
                    {
                        throw new ArgumentOutOfRangeException("CountLimit", value.Value, "CountLimit cannot be less then zero");
                    }
                    navigator_invoke_recurrence_rule_set_count_limit(handle, value.Value);
                }
                else
                {
                    navigator_invoke_recurrence_rule_set_count_limit(handle, 0);
                }
            }
        }

        /// <summary>
        /// Get or set the date limit.
        /// </summary>
        [AvailableSince(10, 3)]
        public DateTime? DateLimit
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                var res = navigator_invoke_recurrence_rule_get_date_limit(handle);
                if (res == IntPtr.Zero)
                {
                    //Util.ThrowExceptionForLastErrno(false);
                    return null;
                }
                return Timer.GetSpecificTime(res).DateTime;
            }
            [AvailableSince(10, 3)]
            set
            {
                Verify();
                if (value.HasValue)
                {
                    var ptr = Timer.CreateSpecificTime(value.Value);
                    navigator_invoke_recurrence_rule_set_date_limit(handle, ptr);
                    Timer.navigator_invoke_specific_time_destroy(ptr);
                }
                else
                {
                    navigator_invoke_recurrence_rule_set_date_limit(handle, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// Get or set the interval of a recurrence rule.
        /// </summary>
        [AvailableSince(10, 3)]
        public int? Interval
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                var res = navigator_invoke_recurrence_rule_get_interval(handle);
                if (res == BPS.BPS_FAILURE)
                {
                    //Util.ThrowExceptionForLastErrno(false);
                    return null;
                }
                return res;
            }
            [AvailableSince(10, 3)]
            set
            {
                Verify();
                if (value.HasValue)
                {
                    if (value.Value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Interval", value.Value, "Interval cannot be less then zero");
                    }
                    navigator_invoke_recurrence_rule_set_interval(handle, value.Value);
                }
                else
                {
                    navigator_invoke_recurrence_rule_set_interval(handle, 0);
                }
            }
        }

        /// <summary>
        /// Get or set the start date.
        /// </summary>
        [AvailableSince(10, 3)]
        public DateTime StartDate
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                var res = navigator_invoke_recurrence_rule_get_start_date(handle);
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
                navigator_invoke_recurrence_rule_set_start_date(handle, ptr);
                Timer.navigator_invoke_specific_time_destroy(ptr);
            }
        }

        /// <summary>
        /// Get or set the minutes of an hour that an event should occur.
        /// </summary>
        [AvailableSince(10, 3)]
        public int[] MinutesOfHour
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                int count;
                var res = navigator_invoke_recurrence_rule_get_minutes_of_hour(handle, out count);
                if (count == BPS.BPS_FAILURE)
                {
                    return null;
                }
                return Util.ParseInt32Array(res, count);
            }
            [AvailableSince(10, 3)]
            set
            {
                VerifyArrayInput(value, "MinutesOfHour", 0, 59);
                navigator_invoke_recurrence_rule_set_minutes_of_hour(handle, value, (uint)value.Length);
            }
        }

        /// <summary>
        /// Get or set the hours in a day that an event should occur.
        /// </summary>
        [AvailableSince(10, 3)]
        public int[] HoursOfDay
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                int count;
                var res = navigator_invoke_recurrence_rule_get_hours_of_day(handle, out count);
                if (count == BPS.BPS_FAILURE)
                {
                    return null;
                }
                return Util.ParseInt32Array(res, count);
            }
            [AvailableSince(10, 3)]
            set
            {
                VerifyArrayInput(value, "HoursOfDay", 0, 23);
                navigator_invoke_recurrence_rule_set_hours_of_day(handle, value, (uint)value.Length);
            }
        }

        /// <summary>
        /// Get or set the days of the week that an event should occur.
        /// </summary>
        [AvailableSince(10, 3)]
        public int[] DaysOfWeek
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                int count;
                var res = navigator_invoke_recurrence_rule_get_days_of_week(handle, out count);
                if (count == BPS.BPS_FAILURE)
                {
                    return null;
                }
                return Util.ParseInt32Array(res, count);
            }
            [AvailableSince(10, 3)]
            set
            {
                VerifyArrayInput(value, "DaysOfWeek", 1, 7);
                navigator_invoke_recurrence_rule_set_days_of_week(handle, value, (uint)value.Length);
            }
        }

        /// <summary>
        /// Get or set the days of the month that an event should occur.
        /// </summary>
        [AvailableSince(10, 3)]
        public int[] DaysOfMonth
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                int count;
                var res = navigator_invoke_recurrence_rule_get_days_of_month(handle, out count);
                if (count == BPS.BPS_FAILURE)
                {
                    return null;
                }
                return Util.ParseInt32Array(res, count);
            }
            [AvailableSince(10, 3)]
            set
            {
                VerifyArrayInput(value, "DaysOfMonth", 1, 31);
                if (Frequency == Frequency.Weekly)
                {
                    throw new InvalidOperationException("Cannot set days of month when Frequency is set to Weekly");
                }
                navigator_invoke_recurrence_rule_set_days_of_month(handle, value, (uint)value.Length);
            }
        }

        /// <summary>
        /// Get or set the months of a year that an event should occur.
        /// </summary>
        [AvailableSince(10, 3)]
        public int[] MonthsOfYear
        {
            [AvailableSince(10, 3)]
            get
            {
                Verify();
                int count;
                var res = navigator_invoke_recurrence_rule_get_months_of_year(handle, out count);
                if (count == BPS.BPS_FAILURE)
                {
                    return null;
                }
                return Util.ParseInt32Array(res, count);
            }
            [AvailableSince(10, 3)]
            set
            {
                VerifyArrayInput(value, "MonthsOfYear", 1, 12);
                navigator_invoke_recurrence_rule_set_months_of_year(handle, value, (uint)value.Length);
            }
        }

        #endregion

        internal IntPtr DangerousGetHandle()
        {
            Verify();
            return handle;
        }

        private void Verify()
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException("RecurrenceRule");
            }
        }

        private void VerifyArrayInput(int[] value, string propertyName, int min, int max)
        {
            Verify();
            if (value == null)
            {
                throw new ArgumentNullException(propertyName);
            }
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] < min || value[i] > max)
                {
                    var comparisionWord = value[i] < min ? "greater" : "less";
                    var comparisionValue = value[i] < min ? min : max;
                    throw new ArgumentOutOfRangeException(string.Format("{0}[{1}]", propertyName, i), value[i], string.Format("{0} must be {1} than or equal to {2}", propertyName, comparisionWord, comparisionValue));
                }
            }
        }

        /// <summary>
        /// Dispose of the recurrence rule.
        /// </summary>
        [AvailableSince(10, 3)]
        public void Dispose()
        {
            if (IsValid)
            {
                if (navigator_invoke_recurrence_rule_destroy(handle) == BPS.BPS_SUCCESS)
                {
                    handle = IntPtr.Zero;
                }
            }
        }
    }
}
