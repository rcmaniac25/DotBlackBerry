using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BlackBerry.BPS
{
    /// <summary>
    /// Possible Payment Service events.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum PaymentCode : int
    {
        /// <summary>
        /// Indicates that a response to a purchase request has been received.
        /// </summary>
        [AvailableSince(10, 0)]
        PurchaseResponse = 0x00,
        /// <summary>
        /// Indicates that a response to a request to retrieve purchase history has been received.
        /// </summary>
        [AvailableSince(10, 0)]
        GetExistingPurchases = 0x01,
        /// <summary>
        /// Indicates that a response to a request to get the price of a digital good has been received.
        /// </summary>
        [AvailableSince(10, 0)]
        GetPrice = 0x02,
        /// <summary>
        /// Indicates that a response to a request to check the subscription status of a digital good has been received.
        /// </summary>
        [AvailableSince(10, 0)]
        CheckExistingSubscription = 0x03,
        /// <summary>
        /// Indicates that a response to a request to cancel a subscription has been received.
        /// </summary>
        [AvailableSince(10, 0)]
        CancelSubscription = 0x04
    }

    /// <summary>
    /// Possible states of a digital good.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum PaymentItemState : int
    {
        /// <summary>
        /// Indicates that the digital good is not a subscription and is owned by the user.
        /// </summary>
        [AvailableSince(10, 0)]
        Owned = 0,
        /// <summary>
        /// Indicates that the user is currently subscribed to the digital good. It's a new subscription.
        /// </summary>
        [AvailableSince(10, 0)]
        NewSubscription,
        /// <summary>
        /// Indicates that the subscription digital good has been refunded. The user is no longer subscribed.
        /// </summary>
        [AvailableSince(10, 0)]
        SubscriptionRefunded,
        /// <summary>
        /// Indicates that the subscription has been cancelled.
        /// </summary>
        [AvailableSince(10, 0)]
        SubscriptionCancelled,
        /// <summary>
        /// Indicates that the user is currently subscribed and they have renewed the subscription.
        /// </summary>
        [AvailableSince(10, 0)]
        SubscriptionRenewed,
        /// <summary>
        /// Indicates that the state of the item is unknown.
        /// </summary>
        [AvailableSince(10, 0)]
        Unknown
    }

    /// <summary>
    /// Possible Payment Service errors.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum PaymentError : int
    {
        // All values except 0, 4, and 5 are from 10.2's paymentservice_error_t, while 4 and 5 are from 10.1's paymentservice_event_get_error_id

        /// <summary>
        /// Not a valid error
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// This error occurs when a user cancels the request.
        /// </summary>
        [AvailableSince(10, 0)]
        UserCancelled = 1,
        /// <summary>
        /// This error occurs when a user attempts to purchase more than one item at a time.
        /// </summary>
        [AvailableSince(10, 0)]
        SystemBusy = 2,
        /// <summary>
        /// Payment Service failed.
        /// </summary>
        [AvailableSince(10, 0)]
        PaymentServiceFailed = 3,

        /// <summary>
        /// Digital good not found.
        /// </summary>
        [AvailableSince(10, 0)]
        NotFound = 4,
        /// <summary>
        /// Digital good already purchased.
        /// </summary>
        [AvailableSince(10, 0)]
        AlreadyPurchased = 5,

        /// <summary>
        /// No network connectivity on device.
        /// </summary>
        [AvailableSince(10, 2)]
        NoNetwork = 8
    }

    /// <summary>
    /// API to allow digital purchase of goods using the BlackBerry Platform Services (BPS) Payment Service API.
    /// </summary>
    [AvailableSince(10, 0)]
    public static class PaymentService
    {
        #region PInvoke

        private const int SUCCESS_RESPONSE = 0;
        private const int FAILURE_RESPONSE = 1;

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_request_events(int flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_stop_events(int flags);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_get_domain();

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_purchase_request([MarshalAs(UnmanagedType.LPStr)]string digital_good_id, [MarshalAs(UnmanagedType.LPStr)]string digital_good_sku,
            [MarshalAs(UnmanagedType.LPStr)]string digital_good_name, [MarshalAs(UnmanagedType.LPStr)]string metadata, [MarshalAs(UnmanagedType.LPStr)]string app_name,
            [MarshalAs(UnmanagedType.LPStr)]string app_icon, [MarshalAs(UnmanagedType.LPStr)]string group_id, out uint request_id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_purchase_request_with_arguments(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_get_existing_purchases_request(bool allow_refresh, [MarshalAs(UnmanagedType.LPStr)]string group_id, out uint request_id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_get_price([MarshalAs(UnmanagedType.LPStr)]string digital_good_id, [MarshalAs(UnmanagedType.LPStr)]string digital_good_sku,
            [MarshalAs(UnmanagedType.LPStr)]string group_id, out uint request_id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_check_existing([MarshalAs(UnmanagedType.LPStr)]string digital_good_id, [MarshalAs(UnmanagedType.LPStr)]string digital_good_sku,
            [MarshalAs(UnmanagedType.LPStr)]string group_id, out uint request_id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_cancel_subscription([MarshalAs(UnmanagedType.LPStr)]string purchase_id, [MarshalAs(UnmanagedType.LPStr)]string group_id, out uint request_id);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_set_connection_mode(bool local);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_create(out IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_destroy(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_set_digital_good_id(IntPtr purchase_arguments, [MarshalAs(UnmanagedType.LPStr)]string digital_good_id);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_purchase_arguments_get_digital_good_id(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_set_digital_good_sku(IntPtr purchase_arguments, [MarshalAs(UnmanagedType.LPStr)]string digital_good_sku);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_purchase_arguments_get_digital_good_sku(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_set_digital_good_name(IntPtr purchase_arguments, [MarshalAs(UnmanagedType.LPStr)]string digital_good_name);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_purchase_arguments_get_digital_good_name(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_set_metadata(IntPtr purchase_arguments, [MarshalAs(UnmanagedType.LPStr)]string metadata);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_purchase_arguments_get_metadata(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_set_extra_parameter(IntPtr purchase_arguments, [MarshalAs(UnmanagedType.LPStr)]string key, [MarshalAs(UnmanagedType.LPStr)]string value);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_purchase_arguments_get_extra_parameter_by_key(IntPtr purchase_arguments, [MarshalAs(UnmanagedType.LPStr)]string key);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_set_app_name(IntPtr purchase_arguments, [MarshalAs(UnmanagedType.LPStr)]string app_name);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_purchase_arguments_get_app_name(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_set_app_icon(IntPtr purchase_arguments, [MarshalAs(UnmanagedType.LPStr)]string app_icon);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_purchase_arguments_get_app_icon(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_set_group_id(IntPtr purchase_arguments, [MarshalAs(UnmanagedType.LPStr)]string group_id);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_purchase_arguments_get_group_id(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_set_vendor_customer_id(IntPtr purchase_arguments, [MarshalAs(UnmanagedType.LPStr)]string vendor_customer_id);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_purchase_arguments_get_vendor_customer_id(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_purchase_arguments_set_vendor_content_id(IntPtr purchase_arguments, [MarshalAs(UnmanagedType.LPStr)]string vendor_content_id);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_purchase_arguments_get_vendor_content_id(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern uint paymentservice_purchase_arguments_get_request_id(IntPtr purchase_arguments);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_event_get_response_code(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_event_get_number_purchases(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern uint paymentservice_event_get_request_id(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_event_get_date(IntPtr ev, uint index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_event_get_digital_good_id(IntPtr ev, uint index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_event_get_digital_good_sku(IntPtr ev, uint index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_event_get_license_key(IntPtr ev, uint index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_event_get_metadata(IntPtr ev, uint index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern int paymentservice_event_get_extra_parameter_count(IntPtr ev, uint index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_event_get_extra_parameter_key_at_index(IntPtr ev, uint index, uint key_index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_event_get_extra_parameter_value_at_index(IntPtr ev, uint index, uint key_index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_event_get_purchase_id(IntPtr ev, uint index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_event_get_start_date(IntPtr ev, uint index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_event_get_end_date(IntPtr ev, uint index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern IntPtr paymentservice_event_get_purchase_initial_period(IntPtr ev, uint index);

        [DllImport(BPS.BPS_LIBRARY)]
        internal static extern PaymentItemState paymentservice_event_get_item_state(IntPtr ev, uint index);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr paymentservice_event_get_price(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr paymentservice_event_get_initial_period(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr paymentservice_event_get_renewal_price(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr paymentservice_event_get_renewal_period(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern bool paymentservice_event_get_subscription_exists(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr paymentservice_event_get_cancelled_purchase_id(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern bool paymentservice_event_get_cancelled(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern int paymentservice_event_get_error_id(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr paymentservice_event_get_error_info(IntPtr ev);

        [DllImport(BPS.BPS_LIBRARY)]
        private static extern IntPtr paymentservice_event_get_error_text(IntPtr ev);

        #endregion

        #region BPS

        /// <summary>
        /// Retrieve the unique domain ID for the Payment Service.
        /// </summary>
        [AvailableSince(10, 0)]
        public static int Domain
        {
            [AvailableSince(10, 0)]
            get
            {
                Util.GetBPSOrException();
                return paymentservice_get_domain();
            }
        }

        /// <summary>
        /// Start receiving payment service events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestEvents()
        {
            Util.GetBPSOrException();
            return paymentservice_request_events(0) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Stop receiving payment service events.
        /// </summary>
        /// <returns>true upon success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool StopEvents()
        {
            Util.GetBPSOrException();
            return paymentservice_stop_events(0) == BPS.BPS_SUCCESS;
        }

        #endregion

        #region Request/Purchase functions

        /// <summary>
        /// Initiate the purchase of a digital good.
        /// </summary>
        /// <param name="goodID">The ID of the digital good to purchase. Use a null value if <paramref name="goodSku"/> should be used to reference the digital good on the server.</param>
        /// <param name="goodSku">The SKU of the digital good to purchase. Use a null value if the <paramref name="goodID"/> should be used to reference the digital good on the server.</param>
        /// <param name="windowGroupID">The window group ID of the application. This ID is required so that the Payment Service can properly display dialogs.</param>
        /// <param name="goodName">The name of the digital good to purchase.</param>
        /// <param name="metadata">The metadata for the digital good.</param>
        /// <param name="appName">The name of the application through which the purchase is being made.</param>
        /// <param name="appIcon">The full URL to an icon to display.</param>
        /// <returns>A request ID that can be used to correlate the response to the request, or zero if an error occured.</returns>
        [AvailableSince(10, 0)]
        public static uint RequestPurchase(string goodID, string goodSku, string windowGroupID, string goodName = null, string metadata = null, string appName = null, string appIcon = null)
        {
            if (goodID == null && goodSku == null)
            {
                throw new ArgumentNullException("goodID, goodSku", "cannot both be null, only one or the other if null.");
            }
            if (string.IsNullOrWhiteSpace(goodID) && string.IsNullOrWhiteSpace(goodSku))
            {
                throw new ArgumentException("cannot both be empty or whitespace", "goodID, goodSku");
            }
            if (windowGroupID == null)
            {
                throw new ArgumentNullException("windowGroupID");
            }
            if (string.IsNullOrWhiteSpace(windowGroupID))
            {
                throw new ArgumentException("cannot be empty or whitespace", "windowGroupID");
            }
            uint reqId;
            if (paymentservice_purchase_request(goodID, goodSku, goodName, metadata, appName, appIcon, windowGroupID, out reqId) == BPS.BPS_SUCCESS)
            {
                return reqId;
            }
            return 0;
        }

        /// <summary>
        /// Initiate the purchase of a digital good.
        /// </summary>
        /// <param name="arguments">The set of arguments to use for the purchase.</param>
        /// <returns>true on success, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool RequestPurchase(PurchaseArguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }
            return paymentservice_purchase_request_with_arguments(arguments.Handle) == BPS.BPS_SUCCESS;
        }

        /// <summary>
        /// Retrieve the previous successful purchases.
        /// </summary>
        /// <param name="allowRefresh">
        /// A value of <c>true</c> specifies that the device should be allowed to refresh the list of purchases from the Payment Service server, When a value of <c>false</c> is specified, the current 
        /// list of cached purchases is returned immediately.
        /// </param>
        /// <param name="windowGroupID">The window group ID of the application.</param>
        /// <returns>A request ID that can be used to correlate the response to the request, or zero if an error occured.</returns>
        [AvailableSince(10, 0)]
        public static uint RequestExistingPurchases(bool allowRefresh, string windowGroupID)
        {
            if (windowGroupID == null)
            {
                throw new ArgumentNullException("windowGroupID");
            }
            if (string.IsNullOrWhiteSpace(windowGroupID))
            {
                throw new ArgumentException("cannot be empty or whitespace", "windowGroupID");
            }
            uint reqId;
            if (paymentservice_get_existing_purchases_request(allowRefresh, windowGroupID, out reqId) == BPS.BPS_SUCCESS)
            {
                return reqId;
            }
            return 0;
        }

        /// <summary>
        /// Retrieve the price of a digital good.
        /// </summary>
        /// <param name="goodID">The digital good ID.</param>
        /// <param name="goodSku">The digital good SKU.</param>
        /// <param name="windowGroupID">The window group ID of the application.</param>
        /// <returns>A request ID that can be used to correlate the response to the request, or zero if an error occured.</returns>
        [AvailableSince(10, 0)]
        public static uint RequestPrice(string goodID, string goodSku, string windowGroupID)
        {
            if (goodID == null && goodSku == null)
            {
                throw new ArgumentNullException("goodID, goodSku", "cannot both be null, only one or the other if null.");
            }
            if (string.IsNullOrWhiteSpace(goodID) && string.IsNullOrWhiteSpace(goodSku))
            {
                throw new ArgumentException("cannot both be empty or whitespace", "goodID, goodSku");
            }
            uint reqId;
            if (paymentservice_get_price(goodID, goodSku, windowGroupID, out reqId) == BPS.BPS_SUCCESS)
            {
                return reqId;
            }
            return 0;
        }

        /// <summary>
        /// Determine whether a subscription digital good is currently active.
        /// </summary>
        /// <param name="goodID">The digital good ID.</param>
        /// <param name="goodSku">The digital good SKU.</param>
        /// <param name="windowGroupID">The window group ID of the application.</param>
        /// <returns>A request ID that can be used to correlate the response to the request, or zero if an error occured.</returns>
        [AvailableSince(10, 0)]
        public static uint CheckExistingGood(string goodID, string goodSku, string windowGroupID)
        {
            if (goodID == null && goodSku == null)
            {
                throw new ArgumentNullException("goodID, goodSku", "cannot both be null, only one or the other if null.");
            }
            if (string.IsNullOrWhiteSpace(goodID) && string.IsNullOrWhiteSpace(goodSku))
            {
                throw new ArgumentException("cannot both be empty or whitespace", "goodID, goodSku");
            }
            uint reqId;
            if (paymentservice_check_existing(goodID, goodSku, windowGroupID, out reqId) == BPS.BPS_SUCCESS)
            {
                return reqId;
            }
            return 0;
        }

        /// <summary>
        /// Determine whether a subscription is currently active.
        /// </summary>
        /// <param name="goodSku">The digital good SKU.</param>
        /// <param name="windowGroupID">The window group ID of the application.</param>
        /// <returns>A request ID that can be used to correlate the response to the request, or zero if an error occured.</returns>
        [AvailableSince(10, 0)]
        public static uint CheckExistingSubscription(string goodSku, string windowGroupID)
        {
            if (goodSku == null)
            {
                throw new ArgumentNullException("goodSku");
            }
            if (string.IsNullOrWhiteSpace(goodSku))
            {
                throw new ArgumentException("cannot be empty or whitespace", "goodSku");
            }
            return CheckExistingGood("-1", goodSku, windowGroupID);
        }

        /// <summary>
        /// Cancel a subscription to a digital good.
        /// </summary>
        /// <param name="purchaseId">The purchase ID of the digital good to cancel the subscription to.</param>
        /// <param name="windowGroupID">The window group ID of the application.</param>
        /// <returns>A request ID that can be used to correlate the response to the request, or zero if an error occured.</returns>
        [AvailableSince(10, 0)]
        public static uint CancelSubscription(string purchaseId, string windowGroupID)
        {
            if (purchaseId == null)
            {
                throw new ArgumentNullException("purchaseId");
            }
            if (string.IsNullOrWhiteSpace(purchaseId))
            {
                throw new ArgumentException("cannot be empty or whitespace", "purchaseId");
            }
            uint reqId;
            if (paymentservice_cancel_subscription(purchaseId, windowGroupID, out reqId) == BPS.BPS_SUCCESS)
            {
                return reqId;
            }
            return 0;
        }

        private static bool localConnection = false;

        /// <summary>
        /// Set or set the connection mode.
        /// </summary>
        [AvailableSince(10, 0)]
        public static bool LocalConnectionOnly
        {
            [AvailableSince(10, 0)]
            get
            {
                return localConnection;
            }
            [AvailableSince(10, 0)]
            set
            {
                if (value != localConnection && paymentservice_set_connection_mode(value) == BPS.BPS_SUCCESS)
                {
                    localConnection = value;
                }
            }
        }

        #endregion

        #region Event functions

        /// <summary>
        /// Get if the payment request was successful or not.
        /// </summary>
        /// <param name="ev">A payment service BPSEvent to test.</param>
        /// <returns>true if the request made to the payment system was successful, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public static bool IsPaymentEventSuccessful(BPSEvent ev)
        {
            if (ev.Domain != Domain)
            {
                throw new ArgumentException("BPSEvent is not a payment service event");
            }
            var res = paymentservice_event_get_response_code(ev.DangerousGetHandle());
            if (res == BPS.BPS_FAILURE)
            {
                Util.ThrowExceptionForLastErrno();
            }
            return res == SUCCESS_RESPONSE;
        }

        private static Purchase[] GetPurchases(BPSEvent ev)
        {
            var purchases = new List<Purchase>();
            var ptr = ev.DangerousGetHandle();
            var requestId = paymentservice_event_get_request_id(ptr);
            var count = paymentservice_event_get_number_purchases(ptr);
            for (uint i = 0; i < count; i++)
            {
                purchases.Add(new Purchase(ptr, i, requestId));
            }
            return purchases.ToArray();
        }

        /// <summary>
        /// Get the purchase response.
        /// </summary>
        /// <param name="ev">The <see cref="PaymentCode.PurchaseResponse">PurchaseResponse</see> BPS event.</param>
        /// <returns>The purchase response, or null if an error occured.</returns>
        [AvailableSince(10, 0)]
        public static Purchase GetPurchaseResponse(BPSEvent ev)
        {
            if (IsPaymentEventSuccessful(ev))
            {
                if (ev.Code != (int)PaymentCode.PurchaseResponse)
                {
                    throw new ArgumentException("BPSEvent is not a purchase response event.");
                }
                var res = GetPurchases(ev);
                if (res.Length > 0)
                {
                    return res[0];
                }
            }
            return null;
        }

        /// <summary>
        /// Get the existing purchases.
        /// </summary>
        /// <param name="ev">The <see cref="PaymentCode.GetExistingPurchases">GetExistingPurchases</see> BPS event.</param>
        /// <returns>An array of existing purchases, or null if an error occured.</returns>
        [AvailableSince(10, 0)]
        public static Purchase[] GetExistingPurchases(BPSEvent ev)
        {
            if (IsPaymentEventSuccessful(ev))
            {
                if (ev.Code != (int)PaymentCode.GetExistingPurchases)
                {
                    throw new ArgumentException("BPSEvent is not an existing purchases event.");
                }
                return GetPurchases(ev);
            }
            return null;
        }

        /// <summary>
        /// Get the response to a price request.
        /// </summary>
        /// <param name="ev">The <see cref="PaymentCode.GetPrice">GetPrice</see> BPS event.</param>
        /// <returns>The result of the request for a digital good or subscription price, or null if the request failed.</returns>
        [AvailableSince(10, 0)]
        public static PaymentPrice GetPriceResponse(BPSEvent ev)
        {
            if (IsPaymentEventSuccessful(ev))
            {
                if (ev.Code != (int)PaymentCode.GetPrice)
                {
                    throw new ArgumentException("BPSEvent is not a price event.");
                }
                var ptr = ev.DangerousGetHandle();
                return new PaymentPrice(Marshal.PtrToStringAnsi(paymentservice_event_get_price(ptr)),
                    Marshal.PtrToStringAnsi(paymentservice_event_get_initial_period(ptr)),
                    Marshal.PtrToStringAnsi(paymentservice_event_get_renewal_price(ptr)),
                    Marshal.PtrToStringAnsi(paymentservice_event_get_renewal_period(ptr)),
                    paymentservice_event_get_request_id(ptr));
            }
            return null;
        }

        /// <summary>
        /// Get the response of a check for an existing subscription request.
        /// </summary>
        /// <param name="ev">The <see cref="PaymentCode.CheckExistingSubscription">CheckExistingSubscription</see> BPS event.</param>
        /// <returns>The result of the check for an existing subscription request, or null if the request failed.</returns>
        [AvailableSince(10, 0)]
        public static SubscriptionExists GetSubscriptionExistsResponse(BPSEvent ev)
        {
            if (IsPaymentEventSuccessful(ev))
            {
                if (ev.Code != (int)PaymentCode.CheckExistingSubscription)
                {
                    throw new ArgumentException("BPSEvent is not a check for existing subscription event.");
                }
                var ptr = ev.DangerousGetHandle();
                return new SubscriptionExists(paymentservice_event_get_subscription_exists(ptr), paymentservice_event_get_request_id(ptr));
            }
            return null;
        }

        /// <summary>
        /// Get the response of a subscription cancellation request.
        /// </summary>
        /// <param name="ev">The <see cref="PaymentCode.CancelSubscription">CancelSubscription</see> BPS event.</param>
        /// <returns>The result of the subscription cancellation request, or null if the request failed.</returns>
        [AvailableSince(10, 0)]
        public static SubscriptionCancellation GetSubscriptionCancellationResponse(BPSEvent ev)
        {
            if (IsPaymentEventSuccessful(ev))
            {
                if (ev.Code != (int)PaymentCode.CancelSubscription)
                {
                    throw new ArgumentException("BPSEvent is not a cancel subscription event.");
                }
                var ptr = ev.DangerousGetHandle();
                return new SubscriptionCancellation(paymentservice_event_get_cancelled(ptr), 
                    Marshal.PtrToStringAnsi(paymentservice_event_get_cancelled_purchase_id(ptr)), 
                    paymentservice_event_get_request_id(ptr));
            }
            return null;
        }

        /// <summary>
        /// Get the error associated with a failed payment service request.
        /// </summary>
        /// <param name="ev">A payment service BPSEvent to retrieve the error from.</param>
        /// <returns>The payment service error. Check ID to make sure it's a valid error, otherwise no error occured.</returns>
        [AvailableSince(10, 0)]
        public static PaymentServiceError GetError(BPSEvent ev)
        {
            if (IsPaymentEventSuccessful(ev))
            {
                return default(PaymentServiceError);
            }
            var ptr = ev.DangerousGetHandle();
            var add = Util.IsCapableOfRunning(10, 2) ? Marshal.PtrToStringAnsi(paymentservice_event_get_error_info(ptr)) : string.Empty;
            return new PaymentServiceError(paymentservice_event_get_error_id(ptr), Marshal.PtrToStringAnsi(paymentservice_event_get_error_text(ptr)), add);
        }

        #endregion
    }

    /// <summary>
    /// A payment service response.
    /// </summary>
    [AvailableSince(10, 0)]
    public class PaymentServiceResponse
    {
        internal PaymentServiceResponse(uint requestID)
        {
            RequestID = requestID;
        }

        /// <summary>
        /// Get the response's request ID.
        /// </summary>
        [AvailableSince(10, 0)]
        public uint RequestID { [AvailableSince(10, 0)]get; private set; }
    }

    /// <summary>
    /// A payment service error.
    /// </summary>
    [AvailableSince(10, 0)]
    public struct PaymentServiceError
    {
        /// <summary>
        /// Get the ID of the error,
        /// </summary>
        [AvailableSince(10, 0)]
        public PaymentError ID { get; private set; }

        /// <summary>
        /// Get the error text of the payment service request.
        /// </summary>
        [AvailableSince(10, 2)]
        public string Error { get; private set; }

        /// <summary>
        /// Get additional information about the error.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Details { get; private set; }

        internal PaymentServiceError(int id, string err, string addErr)
            : this()
        {
            ID = (PaymentError)id;
            Error = err;
            Details = addErr;
        }
    }

    /// <summary>
    /// Arguments for a purchase.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class PurchaseArguments : IDisposable, IDictionary<string, string>
    {
        private IntPtr handle;
        private IDictionary<string, string> extraParams;

        /// <summary>
        /// Create a set purchase arguments.
        /// </summary>
        [AvailableSince(10, 0)]
        public PurchaseArguments()
        {
            Util.GetBPSOrException();
            if (PaymentService.paymentservice_purchase_arguments_create(out handle) != BPS.BPS_SUCCESS)
            {
                Util.ThrowExceptionForLastErrno();
            }
            extraParams = new Dictionary<string, string>();
        }

        /// <summary>
        /// Finalize PurchaseArguments instance.
        /// </summary>
        ~PurchaseArguments()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose PurchaseArguments.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Dispose()
        {
            Verify();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (handle != IntPtr.Zero)
            {
                PaymentService.paymentservice_purchase_arguments_destroy(handle);
                extraParams.Clear();
                handle = IntPtr.Zero;
            }
        }

        private void Verify()
        {
            if (handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("PurchaseArguments");
            }
        }

        #region Properties

        internal IntPtr Handle
        {
            get
            {
                Verify();
                return handle;
            }
        }

        /// <summary>
        /// Get if the arguments are still valid and usable.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return handle != IntPtr.Zero;
            }
        }

        /// <summary>
        /// Get or set the ID of the digital good to purchase.
        /// </summary>
        [AvailableSince(10, 0)]
        public string GoodID
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(PaymentService.paymentservice_purchase_arguments_get_digital_good_id(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                PaymentService.paymentservice_purchase_arguments_set_digital_good_id(handle, value);
            }
        }

        /// <summary>
        /// Get or set the SKU of the digital good to purchase.
        /// </summary>
        [AvailableSince(10, 0)]
        public string GoodSKU
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(PaymentService.paymentservice_purchase_arguments_get_digital_good_sku(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                PaymentService.paymentservice_purchase_arguments_set_digital_good_sku(handle, value);
            }
        }

        /// <summary>
        /// Get or set the name of the digital good to purchase.
        /// </summary>
        [AvailableSince(10, 0)]
        public string GoodName
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(PaymentService.paymentservice_purchase_arguments_get_digital_good_name(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                PaymentService.paymentservice_purchase_arguments_set_digital_good_name(handle, value);
            }
        }

        /// <summary>
        /// Get or set the metadata to the purchase request.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Metadata
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(PaymentService.paymentservice_purchase_arguments_get_metadata(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                PaymentService.paymentservice_purchase_arguments_set_metadata(handle, value);
            }
        }

        /// <summary>
        /// Get or set the name of the application through which the digital good is being purchased.
        /// </summary>
        [AvailableSince(10, 0)]
        public string AppName
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(PaymentService.paymentservice_purchase_arguments_get_app_name(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                PaymentService.paymentservice_purchase_arguments_set_app_name(handle, value);
            }
        }

        /// <summary>
        /// Get or set the application icon to display.
        /// </summary>
        [AvailableSince(10, 0)]
        public string AppIcon
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(PaymentService.paymentservice_purchase_arguments_get_app_icon(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                PaymentService.paymentservice_purchase_arguments_set_app_icon(handle, value);
            }
        }

        /// <summary>
        /// Get or set the window group ID of the application.
        /// </summary>
        [AvailableSince(10, 0)]
        public string WindowGroupID
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(PaymentService.paymentservice_purchase_arguments_get_group_id(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                PaymentService.paymentservice_purchase_arguments_set_group_id(handle, value);
            }
        }

        /// <summary>
        /// Get or set the vendor customer id to be associated with the purchase.
        /// </summary>
        [AvailableSince(10, 0)]
        public string VendorCustomerID
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(PaymentService.paymentservice_purchase_arguments_get_vendor_customer_id(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                PaymentService.paymentservice_purchase_arguments_set_vendor_customer_id(handle, value);
            }
        }

        /// <summary>
        /// Get or set the vendor content id to be associated with the purchase.
        /// </summary>
        [AvailableSince(10, 0)]
        public string VendorContentID
        {
            [AvailableSince(10, 0)]
            get
            {
                Verify();
                return Marshal.PtrToStringAnsi(PaymentService.paymentservice_purchase_arguments_get_vendor_content_id(handle));
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                PaymentService.paymentservice_purchase_arguments_set_vendor_content_id(handle, value);
            }
        }

        /// <summary>
        /// Get the request ID from the purchase. This will exist after a call to <see cref="PaymentService.RequestPurchase(PurchaseArguments)">RequestPurchase</see>.
        /// </summary>
        public uint? RequestID
        {
            get
            {
                Verify();
                uint id = PaymentService.paymentservice_purchase_arguments_get_request_id(handle);
                if (id == 0)
                {
                    return null;
                }
                return id;
            }
        }

        #endregion

        #region ExtraParameters

        // paymentservice_purchase_arguments_get_extra_parameter_by_key is not used, as it's faster to just use the build in dictionary then to check the arguments handle itself

        /// <summary>
        /// Add an extra input parameter to the purchase request.
        /// </summary>
        /// <param name="key">The key of the extra parameter to be set.</param>
        /// <param name="value">The value of the extra parameter to be set.</param>
        [AvailableSince(10, 0)]
        public void Add(string key, string value)
        {
            Verify();
            if (PaymentService.paymentservice_purchase_arguments_set_extra_parameter(handle, key, value) == BPS.BPS_SUCCESS)
            {
                extraParams.Add(key, value);
            }
            else
            {
                Util.ThrowExceptionForLastErrno();
            }
        }

        /// <summary>
        /// Get if an extra input parameter for the given key exists.
        /// </summary>
        /// <param name="key">The key of the extra parameter.</param>
        /// <returns>true if the extra input parameter exists for the given key, false if otherwise.</returns>
        public bool ContainsKey(string key)
        {
            Verify();
            return extraParams.ContainsKey(key);
        }

        /// <summary>
        /// Get all the extra input parameter keys.
        /// </summary>
        public ICollection<string> Keys
        {
            get
            {
                Verify();
                return extraParams.Keys;
            }
        }

        /// <summary>
        /// Remove an extra input parameter.
        /// </summary>
        /// <param name="key">The key of the extra parameter.</param>
        /// <returns>true if the extra input parameter was removed, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool Remove(string key)
        {
            Verify();
            var result = PaymentService.paymentservice_purchase_arguments_set_extra_parameter(handle, key, null) == BPS.BPS_SUCCESS;
            if (result)
            {
                result = extraParams.Remove(key);
                if (!result)
                {
                    PaymentService.paymentservice_purchase_arguments_set_extra_parameter(handle, key, extraParams[key]);
                }
            }
            return result;
        }

        /// <summary>
        /// Try and get an extra inpit parameter.
        /// </summary>
        /// <param name="key">The key of the extra parameter.</param>
        /// <param name="value">The value of the extra parameter if it can be retrieved.</param>
        /// <returns>true if the extra input parameter was retrieved, false if otherwise.</returns>
        public bool TryGetValue(string key, out string value)
        {
            Verify();
            return extraParams.TryGetValue(key, out value);
        }

        /// <summary>
        /// Get all the extra input parameter values.
        /// </summary>
        public ICollection<string> Values
        {
            get
            {
                Verify();
                return extraParams.Values;
            }
        }

        /// <summary>
        /// Get or set an extra input parameter.
        /// </summary>
        /// <param name="key">The key of the extra parameter.</param>
        /// <returns>The value of the extra parameter.</returns>
        [AvailableSince(10, 0)]
        public string this[string key]
        {
            get
            {
                Verify();
                return extraParams[key];
            }
            [AvailableSince(10, 0)]
            set
            {
                Verify();
                if (PaymentService.paymentservice_purchase_arguments_set_extra_parameter(handle, key, value) == BPS.BPS_SUCCESS)
                {
                    extraParams[key] = value;
                }
                else
                {
                    Util.ThrowExceptionForLastErrno();
                }
            }
        }

        /// <summary>
        /// Add an extra input parameter to the purchase request.
        /// </summary>
        /// <param name="item">The extra input parameter to add.</param>
        [AvailableSince(10, 0)]
        public void Add(KeyValuePair<string, string> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Remove all extra input parameters.
        /// </summary>
        [AvailableSince(10, 0)]
        public void Clear()
        {
            Verify();
            foreach (var key in extraParams.Keys)
            {
                // Ignore results
                PaymentService.paymentservice_purchase_arguments_set_extra_parameter(handle, key, null);
            }
            extraParams.Clear();
        }

        /// <summary>
        /// Get if an extra input parameter exists.
        /// </summary>
        /// <param name="item">The extra input parameter to look for.</param>
        /// <returns>true if the extra input parameter exists, false if otherwise.</returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            Verify();
            return extraParams.Contains(item);
        }

        /// <summary>
        /// Obtain a copy the extra input parameters.
        /// </summary>
        /// <param name="array">The destination array to copy the extra input parameters.</param>
        /// <param name="arrayIndex">The starting index of <paramref name="array"/> to copy the values to.</param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            Verify();
            extraParams.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Get the number of extra input parameters contained.
        /// </summary>
        public int Count
        {
            get
            {
                Verify();
                return extraParams.Count;
            }
        }

        /// <summary>
        /// Get if the extra input parameters is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                Verify();
                return extraParams.IsReadOnly;
            }
        }

        /// <summary>
        /// Remove an extra input parameter.
        /// </summary>
        /// <param name="item">The extra input item to remove.</param>
        /// <returns>true if the extra input parameter was removed, false if otherwise.</returns>
        [AvailableSince(10, 0)]
        public bool Remove(KeyValuePair<string, string> item)
        {
            Verify();
            if (extraParams.Contains(item))
            {
                return Remove(item.Key);
            }
            return false;
        }

        /// <summary>
        /// Enumerate the extra input parameters.
        /// </summary>
        /// <returns>An enumeration of the extra input parameters.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            Verify();
            return extraParams.GetEnumerator();
        }

        /// <summary>
        /// Enumerate the extra input parameters.
        /// </summary>
        /// <returns>An enumeration of the extra input parameters.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            Verify();
            return ((System.Collections.IEnumerable)extraParams).GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Representation of a purchase.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class Purchase : PaymentServiceResponse
    {
        internal Purchase(IntPtr evPtr, uint index, uint requestId)
            : base(requestId)
        {
            PurchaseDate = Marshal.PtrToStringAnsi(PaymentService.paymentservice_event_get_date(evPtr, index)); //TODO: parse
            DigitalGoodID = Marshal.PtrToStringAnsi(PaymentService.paymentservice_event_get_digital_good_id(evPtr, index));
            DigitalGoodSKU = Marshal.PtrToStringAnsi(PaymentService.paymentservice_event_get_digital_good_sku(evPtr, index));
            LicenseKey = Marshal.PtrToStringAnsi(PaymentService.paymentservice_event_get_license_key(evPtr, index));
            Metadata = Marshal.PtrToStringAnsi(PaymentService.paymentservice_event_get_metadata(evPtr, index));
            var extraCount = PaymentService.paymentservice_event_get_extra_parameter_count(evPtr, index);
            var extra = new Dictionary<string, string>(extraCount);
            for (uint i = 0; i < extraCount; i++)
            {
                extra.Add(Marshal.PtrToStringAnsi(PaymentService.paymentservice_event_get_extra_parameter_key_at_index(evPtr, index, i)),
                    Marshal.PtrToStringAnsi(PaymentService.paymentservice_event_get_extra_parameter_value_at_index(evPtr, index, i)));
            }
            ExtraParameters = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(extra);
            PurchaseID = Marshal.PtrToStringAnsi(PaymentService.paymentservice_event_get_purchase_id(evPtr, index));
            SubscriptionStartDate = Marshal.PtrToStringAnsi(PaymentService.paymentservice_event_get_start_date(evPtr, index)); //TODO: parse
            SubscriptionEndDate = Marshal.PtrToStringAnsi(PaymentService.paymentservice_event_get_end_date(evPtr, index)); //TODO: parse
            SubscriptionInitalPeriod = Marshal.PtrToStringAnsi(PaymentService.paymentservice_event_get_purchase_initial_period(evPtr, index)); //TODO: parse
            ItemState = PaymentService.paymentservice_event_get_item_state(evPtr, index);
        }

        /// <summary>
        /// The date that the purchase was made.
        /// </summary>
        [AvailableSince(10, 0)]
        public string PurchaseDate { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The digital good ID of the purchase.
        /// </summary>
        [AvailableSince(10, 0)]
        public string DigitalGoodID { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The digital good SKU of the purchase.
        /// </summary>
        [AvailableSince(10, 0)]
        public string DigitalGoodSKU { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The digital good license key of the purchase.
        /// </summary>
        [AvailableSince(10, 0)]
        public string LicenseKey { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The digital good metadata of the purchase.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Metadata { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Extra parameters associated with the purchase.
        /// </summary>
        [AvailableSince(10, 0)]
        public IDictionary<string,string> ExtraParameters { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The unique ID of the purchase.
        /// </summary>
        [AvailableSince(10, 0)]
        public string PurchaseID { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The start date of the purchased digital good subscription.
        /// </summary>
        [AvailableSince(10, 0)]
        public string SubscriptionStartDate { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The end date of the purchased digital good subscription
        /// </summary>
        [AvailableSince(10, 0)]
        public string SubscriptionEndDate { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The initial purchase period of the purchased digital good subscription.
        /// </summary>
        [AvailableSince(10, 0)]
        public string SubscriptionInitalPeriod { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The state of the digital good.
        /// </summary>
        [AvailableSince(10, 0)]
        public PaymentItemState ItemState { [AvailableSince(10, 0)]get; private set; }
    }

    /// <summary>
    /// A price response.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class PaymentPrice : PaymentServiceResponse
    {
        internal PaymentPrice(string price, string initPeriod, string renewalPrice, string renewalPeriod, uint requestID)
            : base(requestID)
        {
            Price = price;
            RenewalPrice = renewalPrice;
            InitialPeriod = initPeriod;
            RenewalPeriod = renewalPeriod;
        }

        /// <summary>
        /// The price of the digital good.
        /// </summary>
        [AvailableSince(10, 0)]
        public string Price { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The renewal price of the digital good subscription.
        /// </summary>
        [AvailableSince(10, 0)]
        public string RenewalPrice { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The initial period of the digital good subscription.
        /// </summary>
        [AvailableSince(10, 0)]
        public string InitialPeriod { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// The renewal period of the digital good subscription.
        /// </summary>
        [AvailableSince(10, 0)]
        public string RenewalPeriod { [AvailableSince(10, 0)]get; private set; }
    }

    /// <summary>
    /// A subscription exists response.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class SubscriptionExists : PaymentServiceResponse
    {
        internal SubscriptionExists(bool exists, uint requestID)
            : base(requestID)
        {
            Exists = exists;
        }

        /// <summary>
        /// Get if the subscription exists.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool Exists { [AvailableSince(10, 0)]get; private set; }
    }

    /// <summary>
    /// A subscription cancellation response.
    /// </summary>
    [AvailableSince(10, 0)]
    public sealed class SubscriptionCancellation : PaymentServiceResponse
    {
        internal SubscriptionCancellation(bool canceled, string purchaseID, uint requestID)
            : base(requestID)
        {
            Canceled = canceled;
            PurchaseID = purchaseID;
        }

        /// <summary>
        /// Get if the subscription was canceled.
        /// </summary>
        [AvailableSince(10, 0)]
        public bool Canceled { [AvailableSince(10, 0)]get; private set; }

        /// <summary>
        /// Get the subscription purchase ID.
        /// </summary>
        [AvailableSince(10, 0)]
        public string PurchaseID { [AvailableSince(10, 0)]get; private set; }
    }
}
