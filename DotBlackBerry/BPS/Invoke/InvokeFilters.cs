using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Invoke
{
    /// <summary>
    /// Target filters for invocation.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class InvokeFilters
    {
        #region PInvoke

        //XXX Should filters_count be IntPtr, UIntPtr, or is int fine?
        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_set_filters([MarshalAs(UnmanagedType.LPStr)]string id, [MarshalAs(UnmanagedType.LPStr)]string target, string[] filters, int filters_count);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_get_filters([MarshalAs(UnmanagedType.LPStr)]string id, [MarshalAs(UnmanagedType.LPStr)]string target);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_event_get_filters_target(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_event_get_filters_count(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_event_get_filter(IntPtr ev, int index);

        #endregion

        private string id;
        private string target;
        private string[] filters;

        /// <summary>
        /// Create a new set of invoke filters.
        /// </summary>
        /// <param name="target">The target key of the target filters.</param>
        /// <param name="id">The ID you want to display on the delivery receipt response.</param>
        [AvailableSince(10, 0)]
        public InvokeFilters(string target, string id)
        {
            if (string.IsNullOrWhiteSpace(target))
            {
                throw new ArgumentException("Target must have content", "target");
            }
            this.id = id;
            this.target = target;
            this.filters = new string[0];
        }

        /// <summary>
        /// Get or set the ID of the invoke filters.
        /// </summary>
        [AvailableSince(10, 0)]
        public string ID
        {
            [AvailableSince(10, 0)]
            get
            {
                return id;
            }
            [AvailableSince(10, 0)]
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Get the target of the invoke filters.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Target
        {
            [AvailableSince(10, 0)]
            get
            {
                return target;
            }
        }

        /// <summary>
        /// Get or set the invoke filters.
        /// </summary>
        [AvailableSince(10, 0)]
        public string[] Filters
        {
            [AvailableSince(10, 0)]
            get
            {
                return filters;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Filters");
                }
                filters = value;
            }
        }

        /// <summary>
        /// Sends the set target filters request to the invocation framework.
        /// </summary>
        /// <returns>true when request was sent successfully, false elsewise.</returns>
        [AvailableSince(10, 0)]
        public bool UpdateFilters()
        {
            Util.GetBPSOrException();
            return navigator_invoke_set_filters(id, target, filters, filters.Length) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Sends the get target filters invocation request to the invocation framework.
        /// </summary>
        /// <param name="target">The target key of the target whose filters are to be retrieved.</param>
        /// <param name="id">The ID you want to display on the delivery receipt response.</param>
        /// <returns>true when request was sent successfully, false elsewise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestFilters(string target, string id)
        {
            Util.GetBPSOrException();
            return navigator_invoke_get_filters(id, target) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get the requested target filters.
        /// </summary>
        /// <param name="ev">The <see cref="NavigatorEvents.InvokeGetFiltersResult">InvokeGetFiltersResult</see> event that was requested.</param>
        /// <returns>The requested target filters.</returns>
        [AvailableSince(10, 0)]
        public static InvokeFilters GetFilters(BPSEvent ev)
        {
            if (ev.Domain != Navigator.Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.InvokeGetFiltersResult)
            {
                throw new ArgumentException("BPSEvent is not a invoke get-filters event");
            }
            var ptr = ev.DangerousGetHandle();
            var id = Marshal.PtrToStringAnsi(Navigator.navigator_event_get_id(ptr));
            var target = Marshal.PtrToStringAnsi(navigator_invoke_event_get_filters_target(ptr));
            var count = navigator_invoke_event_get_filters_count(ptr);

            var filters = new string[count];
            for (var i = 0; i < count; i++)
            {
                filters[i] = Marshal.PtrToStringAnsi(navigator_invoke_event_get_filter(ptr, i));
            }

            var result = new InvokeFilters(target, id);
            result.filters = filters;
            return result;
        }
    }
}
