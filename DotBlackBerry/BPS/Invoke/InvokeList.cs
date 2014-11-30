using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Invoke
{
    //XXX The docs for this are terrible. If anyone has better explanations (or method signatures/usage/etc.), please do a pull request.

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
    /// Helper class for dealing with lists of invocations.
    /// </summary>
    /// <remarks>
    /// Invocation lsist appear to be a little known power feature for card invocations to open and handle multiple invocations at the same time.
    /// To see a short description, read the last couple paraphraphs of <see href="https://developer.blackberry.com/native/reference/cascades/bb__system__invokemanager.html">InvokeManager</see>.
    /// </remarks>
    /// <seealso cref="Invocation.ListID">Invocation.ListID</seealso>
    [AvailableSince(10, 2)]
    public static class InvokeList
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_select_list_item(InvokeListCursorDirection selection);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_event_get_list_id(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_event_get_list_cursor_direction(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int navigator_invoke_event_get_list_item_selection(IntPtr ev);

        #endregion

        /// <summary>
        /// Request that the list item specified by <paramref name="selection"/> is invoked to replace the current list item that this application belongs to.
        /// </summary>
        /// <param name="selection">The list item to invoke.</param>
        /// <returns>true if the request was sent successfully, false if otherwise.</returns>
        [AvailableSince(10, 2)]
        public static bool ReplaceWith(InvokeListCursorDirection selection)
        {
            if (selection == InvokeListCursorDirection.Unspecified)
            {
                throw new ArgumentException("Selection cannot be \"Unspecified\"", "selection");
            }
            else if (!selection.IsValidValue())
            {
                throw new ArgumentException("Unsupported selection value", "selection");
            }
            Util.GetBPSOrException();
            return navigator_invoke_select_list_item(selection) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get the direction the cursor for a list has moved.
        /// </summary>
        /// <param name="ev">The <see cref="NavigatorEvents.InvokeListCursorMoved">InvokeListCursorMoved</see> event to get the cursor movement direction from.</param>
        /// <param name="listID">The list ID that has had it's cursor changed.</param>
        /// <returns>The direction the cursor has moved.</returns>
        [AvailableSince(10, 2)]
        public static InvokeListCursorDirection CursorMoved(BPSEvent ev, out int listID)
        {
            if (ev.Domain != Navigator.Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.InvokeListCursorMoved)
            {
                throw new ArgumentException("BPSEvent is not a invoke list cursor-moved event");
            }
            var ptr = ev.DangerousGetHandle();
            var id = navigator_invoke_event_get_list_id(ptr);
            if (id == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            listID = id;
            var res = navigator_invoke_event_get_list_cursor_direction(ptr);
            if (res == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return (InvokeListCursorDirection)res;
        }

        /// <summary>
        /// Get when a transition has been requested from the currently active list item to a new item.
        /// </summary>
        /// <param name="ev">The <see cref="NavigatorEvents.InvokeListCursorMoved">InvokeListCursorMoved</see> event to get which direction to move on success.</param>
        /// <param name="listID">The list ID that has a selection.</param>
        /// <returns>The direction of the item to move to with <see cref="ReplaceWith">ReplaceWith</see> once the card has completed.</returns>
        [AvailableSince(10, 2)]
        public static InvokeListCursorDirection Selected(BPSEvent ev, out int listID)
        {
            if (ev.Domain != Navigator.Domain)
            {
                throw new ArgumentException("BPSEvent is not a navigator event");
            }
            if ((NavigatorEvents)ev.Code != NavigatorEvents.InvokeListItemSelected)
            {
                throw new ArgumentException("BPSEvent is not a invoke list cursor-selected event");
            }
            var ptr = ev.DangerousGetHandle();
            var id = navigator_invoke_event_get_list_id(ptr);
            if (id == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            listID = id;
            var res = navigator_invoke_event_get_list_item_selection(ptr);
            if (res == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return (InvokeListCursorDirection)res;
        }
    }
}
