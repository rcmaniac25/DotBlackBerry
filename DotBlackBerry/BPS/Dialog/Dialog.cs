using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS.Dialog
{
    /// <summary>
    /// The available input flags for dialogs.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum InputFlags : int
    {
        /// <summary>
        /// Specify that input will not be changed.
        /// </summary>
        [AvailableSince(10, 0)]
        None = 0,
        /// <summary>
        /// Specify that input will be capitalized automatically.
        /// </summary>
        [AvailableSince(10, 0)]
        AutoCapitalize = 1,
        /// <summary>
        /// Specify that input will be corrected automatically.
        /// </summary>
        [AvailableSince(10, 0)]
        AutoCorrect = 1 << 1,
        /// <summary>
        /// Specify that input will be checked for spelling errors.
        /// </summary>
        [AvailableSince(10, 0)]
        SpellCheck = 1 << 2
    }

    /// <summary>
    /// Localized button labels.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum LocalizedButtonLabels
    {
        /// <summary>
        /// The OK label.
        /// </summary>
        [AvailableSince(10, 0)]
        Ok,
        /// <summary>
        /// The Cancel label.
        /// </summary>
        [AvailableSince(10, 0)]
        Cancel,
        /// <summary>
        /// The Cut label.
        /// </summary>
        [AvailableSince(10, 0)]
        Cut,
        /// <summary>
        /// The Copy label.
        /// </summary>
        [AvailableSince(10, 0)]
        Copy,
        /// <summary>
        /// The Paste label.
        /// </summary>
        [AvailableSince(10, 0)]
        Paste,
        /// <summary>
        /// The Select label.
        /// </summary>
        [AvailableSince(10, 0)]
        Select,
        /// <summary>
        /// The Delete label.
        /// </summary>
        [AvailableSince(10, 0)]
        Delete,
        /// <summary>
        /// The View Image label.
        /// </summary>
        [AvailableSince(10, 0)]
        ViewImage,
        /// <summary>
        /// The Save Image label.
        /// </summary>
        [AvailableSince(10, 0)]
        SaveImage,
        /// <summary>
        /// The Save Link As label.
        /// </summary>
        [AvailableSince(10, 0)]
        SaveLinkAs,
        /// <summary>
        /// The Open Link in New Tab label.
        /// </summary>
        [AvailableSince(10, 0)]
        OpenLinkInNewTab,
        /// <summary>
        /// The Copy Link label.
        /// </summary>
        [AvailableSince(10, 0)]
        CopyLink,
        /// <summary>
        /// The Open Link label.
        /// </summary>
        [AvailableSince(10, 0)]
        OpenLink,
        /// <summary>
        /// The Copy Image Link label.
        /// </summary>
        [AvailableSince(10, 0)]
        CopyImageLink,
        /// <summary>
        /// The Clear Field label.
        /// </summary>
        [AvailableSince(10, 0)]
        ClearField,
        /// <summary>
        /// The Cancel Selection label.
        /// </summary>
        [AvailableSince(10, 0)]
        CancelSelection,
        /// <summary>
        /// The Bookmark Link label.
        /// </summary>
        [AvailableSince(10, 0)]
        BookmarkLink
    }

    /// <summary>
    /// A button on a dialog.
    /// </summary>
    [AvailableSince(10, 0)]
    public class DialogButton : System.ComponentModel.INotifyPropertyChanged
    {
        private string label;
        private bool enabled;
        private bool visible;
        private object context;

        /// <summary>
        /// Create a new button for a dialog.
        /// </summary>
        /// <param name="label">The button label.</param>
        /// <param name="enabled">If the button is enabled when the dialog is shown.</param>
        /// <param name="visible">If the button is visible when the dialog is shown.</param>
        /// <param name="context">The button context.</param>
        [AvailableSince(10, 0)]
        public DialogButton(string label, bool enabled = true, bool visible = true, object context = null)
        {
            this.label = label;
            this.enabled = enabled;
            this.visible = visible;
            this.context = context;
        }

        /// <summary>
        /// Property change event.
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        internal void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// Get or set the button label.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Label
        {
            [AvailableSince(10, 0)]
            get
            {
                return label;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (label != value)
                {
                    label = value;
                    OnPropertyChanged("Label");
                }
            }
        }

        /// <summary>
        /// Get or set if the button is enabled.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool Enabled
        {
            [AvailableSince(10, 0)]
            get
            {
                return enabled;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    OnPropertyChanged("Enabled");
                }
            }
        }

        /// <summary>
        /// Get or set if the button is visible.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool Visible
        {
            [AvailableSince(10, 0)]
            get
            {
                return visible;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (visible != value)
                {
                    visible = value;
                    OnPropertyChanged("Visible");
                }
            }
        }

        /// <summary>
        /// Get or set if the button is visible.
        /// </summary>
        [AvailableSince(10, 0)]
        public object Context
        {
            [AvailableSince(10, 0)]
            get
            {
                return context;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (context != value)
                {
                    context = value;
                    OnPropertyChanged("Context");
                }
            }
        }
    }

    /// <summary>
    /// Base dialog window.
    /// </summary>
    [AvailableSince(10, 0)]
    public abstract class Dialog : IDisposable
    {
        #region PInvoke

        [DllImport("bps")]
        private static extern int dialog_request_events(int flags);

        [DllImport("bps")]
        private static extern int dialog_stop_events(int flags);

        [DllImport("bps")]
        private static extern int dialog_get_domain();

        [DllImport("bps")]
        private static extern int dialog_show(IntPtr dialog);

        [DllImport("bps")]
        internal static extern int dialog_update(IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_cancel(IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_destroy(IntPtr dialog);

        [DllImport("bps")]
        private static extern int dialog_set_title_text(IntPtr ev, [MarshalAs(UnmanagedType.LPStr)]string text);

        [DllImport("bps")]
        private static extern int dialog_set_group_id(IntPtr ev, [MarshalAs(UnmanagedType.LPStr)]string group_id);

        [DllImport("bps")]
        private static extern int dialog_set_input_flags(IntPtr dialog, int input_flags);

        [DllImport("bps")]
        private static extern int dialog_set_busy(IntPtr dialog, bool busy);

        [DllImport("bps")]
        private static extern int dialog_set_system(IntPtr dialog, bool system);

        [DllImport("bps")]
        private static extern int dialog_set_priority(IntPtr dialog, bool priority);

        [DllImport("bps")]
        private static extern int dialog_set_pid(IntPtr dialog, IntPtr pid);

        [DllImport("bps")]
        private static extern int dialog_set_enter_key_type(IntPtr dialog, int enter_key_type);

        [DllImport("bps")]
        private static extern int dialog_set_cancel_required(IntPtr dialog, bool cancel_required);

        [DllImport("bps")]
        private static extern int dialog_set_default_button_index(IntPtr dialog, int default_button_index);

        [DllImport("bps")]
        private static extern int dialog_set_button_limit(IntPtr dialog, int button_limit);

        [DllImport("bps")]
        private static extern int dialog_add_button(IntPtr dialog, [MarshalAs(UnmanagedType.LPStr)]string label, bool enabled, IntPtr button_context, bool visible);

        [DllImport("bps")]
        private static extern int dialog_update_button(IntPtr dialog, int index, [MarshalAs(UnmanagedType.LPStr)]string label, bool enabled, IntPtr button_context, bool visible);

        [DllImport("bps")]
        private static extern int dialog_remove_button(IntPtr dialog, int index);

        [DllImport("bps")]
        private static extern IntPtr dialog_event_get_dialog_instance(IntPtr ev);

        [DllImport("bps")]
        internal static extern IntPtr dialog_event_get_error(IntPtr ev);

        [DllImport("bps")]
        internal static extern int dialog_event_get_selected_index(IntPtr ev);

        #endregion

        private const int DIALOG_RESPONSE = 1;

        /* 
         * This is quite nasty. There is no way for us to determine dialog type, so we need to maintain the association outselves
         * that way we can maintain type hierarchy without doing manual tests on dialogs to see what functions fail (which some
         * functions only have set functions, meaning that test would be destructive).
         */
        private static IDictionary<IntPtr, Dialog> dialogs = new ConcurrentDictionary<IntPtr, Dialog>();

        internal IntPtr handle;
        private int buttonCount;
        private int defaultButton;
        private int invalidButtons;
        private ObservableCollection<DialogButton> buttons;
        private bool isVisible;

        private int buttonLimit = 2;
        private VirtualKeyboardEnter keyboardEnter = VirtualKeyboardEnter.Default;
        private string groupID = null;
        private InputFlags inputFlags = InputFlags.AutoCorrect | InputFlags.SpellCheck;
        private bool busy = false;
        private bool cancelRequired = false;
        private string title = null; //XXX

        internal Dialog()
        {
            handle = IntPtr.Zero;
            Util.GetBPSOrException();
            CreateDialog();
            if (handle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Could not create dialog");
            }
            dialogs.Add(handle, this);
            setupButtons();
            isVisible = false;
        }

        internal Dialog(IntPtr ptr)
        {
            handle = ptr;
            Util.GetBPSOrException();
            if (handle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Could not create dialog");
            }
            setupButtons();
            isVisible = false;
            // Don't add to dialog list, this is probably coming from an event, meaning it's already in the list.
        }

        internal abstract void CreateDialog();

        internal virtual DialogEvent GetEventForDialog(IntPtr ev)
        {
            return new GenericDialogEvent(ev, this);
        }

        /// <summary>
        /// Dispose of the Dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("Dialog");
            }
            if (dialog_destroy(handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            dialogs.Remove(handle);
            handle = IntPtr.Zero;
            isVisible = false;

            // Cleanup buttons so nothing gets screwed up if the button is reused
            buttons.CollectionChanged -= buttons_CollectionChanged;
            foreach (var button in buttons)
            {
                button.PropertyChanged -= buttonItem_PropertyChanged;
            }
            buttons.Clear();
        }

        #region BPS

        /// <summary>
        /// Get the unique domain ID for the dialog service.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int Domain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return dialog_get_domain();
            }
        }

        /// <summary>
        /// Start receiving dialog events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestEvents()
        {
            Util.GetBPSOrException();
            return dialog_request_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving dialog events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopEvents()
        {
            Util.GetBPSOrException();
            return dialog_stop_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Get a dialog event from a BPS Event.
        /// </summary>
        /// <param name="ev">A BPSEvent to convert to a DialogEvent.</param>
        /// <returns>The DialogEvent for the event.</returns>
        [AvailableSince(10, 0)]
        public static DialogEvent GetDialogEvent(BPSEvent ev)
        {
            if (ev.Domain != Dialog.Domain)
            {
                throw new ArgumentException("BPSEvent is not a dialog event");
            }
            if (ev.Code != DIALOG_RESPONSE) // There is only one dialog event type right now, so do a simple check against it
            {
                throw new ArgumentException("BPSEvent is an unknown dialog event");
            }
            var evPtr = ev.DangerousGetHandle();
            var diaPtr = dialog_event_get_dialog_instance(evPtr);
            Dialog dia;
            if (!dialogs.TryGetValue(diaPtr, out dia))
            {
                dia = new GenericDialog(diaPtr);
            }
            dia.isVisible = false;
            return dia.GetEventForDialog(evPtr);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or set the title text for a dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Title
        {
            [AvailableSince(10, 0)]
            get
            {
                return title;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (title != value)
                {
                    Util.GetBPSOrException();
                    if (dialog_set_title_text(handle, value) == BPS.BPS_SUCCESS)
                    {
                        title = value;
                        UpdateDialog();
                    }
                }
            }
        }

        /// <summary>
        /// Get or set the window group ID for an application modal dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public string GroupID
        {
            [AvailableSince(10, 0)]
            get
            {
                return groupID;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (groupID != value)
                {
                    Util.GetBPSOrException();
                    if (dialog_set_group_id(handle, value) == BPS.BPS_SUCCESS)
                    {
                        groupID = value;
                    }
                    else
                    {
                        //TODO: log error
                    }
                }
            }
        }

        /// <summary>
        /// Specify input flags on the input field.
        /// </summary>
        [AvailableSince(10, 2)]
        public InputFlags InputFlags
        {
            [AvailableSince(10, 2)]
            get
            {
                return inputFlags;
            }
            [AvailableSince(10, 2)]
            set
            {
                if (inputFlags != value)
                {
                    Util.GetBPSOrException();
                    if (dialog_set_input_flags(handle, (int)value) == BPS.BPS_SUCCESS)
                    {
                        inputFlags = value;
                        UpdateDialog();
                    }
                }
            }
        }

        /// <summary>
        /// Get or set whether to show an activity indicator in a dialog.
        /// </summary>
        [AvailableSince(10, 2)]
        public bool IsBusy
        {
            [AvailableSince(10, 2)]
            get
            {
                return busy;
            }
            [AvailableSince(10, 2)]
            set
            {
                if (busy != value)
                {
                    Util.GetBPSOrException();
                    if (dialog_set_busy(handle, value) == BPS.BPS_SUCCESS)
                    {
                        busy = value;
                        UpdateDialog();
                    }
                }
            }
        }

        /// <summary>
        /// Get if the dialog is visible right now. This is a guess based on actions that have occured within the dialog functions.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool IsVisible
        {
            [AvailableSince(10, 0)]
            get
            {
                return isVisible;
            }
        }

#if BLACKBERRY_INTERNAL_FUNCTIONS

        /// <summary>
        /// Specify whether the dialog is a system dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool IsSystemDialog
        {
            [AvailableSince(10, 0)]
            set
            {
                Util.GetBPSOrException();
                if (dialog_set_system(handle, value) == BPS.BPS_SUCCESS)
                {
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Set the priority for a system dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool IsPrioritySystemDialog
        {
            [AvailableSince(10, 0)]
            set
            {
                Util.GetBPSOrException();
                if (dialog_set_priority(handle, value) == BPS.BPS_SUCCESS)
                {
                    UpdateDialog();
                }
            }
        }

        /// <summary>
        /// Set the process for a system dialog to be associated with.
        /// </summary>
        [AvailableSince(10, 2)]
        public System.Diagnostics.Process SystemDialogProcess
        {
            [AvailableSince(10, 2)]
            set
            {
                Util.GetBPSOrException();
                if (dialog_set_pid(handle, value.Handle) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForErrno();
                }
            }
        }

#endif

        /// <summary>
        /// Specify the enter key type to use on the virtual keyboard.
        /// </summary>
        [AvailableSince(10, 0)]
        public VirtualKeyboardEnter EnterKey
        {
            [AvailableSince(10, 0)]
            get
            {
                return keyboardEnter;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (keyboardEnter != value)
                {
                    Util.GetBPSOrException();
                    if (dialog_set_enter_key_type(handle, VirtualKeyboard.EnterKeyToInt(value)) == BPS.BPS_SUCCESS)
                    {
                        keyboardEnter = value;
                        UpdateDialog();
                    }
                }
            }
        }

        /// <summary>
        /// Specify whether the application is required to cancel a dialog explicitly.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool RequiresExplicitCancel
        {
            [AvailableSince(10, 0)]
            get
            {
                return cancelRequired;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (cancelRequired != value)
                {
                    Util.GetBPSOrException();
                    dialog_set_cancel_required(handle, value);
                    cancelRequired = value;
                }
            }
        }

        #endregion

        #region Functions

        internal bool UpdateDialog(bool throwOnError = true)
        {
            if (isVisible && dialog_update(handle) != BPS.BPS_SUCCESS)
            {
                if (throwOnError)
                {
                    Util.ThrowExceptionForLastErrno();
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Display a dialog.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool Show()
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("Dialog");
            }
            var result = dialog_show(handle) == BPS.BPS_SUCCESS;
            isVisible |= result;
            return result;
        }

        /// <summary>
        /// Cancel a dialog.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool Cancel()
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("Dialog");
            }
            var result = dialog_cancel(handle) == BPS.BPS_SUCCESS;
            isVisible &= !result;
            return result;
        }

        #endregion

        #region Buttons

        /// <summary>
        /// Get or set the default button for the dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public DialogButton DefaultButton
        {
            [AvailableSince(10, 0)]
            get
            {
                if (defaultButton < 0)
                {
                    return null;
                }
                return buttons[defaultButton];
            }
            [AvailableSince(10, 0)]
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("DefaultButton");
                }
                var index = buttons.IndexOf(value);
                if (index < 0)
                {
                    throw new ArgumentException("DialogButton is not part of Dialog");
                }
                if (dialog_set_default_button_index(handle, index) != BPS.BPS_SUCCESS)
                {
                    Util.ThrowExceptionForLastErrno();
                }
            }
        }

        /// <summary>
        /// The dialog's buttons.
        /// </summary>
        [AvailableSince(10, 0)]
        public IList<DialogButton> Buttons
        {
            [AvailableSince(10, 0)]
            get
            {
                return buttons;
            }
        }

        /// <summary>
        /// Get or set the button limit for a dialog.
        /// </summary>
        [AvailableSince(10, 2)]
        public int ButtonLimit
        {
            [AvailableSince(10, 2)]
            get
            {
                return buttonLimit;
            }
            [AvailableSince(10, 2)]
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value", "ButtonLimit must be greater then 1");
                }
                if (value != buttonLimit)
                {
                    if (dialog_set_button_limit(handle, value) != BPS.BPS_SUCCESS)
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    buttonLimit = value;
                }
            }
        }

        private void setupButtons()
        {
            buttons = new ObservableCollection<DialogButton>();
            buttonCount = 0;
            defaultButton = -1;
            invalidButtons = 0;
            buttons.CollectionChanged += buttons_CollectionChanged;
        }

        private void buttonItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var button = (DialogButton)sender;
            var index = buttons.IndexOf(button);
            if (index < 0)
            {
                throw new InvalidOperationException("DialogButton is not part of Dialog");
            }
            if (!UpdateButtonProperty(index, button, e.PropertyName))
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        //XXX If an error occurs, this potentially leaves the collection in incorrect state compared to what dialog actually has.
        private void buttons_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                #region Add

                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems[0] == null)
                    {
                        invalidButtons++;
                    }
                    if (invalidButtons > 0)
                    {
                        throw new InvalidOperationException("Buttons contains null items. All operations are ignored until null buttons are removed.");
                    }
                    if (e.NewStartingIndex == buttonCount)
                    {
                        if (!AddButton(e.NewItems[0] as DialogButton))
                        {
                            Util.ThrowExceptionForLastErrno();
                        }
                        if (defaultButton == -1)
                        {
                            defaultButton = 0;
                            dialog_set_default_button_index(handle, 0);
                        }
                        ((DialogButton)e.NewItems[0]).PropertyChanged += buttonItem_PropertyChanged;
                    }
                    else
                    {
                        // Add current last item a second time at end
                        if (!AddButton(buttons[buttonCount - 1]))
                        {
                            Util.ThrowExceptionForLastErrno();
                        }
                        // Change insert item to have values of new item
                        if (!ReplaceButton(e.NewStartingIndex, e.NewItems[0] as DialogButton))
                        {
                            Util.ThrowExceptionForLastErrno();
                        }
                        ((DialogButton)e.NewItems[0]).PropertyChanged += buttonItem_PropertyChanged;
                        // Update all items "after" the insert item
                        for (var i = e.NewStartingIndex + 1; i < buttonCount; i++)
                        {
                            ReplaceButton(i, buttons[i]);
                        }
                    }
                    buttonCount++;
                    UpdateDialog();
                    break;

                #endregion

                #region Move

                case NotifyCollectionChangedAction.Move:
                    if (invalidButtons > 0)
                    {
                        throw new InvalidOperationException("Buttons contains null items. All operations are ignored until null buttons are removed.");
                    }
                    var startIndex = Math.Min(e.NewStartingIndex, e.OldStartingIndex);
                    var endIndex = Math.Max(e.NewStartingIndex, e.OldStartingIndex);
                    for (var i = startIndex; i <= endIndex; i++)
                    {
                        ReplaceButton(i, buttons[i]);
                    }
                    UpdateDialog();
                    break;

                #endregion

                #region Remove

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems[0] == null)
                    {
                        if (invalidButtons > 0)
                        {
                            invalidButtons--;
                        }
                        return;
                    }
                    if (invalidButtons > 0)
                    {
                        throw new InvalidOperationException("Buttons contains null items. All operations are ignored until null buttons are removed.");
                    }
                    if (buttonCount == 0)
                    {
                        throw new InvalidOperationException("No buttons exist, how is one being removed?");
                    }
                    if (dialog_remove_button(handle, e.OldStartingIndex) == BPS.BPS_SUCCESS)
                    {
                        buttonCount--;
                        if (buttonCount == 0)
                        {
                            defaultButton = -1;
                        }
                        ((DialogButton)e.OldItems[0]).PropertyChanged -= buttonItem_PropertyChanged;
                        UpdateDialog();
                    }
                    else
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    break;

                #endregion

                #region Replace

                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems[0] == null)
                    {
                        invalidButtons++;
                    }
                    if (invalidButtons > 0)
                    {
                        throw new InvalidOperationException("Buttons contains null items. All operations are ignored until null buttons are removed.");
                    }
                    if (!ReplaceButton(e.NewStartingIndex, e.NewItems[0] as DialogButton))
                    {
                        Util.ThrowExceptionForLastErrno();
                    }
                    break;

                #endregion

                #region Reset

                case NotifyCollectionChangedAction.Reset:
                    // Buttons still maintain an event to buttonItem_PropertyChanged... luckily, if the button isn't GC'd, any changes will cause an exception. Offering an incentive to let it be GC'ed
                    while (buttonCount > 0)
                    {
                        if (dialog_remove_button(handle, e.OldStartingIndex) != BPS.BPS_SUCCESS)
                        {
                            Util.ThrowExceptionForLastErrno();
                        }
                        buttonCount--;
                    }
                    defaultButton = -1;
                    invalidButtons = 0;
                    UpdateDialog();
                    break;

                #endregion
            }
        }

        internal virtual bool AddButton(DialogButton button)
        {
            return dialog_add_button(handle, button.Label, button.Enabled, IntPtr.Zero, button.Visible) == BPS.BPS_SUCCESS;
        }

        internal virtual bool ReplaceButton(int index, DialogButton newButton)
        {
            return dialog_update_button(handle, index, newButton.Label ?? "", newButton.Enabled, IntPtr.Zero, newButton.Visible) == BPS.BPS_SUCCESS;
        }

        internal virtual bool UpdateButtonProperty(int index, DialogButton button, string property)
        {
            switch (property)
            {
                case "Label":
                    return dialog_update_button(handle, index, button.Label, button.Enabled, IntPtr.Zero, button.Visible) == BPS.BPS_SUCCESS;
                case "Enabled":
                case "Visible":
                    return dialog_update_button(handle, index, null, button.Enabled, IntPtr.Zero, button.Visible) == BPS.BPS_SUCCESS;
                case "Context":
                    return true; // We don't actually set the context pointer, so don't worry if it changes.
                default:
                    throw new ArgumentException(string.Format("Unknown property: {0}", property));
            }
        }

        #region GetLabelValueForButton

        /// <summary>
        /// Get the string value that, when applied to a button label, will be localized based on the system's locale.
        /// </summary>
        /// <param name="buttonLabel">The button label to get.</param>
        /// <returns>The string value that will be localized.</returns>
        [AvailableSince(10, 0)]
        public static string GetLabelValueForButton(LocalizedButtonLabels buttonLabel)
        {
            switch (buttonLabel)
            {
                case LocalizedButtonLabels.Ok:
                    return "OK";
                case LocalizedButtonLabels.Cancel:
                    return "CANCEL";
                case LocalizedButtonLabels.Cut:
                    return "CUT";
                case LocalizedButtonLabels.Copy:
                    return "COPY";
                case LocalizedButtonLabels.Paste:
                    return "PASTE";
                case LocalizedButtonLabels.Select:
                    return "SELECT";
                case LocalizedButtonLabels.Delete:
                    return "DELETE";
                case LocalizedButtonLabels.ViewImage:
                    return "VIEW_IMAGE";
                case LocalizedButtonLabels.SaveImage:
                    return "SAVE_IMAGE";
                case LocalizedButtonLabels.SaveLinkAs:
                    return "SAVE_LINK_AS";
                case LocalizedButtonLabels.OpenLinkInNewTab:
                    return "OPEN_LINK_NEW_TAB";
                case LocalizedButtonLabels.CopyLink:
                    return "COPY_LINK";
                case LocalizedButtonLabels.OpenLink:
                    return "OPEN_LINK";
                case LocalizedButtonLabels.CopyImageLink:
                    return "COPY_IMAGE_LINK";
                case LocalizedButtonLabels.ClearField:
                    return "CLEAR_FIELD";
                case LocalizedButtonLabels.CancelSelection:
                    return "CANCEL_SELECTION";
                case LocalizedButtonLabels.BookmarkLink:
                    return "BOOKMARK_LINK";
                default:
                    throw new ArgumentException(string.Format("Unknown argument: {0}"), "buttonLabel");
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Events associated with dialogs.
    /// </summary>
    [AvailableSince(10, 0)]
    public abstract class DialogEvent : BPSEvent
    {
        internal DialogEvent(IntPtr ev, Dialog dia)
            : base(ev, false)
        {
            Util.GetBPSOrException();
            var ptr = DangerousGetHandle();
            Dialog = dia;
            Error = Marshal.PtrToStringAnsi(Dialog.dialog_event_get_error(ptr));
            SelectedButton = dia.Buttons[Dialog.dialog_event_get_selected_index(ptr)];
        }

        /// <summary>
        /// Get the dialog instance for the event.
        /// </summary>
        [AvailableSince(10, 0)]
        public Dialog Dialog { get; private set; }

        /// <summary>
        /// Get the error message, if there is one, for the event.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Error { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get the selected button from the dialog.
        /// </summary>
        [AvailableSince(10, 0)]
        public DialogButton SelectedButton { [AvailableSince(10, 0)]get; private set; }
    }

    internal class GenericDialog : Dialog
    {
        public GenericDialog(IntPtr ptr)
            : base(ptr)
        {
        }

        internal override void CreateDialog()
        {
            throw new NotImplementedException();
        }
    }

    internal class GenericDialogEvent : DialogEvent
    {
        public GenericDialogEvent(IntPtr ev, Dialog dia)
            : base(ev, dia)
        {
        }
    }
}
