using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Invoke
{
    /// <summary>
    /// Various utility functions for invocation events.
    /// </summary>
    [AvailableSince(10, 0)]
    public static class InvokeUtility
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_event_get_target(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_event_get_target_type(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern long navigator_invoke_event_get_group_id(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_event_get_error_code(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_event_get_dname(IntPtr ev);

        #endregion

        private static void CheckSupportedEvent(BPSEvent ev, params NavigatorEvents[] types)
        {
            if (ev.Domain != Navigator.Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            var eventType = (NavigatorEvents)ev.Code;
            foreach (var t in types)
            {
                if (t == eventType)
                {
                    return;
                }
            }
            throw new ArgumentException("BPSEvent is not a supported event");
        }

        /// <summary>
        /// Get the key of an invoked target from the BPS event.
        /// </summary>
        /// <param name="ev">The <see cref="NavigatorEvents.InvokeTargetResult">InvokeTargetResult</see> event to get the target key from.</param>
        /// <returns>The key of the target that was invoked, otherwise null on error.</returns>
        [AvailableSince(10, 0)]
        public static string TargetKey(BPSEvent ev)
        {
            CheckSupportedEvent(ev, NavigatorEvents.InvokeTargetResult);
            return Marshal.PtrToStringAnsi(navigator_invoke_event_get_target(ev.DangerousGetHandle()));
        }

        /// <summary>
        /// Get the type of an invoked target from the BPS event.
        /// </summary>
        /// <param name="ev">The <see cref="NavigatorEvents.InvokeTargetResult">InvokeTargetResult</see> event to get the target type from.</param>
        /// <returns>The target type of the invoked target.</returns>
        [AvailableSince(10, 0)]
        public static InvokeTargetType TargetType(BPSEvent ev)
        {
            CheckSupportedEvent(ev, NavigatorEvents.InvokeTargetResult);
            var result = navigator_invoke_event_get_target_type(ev.DangerousGetHandle());
            if (result == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return (InvokeTargetType)result;
        }

        /// <summary>
        /// Get the group ID of an invocation source application from the BPS event.
        /// </summary>
        /// <param name="ev">The <see cref="NavigatorEvents.InvokeTarget">InvokeTarget</see> event to get the invocation source group ID.</param>
        /// <returns></returns>
        [AvailableSince(10, 0)]
        public static long InvokeSourceGroupID(BPSEvent ev)
        {
            CheckSupportedEvent(ev, NavigatorEvents.InvokeTarget);
            var result = navigator_invoke_event_get_group_id(ev.DangerousGetHandle());
            if (result == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return result;
        }

        /// <summary>
        /// Get the error code associated with an invoke request.
        /// </summary>
        /// <param name="ev">The event associated with the invoke request whose result has an error code set.</param>
        /// <returns>The error code of the request.</returns>
        [AvailableSince(10, 2)]
        public static int ErrorCode(BPSEvent ev)
        {
            CheckSupportedEvent(ev, NavigatorEvents.InvokeTargetResult, NavigatorEvents.InvokeSetFiltersResult, NavigatorEvents.InvokeQueryResult);
            var result = navigator_invoke_event_get_error_code(ev.DangerousGetHandle());
            if (result == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return result;
        }

        /// <summary>
        /// Get the package ID (dname) of an invocation source application from the BPS event.
        /// </summary>
        /// <param name="ev">The <see cref="NavigatorEvents.InvokeTarget">InvokeTarget</see> event to get the invocation source package ID (known as a dname).</param>
        /// <returns>The package ID (dname) of the invocation source application.</returns>
        [AvailableSince(10, 0)]
        public static string InvokeSourcePackageID(BPSEvent ev)
        {
            CheckSupportedEvent(ev, NavigatorEvents.InvokeTarget);
            return Marshal.PtrToStringAnsi(navigator_invoke_event_get_dname(ev.DangerousGetHandle()));
        }
    }
}
