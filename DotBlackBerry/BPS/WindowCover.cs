using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Window cover display mode.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum WindowCoverMode
    {
        /// <summary>
        /// Live display mode. Permission is required to set this.
        /// </summary>
        [AvailableSince(10, 0)]
        Live,
        /// <summary>
        /// Alternative window display mode.
        /// </summary>
        [AvailableSince(10, 0)]
        AlternateWindow,
        /// <summary>
        /// File-based display mode.
        /// </summary>
        [AvailableSince(10, 0)]
        File,
        /// <summary>
        /// Capture (sub-region of UI) display mode.
        /// </summary>
        [AvailableSince(10, 0)]
        Capture
    }

    /// <summary>
    /// The window cover image attribute structure.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class WindowCover : IDisposable
    {
        private List<WindowCoverLabel> labels = new List<WindowCoverLabel>();
        private IntPtr handle;
        private bool allowBadges = true; //XXX
        private WindowCoverTransition transition = WindowCoverTransition.Default;
        private WindowCoverMode mode = WindowCoverMode.Capture; //XXX

        /// <summary>
        /// Create a new window cover.
        /// </summary>
        [AvailableSince(10, 0)]
        public WindowCover()
        {
            if (Navigator.navigator_window_cover_attribute_create(out handle) != BPS.BPS_SUCCESS)
            {
                handle = IntPtr.Zero;
                Util.ThrowExceptionForLastErrno();
            }
        }

        /// <summary>
        /// Destroy the window cover.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (IsValid)
            {
                foreach (var label in labels)
                {
                    label.Dispose();
                }
                labels.Clear();
                if (Navigator.navigator_window_cover_attribute_destroy(handle) == BPS.BPS_SUCCESS)
                {
                    handle = IntPtr.Zero;
                }
            }
        }

        private void CheckState()
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException("ScreenCover");
            }
            for (var i = labels.Count - 1; i >= 0; i--)
            {
                if (!labels[i].IsValid)
                {
                    labels.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Get if the cover is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return handle != IntPtr.Zero;
            }
        }

        internal IntPtr DangerousGetHandle()
        {
            return handle;
        }

        /// <summary>
        /// Get the window cover display mode.
        /// </summary>
        [AvailableSince(10, 0)]
        public WindowCoverMode Mode
        {
            [AvailableSince(10, 0)]
            get
            {
                CheckState();
                return mode;
            }
        }

        /// <summary>
        /// Change display mode to either live or alternaive window display mode.
        /// </summary>
        /// <param name="live">true if mode should be live, false if it should be alternative window (window is sourced from <see cref="BlackBerry.Screen.Property.AlternativeWindow"/>).</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool ChangeDisplayMode(bool live)
        {
            CheckState();
            var success = (live ? Navigator.navigator_window_cover_attribute_set_live(handle) : Navigator.navigator_window_cover_attribute_set_alternate_window(handle)) == BPS.BPS_SUCCESS;
            if (success)
            {
                mode = live ? WindowCoverMode.Live : WindowCoverMode.AlternateWindow;
            }
            return success;
        }

        /// <summary>
        /// Change display mode to a file-based display mode.
        /// </summary>
        /// <param name="file">Path to an image file that will be used as the cover.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool ChangeDisplayMode(string file)
        {
            CheckState();
            var success = Navigator.navigator_window_cover_attribute_set_file(handle, file) == BPS.BPS_SUCCESS;
            if (success)
            {
                mode = WindowCoverMode.File;
            }
            return success;
        }

        /// <summary>
        /// Change display mode to a capture display mode.
        /// </summary>
        /// <param name="x">The X-axis origin.</param>
        /// <param name="y">The Y-axis origin.</param>
        /// <param name="width">The capture width.</param>
        /// <param name="height">The capture height.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool ChangeDisplayMode(int x, int y, int width, int height)
        {
            CheckState();
            var success = Navigator.navigator_window_cover_attribute_set_capture(handle, x, y, width, height) == BPS.BPS_SUCCESS;
            if (success)
            {
                mode = WindowCoverMode.Capture;
            }
            return success;
        }

        /// <summary>
        /// Change display mode to a capture display mode.
        /// </summary>
        /// <param name="size">The capture size to use.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool ChangeDisplayMode(Rectangle size)
        {
            return ChangeDisplayMode(size.X, size.Y, size.Width, size.Height);
        }

        /// <summary>
        /// Get or set whether badges will be allowed on the window cover.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool AllowBadges
        {
            [AvailableSince(10, 0)]
            get
            {
                CheckState();
                return allowBadges;
            }
            [AvailableSince(10, 0)]
            set
            {
                CheckState();
                if (value != allowBadges && Navigator.navigator_window_cover_attribute_set_allow_badges(handle, value) == BPS.BPS_SUCCESS)
                {
                    allowBadges = value;
                }
            }
        }

        /// <summary>
        /// Get or set the type of transition to use when displaying a new window cover.
        /// </summary>
        [AvailableSince(10, 0)]
        public WindowCoverTransition Transition
        {
            [AvailableSince(10, 0)]
            get
            {
                CheckState();
                return transition;
            }
            [AvailableSince(10, 0)]
            set
            {
                CheckState();
                if (value != transition)
                {
                    if (!value.IsValidValue())
                    {
                        throw new ArgumentException("not a valid transition.", "Transition");
                    }
                    if (Navigator.navigator_window_cover_attribute_set_transition(handle, value) == BPS.BPS_SUCCESS)
                    {
                        transition = value;
                    }
                }
            }
        }

        /// <summary>
        /// Add a label to the window cover.
        /// </summary>
        /// <param name="text">The text for the label.</param>
        /// <returns>The label, or null if an error occured.</returns>
        [AvailableSince(10, 0)]
        public WindowCoverLabel AddLabel(string text)
        {
            CheckState();
            IntPtr label;
            if (Navigator.navigator_window_cover_attribute_add_label(handle, text, out label) == BPS.BPS_SUCCESS)
            {
                var result = new WindowCoverLabel(label, text, this);
                labels.Add(result);
                return result;
            }
            return null;
        }
    }
}
