using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Text for the Enter key on the virtual keyboard.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum VirtualKeyboardEnter : int
    {
        /// <summary>
        /// The default Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Default = 0,
        /// <summary>
        /// Display "Go" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Go = 1,
        /// <summary>
        /// Display "Join" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Join = 2,
        /// <summary>
        /// Display "Next" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Next = 3,
        /// <summary>
        /// Display "Search" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Search = 4,
        /// <summary>
        /// Display "Send" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Send = 5,
        /// <summary>
        /// Display "Submit" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Submit = 6,
        /// <summary>
        /// Display "Done" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Done = 7,
        /// <summary>
        /// Display "Connect" on the Enter key.
        /// </summary>
        [AvailableSince(10, 0)]
        Connect = 8,
        /// <summary>
        /// Display "Replace" on the Enter key.
        /// </summary>
        [AvailableSince(10, 2)]
        Replace = 9
    }

    /// <summary>
    /// Virtual keyboard.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class VirtualKeyboard
    {
        //TODO
    }
}
