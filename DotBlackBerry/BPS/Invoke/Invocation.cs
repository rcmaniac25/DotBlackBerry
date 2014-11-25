using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Invoke
{
    /// <summary>
    /// The possible invocation target types.
    /// </summary>
    [AvailableSince(10, 0), Flags]
    public enum InvokeTargetType : int
    {
        /// <summary>
        /// Indicates that the target type is unspecified.
        /// </summary>
        [AvailableSince(10, 0)]
        Unspecified = 0x00,
        /// <summary>
        /// Indicates that the target is an application. Applications are software designed to perform specific tasks.
        /// </summary>
        [AvailableSince(10, 0)]
        Application = 0x01,
        /// <summary>
        /// Indicates that the target is a card.
        /// </summary>
        [AvailableSince(10, 0)]
        Card = 0x02,

        // There is also type viewer, but it's not expected that .BlackBerry will run on an OS that uses it

        /// <summary>
        /// Indicates that the target is a service. The meaning of a service value is reserved for future use.
        /// </summary>
        [AvailableSince(10, 0)]
        Service = 0x08,
        /// <summary>
        /// Indicates that the matched targets belonging to the current application should be returned in the query result.
        /// </summary>
        [AvailableSince(10, 0)]
        Self = 0x10,
        /// <summary>
        /// Indicates that the target is a headless application.
        /// </summary>
        [AvailableSince(10, 3)]
        Headless = 0x40
    }

    /// <summary>
    /// The possible application perimeter type values.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum InvokePerimeterType : int
    {
        /// <summary>
        /// Indicates that the perimeter type is unspecified.
        /// </summary>
        [AvailableSince(10, 0)]
        Unspecified = 0,
        /// <summary>
        /// Indicates that the application should run in the personal perimeter.
        /// </summary>
        [AvailableSince(10, 0)]
        Personal = 1,
        /// <summary>
        /// Indicates that the application should run in the enterprise perimeter.
        /// </summary>
        [AvailableSince(10, 0)]
        Enterprise = 2
    }

    /// <summary>
    /// The possible transfer modes for files specified in invocation requests.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum InvokeFileTransferMode : int
    {
        /// <summary>
        /// Indicates that the file transfer mode has not been specified and the default logic should apply.
        /// </summary>
        [AvailableSince(10, 0)]
        Unspecified = 0,
        /// <summary>
        /// Indicates that the file transfer handling should be skipped and the specified file URI should be passed to the target as-is.
        /// </summary>
        [AvailableSince(10, 0)]
        Preserve = 1,
        /// <summary>
        /// Indicates that the file should be transfered as a read only copy of the file specified in the URI attribute.
        /// </summary>
        [AvailableSince(10, 0)]
        CopyReadOnly = 2,
        /// <summary>
        /// Indicates that the file should be transfered as a read/write copy of the file specified in the URI attribute.
        /// </summary>
        [AvailableSince(10, 0)]
        CopyReadWrite = 3,
        /// <summary>
        /// Indicates that the file should be transfered as a link to the file specified in the URI attribute.
        /// </summary>
        [AvailableSince(10, 0)]
        Link = 4
    }

    /// <summary>
    /// The possible directions of movement for an invoke list cursor.
    /// </summary>
    [AvailableSince(10, 2)]
    public enum InvokeListCursorDirection : int
    {
        /// <summary>
        /// Indicates that the cursor's direction is determined by the application.
        /// </summary>
        [AvailableSince(10, 2)]
        Unspecified = 0,
        /// <summary>
        /// Indicates that the cursor's direction is towards the next list item.
        /// </summary>
        [AvailableSince(10, 2)]
        Next = 1,
        /// <summary>
        /// Indicates that the cursor's direction is towards the previous list item.
        /// </summary>
        [AvailableSince(10, 2)]
        Previous = 2
    }

    /// <summary>
    /// Device and application invocation.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class Invocation : IDisposable
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_create(out IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_destroy(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_id(IntPtr invocation, [MarshalAs(UnmanagedType.LPStr)]string id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_target(IntPtr invocation, [MarshalAs(UnmanagedType.LPStr)]string target);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_source(IntPtr invocation, [MarshalAs(UnmanagedType.LPStr)]string source);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_action(IntPtr invocation, [MarshalAs(UnmanagedType.LPStr)]string action);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_type(IntPtr invocation, [MarshalAs(UnmanagedType.LPStr)]string type);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_uri(IntPtr invocation, [MarshalAs(UnmanagedType.LPStr)]string uri);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_file_transfer_mode(IntPtr invocation, InvokeFileTransferMode transfer_mode);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_data(IntPtr invocation, IntPtr data, int data_length);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_perimeter(IntPtr invocation, InvokePerimeterType perimeter);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_metadata(IntPtr invocation, [MarshalAs(UnmanagedType.LPStr)]string metadata);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_target_type_mask(IntPtr invocation, InvokeTargetType target_type_mask);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_set_list_id(IntPtr invocation, int list_id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_invocation_get_id(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_invocation_get_target(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_invocation_get_source(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_invocation_get_action(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_invocation_get_type(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_invocation_get_uri(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_get_file_transfer_mode(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_get_data_length(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_invocation_get_data(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern InvokePerimeterType navigator_invoke_invocation_get_perimeter(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_invocation_get_metadata(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_get_list_id(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern InvokeTargetType navigator_invoke_invocation_get_target_type_mask(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_event_get_invocation(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_invocation_send(IntPtr invocation);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_uri_to_local_path([MarshalAs(UnmanagedType.LPStr)]string uri);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_invoke_local_path_to_uri([MarshalAs(UnmanagedType.LPStr)]string path);

        #endregion

        private IntPtr handle;
        private BPSEvent ev;

        /// <summary>
        /// Create a new invocation.
        /// </summary>
        [AvailableSince(10, 0)]
        public Invocation()
        {
            Util.GetBPSOrException();
            if (navigator_invoke_invocation_create(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            ev = null;
            IsDisposable = true;
        }

        private Invocation(IntPtr handle, BPSEvent ev)
        {
            this.handle = handle;
            this.ev = ev;
            IsDisposable = false;
        }

        #region Properties

        /// <summary>
        /// Get if the invocation is still valid and usable.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (handle != IntPtr.Zero && ((ev == null) ^ ev.IsValid))
                {
                    return navigator_invoke_invocation_get_target(handle) != IntPtr.Zero ||
                        navigator_invoke_invocation_get_action(handle) != IntPtr.Zero ||
                        navigator_invoke_invocation_get_type(handle) != IntPtr.Zero;
                }
                return false;
            }
        }

        /// <summary>
        /// Get if the invocation can, and should, be destroyed when no longer needed.
        /// </summary>
        public bool IsDisposable { get; private set; }

        /// <summary>
        /// Get or set the ID of the invocation.
        /// </summary>
        [AvailableSince(10, 0)]
        public string ID
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_invocation_get_id(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify("ID");
                navigator_invoke_invocation_set_id(handle, value);
            }
        }

        /// <summary>
        /// Get or set the target of the invocation.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Target
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_invocation_get_target(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                //XXX Should the format be checked? Instinct says no...
                Verify("Target");
                navigator_invoke_invocation_set_target(handle, value);
            }
        }

        /// <summary>
        /// Get or set the source from the invocation.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Source
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_invocation_get_source(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify("Source");
                navigator_invoke_invocation_set_source(handle, value);
            }
        }

        /// <summary>
        /// Get or set the action from the invocation.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Action
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_invocation_get_action(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                //XXX Should the format be checked? Instinct says no...
                Verify("Action");
                navigator_invoke_invocation_set_action(handle, value);
            }
        }

        //TODO: Make sure common Actions have an enum
        //-make sure bb.action.system.TIMER_FIRED is on there (when timer fires)

        /// <summary>
        /// Get or set the type of the invocation.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Type
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_invocation_get_type(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify("Type");
                navigator_invoke_invocation_set_type(handle, value);
            }
        }

        /// <summary>
        /// Get or set the URI of the invocation.
        /// </summary>
        [AvailableSince(10, 0)]
        public Uri Uri
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return new Uri(Marshal.PtrToStringAnsi(navigator_invoke_invocation_get_type(handle)));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify("Uri");
                navigator_invoke_invocation_set_uri(handle, value.ToString());
            }
        }

        /// <summary>
        /// Get or set the file transfer mode of the invocation.
        /// </summary>
        [AvailableSince(10, 0)]
        public InvokeFileTransferMode FileTransferMode
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                var ret = navigator_invoke_invocation_get_file_transfer_mode(handle);
                if (ret == BPS.BPS_FAILURE)
                {
                    Util.GetExceptionForLastErrno();
                }
                return (InvokeFileTransferMode)ret;
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify("FileTransferMode");
                navigator_invoke_invocation_set_file_transfer_mode(handle, value);
            }
        }

        /// <summary>
        /// Get or set the arbitrary data of the invocation.
        /// </summary>
        [AvailableSince(10, 0)]
        public object Data
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                var size = navigator_invoke_invocation_get_data_length(handle);
                if (size <= 0)
                {
                    return null;
                }
                return Util.RawDeserializeFromPointer(navigator_invoke_invocation_get_data(handle), (uint)size);
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify("Data");
                uint size;
                var ptr = Util.RawSerializeToPointer(value, out size);
                try
                {
                    navigator_invoke_invocation_set_data(handle, ptr, (int)size);
                }
                finally
                {
                    // Precaution
                    Util.FreeRawSerializePointer(ptr);
                }
            }
        }
        
#if BLACKBERRY_INTERNAL_FUNCTIONS
        /// <summary>
        /// Get or set the perimeter of the invocation.
        /// </summary>
#else
        /// <summary>
        /// Get the perimeter from the invocation.
        /// </summary>
#endif
        [AvailableSince(10, 0)]
        public InvokePerimeterType Perimeter
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return navigator_invoke_invocation_get_perimeter(handle);
            }
#if BLACKBERRY_INTERNAL_FUNCTIONS
            [AvailableSince(10, 0)]
            set
            {
                Verify("Perimeter");
                navigator_invoke_invocation_set_perimeter(handle, value);
            }
#endif
        }

        /// <summary>
        /// Get or set the metadata with which the application should be invoked.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Metadata
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(navigator_invoke_invocation_get_metadata(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify("Metadata");
                navigator_invoke_invocation_set_metadata(handle, value);
            }
        }

        /// <summary>
        /// Get or set the target type mask of an invocation.
        /// </summary>
        [AvailableSince(10, 0)]
        public InvokeTargetType TargetType
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return navigator_invoke_invocation_get_target_type_mask(handle);
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify("TargetType");
                navigator_invoke_invocation_set_target_type_mask(handle, value);
            }
        }

        /// <summary>
        /// Get or set the list id to associate with the invocation.
        /// </summary>
        [AvailableSince(10, 2)]
        public int? ListID
        {
            [AvailableSince(10, 2)]
            get
            {
                Verify();
                var res = navigator_invoke_invocation_get_list_id(handle);
                if (res == 0)
                {
                    return null;
                }
                return res;
            }
            [AvailableSince(10, 2)]
            set
            {
                Verify("ListID");
                if (value.HasValue && value.Value == 0)
                {
                    throw new ArgumentException("List ID cannot be zero.");
                }
                navigator_invoke_invocation_set_list_id(handle, value.HasValue ? value.Value : 0);
            }
        }

        #endregion

        /// <summary>
        /// Request an invocation to a target.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool Send()
        {
            Verify();
            return navigator_invoke_invocation_send(handle) == BPS.BPS_SUCCESS;
        }

        private void Verify(string setter = null)
        {
            if (handle == IntPtr.Zero || (ev != null && !ev.IsValid))
            {
                throw new ObjectDisposedException("Invocation");
            }
            else if (setter != null && ev != null)
            {
                throw new InvalidOperationException(string.Format("Cannot set {0} on invocation retrieved from BPS event.", setter));
            }
        }

        /// <summary>
        /// Disposes the Invocation.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                if (!IsDisposable)
                {
                    throw new InvalidOperationException("Cannot delete an invocation retrieved from an event.");
                }
                if (navigator_invoke_invocation_destroy(handle) == BPS.BPS_SUCCESS)
                {
                    handle = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Retrieve the invocation structure pointer from the BPS event.
        /// </summary>
        /// <param name="ev">The <see cref="NavigatorEvents.InvokeTarget">InvokeTarget</see> event to extract the invocation from.</param>
        /// <returns>The invocation retrieved from the BPS event.</returns>
        [AvailableSince(10, 0)]
        public static Invocation GetInvocation(BPSEvent ev)
        {
            if (ev.Domain != Navigator.Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.InvokeTarget)
            {
                throw new ArgumentException("BPSEvent is not a invoke target event");
            }
            var ptr = ev.DangerousGetHandle();
            var res = navigator_invoke_event_get_invocation(ptr);
            if (res == IntPtr.Zero)
            {
                var error = Marshal.PtrToStringAnsi(Navigator.navigator_event_get_err(ptr));
                if (error == null)
                {
                    throw new InvalidOperationException("An unkown error occured while trying to get the invocation.");
                }
                switch (error.ToUpperInvariant())
                {
                    case "INVOKE_NO_TARGET_ERROR":
                        throw new InvalidOperationException("There is no target identified by the invocation.");
                    case "INVOKE_BAD_REQUEST_ERROR":
                        throw new InvalidOperationException("The invocation request specifications do not conform to the permitted parameters of the handler. For example, an image sharing invocation being sent to a target application that cannot share images would result in this error.");
                    case "INVOKE_INTERNAL_ERROR":
                        throw new InvalidOperationException("A generic error occured in the internal framework while attempting to retrieve the Invocation.");
                    case "INVOKE_TARGET_ERROR":
                        throw new InvalidOperationException("A generic error occured with the target handler.");
                    default:
                        throw new InvalidOperationException("An error occured while trying to get the invocation.", new Exception(error));
                }
            }
            return new Invocation(res, ev);
        }

        /// <summary>
        /// Convert a percent-encoded file URI to a file path.
        /// </summary>
        /// <param name="uri">The URI encoded file path. It needs to start with "file:///".</param>
        /// <returns>The local file path upon success, null on error.</returns>
        [AvailableSince(10, 0)]
        public static string UriToLocalPath(Uri uri)
        {
            if (!uri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("Must start with \"file:///\"", "uri");
            }
            Util.GetBPSOrException();
            var ret = navigator_invoke_uri_to_local_path(uri.ToString());
            if (ret == IntPtr.Zero)
            {
                return null;
            }
            using (var str = new BPSString(ret))
            {
                return str.Value;
            }
        }

        /// <summary>
        /// Convert an absolute file path to a percent-encoded file URI.
        /// </summary>
        /// <param name="localPath">The absolute file path. It needs to start with "/".</param>
        /// <returns>A percent-encoded file URI that corresponds to path, or null on error.</returns>
        [AvailableSince(10, 0)]
	    public static Uri LocalPathToUri(string localPath)
        {
            if (!localPath.StartsWith("/"))
            {
                throw new ArgumentException("Must start with \"/\"", "localPath");
            }
            Util.GetBPSOrException();
            var ret = navigator_invoke_local_path_to_uri(localPath);
            if (ret == IntPtr.Zero)
            {
                return null;
            }
            using (var str = new BPSString(ret))
            {
                return new Uri(str.Value);
            }
        }
    }
}
