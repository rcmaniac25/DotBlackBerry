using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Invoke
{
    /// <summary>
    /// Invocation query result.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class InvokeQueryResult
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_event_get_query_result_action_count(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_event_get_query_result_action(IntPtr ev, int index);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_result_action_get_name(IntPtr action);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_result_action_get_icon(IntPtr action);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_result_action_get_label(IntPtr action);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_result_action_get_default_target(IntPtr action);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_query_result_action_get_target_count(IntPtr action);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_result_action_get_target(IntPtr action, int index);

        #endregion

        private IntPtr handle;
        private BPSEvent ev;

        private InvokeQueryResult(IntPtr handle, BPSEvent ev)
        {
            this.handle = handle;
            this.ev = ev;
        }

        #region Properties

        /// <summary>
        /// Get if the result is still valid and usable.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return handle != IntPtr.Zero && ev.IsValid;
            }
        }

        /// <summary>
        /// Get the name of the invocation query result.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Name
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_query_result_action_get_name(handle));
            }
        }

        /// <summary>
        /// Get the icon of the invocation query result.
        /// </summary>
        [AvailableSince(10, 0)]
        public Uri Icon
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return new Uri(Marshal.PtrToStringAnsi(navigator_invoke_query_result_action_get_icon(handle)));
            }
        }

        /// <summary>
        /// Get the label of the invocation query result.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Label
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Util.PtrToStringUTF8(navigator_invoke_query_result_action_get_label(handle));
            }
        }

        /// <summary>
        /// Get the default target of the invocation query result.
        /// </summary>
        [AvailableSince(10, 0)]
        public string DefaultTarget
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_query_result_action_get_default_target(handle));
            }
        }

        #endregion

        /// <summary>
        /// Get the targets from the result.
        /// </summary>
        /// <returns>The result targets.</returns>
        [AvailableSince(10, 0)]
        public InvokeQueryTarget[] GetTargets()
        {
            Verify();
            var count = navigator_invoke_query_result_action_get_target_count(handle);
            if (count < 0)
            {
                return null;
            }
            else if (count == 0)
            {
                return new InvokeQueryTarget[0];
            }
            //XXX Should this maybe be an enumeration? As in, someone is only looking for one particular element and don't care about the others... so we don't need to get "all of them"
            var results = new InvokeQueryTarget[count];
            for (var i = 0; i < count; i++)
            {
                results[i] = new InvokeQueryTarget(navigator_invoke_query_result_action_get_target(handle, i), ev);
            }
            return results;
        }

        private void Verify()
        {
            if (!ev.IsValid)
            {
                handle = IntPtr.Zero;
            }
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("InvokeQueryResult");
            }
        }

        /// <summary>
        /// Retrieve the results of an invocation query.
        /// </summary>
        /// <param name="ev">The <see cref="NavigatorEvents.InvokeQueryResult">InvokeQueryResult</see> event to extract the results from.</param>
        /// <returns>The invocation query results retrieved from the BPS event, or null if an error occured.</returns>
        [AvailableSince(10, 0)]
        public static InvokeQueryResult[] GetResults(BPSEvent ev)
        {
            if (ev.Domain != Navigator.Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.InvokeQueryResult)
            {
                throw new ArgumentException("BPSEvent is not a invoke query result event");
            }
            var ptr = ev.DangerousGetHandle();
            var count = navigator_invoke_event_get_query_result_action_count(ptr);
            if (count < 0)
            {
                return null;
            }
            else if (count == 0)
            {
                return new InvokeQueryResult[0];
            }
            //XXX Should this maybe be an enumeration? As in, someone is only looking for one particular element and don't care about the others... so we don't need to get "all of them"
            var results = new InvokeQueryResult[count];
            for (var i = 0; i < count; i++)
            {
                results[i] = new InvokeQueryResult(navigator_invoke_event_get_query_result_action(ptr, i), ev);
            }
            return results;
        }
    }
}
