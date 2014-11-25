using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Invoke
{
    /// <summary>
    /// The possible invocation query action type values.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum InvokeQueryActionType : int
    {
        /// <summary>
        /// Indicates that the query action type is unspecified.
        /// </summary>
        [AvailableSince(10, 0)]
        Unspecifier = 0,
        /// <summary>
        /// Indicates that the query results are filtered to only include those that support menu actions. Menu actions have a defined icon and label associated with them.
        /// </summary>
        [AvailableSince(10, 0)]
        Menu = 1,
        /// <summary>
        /// Indicates that the query results include all viable targets regardless of their action type(s).
        /// </summary>
        [AvailableSince(10, 0)]
        All = 2
    }

    /// <summary>
    /// Create and control queries about the invocation system.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class InvokeQuery : IDisposable
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_query_create(out IntPtr query);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_query_destroy(IntPtr query);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_query_set_id(IntPtr query, [MarshalAs(UnmanagedType.LPStr)]string id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_query_set_action(IntPtr query, [MarshalAs(UnmanagedType.LPStr)]string action);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_query_set_type(IntPtr query, [MarshalAs(UnmanagedType.LPStr)]string type);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_query_set_file_uri(IntPtr query, [MarshalAs(UnmanagedType.LPStr)]string file_uri);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_query_set_target_type_mask(IntPtr query, InvokeTargetType target_type_mask);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_query_set_action_type(IntPtr query, InvokeQueryActionType action_type);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_query_set_perimeter(IntPtr query, InvokePerimeterType perimeter);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_get_id(IntPtr query);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_get_action(IntPtr query);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_get_type(IntPtr query);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_query_get_file_uri(IntPtr query);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern InvokeTargetType navigator_invoke_query_get_target_type_mask(IntPtr query);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern InvokeQueryActionType navigator_invoke_query_get_action_type(IntPtr query);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern InvokePerimeterType navigator_invoke_query_get_perimeter(IntPtr query);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_query_send(IntPtr query);

        #endregion

        private IntPtr handle;

        /// <summary>
        /// Create an invoke query.
        /// </summary>
        [AvailableSince(10, 0)]
        public InvokeQuery()
        {
            Util.GetBPSOrException();
            if (navigator_invoke_query_create(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        /// <summary>
        /// Finalize InvokeQuery instance.
        /// </summary>
        ~InvokeQuery()
        {
            Dispose(false);
        }

        #region Properties

        /// <summary>
        /// Get if the query is still valid and usable.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return handle != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Get or set the ID of the query.
        /// </summary>
        [AvailableSince(10, 0)]
        public string ID
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_query_get_id(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                navigator_invoke_query_set_id(handle, value);
            }
        }

        /// <summary>
        /// Get or set the action of the query.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Action
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_query_get_action(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                navigator_invoke_query_set_action(handle, value);
            }
        }

        /// <summary>
        /// Get or set the type of the query.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Type
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_query_get_type(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                navigator_invoke_query_set_type(handle, value);
            }
        }

        /// <summary>
        /// Get or set the URI of the query.
        /// </summary>
        [AvailableSince(10, 0)]
        public Uri FileUri
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return new Uri(Marshal.PtrToStringAnsi(navigator_invoke_query_get_file_uri(handle)));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                navigator_invoke_query_set_file_uri(handle, value.ToString());
            }
        }

        /// <summary>
        /// Get or set the target type mask of the query.
        /// </summary>
        [AvailableSince(10, 0)]
        public InvokeTargetType TargetType
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return navigator_invoke_query_get_target_type_mask(handle);
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                navigator_invoke_query_set_target_type_mask(handle, value);
            }
        }

        /// <summary>
        /// Get or set the action type of the query.
        /// </summary>
        [AvailableSince(10, 0)]
        public InvokeQueryActionType ActionType
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return navigator_invoke_query_get_action_type(handle);
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                navigator_invoke_query_set_action_type(handle, value);
            }
        }

#if BLACKBERRY_INTERNAL_FUNCTIONS
        /// <summary>
        /// Get or set the perimeter of the query.
        /// </summary>
#else
        /// <summary>
        /// Get the perimeter from the query.
        /// </summary>
#endif
        [AvailableSince(10, 0)]
        public InvokePerimeterType Perimeter
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return navigator_invoke_query_get_perimeter(handle);
            }
#if BLACKBERRY_INTERNAL_FUNCTIONS
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                navigator_invoke_query_set_perimeter(handle, value);
            }
#endif
        }

        #endregion

        /// <summary>
        /// Dispose InvokeQuery.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            Verify();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (handle != IntPtr.Zero)
            {
                navigator_invoke_query_destroy(handle);
                handle = IntPtr.Zero;
            }
        }

        private void Verify()
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("InvokeQuery");
            }
        }

        /// <summary>
        /// Send the query request to the invocation framework.
        /// </summary>
        /// <returns>true for success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool Send()
        {
            Verify();
            return navigator_invoke_query_send(handle) == BPS.BPS_SUCCESS;
        }
    }
}
