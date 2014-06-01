using System;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Dialog
{
    /// <summary>
    /// The pre-defined icons for context menus.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum ContextMenuIcon : int
    {
        /// <summary>
        /// The cut icon.
        /// </summary>
        [AvailableSince(10, 0)]
        Cut = 0,
        /// <summary>
        /// The copy icon.
        /// </summary>
        [AvailableSince(10, 0)]
        Copy = 1,
        /// <summary>
        /// The paste icon.
        /// </summary>
        [AvailableSince(10, 0)]
        Paste = 2,
        /// <summary>
        /// The delete icon.
        /// </summary>
        [AvailableSince(10, 0)]
        Delete = 3,
        /// <summary>
        /// The select icon.
        /// </summary>
        [AvailableSince(10, 0)]
        Select = 4,
        /// <summary>
        /// The cancel icon.
        /// </summary>
        [AvailableSince(10, 0)]
        Cancel = 5,
        /// <summary>
        /// The view image icon.
        /// </summary>
        [AvailableSince(10, 0)]
        ViewImage = 6,
        /// <summary>
        /// The save image icon.
        /// </summary>
        [AvailableSince(10, 0)]
        SaveImage = 7,
        /// <summary>
        /// The save link as icon.
        /// </summary>
        [AvailableSince(10, 0)]
        SaveLinkAs = 8,
        /// <summary>
        /// The open link in new tab icon.
        /// </summary>
        [AvailableSince(10, 0)]
        OpenLinkNewTab = 9,
        /// <summary>
        /// The open link icon.
        /// </summary>
        [AvailableSince(10, 0)]
        OpenLink = 10,
        /// <summary>
        /// The copy link icon.
        /// </summary>
        [AvailableSince(10, 0)]
        CopyLink = 11,
        /// <summary>
        /// The copy image link icon.
        /// </summary>
        [AvailableSince(10, 0)]
        CopyImageLink = 12,
        /// <summary>
        /// The clear field icon.
        /// </summary>
        [AvailableSince(10, 0)]
        ClearField = 13,
        /// <summary>
        /// The cancel selection icon.
        /// </summary>
        [AvailableSince(10, 0)]
        CancelSelection = 14,
        /// <summary>
        /// The bookmark icon.
        /// </summary>
        [AvailableSince(10, 0)]
        Bookmark = 15,
        /// <summary>
        /// No icon.
        /// </summary>
        [AvailableSince(10, 0)]
        NoIcon = 16,
        /// <summary>
        /// For internal use only.
        /// </summary>
        [AvailableSince(10, 0)]
        KeepIcon = 17
    }

    /// <summary>
    /// A context menu dialog shows a menu of buttons, each of which has text and/or icons.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class ContextMenu : Dialog
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_create_context_menu(out IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_context_menu_add_button(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string label, bool enabled, IntPtr button_context, bool visible, int icon);

        [DllImport("bps")]
        private static extern int dialog_context_menu_update_button(IntPtr dialog, int index, [MarshalAs(UnmanagedType.LPStr)]string label, bool enabled, IntPtr button_context, bool visible, int icon);

        #endregion

        internal override void CreateDialog()
        {
            if (dialog_create_context_menu(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        #region Buttons

        internal override bool AddButton(DialogButton button)
        {
            var icon = button is ContentMenuButton ? ((ContentMenuButton)button).Icon : ContextMenuIcon.NoIcon;
            return dialog_context_menu_add_button(handle, button.Label, button.Enabled, IntPtr.Zero, button.Visible, (int)icon) == BPS.BPS_SUCCESS;
        }

        internal override bool ReplaceButton(int index, DialogButton newButton)
        {
            var icon = newButton is ContentMenuButton ? ((ContentMenuButton)newButton).Icon : ContextMenuIcon.NoIcon;
            return dialog_context_menu_update_button(handle, index, newButton.Label, newButton.Enabled, IntPtr.Zero, newButton.Visible, (int)icon) == BPS.BPS_SUCCESS;
        }

        internal override bool UpdateButtonProperty(int index, DialogButton button, string property)
        {
            switch (property)
            {
                case "Label":
                    return dialog_context_menu_update_button(handle, index, button.Label, button.Enabled, IntPtr.Zero, button.Visible, (int)ContextMenuIcon.KeepIcon) == BPS.BPS_SUCCESS;
                case "Enabled":
                case "Visible":
                    return dialog_context_menu_update_button(handle, index, null, button.Enabled, IntPtr.Zero, button.Visible, (int)ContextMenuIcon.KeepIcon) == BPS.BPS_SUCCESS;
                case "Context":
                    return true; // We don't actually set the context pointer, so don't worry if it changes.
                case "Icon":
                    return dialog_context_menu_update_button(handle, index, null, button.Enabled, IntPtr.Zero, button.Visible, (int)((ContentMenuButton)button).Icon) == BPS.BPS_SUCCESS;
                default:
                    throw new ArgumentException(string.Format("Unknown property: {0}", property));
            }
        }

        #endregion
    }

    /// <summary>
    /// A button for a context menu.
    /// </summary>
    [AvailableSince(10, 0)]
    public class ContentMenuButton : DialogButton
    {
        /// <summary>
        /// Create a new button for a context menu.
        /// </summary>
        /// <param name="label">The button label.</param>
        /// <param name="icon">The button icon.</param>
        /// <param name="enabled">If the button is enabled when the dialog is shown.</param>
        /// <param name="visible">If the button is visible when the dialog is shown.</param>
        /// <param name="context">The button context.</param>
        [AvailableSince(10, 0)]
        public ContentMenuButton(string label, ContextMenuIcon icon, bool enabled = true, bool visible = true, object context = null)
            : base(label, enabled, visible, context)
        {
            if (icon == ContextMenuIcon.KeepIcon)
            {
                throw new ArgumentException("icon cannot be KeepIcon", "icon");
            }
            this.icon = icon;
        }

        private ContextMenuIcon icon;

        /// <summary>
        /// Get or set the menu icon.
        /// </summary>
        [AvailableSince(10, 0)]
        public ContextMenuIcon Icon
        {
            [AvailableSince(10, 0)]
            get
            {
                return icon;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (icon != value && icon != ContextMenuIcon.KeepIcon)
                {
                    icon = value;
                    OnPropertyChanged("Icon");
                }
            }
        }
    }
}
