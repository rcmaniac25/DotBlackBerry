using System;
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
        /// Indicates that a request to change the orientation with <see cref="SetOrientation"/> has completed.
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
        /// Indicates that the card application should resize its buffer and call the <see cref="CardResized"/> function when finished.
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

    #endregion

    /// <summary>
    /// Send and recieve messages from the navigator, which controls how applications appear on the device.
    /// </summary>
    [AvailableSince(10, 0)]
    public static class Navigator
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int navigator_request_events(NavigatorRequestEvents flags);

        [DllImport("bps")]
        private static extern int navigator_stop_events(int flags);

        [DllImport("bps")]
        private static extern int navigator_get_domain();

        [DllImport("bps")]
        private static extern int navigator_add_uri([MarshalAs(UnmanagedType.LPStr)]string icon_path, [MarshalAs(UnmanagedType.LPStr)]string icon_label, [MarshalAs(UnmanagedType.LPStr)]string default_category, [MarshalAs(UnmanagedType.LPStr)]string url, [In, Out]ref IntPtr err);

        [DllImport("bps")]
        private static extern int navigator_extend_timeout(int extension, [In, Out]ref IntPtr err);

        [DllImport("bps")]
        private static extern int navigator_extend_terminate();

        [DllImport("bps")]
        private static extern int navigator_request_swipe_start();

        [DllImport("bps")]
        private static extern int navigator_stop_swipe_start();

        [DllImport("bps")]
        private static extern int navigator_rotation_lock(bool locked);

        [DllImport("bps")]
        private static extern int navigator_set_orientation(int angle, [In, Out]ref IntPtr id);

        [DllImport("bps")]
        private static extern int navigator_set_orientation_mode(int mode, [In, Out]ref IntPtr id);

        [DllImport("bps")]
        private static extern int navigator_set_window_angle(int angle);

        [DllImport("bps")]
        private static extern int navigator_rotation_effect(bool effect);

        [DllImport("bps")]
        private static extern int navigator_set_close_prompt([MarshalAs(UnmanagedType.LPStr)]string title, [MarshalAs(UnmanagedType.LPStr)]string message);

        [DllImport("bps")]
        private static extern int navigator_clear_close_prompt();

        [DllImport("bps")]
        private static extern int navigator_set_badge(NavigatorBadge badge);

        [DllImport("bps")]
        private static extern int navigator_clear_badge();

        [DllImport("bps")]
        private static extern int navigator_set_keyboard_tracking(bool track);

        [DllImport("bps")]
        private static extern int navigator_event_get_severity(IntPtr ev);

        [DllImport("bps")]
        private static extern WindowState navigator_event_get_window_state(IntPtr ev);

        [DllImport("bps")]
        private static extern IntPtr navigator_event_get_groupid(IntPtr ev);

        [DllImport("bps")]
        private static extern int navigator_event_get_orientation_angle(IntPtr ev);

        [DllImport("bps")]
        private static extern OrientationMode navigator_event_get_orientation_mode(IntPtr ev);

        [DllImport("bps")]
        private static extern int navigator_event_get_orientation_size_width(IntPtr ev);

        [DllImport("bps")]
        private static extern int navigator_event_get_orientation_size_height(IntPtr ev);

        [DllImport("bps")]
        private static extern KeyboardState navigator_event_get_keyboard_state(IntPtr ev);

        [DllImport("bps")]
        private static extern int navigator_event_get_keyboard_position(IntPtr ev);

        [DllImport("bps")]
        private static extern int navigator_event_get_window_cover_height(IntPtr ev);

        [DllImport("bps")]
        private static extern int navigator_event_get_window_cover_width(IntPtr ev);

        [DllImport("bps")]
        private static extern IntPtr navigator_event_get_data(IntPtr ev);

        [DllImport("bps")]
        private static extern IntPtr navigator_event_get_id(IntPtr ev);

        [DllImport("bps")]
        private static extern IntPtr navigator_event_get_err(IntPtr ev);

        [DllImport("bps")]
        private static extern void navigator_orientation_check_response(IntPtr ev, bool will_rotate);

        [DllImport("bps")]
        private static extern int navigator_orientation_check_response_id([MarshalAs(UnmanagedType.LPStr)]string id, bool will_rotate);

        [DllImport("bps")]
        private static extern void navigator_done_orientation(IntPtr ev);

        [DllImport("bps")]
        private static extern int navigator_done_orientation_id([MarshalAs(UnmanagedType.LPStr)]string id);

        [DllImport("bps")]
        private static extern int navigator_close_window();

        [DllImport("bps")]
        private static extern int navigator_pooled_response([MarshalAs(UnmanagedType.LPStr)]string id);

        [DllImport("bps")]
        private static extern int navigator_get_device_lock_state();

        //TODO

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
                throw new ArgumentOutOfRangeException("extension", "Extension cannot be negative.");
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

        //TODO
    }
}
