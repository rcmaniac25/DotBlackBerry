using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Possible battery charger states.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum ChargerState : int
    {
        /// <summary>
        /// A battery-related system error.
        /// </summary>
        [AvailableSince(10, 0)]
        Error = 0,
        /// <summary>
        /// The charger is invalid or weak.
        /// </summary>
        [AvailableSince(10, 0)]
        Bad = 1,
        /// <summary>
        /// No charger is present.
        /// </summary>
        [AvailableSince(10, 0)]
        None = 2,
        /// <summary>
        /// The charger is connected, but not charging since the battery is fully charged.
        /// </summary>
        [AvailableSince(10, 0)]
        PluggedInNotCharging = 3,
        /// <summary>
        /// The charger is connected and the battery is being charged.
        /// </summary>
        [AvailableSince(10, 0)]
        Charging = 4
    }

    /// <summary>
    /// Possible battery charging states.
    /// </summary>
    [AvailableSince(10, 2)]
    public enum ChargingState : int
    {
        /// <summary>
        /// Not charging.
        /// </summary>
        [AvailableSince(10, 2)]
        NotCharging = 0,
        /// <summary>
        /// Trickle charging.
        /// </summary>
        [AvailableSince(10, 2)]
        TrickleCharging = 1,
        /// <summary>
        /// Charging with constant current.
        /// </summary>
        [AvailableSince(10, 2)]
        ConstantCurrent = 2,
        /// <summary>
        /// Charging with constant voltage.
        /// </summary>
        [AvailableSince(10, 2)]
        ConstantVoltage = 3,
        /// <summary>
        /// Done charging.
        /// </summary>
        [AvailableSince(10, 2)]
        Done = 4
    }

    /// <summary>
    /// Battery and power related info.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class BatteryInfo : IDisposable
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int battery_request_events(int flags);

        [DllImport("bps")]
        private static extern int battery_stop_events(int flags);

        [DllImport("bps")]
        private static extern int battery_get_domain();

        [DllImport("bps")]
        private static extern IntPtr battery_event_get_info(IntPtr ev);

        [DllImport("bps")]
        private static extern int battery_get_info(out IntPtr info);

        [DllImport("bps")]
        private static extern void battery_free_info([In, Out]ref IntPtr info);

        [DllImport("bps")]
        private static extern bool battery_info_is_battery_ready(IntPtr info);

        [DllImport("bps")]
        private static extern bool battery_info_is_battery_present(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_id(IntPtr info);

        [DllImport("bps")]
        private static extern bool battery_info_is_battery_ok(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_state_of_charge(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_state_of_health(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_time_to_empty(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_time_to_full(IntPtr info);

        [DllImport("bps")]
        private static extern IntPtr battery_info_get_battery_name(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_voltage(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_available_energy(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_average_current(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_average_power(IntPtr info);

        [DllImport("bps")]
        private static extern bool battery_info_get_battery_alert(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_cycle_count(IntPtr info);

        [DllImport("bps")]
        private static extern float battery_info_get_battery_temperature(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_design_capacity(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_full_available_capacity(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_full_charge_capacity(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_max_load_current(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_max_load_time_to_empty(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_nominal_available_capacity(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_battery_time_to_empty_at_constant_power(IntPtr info);

        [DllImport("bps")]
        private static extern bool battery_info_is_charger_ready(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_charger_info(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_charger_max_input_current(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_charger_max_charge_current(IntPtr info);

        [DllImport("bps")]
        private static extern IntPtr battery_info_get_charger_name(IntPtr info);

        [DllImport("bps")]
        private static extern bool battery_info_is_system_ready(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_system_voltage(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_system_input_current_monitor(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_system_charging_state(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_system_max_voltage(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_system_min_voltage(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_system_charge_current(IntPtr info);

        [DllImport("bps")]
        private static extern IntPtr battery_info_get_device_name(IntPtr info);

        [DllImport("bps")]
        private static extern int battery_info_get_version(IntPtr info);

        #endregion

        private const int BATTERY_INFO = 1;

        private const int BATTERY_TIME_NA = 65535;
        private const int BATTERY_INVALID_VALUE = 80000000;

        private IntPtr handle;
        private bool isDisposable;

        /// <summary>
        /// Retrieve the current the battery information.
        /// </summary>
        [AvailableSince(10, 0)]
        public BatteryInfo()
            : this(IntPtr.Zero, true)
        {
            Util.GetBPSOrException();
            if (battery_get_info(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        private BatteryInfo(IntPtr ptr, bool isDisposable)
        {
            this.handle = ptr;
            this.isDisposable = isDisposable;
        }

        #region Inner Classes

        #region BatteryData

        /// <summary>
        /// Battery specific data.
        /// </summary>
        [AvailableSince(10, 0)]
        public sealed class BatteryData
        {
            private BatteryInfo info;

            internal BatteryData(BatteryInfo info)
            {
                this.info = info;
            }

            /// <summary>
            /// Determine whether the battery is ready. If this returns false, all other battery properties will fail.
            /// </summary>
            [AvailableSince(10, 0)]
            public bool IsReady
            {
                [AvailableSince(10, 0)]
                get
                {
                    if (info.handle == IntPtr.Zero)
                    {
                        throw new ObjectDisposedException("BatteryInfo");
                    }
                    return battery_info_is_battery_ready(info.handle);
                }
            }

            /// <summary>
            /// Determine whether the battery is present.
            /// </summary>
            [AvailableSince(10, 0)]
            public bool IsPresent
            {
                [AvailableSince(10, 0)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    return battery_info_is_battery_present(info.handle);
                }
            }

            /// <summary>
            /// Retrieve the battery ID.
            /// </summary>
            [AvailableSince(10, 0)]
            public int ID
            {
                [AvailableSince(10, 0)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    return battery_info_get_battery_id(info.handle);
                }
            }

            /// <summary>
            /// Retrieve status of the battery.
            /// </summary>
            [AvailableSince(10, 0)]
            public bool IsOk
            {
                [AvailableSince(10, 0)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    return battery_info_is_battery_ok(info.handle);
                }
            }

            /// <summary>
            /// Retrieve the state of charge from a battery information structure.
            /// </summary>
            [AvailableSince(10, 0)]
            public int ChargePercentage
            {
                [AvailableSince(10, 0)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    return battery_info_get_state_of_charge(info.handle);
                }
            }

            /// <summary>
            /// Retrieve the state of health from a battery information structure.
            /// </summary>
            [AvailableSince(10, 0)]
            public int HealthPrecentage
            {
                [AvailableSince(10, 0)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    return battery_info_get_state_of_health(info.handle);
                }
            }

            /// <summary>
            /// Retrieve the amount of time before the battery is fully discharged (empty).
            /// </summary>
            [AvailableSince(10, 0)]
            public Nullable<TimeSpan> TimeToEmpty
            {
                [AvailableSince(10, 0)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var time = battery_info_get_time_to_empty(info.handle);
                    if (time == BPS.BPS_FAILURE)
                    {
                        throw new InvalidOperationException("An error occured when retrieving time-to-empty.");
                    }
                    if (time == BATTERY_TIME_NA)
                    {
                        return null;
                    }
                    return TimeSpan.FromMinutes(time);
                }
            }

            /// <summary>
            /// Retrieve the amount of time to fully charge the battery.
            /// </summary>
            [AvailableSince(10, 0)]
            public Nullable<TimeSpan> TimeToFull
            {
                [AvailableSince(10, 0)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var time = battery_info_get_time_to_full(info.handle);
                    if (time == BPS.BPS_FAILURE)
                    {
                        throw new InvalidOperationException("An error occured when retrieving time-to-full.");
                    }
                    if (time == BATTERY_TIME_NA)
                    {
                        return null;
                    }
                    return TimeSpan.FromMinutes(time);
                }
            }

            /// <summary>
            /// Retrieve the battery name.
            /// </summary>
            [AvailableSince(10, 2)]
            public string Name
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    return Marshal.PtrToStringAnsi(battery_info_get_battery_name(info.handle));
                }
            }

            /// <summary>
            /// Retrieve the battery voltage.
            /// </summary>
            [AvailableSince(10, 2)]
            public int Voltage
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var result = battery_info_get_battery_voltage(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the available energy remaining in the battery (mWh).
            /// </summary>
            [AvailableSince(10, 2)]
            public int AvailableEnergy
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var result = battery_info_get_battery_available_energy(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the battery average current (mA).
            /// </summary>
            [AvailableSince(10, 2)]
            public int AverageCurrent
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var result = battery_info_get_battery_average_current(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the battery average power (mW).
            /// </summary>
            [AvailableSince(10, 2)]
            public int AveragePower
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var result = battery_info_get_battery_average_power(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Determine whether the battery alert is triggered.
            /// </summary>
            [AvailableSince(10, 2)]
            public bool IsAlert
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    return battery_info_get_battery_alert(info.handle);
                }
            }

            /// <summary>
            /// Retrieve the battery cycle count.
            /// </summary>
            [AvailableSince(10, 2)]
            public int CycleCount
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var result = battery_info_get_battery_cycle_count(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the battery temperature (Celsius).
            /// </summary>
            [AvailableSince(10, 2)]
            public float Temperature
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var result = battery_info_get_battery_temperature(info.handle);
                    if (float.IsNaN(result))
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the battery design capacity (mAh).
            /// </summary>
            [AvailableSince(10, 2)]
            public int DesignCapacity
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var result = battery_info_get_battery_design_capacity(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the battery full available capacity (mAh).
            /// </summary>
            [AvailableSince(10, 2)]
            public int FullAvailableCapacity
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var result = battery_info_get_battery_full_available_capacity(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the battery full charge capacity (mAh).
            /// </summary>
            [AvailableSince(10, 2)]
            public int FullChargeCapacity
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var result = battery_info_get_battery_full_charge_capacity(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the battery maximum load current (mA).
            /// </summary>
            [AvailableSince(10, 2)]
            public int MaxLoadCurrent
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var result = battery_info_get_battery_max_load_current(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the battery maximum load time to empty.
            /// </summary>
            [AvailableSince(10, 2)]
            public Nullable<TimeSpan> TimeToEmptyAtMaxLoad
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var time = battery_info_get_battery_max_load_time_to_empty(info.handle);
                    if (time == BATTERY_INVALID_VALUE)
                    {
                        return null;
                    }
                    return TimeSpan.FromMinutes(time);
                }
            }

            /// <summary>
            /// Retrieve the battery nominal available capacity (mAh).
            /// </summary>
            [AvailableSince(10, 2)]
            public int NominalAvailableCapacity
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var result = battery_info_get_battery_nominal_available_capacity(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the battery time to empty at constant power.
            /// </summary>
            [AvailableSince(10, 2)]
            public Nullable<TimeSpan> TimeToEmptyAtConstantPower
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Battery info is is not ready.");
                    }
                    var time = battery_info_get_battery_time_to_empty_at_constant_power(info.handle);
                    if (time == BATTERY_INVALID_VALUE)
                    {
                        return null;
                    }
                    return TimeSpan.FromMinutes(time);
                }
            }
        }

        #endregion

        #region ChargerData

        /// <summary>
        /// Charger specific data.
        /// </summary>
        [AvailableSince(10, 0)]
        public sealed class ChargerData
        {
            private BatteryInfo info;

            internal ChargerData(BatteryInfo info)
            {
                this.info = info;
            }

            /// <summary>
            /// Determine whether the charger is ready. If this returns false, all other charger properties will fail.
            /// </summary>
            [AvailableSince(10, 2)]
            public bool IsReady
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (info.handle == IntPtr.Zero)
                    {
                        throw new ObjectDisposedException("BatteryInfo");
                    }
                    return battery_info_is_charger_ready(info.handle);
                }
            }

            /// <summary>
            /// Retrieve the state of the charger.
            /// </summary>
            [AvailableSince(10, 0)]
            public ChargerState State
            {
                [AvailableSince(10, 0)]
                get
                {
                    /*if (!IsReady)
                    {
                        throw new InvalidOperationException("Charger info is is not ready.");
                    }*/
                    var result = battery_info_get_charger_info(info.handle);
                    if (result == BPS.BPS_FAILURE)
                    {
                        throw new NotSupportedException();
                    }
                    return (ChargerState)result;
                }
            }

            /// <summary>
            /// Retrieve the charger maximum input current (mA).
            /// </summary>
            [AvailableSince(10, 2)]
            public int MaxInputCurrent
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Charger info is is not ready.");
                    }
                    var result = battery_info_get_charger_max_input_current(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the charger maximum charge current (mA).
            /// </summary>
            [AvailableSince(10, 2)]
            public int MaxChargeCurrent
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Charger info is is not ready.");
                    }
                    var result = battery_info_get_charger_max_charge_current(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the charger name.
            /// </summary>
            [AvailableSince(10, 2)]
            public string Name
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("Charger info is is not ready.");
                    }
                    return Marshal.PtrToStringAnsi(battery_info_get_charger_name(info.handle));
                }
            }
        }

        #endregion

        #region SystemData

        /// <summary>
        /// System specific data.
        /// </summary>
        [AvailableSince(10, 2)]
        public sealed class SystemData
        {
            private BatteryInfo info;

            internal SystemData(BatteryInfo info)
            {
                this.info = info;
            }

            /// <summary>
            /// Determine whether the system is ready. If this returns false, all other system properties will fail.
            /// </summary>
            [AvailableSince(10, 2)]
            public bool IsReady
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (info.handle == IntPtr.Zero)
                    {
                        throw new ObjectDisposedException("BatteryInfo");
                    }
                    return battery_info_is_system_ready(info.handle);
                }
            }

            /// <summary>
            /// Retrieve the system voltage (mV).
            /// </summary>
            [AvailableSince(10, 2)]
            public int Voltage
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("System info is is not ready.");
                    }
                    var result = battery_info_get_system_voltage(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the system input current monitor (mA).
            /// </summary>
            [AvailableSince(10, 2)]
            public int InputCurrentMonitor
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("System info is is not ready.");
                    }
                    var result = battery_info_get_system_input_current_monitor(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the system charging state.
            /// </summary>
            [AvailableSince(10, 2)]
            public ChargingState ChargingState
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("System info is is not ready.");
                    }
                    var result = battery_info_get_system_charging_state(info.handle);
                    if (result == BPS.BPS_FAILURE)
                    {
                        throw new NotSupportedException();
                    }
                    return (ChargingState)result;
                }
            }

            /// <summary>
            /// Retrieve the system maximum voltage (mV).
            /// </summary>
            [AvailableSince(10, 2)]
            public int MaxVoltage
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("System info is is not ready.");
                    }
                    var result = battery_info_get_system_max_voltage(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the system minimum voltage (mV).
            /// </summary>
            [AvailableSince(10, 2)]
            public int MinVoltage
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("System info is is not ready.");
                    }
                    var result = battery_info_get_system_min_voltage(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }

            /// <summary>
            /// Retrieve the system charge current (mA).
            /// </summary>
            [AvailableSince(10, 2)]
            public int CurrentCharge
            {
                [AvailableSince(10, 2)]
                get
                {
                    if (!IsReady)
                    {
                        throw new InvalidOperationException("System info is is not ready.");
                    }
                    var result = battery_info_get_system_charge_current(info.handle);
                    if (result == BATTERY_INVALID_VALUE)
                    {
                        throw new NotSupportedException();
                    }
                    return result;
                }
            }
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Get battery specific data.
        /// </summary>
        [AvailableSince(10, 0)]
        public BatteryData Battery
        {
            [AvailableSince(10, 0)]
            get
            {
                return new BatteryData(this);
            }
        }

        /// <summary>
        /// Get charger specific data.
        /// </summary>
        [AvailableSince(10, 0)]
        public ChargerData Charger
        {
            [AvailableSince(10, 0)]
            get
            {
                return new ChargerData(this);
            }
        }

        /// <summary>
        /// Get system specific data.
        /// </summary>
        [AvailableSince(10, 2)]
        public SystemData System
        {
            [AvailableSince(10, 2)]
            get
            {
                Util.ThrowIfUnsupported(10, 2);
                return new SystemData(this);
            }
        }

        /// <summary>
        /// Retrieve the device name.
        /// </summary>
        [AvailableSince(10, 0)]
        public string DeviceName
        {
            [AvailableSince(10, 0)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("BatteryInfo");
                }
                return Marshal.PtrToStringAnsi(battery_info_get_device_name(handle));
            }
        }

        /// <summary>
        /// Retrieve the Battery API version.
        /// </summary>
        [AvailableSince(10, 0)]
        public int Version
        {
            [AvailableSince(10, 0)]
            get
            {
                if (handle == IntPtr.Zero)
                {
                    throw new ObjectDisposedException("BatteryInfo");
                }
                return battery_info_get_version(handle);
            }
        }

        #endregion

        /// <summary>
        /// Dispose of the battery info.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (!isDisposable)
            {
                throw new InvalidOperationException("BatteryInfo cannot be disposed directly");
            }
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("BatteryInfo");
            }
            battery_free_info(ref handle);
            handle = IntPtr.Zero;
        }

        #region BPS

        /// <summary>
        /// Retrieve the unique domain ID for the battery service.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int Domain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return battery_get_domain();
            }
        }

        /// <summary>
        /// Start receiving battery events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestEvents()
        {
            Util.GetBPSOrException();
            return battery_request_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving battery events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopEvents()
        {
            Util.GetBPSOrException();
            return battery_stop_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get battery state from a BPS Event.
        /// </summary>
        /// <param name="ev">A BPSEvent to convert to BatteryInfo.</param>
        /// <returns>The BatteryInfo for the event.</returns>
        [AvailableSince(10, 0)]
        public static BatteryInfo GetBatteryInfoForEvent(BPSEvent ev)
        {
            if (ev.Domain != BatteryInfo.Domain)
            {
                throw new ArgumentException("BPSEvent is not a battery info event");
            }
            var code = ev.Code;
            if (code != BATTERY_INFO)
            {
                throw new ArgumentException("BPSEvent is an unknown battery info event");
            }
            return new BatteryInfo(ev.DangerousGetHandle(), false);
        }

        #endregion
    }
}
