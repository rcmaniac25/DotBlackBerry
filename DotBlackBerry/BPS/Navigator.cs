using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    #region Enums

    /// <summary>
    /// What events to request from the navigator.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum NavigatorRequestEvents : int
    {
        /// <summary>
        /// Enables all default events.
        /// </summary>
        [AvailableSince(10, 0)]
        Default = 0,
        /// <summary>
        /// Enables all default events including extended data.
        /// </summary>
        [AvailableSince(10, 0)]
        ExtendedData = 1
    }

    #region NavigatorEvents

    /// <summary>
    /// Event codes for navigator service events.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum NavigatorEvents : uint
    {
        /// <summary>
        /// Indicates that the application is a registered URL handler, and the navigator is invoking a URL type on the application.
        /// </summary>
        [AvailableSince(10, 0)]
        Invoke = 0x01,
        /// <summary>
        /// Indicates that the user has quit the application, the device is rebooting, or some other event has occurred that results in the application needing to terminate.
        /// </summary>
        [AvailableSince(10, 0)]
        Exit = 0x02,
        /// <summary>
        /// Indicates that the state of the application window has changed.
        /// </summary>
        [AvailableSince(10, 0)]
        WindowState = 0x03,
        /// <summary>
        /// Indicates that the user has performed a downward swipe gesture from the top of the device screen. By convention, this gesture displays a menu.
        /// </summary>
        [AvailableSince(10, 0)]
        SwipeDown = 0x04,
        /// <summary>
        /// Indicates that the user has started a swipe gesture.
        /// </summary>
        [AvailableSince(10, 0)]
        SwipeStart = 0x05,
        /// <summary>
        /// Indicates that the device is low on memory.
        /// </summary>
        [AvailableSince(10, 0)]
        LowMemory = 0x06,
        /// <summary>
        /// Indicates that the device has rotated.
        /// </summary>
        [AvailableSince(10, 0)]
        OrientationCheck = 0x07,
        /// <summary>
        /// Indicates that an application should resize its screen in response to the rotation of the device.
        /// </summary>
        [AvailableSince(10, 0)]
        Orientation = 0x08,
        /// <summary>
        /// Indicates that the user has performed a swipe gesture from the bottom left of the device screen towards the top right.
        /// </summary>
        [AvailableSince(10, 0)]
        Back = 0x09,
        /// <summary>
        /// Indicates that the application window has become active (for example, if the application window changes to full screen from being hidden).
        /// </summary>
        [AvailableSince(10, 0)]
        WindowActive = 0x0a,
        /// <summary>
        /// Indicates that the application window has become inactive (for example, if the application window changes to hidden from being full screen).
        /// </summary>
        [AvailableSince(10, 0)]
        WindowInactive = 0x0b,
        /// <summary>
        /// Indicates that the device has finished rotating.
        /// </summary>
        [AvailableSince(10, 0)]
        OrientationDone = 0x0c,
        /// <summary>
        /// Indicates that a request to change the orientation using <see cref="BlackBerry.BPS.Navigator.SetOrientation(BlackBerry.BPS.NavigatorOrientation)"/> or <see cref="BlackBerry.BPS.Navigator.SetOrientation(BlackBerry.BPS.OrientationMode)"/> has completed.
        /// </summary>
        [AvailableSince(10, 0)]
        OrientationResult = 0x0d,
        /// <summary>
        /// Indicates that the corporate or enterprise application is locked.
        /// </summary>
        [AvailableSince(10, 0)]
        WindowLock = 0x0e,
        /// <summary>
        /// Indicates that the corporate or enterprise application is unlocked.
        /// </summary>
        [AvailableSince(10, 0)]
        WindowUnlock = 0x0f,
        /// <summary>
        /// Indicates an invocation for the target was received.
        /// </summary>
        [AvailableSince(10, 0)]
        InvokeTarget = 0x10,
        /// <summary>
        /// Indicates an invocation query result was received.
        /// </summary>
        [AvailableSince(10, 0)]
        InvokeQueryResult = 0x11,
        /// <summary>
        /// Indicates a viewer invocation was received.
        /// </summary>
        [AvailableSince(10, 0)]
        InvokeViewer = 0x12,
        /// <summary>
        /// Indicates an invocation target response was received.
        /// </summary>
        [AvailableSince(10, 0)]
        InvokeTargetResult = 0x13,
        /// <summary>
        /// Indicates an invocation viewer response was received.
        /// </summary>
        [AvailableSince(10, 0)]
        InvokeViewerResult = 0x14,
        /// <summary>
        /// If the current process is the parent application of the viewer it indicates that the request message from the viewer was received. 
        /// If the current process is the viewer it indicates that the request message from the parent application was received.
        /// </summary>
        [AvailableSince(10, 0)]
        InvokeViewerRelay = 0x15,
        /// <summary>
        /// Indicates that the invocation viewer has terminated.
        /// </summary>
        [AvailableSince(10, 0)]
        InvokeViewerStopped = 0x16,
        /// <summary>
        /// Indicates that the the keyboard has changed state.
        /// </summary>
        [AvailableSince(10, 0)]
        KeyboardState = 0x17,
        /// <summary>
        /// Indicates that the keyboard has changed position.
        /// </summary>
        [AvailableSince(10, 0)]
        KeyboardPosition = 0x18,
        /// <summary>
        /// If the current process is the parent application of the viewer it indicates that the response message from the viewer was received. 
        /// If the current process is the viewer it indicates that the response message from the parent application was received. In case of an error in 
        /// delivering the request message to the peer the event contains an error message.
        /// </summary>
        [AvailableSince(10, 0)]
        InvokeViewerRelayResult = 0x19,
        /// <summary>
        /// Indicates that the device has been locked or unlocked.
        /// </summary>
        [AvailableSince(10, 0)]
        DeviceLockState = 0x1a,
        /// <summary>
        /// Provide details about the window cover. Occurs on application startup.
        /// </summary>
        [AvailableSince(10, 0)]
        WindowCover = 0x1b,
        /// <summary>
        /// Occurs when navigator displays the application's window cover.
        /// </summary>
        [AvailableSince(10, 0)]
        WindowCoverEnter = 0x1c,
        /// <summary>
        /// Occurs when the navigator removes the application's window cover.
        /// </summary>
        [AvailableSince(10, 0)]
        WindowCoverExit = 0x1d,
        /// <summary>
        /// Indicates that the card peek action has started.
        /// </summary>
        [AvailableSince(10, 0)]
        CardPeekStarted = 0x1e,
        /// <summary>
        /// Indicates that the card peek action has stopped.
        /// </summary>
        [AvailableSince(10, 0)]
        CardPeekStopped = 0x1f,
        /// <summary>
        /// Indicates that the card application should resize its buffer and call the <see cref="Navigator.CardResized"/> function when finished.
        /// </summary>
        [AvailableSince(10, 0)]
        CardResize = 0x20,
        /// <summary>
        /// Indicates to the parent of a card application that the child card has been closed.
        /// </summary>
        [AvailableSince(10, 0)]
        ChildCardClosed = 0x21,
        /// <summary>
        /// Indicates that the card has been closed and is being pooled.
        /// </summary>
        [AvailableSince(10, 0)]
        CardClosed = 0x22,
        /// <summary>
        /// Indicates a get invoke target filters result was received.
        /// </summary>
        [AvailableSince(10, 0)]
        InvokeGetFiltersResult = 0x23,
        /// <summary>
        /// Occurs when the Adaptive Partition Scheduler will move the application to a different partition (background, foreground, or stopped).
        /// </summary>
        AppState = 0x24,
        /// <summary>
        /// Indicates a set invoke target filters result was received.
        /// </summary>
        [AvailableSince(10, 0)]
        InvokeSetFiltersResult = 0x25,
        /// <summary>
        /// Indicates that the peek action of this card has started.
        /// </summary>
        [AvailableSince(10, 0)]
        PeekStarted = 0x26,
        /// <summary>
        /// Indicates that the peek action of this card has stopped.
        /// </summary>
        [AvailableSince(10, 0)]
        PeekStopped = 0x27,
        /// <summary>
        /// Indicates that the Navigator is ready to display the card's window.
        /// </summary>
        [AvailableSince(10, 0)]
        CardReadyCheck = 0x28,
        /// <summary>
        /// Indicates that the navigator would like to pool your application or card. Pooled means that the application is still running as a process but its window is not visible to the user.
        /// </summary>
        [AvailableSince(10, 0)]
        Pooled = 0x29,
        /// <summary>
        /// Informs the app what the rotated window's width and height will be.
        /// </summary>
        [AvailableSince(10, 0)]
        OrientationSize = 0x2a,
        /// <summary>
        /// Informs the app that the cursor of an invoke list is being moved.
        /// </summary>
        [AvailableSince(10, 2)]
        InvokeListCursorMoved = 0x2b,
        /// <summary>
        /// Informs the app that an invoke list item has been selected.
        /// </summary>
        [AvailableSince(10, 2)]
        InvokeListItemSelected = 0x2c,
        /// <summary>
        /// Indicates a timer registration result was received.
        /// </summary>
        [AvailableSince(10, 3)]
        InvokeTimerRegistration = 0x2d,
        /// <summary>
        /// Indicates the user pressed a system key.
        /// </summary>
        [AvailableSince(10, 3, 1)]
        SystemKeyPress = 0x2e,
        /// <summary>
        /// Indicates that the event is not any of the above event types. It could be a custom event.
        /// </summary>
        [AvailableSince(10, 0)]
        Other = 0xff
    }

    #endregion

    /// <summary>
    /// Navigator window states.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum WindowState : int
    {
        /// <summary>
        /// The application occupies the full display and should be operating normally.
        /// </summary>
        [AvailableSince(10, 0)]
        Fullscreen = 0,
        /// <summary>
        /// The application is reduced to a thumbnail as the user switches applications.
        /// </summary>
        [AvailableSince(10, 0)]
        Thumbnail = 1,
        /// <summary>
        /// The application is no longer visible to the user, for any reason.
        /// </summary>
        [AvailableSince(10, 0)]
        Invisible = 2
    }

    /// <summary>
    /// The different run partitions an application can be placed into.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum AppState : int
    {
        /// <summary>
        /// The application is placed into the foreground partition.
        /// </summary>
        [AvailableSince(10, 0)]
        Foreground = 0,
        /// <summary>
        /// The application is placed into the background partition.
        /// </summary>
        [AvailableSince(10, 0)]
        Background = 1,
        /// <summary>
        /// The application will shortly be placed into the stopped partition.
        /// </summary>
        [AvailableSince(10, 0)]
        Stopping = 2
    }

    /// <summary>
    /// Navigator card peeking types.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum PeekType : int
    {
        /// <summary>
        /// Indicates that the peek action is to the bottom of the card stack. The root of the selected card is revealed.
        /// </summary>
        [AvailableSince(10, 0)]
        Root = 0,
        /// <summary>
        /// Indicates that the peek action is to the previous card. The parent of the selected card is revealed.
        /// </summary>
        [AvailableSince(10, 0)]
        Parent = 1
    }

    /// <summary>
    /// Screen orientation modes.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum OrientationMode : int
    {
        /// <summary>
        /// Indicates that the screen is in landscape mode (the longer sides of the device are positioned at the bottom and top while the shorter sides are on the sides).
        /// </summary>
        [AvailableSince(10, 0)]
        Landscape = 0,
        /// <summary>
        /// Indicates that the screen is in portrait mode. (the shorter sides of the device are positioned at the bottom and top while the longer sides are on the sides).
        /// </summary>
        [AvailableSince(10, 0)]
        Portrait = 1
    }

    /// <summary>
    /// Application orientations.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum NavigatorOrientation : int
    {
        /// <summary>
        /// Indicate that the "top" edge of the application is facing up on the screen (the application appears to be correctly oriented).
        /// </summary>
        [AvailableSince(10, 0)]
        TopUp = 0,
        /// <summary>
        /// Indicate that the "right" edge of the application is facing up on the screen (the application appears to be lying on its left side).
        /// </summary>
        [AvailableSince(10, 0)]
        RightUp = 90,
        /// <summary>
        /// Indicate that the "bottom" edge of the application is facing up on the screen (the application appears to be upside-down).
        /// </summary>
        [AvailableSince(10, 0)]
        BottomUp = 180,
        /// <summary>
        /// Indicate that the "left" edge of the application is facing up on the screen (the application appears to be lying on its right side).
        /// </summary>
        [AvailableSince(10, 0)]
        LeftUp = 270
    }

    /// <summary>
    /// Keyboard state.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum KeyboardState : int
    {
        /// <summary>
        /// Indicates that the keyboard is in an unrecognized state.
        /// </summary>
        [AvailableSince(10, 0)]
        Unrecognized = 0,
        /// <summary>
        /// Indicates that the keyboard is opening.
        /// </summary>
        [AvailableSince(10, 0)]
        Opening = 1,
        /// <summary>
        /// Indicates that the keyboard is opened.
        /// </summary>
        [AvailableSince(10, 0)]
        Opened = 2,
        /// <summary>
        /// Indicates that the keyboard is closing.
        /// </summary>
        [AvailableSince(10, 0)]
        Closing = 3,
        /// <summary>
        /// Indicates that the keyboard is closed.
        /// </summary>
        [AvailableSince(10, 0)]
        Closed = 4
    }

    /// <summary>
    /// Navigator window and icon badges.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum NavigatorBadge : int
    {
        /// <summary>
        /// ndicates that the badge is a splat. A splat appears as a white star in a red circle.
        /// </summary>
        [AvailableSince(10, 0)]
        Splat = 0
    }

    /// <summary>
    /// Device lock states
    /// </summary>
    [AvailableSince(10, 0)]
    public enum LockState : int
    {
        /// <summary>
        /// The device is unlocked.
        /// </summary>
        [AvailableSince(10, 0)]
        Unlocked = 0,
        /// <summary>
        /// The device is locked.
        /// </summary>
        [AvailableSince(10, 0)]
        Locked = 1,
        /// <summary>
        /// The device is locked, and a password is required to unlock.
        /// </summary>
        [AvailableSince(10, 0)]
        PasswordLocked = 2
    }

    /// <summary>
    /// Window cover transitions.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum WindowCoverTransition : int
    {
        /// <summary>
        /// Use the default effect when drawing the cover.
        /// </summary>
        [AvailableSince(10, 0)]
        Default = 0,
        /// <summary>
        /// Don't use a transition effect when drawing the cover.
        /// </summary>
        [AvailableSince(10, 0)]
        None = 1,
        /// <summary>
        /// Use a slide effect when drawing the cover.
        /// </summary>
        [AvailableSince(10, 0)]
        Slide = 2,
        /// <summary>
        /// Use a fade effect when drawing the cover.
        /// </summary>
        [AvailableSince(10, 0)]
        Fade = 3
    }

    /// <summary>
    /// Navigator system keys.
    /// </summary>
    [AvailableSince(10, 3, 1)]
    public enum SystemKey : int
    {
        /// <summary>
        /// The 'Send' system key.
        /// </summary>
        [AvailableSince(10, 3, 1)]
        Send = 0,
        /// <summary>
        /// The 'End' system key.
        /// </summary>
        [AvailableSince(10, 3, 1)]
        End = 1,
        /// <summary>
        /// The 'Back' system key.
        /// </summary>
        [AvailableSince(10, 3, 1)]
        Back = 2
    }

    #endregion

    /// <summary>
    /// Send and recieve messages from the navigator, which controls how applications appear on the device.
    /// </summary>
    [AvailableSince(10, 0)]
    public static class Navigator
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_request_events(NavigatorRequestEvents flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_stop_events(int flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_get_domain();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_add_uri([MarshalAs(UnmanagedType.LPStr)]string icon_path, [MarshalAs(UnmanagedType.LPStr)]string icon_label, [MarshalAs(UnmanagedType.LPStr)]string default_category, [MarshalAs(UnmanagedType.LPStr)]string url, [In, Out]ref IntPtr err);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_extend_timeout(int extension, [In, Out]ref IntPtr err);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_extend_terminate();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_request_swipe_start();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_stop_swipe_start();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_rotation_lock(bool locked);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_set_orientation(NavigatorOrientation angle, [In, Out]ref IntPtr id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_set_orientation_mode(OrientationMode mode, [In, Out]ref IntPtr id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_set_window_angle(int angle);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_rotation_effect(bool effect);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_set_close_prompt([MarshalAs(UnmanagedType.LPStr)]string title, [MarshalAs(UnmanagedType.LPStr)]string message);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_clear_close_prompt();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_set_badge(NavigatorBadge badge);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_clear_badge();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_set_keyboard_tracking(bool track);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_severity(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern WindowState navigator_event_get_window_state(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_event_get_groupid(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_orientation_angle(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern OrientationMode navigator_event_get_orientation_mode(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_orientation_size_width(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_orientation_size_height(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_keyboard_state(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_keyboard_position(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_window_cover_height(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_window_cover_width(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_event_get_data(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_event_get_id(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr navigator_event_get_err(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern void navigator_orientation_check_response(IntPtr ev, bool will_rotate);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_orientation_check_response_id([MarshalAs(UnmanagedType.LPStr)]string id, bool will_rotate);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern void navigator_done_orientation(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_done_orientation_id([MarshalAs(UnmanagedType.LPStr)]string id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_close_window();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_pooled_response([MarshalAs(UnmanagedType.LPStr)]string id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_get_device_lock_state();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_raw_write(IntPtr data, uint length);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_event_get_extended_data(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern uint navigator_event_get_extended_data_length(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_device_lock_state(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_app_state(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_attribute_create(out IntPtr attribute);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_attribute_destroy(IntPtr attribute);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_attribute_set_live(IntPtr attribute);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_attribute_set_alternate_window(IntPtr attribute);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_attribute_set_file(IntPtr attribute, [MarshalAs(UnmanagedType.LPStr)]string file);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_attribute_set_capture(IntPtr attribute, int x, int y, int width, int height);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_attribute_set_allow_badges(IntPtr attribute, bool is_allowed);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_attribute_set_transition(IntPtr attribute, WindowCoverTransition transition);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_attribute_add_label(IntPtr attribute, [MarshalAs(UnmanagedType.LPStr)]string text, out IntPtr label);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_label_destroy(IntPtr label);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_label_set_color(IntPtr label, byte red, byte green, byte blue);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_label_set_text(IntPtr label, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_label_set_size(IntPtr label, int size);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int navigator_window_cover_label_set_wrap_text(IntPtr label, bool wrap_text);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_window_cover_update(IntPtr attribute);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_window_cover_reset();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_card_peek(PeekType peek_type);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_card_request_card_ready_check(bool check);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_card_send_card_ready();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_card_peek_type(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_peek_type(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_card_peek_stopped_swipe_away(IntPtr ev, out bool is_swipe_away);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_peek_stopped_swipe_away(IntPtr ev, out bool is_swipe_away);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_card_width(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_card_height(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_card_edge(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_card_orientation(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_card_close([MarshalAs(UnmanagedType.LPStr)]string reason, [MarshalAs(UnmanagedType.LPStr)]string type, IntPtr data);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_event_get_card_closed_reason(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_event_get_card_closed_data_type(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_event_get_card_closed_data(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_card_close_child();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_card_resized([MarshalAs(UnmanagedType.LPStr)]string id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_card_swipe_away();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_set_wallpaper([MarshalAs(UnmanagedType.LPStr)]string filepath);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_set_lockscreen_wallpaper([MarshalAs(UnmanagedType.LPStr)]string filepath);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_event_get_syskey_key(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr navigator_event_get_syskey_id(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_syskey_press_response([MarshalAs(UnmanagedType.LPStr)]string id, bool will_handle);

        #endregion

        #region BPS

        /// <summary>
        /// Start receiving navigator events.
        /// </summary>
        /// <param name="flags">The types of events to deliver.</param>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestEvents(NavigatorRequestEvents flags = NavigatorRequestEvents.Default)
        {
            Util.GetBPSOrException();
            return navigator_request_events(flags) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving navigator events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopEvents()
        {
            Util.GetBPSOrException();
            return navigator_stop_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get the unique domain ID for the navigator service.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int Domain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return navigator_get_domain();
            }
        }

        #endregion

        #region Cleanup

        private static int cleanupIsSetup = 0;
        private static ConcurrentSet<IntPtr> cardClosedPointersToCleanup = new ConcurrentSet<IntPtr>();

        private static void Cleanup()
        {
            var pointers = cardClosedPointersToCleanup.ToArray();
            cardClosedPointersToCleanup.Clear();
            foreach (var ptr in pointers)
            {
                Mono.Unix.UnixMarshal.FreeHeap(ptr);
            }
        }

        private static void VerifyCleanupSetup()
        {
            if (System.Threading.Interlocked.CompareExchange(ref cleanupIsSetup, 1, 0) == 0)
            {
                BPS.RegisterCleanup(Cleanup, true);
            }
        }

        #endregion

        /// <summary>
        /// Create a navigator icon that, when launched, invokes the corresponding application based on the URI value.
        /// </summary>
        /// <param name="iconPath">The path to the icon image.</param>
        /// <param name="iconLabel">The label to apply to the icon image.</param>
        /// <param name="category">The navigator tray that the icon should appear in.</param>
        /// <param name="applicationUrl">The URI of the application to launch.</param>
        /// <param name="throwExceptionOnFailure">true if an exception should be thrown on error, false if no exception should be thrown.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool AddUri(string iconPath, string iconLabel, string category, Uri applicationUrl, bool throwExceptionOnFailure = false)
        {
            Util.GetBPSOrException();
            var err = IntPtr.Zero;
            var success = navigator_add_uri(iconPath, iconLabel, category, applicationUrl.ToString(), ref err) == BPS.BPS_SUCCESS;
            if (!success)
            {
                if (throwExceptionOnFailure)
                {
                    string errorStr;
                    using (var error = new BPSString(err))
                    {
                        errorStr = error.Value;
                    }
                    throw new InvalidOperationException(errorStr);
                }
                else
                {
                    BPS.bps_free(err);
                }
            }
            return success;
        }

        /// <summary>
        /// Extend the time allowed for the application to create its application window at application start.
        /// </summary>
        /// <param name="extension">The total time that the application expects to need before it can create its application window.</param>
        /// <param name="throwExceptionOnFailure">true if an exception should be thrown on error, false if no exception should be thrown.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool ExtendStartupTimeout(TimeSpan extension, bool throwExceptionOnFailure = false)
        {
            Util.GetBPSOrException();
            if (extension.TotalMilliseconds < 0)
            {
                throw new ArgumentOutOfRangeException("extension", extension, "Extension cannot be negative.");
            }
            if (extension.TotalMilliseconds == 0)
            {
                return true;
            }
            var err = IntPtr.Zero;
            var success = navigator_extend_timeout((int)extension.TotalMilliseconds, ref err) == BPS.BPS_SUCCESS;
            if (!success)
            {
                if (throwExceptionOnFailure)
                {
                    string errorStr;
                    using (var error = new BPSString(err))
                    {
                        errorStr = error.Value;
                    }
                    throw new InvalidOperationException(errorStr);
                }
                else
                {
                    BPS.bps_free(err);
                }
            }
            return success;
        }

        /// <summary>
        /// Extend the time allowed for the application to exit before it is forcibly terminated.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool ExtendTerminate()
        {
            Util.GetBPSOrException();
            return navigator_extend_terminate() == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Send a navigator SwipeStart request.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestSwipeStart()
        {
            Util.GetBPSOrException();
            return navigator_request_swipe_start() == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop the navigator from sending SwipeStart events.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopSwipeStart()
        {
            Util.GetBPSOrException();
            return navigator_stop_swipe_start() == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Specify the orientation of your application as locked or not locked
        /// </summary>
        /// <param name="locked">true the orientation of your application is locked, false the orientation of your application is not locked.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool LockRotation(bool locked)
        {
            Util.GetBPSOrException();
            return navigator_rotation_lock(locked) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Set the orientation in the navigator based on angle.
        /// </summary>
        /// <param name="angle">The angle of the orientation to set.</param>
        /// <returns>The ID used in the orientation request, or null if an error occured.</returns>
        [AvailableSince(10, 0)]
        public static string SetOrientation(NavigatorOrientation angle)
        {
            if (!angle.IsValidValue())
            {
                throw new ArgumentException("not a valid orientation value.", "angle");
            }
            var id = IntPtr.Zero;
            var success = navigator_set_orientation(angle, ref id) == BPS.BPS_SUCCESS;
            if (success)
            {
                using (var str = new BPSString(id))
                {
                    return str.Value;
                }
            }
            else
            {
                BPS.bps_free(id);
            }
            return null;
        }

        //Might be good to have a generic function to handle both SetOrientation cases
        /// <summary>
        /// Set the orientation in the navigator based on landscape or portrait.
        /// </summary>
        /// <param name="mode">Orientation mode</param>
        /// <returns>The ID used in the orientation request, or null if an error occured.</returns>
        [AvailableSince(10, 0)]
        public static string SetOrientation(OrientationMode mode)
        {
            if (!mode.IsValidValue())
            {
                throw new ArgumentException("not a valid orientation mode.", "mode");
            }
            var id = IntPtr.Zero;
            var success = navigator_set_orientation_mode(mode, ref id) == BPS.BPS_SUCCESS;
            if (success)
            {
                using (var str = new BPSString(id))
                {
                    return str.Value;
                }
            }
            else
            {
                BPS.bps_free(id);
            }
            return null;
        }

        /// <summary>
        /// Set the window angle in the navigator.
        /// </summary>
        /// <param name="angle">The angle of the window to set.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool SetWindowAngle(int angle)
        {
            Util.GetBPSOrException();
            return navigator_set_window_angle(angle) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Set the rotation effect in the navigator.
        /// </summary>
        /// <param name="effect">true to enable the rotation effect, false to disable it.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool EnableRotationEffect(bool effect)
        {
            Util.GetBPSOrException();
            return navigator_rotation_effect(effect) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Set the close prompt in the navigator.
        /// </summary>
        /// <param name="title">The title of the close prompt dialog to set.</param>
        /// <param name="message">The message of the close prompt dialog to set.</param>
        /// <returns>true on success, false if otherwise.</returns>
        /// <seealso cref="ClearClosePrompt"/>
        [AvailableSince(10, 0)]
        public static bool SetClosePrompt(string title, string message)
        {
            char[] invalidChars = new char[] { ',', '"' };
            if (title.IndexOfAny(invalidChars) >= 0 || message.IndexOfAny(invalidChars) >= 0)
            {
                throw new ArgumentException("title and message cannot contain commas (,) or quotes(\")");
            }
            Util.GetBPSOrException();
            return navigator_set_close_prompt(title, message) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Clear the close prompt in the navigator.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        /// <seealso cref="SetClosePrompt"/>
        [AvailableSince(10, 0)]
        public static bool ClearClosePrompt()
        {
            Util.GetBPSOrException();
            return navigator_clear_close_prompt() == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Set a badge on the application's icon and window frame in the navigator.
        /// </summary>
        /// <param name="badge">The badge to set.</param>
        /// <returns>true on success, false if otherwise.</returns>
        /// <seealso cref="ClearBadge"/>
        [AvailableSince(10, 0)]
        public static bool SetBadge(NavigatorBadge badge)
        {
            if (!badge.IsValidValue())
            {
                throw new ArgumentException("not a valid application badge.", "badge");
            }
            Util.GetBPSOrException();
            return navigator_set_badge(badge) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Clears a badge from the application's icon and window frame in the navigator.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        /// <seealso cref="SetBadge"/>
        [AvailableSince(10, 0)]
        public static bool ClearBadge()
        {
            Util.GetBPSOrException();
            return navigator_clear_badge() == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Turns keyboard tracking on or off.
        /// </summary>
        /// <param name="track">Whether to turn tracking on or off.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool EnableKeyboardTracking(bool track)
        {
            Util.GetBPSOrException();
            return navigator_set_keyboard_tracking(track) == BPS.BPS_SUCCESS;
        }

        #region Events

        /// <summary>
        /// Get the severity of a LowMemory event.
        /// </summary>
        /// <param name="ev">The LowMemory event to extract the severity from.</param>
        /// <returns>The severity (increasing amounts indicates a higher level of severity) from the event.</returns>
        [AvailableSince(10, 0)]
        public static int GetLowMemorySeverity(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.LowMemory)
            {
                throw new ArgumentException("BPSEvent is not a low memory event");
            }
            Util.GetBPSOrException();
            var result = navigator_event_get_severity(ev.DangerousGetHandle());
            if (result == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not retrieve severity from low memory event.");
            }
            return result;
        }

        /// <summary>
        /// Get the current window state from an event.
        /// </summary>
        /// <param name="ev">The WindowState event to extract the window state from.</param>
        /// <returns>The window state from the event.</returns>
        [AvailableSince(10, 0)]
        public static WindowState GetWindowState(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.WindowState)
            {
                throw new ArgumentException("BPSEvent is not a window state event");
            }
            Util.GetBPSOrException();
            return navigator_event_get_window_state(ev.DangerousGetHandle());
        }

        /// <summary>
        /// Get the group ID from an event.
        /// </summary>
        /// <param name="ev">The WindowState, WindowActive, or WindowInactive event to extract the group ID from.</param>
        /// <returns>The group ID from the event.</returns>
        [AvailableSince(10, 0)]
        public static string GetGroupID(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            switch ((NavigatorEvents)ev.Code)
            {
                case NavigatorEvents.WindowState:
                case NavigatorEvents.WindowActive:
                case NavigatorEvents.WindowInactive:
                    break;
                default:
                    throw new ArgumentException("BPSEvent is not a window state, window active, or window inactive event");
            }
            Util.GetBPSOrException();
            return Marshal.PtrToStringAnsi(navigator_event_get_groupid(ev.DangerousGetHandle()));
        }

        /// <summary>
        /// Get the orientation angle from a navigator event.
        /// </summary>
        /// <param name="ev">THe orientation-specific event to extract the orientation angle from, excludes OrientationSize.</param>
        /// <returns>The orientation angle from the event.</returns>
        [AvailableSince(10, 0)]
        public static int GetOrientationAngle(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            switch ((NavigatorEvents)ev.Code)
            {
                case NavigatorEvents.Orientation:
                case NavigatorEvents.OrientationCheck:
                case NavigatorEvents.OrientationDone:
                case NavigatorEvents.OrientationResult:
                    break;
                default:
                    throw new ArgumentException("BPSEvent is not a orientation-specific event");
            }
            Util.GetBPSOrException();
            return navigator_event_get_orientation_angle(ev.DangerousGetHandle());
        }

        /// <summary>
        /// Get the orientation mode from a navigator event.
        /// </summary>
        /// <param name="ev">The Orientation or OrientationCheck event to extract the orientation mode from.</param>
        /// <returns>OrientationMode</returns>
        [AvailableSince(10, 0)]
        public static OrientationMode GetOrientationMode(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            switch ((NavigatorEvents)ev.Code)
            {
                case NavigatorEvents.Orientation:
                case NavigatorEvents.OrientationCheck:
                    break;
                default:
                    throw new ArgumentException("BPSEvent is not a orientation or orientation check event");
            }
            Util.GetBPSOrException();
            return navigator_event_get_orientation_mode(ev.DangerousGetHandle());
        }

        /// <summary>
        /// Get the size from a navigator orientation size event.
        /// </summary>
        /// <param name="ev">The OrientationSize event.</param>
        /// <returns>The size of the window after rotation, in pixels.</returns>
        [AvailableSince(10, 0)]
        public static Size GetOrientationSize(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.OrientationSize)
            {
                throw new ArgumentException("BPSEvent is not a orientation size event");
            }
            Util.GetBPSOrException();
            var evPtr = ev.DangerousGetHandle();
            return new Size(navigator_event_get_orientation_size_width(evPtr), navigator_event_get_orientation_size_height(evPtr));
        }

        /// <summary>
        /// Get the keyboard state from a navigator event.
        /// </summary>
        /// <param name="ev">The KeyboardState event.</param>
        /// <returns>The state of the keyboard.</returns>
        [AvailableSince(10, 0)]
        public static KeyboardState GetKeyboardState(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.OrientationSize)
            {
                throw new ArgumentException("BPSEvent is not a keyboard state event");
            }
            Util.GetBPSOrException();
            var result = navigator_event_get_keyboard_state(ev.DangerousGetHandle());
            if (result == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not retrieve keyboard state.");
            }
            return (KeyboardState)result;
        }

        /// <summary>
        /// Get the keyboard position from a navigator event.
        /// </summary>
        /// <param name="ev">The KeyboardPosition event.</param>
        /// <returns>The y offset in pixels of the top of the keyboard.</returns>
        [AvailableSince(10, 0)]
        public static int GetKeyboardPosition(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.KeyboardPosition)
            {
                throw new ArgumentException("BPSEvent is not a keyboard position event");
            }
            Util.GetBPSOrException();
            var result = navigator_event_get_keyboard_position(ev.DangerousGetHandle());
            if (result == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not retrieve keyboard position.");
            }
            return result;
        }

        /// <summary>
        /// Get the size of the window cover.
        /// </summary>
        /// <param name="ev">The WindowCover event.</param>
        /// <returns>The size of the window cover.</returns>
        [AvailableSince(10, 0)]
        public static Size GetWindowCoverSize(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.WindowCover)
            {
                throw new ArgumentException("BPSEvent is not a window cover event");
            }
            Util.GetBPSOrException();
            var evPtr = ev.DangerousGetHandle();
            var w = navigator_event_get_window_cover_width(evPtr);
            if (w == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not get width of window cover", Util.GetExceptionForLastErrno());
            }
            var h = navigator_event_get_window_cover_height(evPtr);
            if (h == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not get height of window cover", Util.GetExceptionForLastErrno());
            }
            return new Size(w, h);
        }

        /// <summary>
        /// Get the data from a navigator invoke event.
        /// </summary>
        /// <param name="ev">The Invoke event to extract the data from.</param>
        /// <returns>The data field from the event.</returns>
        [AvailableSince(10, 0)]
        public static string GetData(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.Invoke)
            {
                throw new ArgumentException("BPSEvent is not a invoke event");
            }
            Util.GetBPSOrException();
            // Invoke events are caused by navigator_invoke calls, which has been deprecated and isn't included in this library. Even so, data should be the remainder of the URL used for invocation. AKA, a string.
            return Marshal.PtrToStringAnsi(navigator_event_get_data(ev.DangerousGetHandle()));
        }

        /// <summary>
        /// Get the ID from a navigator event.
        /// </summary>
        /// <param name="ev">The event to extract the ID from.</param>
        /// <returns>The ID field from the event.</returns>
        /// <seealso href="http://developer.blackberry.com/native/reference/core/com.qnx.doc.bps.lib_ref/topic/navigator_event_get_id.html#description">Supported Events</seealso>
        [AvailableSince(10, 0)]
        public static string GetID(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            switch ((NavigatorEvents)ev.Code)
            {
                case NavigatorEvents.Orientation:
                case NavigatorEvents.OrientationCheck:
                case NavigatorEvents.OrientationResult:
                case NavigatorEvents.InvokeTargetResult:
                case NavigatorEvents.InvokeQueryResult:
                case NavigatorEvents.InvokeViewerResult:
                case NavigatorEvents.InvokeViewerRelay:
                case NavigatorEvents.InvokeViewerRelayResult:
                case NavigatorEvents.InvokeGetFiltersResult:
                case NavigatorEvents.InvokeSetFiltersResult:
                case NavigatorEvents.Pooled:
                case NavigatorEvents.InvokeTimerRegistration:
                    break;
                default:
                    throw new ArgumentException("BPSEvent is not a supported event");
            }
            Util.GetBPSOrException();
            return Marshal.PtrToStringAnsi(navigator_event_get_id(ev.DangerousGetHandle()));
        }

        /// <summary>
        /// Get the error message from a navigator event.
        /// </summary>
        /// <param name="ev">The event to extract the error message from.</param>
        /// <returns>The error message from the event, or null if there is no error message.</returns>
        /// <seealso href="http://developer.blackberry.com/native/reference/core/com.qnx.doc.bps.lib_ref/topic/navigator_event_get_err.html#description">Supported Events</seealso>
        [AvailableSince(10, 0)]
        public static string GetError(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            switch ((NavigatorEvents)ev.Code)
            {
                case NavigatorEvents.OrientationResult:
                case NavigatorEvents.InvokeTargetResult:
                case NavigatorEvents.InvokeQueryResult:
                case NavigatorEvents.InvokeViewerResult:
                case NavigatorEvents.InvokeViewerRelayResult:
                case NavigatorEvents.InvokeGetFiltersResult:
                case NavigatorEvents.InvokeSetFiltersResult:
                case NavigatorEvents.InvokeTimerRegistration:
                    break;
                default:
                    // It can be assumed that this function will be called for more then just supported events. So pretend that there is no error with them.
                    //throw new ArgumentException("BPSEvent is not a supported event");
                    return null;
            }
            Util.GetBPSOrException();
            return Marshal.PtrToStringAnsi(navigator_event_get_err(ev.DangerousGetHandle()));
        }

        /// <summary>
        /// Specify whether your application intends to rotate.
        /// </summary>
        /// <param name="ev">The OrientationCheck event.</param>
        /// <param name="willRotate">true if your application intends to rotate, false if your application does not intend to rotate.</param>
        [AvailableSince(10, 0)]
        public static void OrientationCheckResponse(BPSEvent ev, bool willRotate)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.OrientationCheck)
            {
                throw new ArgumentException("BPSEvent is not a orientation check event");
            }
            Util.GetBPSOrException();
            navigator_orientation_check_response(ev.DangerousGetHandle(), willRotate);
        }

        /// <summary>
        /// Specify whether your application intends to rotate.
        /// </summary>
        /// <param name="id">The ID, as retrieved from the OrientationCheck event with <see cref="GetID"/>.</param>
        /// <param name="willRotate">true if your application intends to rotate, false if your application does not intend to rotate.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool OrientationCheckResponse(string id, bool willRotate)
        {
            Util.GetBPSOrException();
            return navigator_orientation_check_response_id(id, willRotate) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Indicate that your application is finished rotating.
        /// </summary>
        /// <param name="ev">The original Orientation event.</param>
        [AvailableSince(10, 0)]
        public static void OrientationDone(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.Orientation)
            {
                throw new ArgumentException("BPSEvent is not a orientation event");
            }
            Util.GetBPSOrException();
            navigator_done_orientation(ev.DangerousGetHandle());
        }

        /// <summary>
        /// Indicate that your application is finished rotating.
        /// </summary>
        /// <param name="id">The ID, as retrieved from the Orientation event with <see cref="GetID"/>.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool OrientationDone(string id)
        {
            Util.GetBPSOrException();
            return navigator_done_orientation_id(id) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Indicate that your application wants to be pooled.
        /// </summary>
        /// <param name="id">The ID, as retrieved from the Pooled event using <see cref="GetID"/>.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestPooling(string id)
        {
            Util.GetBPSOrException();
            return navigator_pooled_response(id) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Gets extended data from the event if it is available.
        /// </summary>
        /// <param name="ev">The original event.</param>
        /// <returns>A buffer of extended data, null if no data was available. An error will be thrown if one occurs.</returns>
        [AvailableSince(10, 0)]
        public static byte[] GetExtendedData(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            var evPtr = ev.DangerousGetHandle();
            var data = navigator_event_get_extended_data(evPtr);
            var exp = Util.GetExceptionForLastErrno();
            if (exp != null)
            {
                throw new InvalidOperationException("Could not get extended data from event", exp);
            }
            if (data == IntPtr.Zero)
            {
                return null;
            }
            var dataLen = navigator_event_get_extended_data_length(evPtr);
            var result = new byte[dataLen];
            Marshal.Copy(data, result, 0, (int)dataLen);
            return result;
        }

        /// <summary>
        /// Get the device lock state from a DeviceLockState event.
        /// </summary>
        /// <param name="ev">A DeviceLockState event.</param>
        /// <returns>Device lock state.</returns>
        [AvailableSince(10, 0)]
        public static LockState GetLockState(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.DeviceLockState)
            {
                throw new ArgumentException("BPSEvent is not a device lock state event");
            }
            Util.GetBPSOrException();
            var res = navigator_event_get_device_lock_state(ev.DangerousGetHandle());
            if (res == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not get lock state");
            }
            return (LockState)res;
        }

        /// <summary>
        /// Get the app state from a AppState event.
        /// </summary>
        /// <param name="ev">The AppState event.</param>
        /// <returns>AppState.</returns>
        [AvailableSince(10, 0)]
        public static AppState GetAppState(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.AppState)
            {
                throw new ArgumentException("BPSEvent is not a app state event");
            }
            Util.GetBPSOrException();
            var res = navigator_event_get_app_state(ev.DangerousGetHandle());
            if (res == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not get app state");
            }
            return (AppState)res;
        }

        /// <summary>
        /// Retrieve the type of the card peek action.
        /// </summary>
        /// <param name="ev">The CardPeekStarted event.</param>
        /// <returns>The card peek type.</returns>
        [AvailableSince(10, 0)]
        public static PeekType GetCardPeekType(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.CardPeekStarted)
            {
                throw new ArgumentException("BPSEvent is not a card peek started event");
            }
            Util.GetBPSOrException();
            var res = navigator_event_get_card_peek_type(ev.DangerousGetHandle());
            if (res == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not get card peek type.", Util.GetExceptionForLastErrno());
            }
            return (PeekType)res;
        }

        /// <summary>
        /// Retrieve the type of the peek action initiated on this card.
        /// </summary>
        /// <param name="ev">The PeekStarted event.</param>
        /// <returns>The peek type.</returns>
        [AvailableSince(10, 0)]
        public static PeekType GetPeekType(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.PeekStarted)
            {
                throw new ArgumentException("BPSEvent is not a peek started event");
            }
            Util.GetBPSOrException();
            var res = navigator_event_get_peek_type(ev.DangerousGetHandle());
            if (res == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not get peek type.", Util.GetExceptionForLastErrno());
            }
            return (PeekType)res;
        }

        /// <summary>
        /// Retrieve whether a card peek stopped due to a swipe away gesture.
        /// </summary>
        /// <param name="ev">The CardPeekStopped event.</param>
        /// <returns>true if the card peek action stopped due to a swipe away gesture, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool GetSwipeAwayStoppedCardPeek(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.CardPeekStopped)
            {
                throw new ArgumentException("BPSEvent is not a card peek stopped event");
            }
            Util.GetBPSOrException();
            bool swipeAway;
            var res = navigator_event_get_card_peek_stopped_swipe_away(ev.DangerousGetHandle(), out swipeAway) == BPS.BPS_SUCCESS;
            if (res)
            {
                return swipeAway;
            }
            return false;
        }

        /// <summary>
        /// Retrieve whether this card's peek stopped due to a swipe away gesture.
        /// </summary>
        /// <param name="ev">The PeekStopped event.</param>
        /// <returns>true if the card peek action stopped due to a swipe away gesture, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool GetSwipeAwayStoppedPeek(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.PeekStopped)
            {
                throw new ArgumentException("BPSEvent is not a peek stopped event");
            }
            Util.GetBPSOrException();
            bool swipeAway;
            var res = navigator_event_get_peek_stopped_swipe_away(ev.DangerousGetHandle(), out swipeAway) == BPS.BPS_SUCCESS;
            if (res)
            {
                return swipeAway;
            }
            return false;
        }

        /// <summary>
        /// Retrieve the card size from the card resize event.
        /// </summary>
        /// <param name="ev">The CardResize event.</param>
        /// <returns>The card size.</returns>
        [AvailableSince(10, 0)]
        public static Size GetCardSize(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.CardResize)
            {
                throw new ArgumentException("BPSEvent is not a card resize event");
            }
            Util.GetBPSOrException();
            var evPtr = ev.DangerousGetHandle();
            var w = navigator_event_get_card_width(evPtr);
            if (w == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not get width of card", Util.GetExceptionForLastErrno());
            }
            var h = navigator_event_get_card_height(evPtr);
            if (h == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not get height of card", Util.GetExceptionForLastErrno());
            }
            return new Size(w, h);
        }

        /// <summary>
        /// Retrieve the card edge type from the card resize event.
        /// </summary>
        /// <param name="ev">The CardResize event.</param>
        /// <returns>The card edge.</returns>
        [AvailableSince(10, 0)]
        public static NavigatorOrientation GetCardEdge(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.CardResize)
            {
                throw new ArgumentException("BPSEvent is not a card resize event");
            }
            Util.GetBPSOrException();
            var res = navigator_event_get_card_edge(ev.DangerousGetHandle());
            if (res == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return (NavigatorOrientation)res;
        }

        /// <summary>
        /// Retrieve the orientation type from the card resize event.
        /// </summary>
        /// <param name="ev">The CardResize event.</param>
        /// <returns>The card resize type.</returns>
        [AvailableSince(10, 0)]
        public static OrientationMode GetCardOrientation(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.CardResize)
            {
                throw new ArgumentException("BPSEvent is not a card resize event");
            }
            Util.GetBPSOrException();
            var res = navigator_event_get_card_orientation(ev.DangerousGetHandle());
            if (res == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return (OrientationMode)res;
        }

        /// <summary>
        /// Retrieve the reason for a card closure.
        /// </summary>
        /// <param name="ev">The ChildCardClosed or CardClosed event that informed the application of the card closure.</param>
        /// <returns>The reason why the card was closed.</returns>
        [AvailableSince(10, 0)]
        public static string GetCardClosedReason(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            switch ((NavigatorEvents)ev.Code)
            {
                case NavigatorEvents.ChildCardClosed:
                case NavigatorEvents.CardClosed:
                    break;
                default:
                    throw new ArgumentException("BPSEvent is not a ChildCardClosed or CardClosed event");
            }
            Util.GetBPSOrException();
            return Marshal.PtrToStringAnsi(navigator_event_get_card_closed_reason(ev.DangerousGetHandle()));
        }

        /// <summary>
        /// Retrieve the type of data passed by the child card upon closure
        /// </summary>
        /// <param name="ev">The ChildCardClosed event that informed the parent application of the child card closure.</param>
        /// <returns>The type of the data passed from the child card.</returns>
        [AvailableSince(10, 0)]
        public static string GetCardClosedDataType(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.ChildCardClosed)
            {
                throw new ArgumentException("BPSEvent is not a child card closed event");
            }
            Util.GetBPSOrException();
            return Marshal.PtrToStringAnsi(navigator_event_get_card_closed_data_type(ev.DangerousGetHandle()));
        }

        /// <summary>
        /// Retrieve the data passed by the child card upon closure.
        /// </summary>
        /// <param name="ev">The ChildCardClosed event that informed the parent application of the child card closure.</param>
        /// <returns>The data passed from the child card.</returns>
        [AvailableSince(10, 0)]
        public static string GetCardClosedData(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.ChildCardClosed)
            {
                throw new ArgumentException("BPSEvent is not a child card closed event");
            }
            Util.GetBPSOrException();
            return Mono.Unix.UnixMarshal.PtrToString(navigator_event_get_card_closed_data(ev.DangerousGetHandle()));
        }

        /// <summary>
        /// Get the system key from a navigator system key event.
        /// </summary>
        /// <param name="ev">The SystemKeyPress event to extract the system key from.</param>
        /// <returns>The system key from the event.</returns>
        [AvailableSince(10, 3, 1)]
        public static SystemKey GetSystemKey(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.SystemKeyPress)
            {
                throw new ArgumentException("BPSEvent is not a system key press event");
            }
            Util.GetBPSOrException();
            var res = navigator_event_get_syskey_key(ev.DangerousGetHandle());
            if (res == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return (SystemKey)res;
        }

        /// <summary>
        /// Get the system key id from a navigator system key event.
        /// </summary>
        /// The ID extracted from the event with this function should be passed into <see cref="WillHandleSystemKey"/> which
        /// should be called as soon as possible after recieving the SystemKeyPress event.
        /// <param name="ev">The SystemKeyPress event to extract the system key id from.</param>
        /// <returns>The system key id from the event.</returns>
        [AvailableSince(10, 3, 1)]
        public static string GetSystemKeyID(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.SystemKeyPress)
            {
                throw new ArgumentException("BPSEvent is not a system key press event");
            }
            Util.GetBPSOrException();
            return Marshal.PtrToStringAnsi(navigator_event_get_syskey_id(ev.DangerousGetHandle()));
        }

        #endregion

        /// <summary>
        /// Inform navigator that the app wishes to exit.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestExit()
        {
            Util.GetBPSOrException();
            return navigator_close_window() == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Query the navigator to determine the lock state.
        /// </summary>
        /// <returns>The current lock state of the device.</returns>
        [AvailableSince(10, 0)]
        public static LockState GetCurrentLockState()
        {
            Util.GetBPSOrException();
            var res = navigator_get_device_lock_state();
            if (res == BPS.BPS_FAILURE)
            {
                throw new InvalidOperationException("Could not get lock state");
            }
            return (LockState)res;
        }

        /// <summary>
        /// Sends data to the navigator service.
        /// </summary>
        /// <param name="data">The data to be sent.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RawWrite(byte[] data)
        {
            Util.GetBPSOrException();
            var ptr = Util.SerializeToPointer(data, GCHandleType.Pinned);
            int result = navigator_raw_write(ptr, (uint)data.Length);
            Util.FreeSerializePointer(ptr);
            return result == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Update the window cover used by the navigator.
        /// </summary>
        /// <param name="cover">A window cover to use.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool UpdateWindowCover(WindowCover cover)
        {
            if (cover == null)
            {
                throw new ArgumentNullException("cover");
            }
            if (!cover.IsValid)
            {
                throw new ArgumentException("Window cover is not valid.", "cover");
            }
            Util.GetBPSOrException();
            return navigator_window_cover_update(cover.DangerousGetHandle()) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Reset the window cover to the system default.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool ResetWindowCover()
        {
            Util.GetBPSOrException();
            return navigator_window_cover_reset() == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Request the Navigator to perform the card peek action.
        /// </summary>
        /// <param name="type">The type of peek to perform.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestCardPeek(PeekType type)
        {
            if (!type.IsValidValue())
            {
                throw new ArgumentException("not a valid peek type.", "type");
            }
            Util.GetBPSOrException();
            return navigator_card_peek(type) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Request the Navigator to notify the card when its window is ready.
        /// </summary>
        /// <param name="check">Whether to be notified by navigator before the card's window is shown.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestCardReadyCheck(bool check)
        {
            Util.GetBPSOrException();
            return navigator_card_request_card_ready_check(check) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Notify the Navigator to display the card's window.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestCardReady()
        {
            Util.GetBPSOrException();
            return navigator_card_send_card_ready() == BPS.BPS_SUCCESS;
        }

        #region CloseCard

        private static bool CloseCardInternal(string reason, string type, string data)
        {
            Util.GetBPSOrException();
            var dataPtr = IntPtr.Zero;
            if (data != null)
            {
                // Would rather not allocate memory, but this will let the GC work and then we can cleanup code later
                dataPtr = Mono.Unix.UnixMarshal.StringToHeap(data);
                if (dataPtr == IntPtr.Zero)
                {
                    throw new InvalidOperationException("CloseCardInternal: Could not convert data to pointer");
                }
                VerifyCleanupSetup();
                cardClosedPointersToCleanup.Add(dataPtr);
            }
            return navigator_card_close(reason, type, dataPtr) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Close the card.
        /// </summary>
        /// <param name="reason">The application level description of why the card was closed.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool CloseCard(string reason = null)
        {
            return CloseCardInternal(reason, null, null);
        }

        /// <summary>
        /// Close the card.
        /// </summary>
        /// <param name="type">The type and encoding of the closed card's response data.</param>
        /// <param name="data">The data being returned to the parent from the closed card.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool CloseCard(string type, string data)
        {
            if (data != null && type == null)
            {
                throw new ArgumentNullException("Type and encoding is required if data is not null", "data");
            }
            return CloseCardInternal(null, type, data);
        }

        /// <summary>
        /// Close the card.
        /// </summary>
        /// <param name="reason">The application level description of why the card was closed.</param>
        /// <param name="type">The type and encoding of the closed card's response data.</param>
        /// <param name="data">The data being returned to the parent from the closed card.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool CloseCard(string reason, string type, string data)
        {
            if (data != null && type == null)
            {
                throw new ArgumentNullException("Type and encoding is required if data is not null", "data");
            }
            return CloseCardInternal(reason, type, data);
        }

        #endregion

        /// <summary>
        /// Close the card.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool CloseCardChild()
        {
            Util.GetBPSOrException();
            return navigator_card_close_child() == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Inform the Navigator that the card has been resized.
        /// </summary>
        /// <param name="id">The ID retrieved from the @c CardResize event corresponding to the card resize instance.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool CardResized(string id)
        {
            Util.GetBPSOrException();
            return navigator_card_resized(id) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Inform the Navigator that a swipe away gesture has been performed.
        /// </summary>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool SwipeAwayCard()
        {
            Util.GetBPSOrException();
            return navigator_card_swipe_away() == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Change the device's wallpaper.
        /// </summary>
        /// <param name="file">Path to an image file that will be used as the wallpaper.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool SetWallpaper(string file)
        {
            Util.GetBPSOrException();
            return navigator_set_wallpaper(file) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Change the device's lockscreen wallpaper.
        /// </summary>
        /// <param name="file">Path to an image file that will be used as the lockscreen wallpaper.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 3)]
        public static bool SetLockscreenWallpaper(string file)
        {
            Util.GetBPSOrException();
            return navigator_set_lockscreen_wallpaper(file) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Indicate whether the app handles a system key event.
        /// </summary>
        /// <param name="id">The id that was returned in the NavigatorEvents.SystemKeyPress event.</param>
        /// <param name="willHandle">True if your app will handle the event, false if it will not handle the event.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 3, 1)]
        public static bool WillHandleSystemKey(string id, bool willHandle)
        {
            Util.GetBPSOrException();
            return navigator_syskey_press_response(id, willHandle) == BPS.BPS_SUCCESS;
        }
    }
}
