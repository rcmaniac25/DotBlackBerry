using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Invoke
{
    #region Enums

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
    /// Common invocation actions. Generally, anything that starts with "bb.action".
    /// </summary>
    public enum CommonInvokeActions
    {
        /// <summary>
        /// An unknown, or custom action that isn't avaliable within this enumeration.
        /// </summary>
        Unknown,
        /// <summary>
        /// bb.action.ADD
        /// </summary>
        Add,
        /// <summary>
        /// bb.action.ADDTOCONTACT
        /// </summary>
        AddToContact,
        /// <summary>
        /// bb.action.BBMCHAT
        /// </summary>
        BBMChat,
        /// <summary>
        /// bb.action.INVITEBBM
        /// </summary>
        BBMInvite,
        /// <summary>
        /// bb.action.BBMCONF
        /// </summary>
        BBMMultiPersonChat,
        /// <summary>
        /// bb.action.OPENBBMCHANNEL
        /// </summary>
        BBMOpenChannel,
        /// <summary>
        /// bb.action.CAPTURE
        /// </summary>
        Capture,
        /// <summary>
        /// bb.action.CHAT
        /// </summary>
        Chat,
        /// <summary>
        /// bb.action.COMPOSE
        /// </summary>
        Compose,
        /// <summary>
        /// bb.action.CREATE
        /// </summary>
        Create,
        /// <summary>
        /// bb.action.DELETE
        /// </summary>
        Delete,
        /// <summary>
        /// bb.action.DIAL
        /// </summary>
        Dial,
        /// <summary>
        /// bb.action.DIRECT_RESPONSE
        /// </summary>
        DirectResponse,
        /// <summary>
        /// bb.action.EDIT
        /// </summary>
        Edit,
        /// <summary>
        /// bb.action.EMERGENCY_CALL
        /// </summary>
        EmergencyCall,
        /// <summary>
        /// bb.action.FORWARD
        /// </summary>
        Forward,
        /// <summary>
        /// bb.action.INVITE
        /// </summary>
        Invite,
        /// <summary>
        /// bb.action.OPEN
        /// </summary>
        Open,
        /// <summary>
        /// bb.action.PUSH
        /// </summary>
        Push,
        /// <summary>
        /// bb.action.REPLY
        /// </summary>
        Reply,
        /// <summary>
        /// bb.action.REPLYALL
        /// </summary>
        ReplyAll,
        /// <summary>
        /// bb.action.SEARCH.EXTENDED
        /// </summary>
        SearchExtended,
        /// <summary>
        /// bb.action.SEARCH.SOURCE
        /// </summary>
        SearchSource,
        /// <summary>
        /// bb.action.SENDEMAIL
        /// </summary>
        SendEmail,
        /// <summary>
        /// bb.action.SENDTEXT
        /// </summary>
        SendText,
        /// <summary>
        /// bb.action.SET
        /// </summary>
        Set,
        /// <summary>
        /// bb.action.SETUP
        /// </summary>
        Setup,
        /// <summary>
        /// bb.action.SHARE
        /// </summary>
        Share,
        /// <summary>
        /// Action for when a <see cref="BlackBerry.BPS.Invoke.Timer">Timer</see> triggers. bb.action.system.TIMER_FIRED
        /// </summary>
        TimerFired,
        /// <summary>
        /// bb.action.VIEW
        /// </summary>
        View
    }

    #endregion

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
                    //XXX May not be 100% valid... might be better to remove it entirely...
                    return navigator_invoke_invocation_get_target(handle) != IntPtr.Zero ||
                        navigator_invoke_invocation_get_action(handle) != IntPtr.Zero ||
                        navigator_invoke_invocation_get_type(handle) != IntPtr.Zero ||
                        navigator_invoke_invocation_get_uri(handle) != IntPtr.Zero;
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
        /// Get or set the action for the invocation.
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

        /// <summary>
        /// Get or set the action for the invocation using a common action.
        /// </summary>
        public CommonInvokeActions CommonAction
        {
            get
            {
                return ParseCommonAction(Action);
            }
            set
            {
                Action = TranslateCommonAction(value);
            }
        }

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
            //XXX Is this even possible? What if the app recieves just some simple ASCII? Even if a single-op deserialization can occur, it would have to evaluate and test to find out what it is.
            //XXX Perhaps just a byte array to make life easier, and offer serialization options to the user? What about other areas where data is an accepted property?
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
        /// <seealso cref="InvokeList"/>
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
                if (value.HasValue && value.Value <= 0)
                {
                    throw new ArgumentException("List ID cannot be less than or equal to zero.", "ListID");
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

        #region Common Actions

        private static CommonInvokeActions ParseCommonAction(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
            {
                return CommonInvokeActions.Unknown;
            }
            switch (action.ToLower())
            {
                case "bb.action.add":
                    return CommonInvokeActions.Add;
                case "bb.action.addtocontact":
                    return CommonInvokeActions.AddToContact;
                case "bb.action.bbmchat":
                    return CommonInvokeActions.BBMChat;
                case "bb.action.invitebbm":
                    return CommonInvokeActions.BBMInvite;
                case "bb.action.bbmconf":
                    return CommonInvokeActions.BBMMultiPersonChat;
                case "bb.action.openbbmchannel":
                    return CommonInvokeActions.BBMOpenChannel;
                case "bb.action.capture":
                    return CommonInvokeActions.Capture;
                case "bb.action.chat":
                    return CommonInvokeActions.Chat;
                case "bb.action.compose":
                    return CommonInvokeActions.Compose;
                case "bb.action.create":
                    return CommonInvokeActions.Create;
                case "bb.action.delete":
                    return CommonInvokeActions.Delete;
                case "bb.action.dial":
                    return CommonInvokeActions.Dial;
                case "bb.action.direct_response":
                    return CommonInvokeActions.DirectResponse;
                case "bb.action.edit":
                    return CommonInvokeActions.Edit;
                case "bb.action.emergency_call":
                    return CommonInvokeActions.EmergencyCall;
                case "bb.action.forward":
                    return CommonInvokeActions.Forward;
                case "bb.action.invite":
                    return CommonInvokeActions.Invite;
                case "bb.action.open":
                    return CommonInvokeActions.Open;
                case "bb.action.push":
                    return CommonInvokeActions.Push;
                case "bb.action.reply":
                    return CommonInvokeActions.Reply;
                case "bb.action.replyall":
                    return CommonInvokeActions.ReplyAll;
                case "bb.action.search.extended":
                    return CommonInvokeActions.SearchExtended;
                case "bb.action.search.source":
                    return CommonInvokeActions.SearchSource;
                case "bb.action.sendemail":
                    return CommonInvokeActions.SendEmail;
                case "bb.action.sendtext":
                    return CommonInvokeActions.SendText;
                case "bb.action.set":
                    return CommonInvokeActions.Set;
                case "bb.action.setup":
                    return CommonInvokeActions.Setup;
                case "bb.action.share":
                    return CommonInvokeActions.Share;
                case "bb.action.system.timer_fired":
                    return CommonInvokeActions.TimerFired;
                case "bb.action.view":
                    return CommonInvokeActions.View;
                default:
                    return CommonInvokeActions.Unknown;
            }
        }

        private static string TranslateCommonAction(CommonInvokeActions action)
        {
            if (action == CommonInvokeActions.Unknown || !action.IsValidValue())
            {
                throw new ArgumentException(action == CommonInvokeActions.Unknown ? "Cannot be \"Unknown\"" : "Unknown action", "CommonAction");
            }
            switch (action)
            {
                case CommonInvokeActions.Add:
                    return "bb.action.ADD";
                case CommonInvokeActions.AddToContact:
                    return "bb.action.ADDTOCONTACT";
                case CommonInvokeActions.BBMChat:
                    return "bb.action.BBMCHAT";
                case CommonInvokeActions.BBMInvite:
                    return "bb.action.INVITEBBM";
                case CommonInvokeActions.BBMMultiPersonChat:
                    return "bb.action.BBMCONF";
                case CommonInvokeActions.BBMOpenChannel:
                    return "bb.action.OPENBBMCHANNEL";
                case CommonInvokeActions.Capture:
                    return "bb.action.CAPTURE";
                case CommonInvokeActions.Chat:
                    return "bb.action.CHAT";
                case CommonInvokeActions.Compose:
                    return "bb.action.COMPOSE";
                case CommonInvokeActions.Create:
                    return "bb.action.CREATE";
                case CommonInvokeActions.Delete:
                    return "bb.action.DELETE";
                case CommonInvokeActions.Dial:
                    return "bb.action.DIAL";
                case CommonInvokeActions.DirectResponse:
                    return "bb.action.DIRECT_RESPONSE";
                case CommonInvokeActions.Edit:
                    return "bb.action.EDIT";
                case CommonInvokeActions.EmergencyCall:
                    return "bb.action.EMERGENCY_CALL";
                case CommonInvokeActions.Forward:
                    return "bb.action.FORWARD";
                case CommonInvokeActions.Invite:
                    return "bb.action.INVITE";
                case CommonInvokeActions.Open:
                    return "bb.action.OPEN";
                case CommonInvokeActions.Push:
                    return "bb.action.PUSH";
                case CommonInvokeActions.Reply:
                    return "bb.action.REPLY";
                case CommonInvokeActions.ReplyAll:
                    return "bb.action.REPLYALL";
                case CommonInvokeActions.SearchExtended:
                    return "bb.action.SEARCH.EXTENDED";
                case CommonInvokeActions.SearchSource:
                    return "bb.action.SEARCH.SOURCE";
                case CommonInvokeActions.SendEmail:
                    return "bb.action.SENDEMAIL";
                case CommonInvokeActions.SendText:
                    return "bb.action.SENDTEXT";
                case CommonInvokeActions.Set:
                    return "bb.action.SET";
                case CommonInvokeActions.Setup:
                    return "bb.action.SETUP";
                case CommonInvokeActions.Share:
                    return "bb.action.SHARE";
                case CommonInvokeActions.TimerFired:
                    return "bb.action.system.TIMER_FIRED";
                case CommonInvokeActions.View:
                    return "bb.action.VIEW";
                default:
                    return "Invalid action. Should not've gotten here. Please file a bug";
            }
        }

        #endregion
    }
}
