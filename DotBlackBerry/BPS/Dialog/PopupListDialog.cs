using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Dialog
{
    /// <summary>
    /// Types of popup list items.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum PopupListItemType
    {
        /// <summary>
        /// Text items.
        /// </summary>
        [AvailableSince(10, 0)]
        Text,
        /// <summary>
        /// Header items.
        /// </summary>
        [AvailableSince(10, 0)]
        Header,
        /// <summary>
        /// Seperator items.
        /// </summary>
        [AvailableSince(10, 0)]
        Separator
    }

    /// <summary>
    /// An item within the popup list.
    /// </summary>
    [AvailableSince(10, 0)]
    public struct PopupListItem
    {
        /// <summary>
        /// Get or set the type of popup list item.
        /// </summary>
        [AvailableSince(10, 0)]
        public PopupListItemType Type { get; set; }

        /// <summary>
        /// The text that this item contains. This is required.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Text { get; set; }

        /// <summary>
        /// Get if the item is selected. This is only valid for Text items. Cannot be used with IsDisabled.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool IsSelected { get; set; }

        /// <summary>
        /// Get if the item is disabled. This is only valid for Text items. Cannot be used with IsSelected.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool IsDisabled { get; set; }
    }

    /// <summary>
    /// A popup list dialog displays a simple popup list along with a title, selected items, and buttons.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class PopupListDialog : Dialog
    {
        #region PInvoke

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_create_popuplist(out IntPtr dialog);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_popuplist_multiselect(IntPtr dialog, bool multi_select);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_popuplist_allow_deselect(IntPtr dialog, bool allow_deselect);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_popuplist_cancel_on_selection(IntPtr dialog, bool cancel_on_selection);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_popuplist_show_basic_selection(IntPtr dialog, bool show_basic_selection);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_popuplist_items(IntPtr dialog, string[] items, int num_items);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_popuplist_selected_indices(IntPtr dialog, int[] selected_indices, int num_items);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_popuplist_disabled_indices(IntPtr dialog, int[] disabled_indices, int num_items);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_popuplist_header_indices(IntPtr dialog, int[] header_indices, int num_items);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_popuplist_separator_indices(IntPtr dialog, int[] separator_indices, int num_items);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int dialog_set_popuplist_scroll_to_index(IntPtr dialog, int index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int dialog_event_get_popuplist_selected_indices(IntPtr ev, out IntPtr selected_indices, out int num_indices);

        #endregion

        private PopupListItem[] items;
        private int firstVisible = 0;
        private bool multiselect = false;
        private bool deselect = true;
        private bool cancelOnSelect = false;
        private bool basicSelect = false;

        /// <summary>
        /// Create a new popup list.
        /// </summary>
        [AvailableSince(10, 0)]
        public PopupListDialog()
            : base()
        {
            items = null;
        }

        internal override void CreateDialog()
        {
            if (dialog_create_popuplist(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        internal override DialogEvent GetEventForDialog(IntPtr ev)
        {
            return new PopupListEvent(ev, this);
        }

        #region Items

        /// <summary>
        /// Get or set the items contained within the popup list.
        /// </summary>
        [AvailableSince(10, 0)]
        public PopupListItem[] Items
        {
            [AvailableSince(10, 0)]
            get
            {
                if (items != null)
                {
                    return (PopupListItem[])items.Clone();
                }
                return items;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (items == null || !items.SequenceEqual(value))
                {
                    ValidateItems(value);

                    // Text
                    var text = value.Select(item => string.IsNullOrWhiteSpace(item.Text) ? "" : item.Text).ToArray();
                    if (dialog_set_popuplist_items(handle, text, text.Length) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }

                    // Selected/disabled
                    SetIndicies(value, item => item.IsSelected, dialog_set_popuplist_selected_indices);
                    SetIndicies(value, item => item.IsDisabled, dialog_set_popuplist_disabled_indices);

                    // Types
                    SetIndicies(value, item => item.Type == PopupListItemType.Header, dialog_set_popuplist_header_indices);
                    SetIndicies(value, item => item.Type == PopupListItemType.Separator, dialog_set_popuplist_separator_indices);

                    UpdateDialog();

                    // Copy
                    items = (PopupListItem[])value.Clone();
                }
            }
        }

        private void SetIndicies(PopupListItem[] items, Func<PopupListItem, bool> test, Func<IntPtr, int[], int, int> setFunc)
        {
            var indices = (from ele in items.Select((value, index) => new { value, index })
                           where test(ele.value)
                           select ele.index).ToArray();
            if (indices.Length > 0 && setFunc(handle, indices, indices.Length) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        private void ValidateItems(PopupListItem[] items)
        {
            if (items == null)
            {
                throw new NullReferenceException();
            }
            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];
                if (item.Type < PopupListItemType.Text || item.Type > PopupListItemType.Separator)
                {
                    throw new ArgumentOutOfRangeException(string.Format("Items[{0}]", i), item.Type, "Not a valid PopupListItemType");
                }
                if (item.Type != PopupListItemType.Text)
                {
                    if (item.IsSelected)
                    {
                        throw new ArgumentException("IsSelected can only be used on Text types", string.Format("Items[{0}]", i));
                    }
                    else if (item.IsDisabled)
                    {
                        throw new ArgumentException("IsDisabled can only be used on Text types", string.Format("Items[{0}]", i));
                    }
                }
                else if (item.IsSelected && item.IsDisabled)
                {
                    throw new ArgumentException("IsSelected and IsDisabled cannot be used together", string.Format("Items[{0}]", i));
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or set the index of the first visible item in a popup list dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public int FirstVisible
        {
            [AvailableSince(10, 0)]
            get
            {
                return firstVisible;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (firstVisible != value)
                {
                    if (value < 0 || value > items.Length)
                    {
                        throw new ArgumentOutOfRangeException("FirstVisible", value, "0 <= FirstVisible < Items.Length");
                    }
                    if (dialog_set_popuplist_scroll_to_index(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    firstVisible = value;
                }
            }
        }

        /// <summary>
        /// Specify whether users can select multiple items in a popup list dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool MultiSelect
        {
            [AvailableSince(10, 0)]
            get
            {
                return multiselect;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (multiselect != value)
                {
                    if (dialog_set_popuplist_multiselect(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    multiselect = value;
                }
            }
        }

        /// <summary>
        /// Specify whether users can deselect items in a popup list dialog.
        /// </summary>
        [AvailableSince(10, 2)]
        public bool AllowDeselect
        {
            [AvailableSince(10, 2)]
            get
            {
                return deselect;
            }
            [AvailableSince(10, 2)]
            set
            {
                if (deselect != value)
                {
                    if (dialog_set_popuplist_allow_deselect(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    deselect = value;
                }
            }
        }

        /// <summary>
        /// Specify whether the popup list dialog is cancelled when an item is selected.
        /// </summary>
        [AvailableSince(10, 2)]
        public bool CancelOnSelection
        {
            [AvailableSince(10, 2)]
            get
            {
                return cancelOnSelect;
            }
            [AvailableSince(10, 2)]
            set
            {
                if (cancelOnSelect != value)
                {
                    if (dialog_set_popuplist_cancel_on_selection(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    cancelOnSelect = value;
                }
            }
        }

        /// <summary>
        /// Specify whether to show a basic selection list in the popup list dialog.
        /// </summary>
        [AvailableSince(10, 2)]
        public bool ShowAsBasicSelection
        {
            [AvailableSince(10, 2)]
            get
            {
                return basicSelect;
            }
            [AvailableSince(10, 2)]
            set
            {
                if (basicSelect != value)
                {
                    if (dialog_set_popuplist_show_basic_selection(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    basicSelect = value;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Event associated with a prompt dialog.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class PopupListEvent : DialogEvent
    {
        internal PopupListEvent(IntPtr ev, PopupListDialog dia)
            : base(ev, dia)
        {
            int count;
            IntPtr ptr;
            if (PopupListDialog.dialog_event_get_popuplist_selected_indices(DangerousGetHandle(), out ptr, out count) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            using (var handle = new BPSIntArray(ptr, count))
            {
                SelectedIndicies = handle;
            }
        }

        /// <summary>
        /// Get the contents of the prompt dialog input field.
        /// </summary>
        [AvailableSince(10, 0)]
        public int[] SelectedIndicies { [AvailableSince(10, 0)]get; private set; }
    }
}
