using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// The window cover text attribute structure.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class WindowCoverLabel : IDisposable
    {
        private IntPtr handle;
        private WindowCover owner;
        private Color color = Color.White; //XXX
        private string label;
        private int fontSize = 12; //XXX
        private bool wrapText = false; //XXX

        internal WindowCoverLabel(IntPtr handle, string label, WindowCover owner)
        {
            this.handle = handle;
            this.label = label;
            this.owner = owner;
        }

        /// <summary>
        /// Destroy the window cover label.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (IsValid)
            {
                if (Navigator.navigator_window_cover_label_destroy(handle) == BPS.BPS_SUCCESS)
                {
                    handle = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Get if the label is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return handle != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Get the owner of the label.
        /// </summary>
        public WindowCover WindowCover
        {
            get
            {
                return owner;
            }
        }

        /// <summary>
        /// Get or set label color. Alpha is ignored.
        /// </summary>
        [AvailableSince(10, 0)]
        public Color Color
        {
            [AvailableSince(10, 0)]
            get
            {
                if (!IsValid)
                {
                    throw new ObjectDisposedException("WindowCoverLabel");
                }
                return color;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (!IsValid)
                {
                    throw new ObjectDisposedException("WindowCoverLabel");
                }
                if (value != color && Navigator.navigator_window_cover_label_set_color(handle, value.R, value.G, value.B) == BPS.BPS_SUCCESS)
                {
                    color = value;
                }
            }
        }

        /// <summary>
        /// Get or set label text.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Label
        {
            [AvailableSince(10, 0)]
            get
            {
                if (!IsValid)
                {
                    throw new ObjectDisposedException("WindowCoverLabel");
                }
                return label;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (!IsValid)
                {
                    throw new ObjectDisposedException("WindowCoverLabel");
                }
                if (value != label && Navigator.navigator_window_cover_label_set_text(handle, value) == BPS.BPS_SUCCESS)
                {
                    label = value;
                }
            }
        }

        /// <summary>
        /// Get or set font size of the label's text.
        /// </summary>
        [AvailableSince(10, 0)]
        public int FontSize
        {
            [AvailableSince(10, 0)]
            get
            {
                if (!IsValid)
                {
                    throw new ObjectDisposedException("WindowCoverLabel");
                }
                return fontSize;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (!IsValid)
                {
                    throw new ObjectDisposedException("WindowCoverLabel");
                }
                if (value != fontSize)
                {
                    if (value <= 0)
                    {
                        throw new ArgumentOutOfRangeException("FontSize", value, "must be greater than 0");
                    }
                    if (Navigator.navigator_window_cover_label_set_size(handle, value) == BPS.BPS_SUCCESS)
                    {
                        fontSize = value;
                    }
                }
            }
        }

        /// <summary>
        /// Get or set whether text will wrap.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool WrapText
        {
            [AvailableSince(10, 0)]
            get
            {
                if (!IsValid)
                {
                    throw new ObjectDisposedException("WindowCoverLabel");
                }
                return wrapText;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (!IsValid)
                {
                    throw new ObjectDisposedException("WindowCoverLabel");
                }
                if (value != wrapText && Navigator.navigator_window_cover_label_set_wrap_text(handle, value) == BPS.BPS_SUCCESS)
                {
                    wrapText = value;
                }
            }
        }
    }
}
