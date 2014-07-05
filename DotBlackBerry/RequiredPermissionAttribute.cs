using System;
using System.Collections.Generic;

namespace BlackBerry
{
    #region Known Permissions

    /// <summary>
    /// Known permissions for BlackBerry functionality.
    /// </summary>
    [AvailableSince(10, 0)]
    public enum Permission
    {
        /// <summary>
        /// Unknown permission. This may just be an undocumented permission or it could have been a misspelled permission.
        /// </summary>
        [AvailableSince(10, 0)]
        Unknown,
        /// <summary>
        /// Connect to BlackBerry Messenger (BBM). You can use this permission to access contact lists and user profiles, invite BBM contacts to download your app, 
        /// initiate BBM chats and share content from within your app, and stream data between apps.
        /// </summary>
        [AvailableSince(10, 0)]
        BBM,
        /// <summary>
        /// Access the calendar on the device. This access includes viewing, adding, and deleting calendar appointments.
        /// </summary>
        [AvailableSince(10, 0)]
        Calendar,
        /// <summary>
        /// Access data that's received from the cameras on the device. With this permission, your app can take pictures, record videos, and use the flash.
        /// </summary>
        [AvailableSince(10, 0)]
        Camera,
        /// <summary>
        /// Take a screen shot or video of any information visible on the screen of the device. This permission also allows the app to share the user's screen.
        /// </summary>
        [AvailableSince(10, 2)]
        CaptureScreen,
        /// <summary>
        /// Access the contacts that are stored on the device. This access includes viewing, creating, and deleting contacts.
        /// </summary>
        [AvailableSince(10, 0)]
        Contacts,
        /// <summary>
        /// Access unique device identifiers, such as the PIN or the serial number. This permission also allows you to access SIM card information on the device.
        /// </summary>
        [AvailableSince(10, 1)]
        DeviceIdentifyingInformation,
        /// <summary>
        /// Access the email and PIN messages that are stored on the device. This access includes viewing, creating, sending, and deleting messages.
        /// </summary>
        [AvailableSince(10, 0)]
        EmailAndPin,
        /// <summary>
        /// Access gamepad functionality. This permission also indicates that the app has official gamepad support in the BlackBerry World storefront.
        /// </summary>
        [AvailableSince(10, 0)]
        Gamepad,
        /// <summary>
        /// Create a custom account that’s accessible in the BlackBerry Hub.
        /// </summary>
        [AvailableSince(10, 2)]
        HubAccounts,
        /// <summary>
        /// Integrate with the BlackBerry Hub. With this permission, your app can create and manage data in the BlackBerry Hub.
        /// </summary>
        [AvailableSince(10, 2)]
        HubIntegration,
        /// <summary>
        /// Use the Internet connection from a Wi-Fi, wired, or other type of connection to access locations that are not local on the device.
        /// </summary>
        [AvailableSince(10, 0)]
        Internet,
        /// <summary>
        /// Control the LED on the device.
        /// </summary>
        [AvailableSince(10, 0)]
        LED,
        /// <summary>
        /// Access the current location of the device, as well as locations that the user has saved.
        /// </summary>
        [AvailableSince(10, 0)]
        Location,
        /// <summary>
        /// Access the audio stream from the microphone on the device.
        /// </summary>
        [AvailableSince(10, 0)]
        Microphone,
        /// <summary>
        /// Access user information on the device, such as the first name, last name, and BlackBerry ID username of the user currently associated with this device.
        /// </summary>
        [AvailableSince(10, 0)]
        MyContactInfo,
        /// <summary>
        /// Reduce the width of the region along the bottom bezel of the device that accepts swipe-up gestures. When you use this permission, swipe-up gestures are 
        /// recognized in a more narrow area along the bottom bezel.
        /// </summary>
        [AvailableSince(10, 0)]
        NarrowSwipeUp,
        /// <summary>
        /// Access the content that's stored in notebooks on the device. This access includes adding entries to, and deleting entries from, the notebooks.
        /// </summary>
        [AvailableSince(10, 0)]
        Notebooks,
        /// <summary>
        /// Change global notification settings. Apps have permission to read their own notification settings.
        /// </summary>
        [AvailableSince(10, 2)]
        NotificationControl,
        /// <summary>
        /// Determine when a user is on a phone call. This access also allows an app to access the phone number assigned to the device and send DTMF (Dual Tone Multi-Frequency) tones.
        /// </summary>
        [AvailableSince(10, 0)]
        Phone,
        /// <summary>
        /// Add audio to a phone call.
        /// </summary>
        [AvailableSince(10, 3)]
        PhoneAudioOverlay,
        /// <summary>
        /// View the status of phone calls that are in progress and the phone number of the remote party.
        /// </summary>
        [AvailableSince(10, 3)]
        PhoneCallDetails,
        /// <summary>
        /// View the logs of previous incoming or outgoing phone calls.
        /// </summary>
        [AvailableSince(10, 3)]
        PhoneCallLogs,
        /// <summary>
        /// Start an outgoing phone call without asking the user to confirm the phone call.
        /// </summary>
        [AvailableSince(10, 3)]
        PhoneCallNoPrompt,
        /// <summary>
        /// Control the current phone call. This access includes ending a phone call and sending DTMF tones to the phone.
        /// </summary>
        [AvailableSince(10, 2)]
        PhoneControl,
        /// <summary>
        /// Record phone calls. This access includes recording an ongoing phone call and saving the audio to a file.
        /// </summary>
        [AvailableSince(10, 3)]
        PhoneRecording,
        /// <summary>
        /// Post notifications to the notification area of the device screen.
        /// </summary>
        [AvailableSince(10, 0)]
        PostNotifications,
        /// <summary>
        /// Access the Push Service to receive and request push messages.
        /// </summary>
        [AvailableSince(10, 0)]
        Push,
        /// <summary>
        /// Perform background processing. Without this permission, your app stops all processing when the user changes focus to another app.
        /// </summary>
        [AvailableSince(10, 0)]
        RunAsActiveFrame,
        /// <summary>
        /// Perform certain tasks in the background, without opening the app, for a short period of time.
        /// </summary>
        [AvailableSince(10, 2)]
        RunInBackground,
        /// <summary>
        /// Run in the background always. You must request access before your app can run as a long-running headless app.
        /// </summary>
        [AvailableSince(10, 2)]
        RunInBackgroundContinuously,
        /// <summary>
        /// Read and write files that are shared between all apps on the device. With this permission, your app can access pictures, music, documents, and other files 
        /// that are stored on the user's device, at a remote storage provider, or on a media card.
        /// </summary>
        [AvailableSince(10, 0)]
        SharedFiles,
        /// <summary>
        /// Encrypt, decrypt, sign, and verify data using a smart card.
        /// </summary>
        [AvailableSince(10, 3)]
        SmartCard,
        /// <summary>
        /// Allow third-party smart card drivers and smart card reader drivers to integrate with the Smart Card service.
        /// </summary>
        [AvailableSince(10, 3)]
        SmartCardDriver,
        /// <summary>
        /// Use APDU (Application Protocol Data Unit) for custom commands.
        /// </summary>
        [AvailableSince(10, 3)]
        SmartCardExtended,
        /// <summary>
        /// Access the text messages that are stored on the device. This access includes viewing, creating, sending, and deleting text messages.
        /// </summary>
        [AvailableSince(10, 0)]
        TextMessages,
        /// <summary>
        /// Receive Wi-Fi event notifications such as Wi-Fi scan results or changes in the Wi-Fi connection state.
        /// </summary>
        [AvailableSince(10, 2)]
        WiFiConnection
    }

    #endregion

    /// <summary>
    /// Attribute to define the required permission to use the functionality.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class RequiredPermissionAttribute : Attribute
    {
        #region Permissions

        struct PermissionInfo
        {
            public Permission Permission;
            public bool Prompted;
            public bool Restricted;
        }

        private static Dictionary<string, PermissionInfo> permissionToInfo = new Dictionary<string, PermissionInfo>();
        private static Dictionary<Permission, string> permissionToString = new Dictionary<Permission, string>();

        private static void AddPermission(Permission perm, string permStr, bool prompted = true, bool restricted = false)
        {
            permissionToInfo.Add(permStr, new PermissionInfo() { Permission = perm, Prompted = prompted, Restricted = restricted });
            permissionToString.Add(perm, permStr);
        }

        static RequiredPermissionAttribute()
        {
            AddPermission(Permission.BBM, "bbm_connect");
            AddPermission(Permission.Calendar, "access_pimdomain_calendars");
            AddPermission(Permission.Camera, "use_camera");
            AddPermission(Permission.CaptureScreen, "use_camera_desktop");
            AddPermission(Permission.Contacts, "access_pimdomain_contacts");
            AddPermission(Permission.DeviceIdentifyingInformation, "read_device_identifying_information");
            AddPermission(Permission.EmailAndPin, "access_pimdomain_messages");
            AddPermission(Permission.Gamepad, "use_gamepad", false);
            AddPermission(Permission.HubAccounts, "_sys__manage_pimdomain_external_accounts", false, true);
            AddPermission(Permission.HubIntegration, "_sys_access_pim_unified", false, true);
            AddPermission(Permission.Internet, "access_internet", false);
            AddPermission(Permission.LED, "access_location_services");
            AddPermission(Permission.Location, "access_led_control", false); //XXX is this prompted?
            AddPermission(Permission.Microphone, "record_audio");
            AddPermission(Permission.MyContactInfo, "read_personally_identifiable_information");
            AddPermission(Permission.NarrowSwipeUp, "narrow_landscape_exit", false);
            AddPermission(Permission.Notebooks, "access_pimdomain_notebooks");
            AddPermission(Permission.NotificationControl, "access_notify_settings_control");
            AddPermission(Permission.Phone, "access_phone");
            AddPermission(Permission.PhoneAudioOverlay, "_sys_inject_voice", restricted: true);
            AddPermission(Permission.PhoneCallDetails, "read_phonecall_details", restricted: true);
            AddPermission(Permission.PhoneCallLogs, "access_pimdomain_calllogs", restricted: true);
            AddPermission(Permission.PhoneCallNoPrompt, "_sys_start_phone_call", restricted: true);
            AddPermission(Permission.PhoneControl, "control_phone");
            AddPermission(Permission.PhoneRecording, "_sys_record_voice", restricted: true);
            AddPermission(Permission.PostNotifications, "post_notification", false);
            AddPermission(Permission.Push, "_sys_use_consumer_push", false, restricted: true);
            AddPermission(Permission.RunAsActiveFrame, "run_when_backgrounded", false);
            AddPermission(Permission.RunInBackground, "_sys_run_headless", false, true);
            AddPermission(Permission.RunInBackgroundContinuously, "_sys_headless_nostop", false, true);
            AddPermission(Permission.SharedFiles, "access_shared");
            AddPermission(Permission.SmartCard, "_sys_access_smartcard_api", false, true);
            AddPermission(Permission.SmartCardDriver, "_sys_smart_card_driver", false, true);
            AddPermission(Permission.SmartCardExtended, "_sys_access_extended_smart_card_functionality", false, true);
            AddPermission(Permission.TextMessages, "access_sms_mms");
            AddPermission(Permission.WiFiConnection, "access_wifi_public", false);
        }

        #endregion

        /// <summary>
        /// Create a new require permission attribute.
        /// </summary>
        /// <param name="permission">The raw permission string of the permission.</param>
        public RequiredPermissionAttribute(string permission)
        {
            // Don't do checks for if a permission exists, as some permissions are private and undocumented
            PermissionString = permission;
        }

        /// <summary>
        /// Create a new require permission attribute.
        /// </summary>
        /// <param name="permission">The permission.</param>
        public RequiredPermissionAttribute(Permission permission)
        {
            if (permission == Permission.Unknown)
            {
                throw new ArgumentException("Permission cannot be unknown");
            }
            if (!permissionToString.ContainsKey(permission))
            {
                throw new ArgumentException("Permission isn't known");
            }
            PermissionString = permissionToString[permission];
        }

        /// <summary>
        /// Raw permission string.
        /// </summary>
        public string PermissionString { get; private set; }

        /// <summary>
        /// Permission.
        /// </summary>
        public Permission Permission
        {
            get
            {
                return ConvertStringToPermission(PermissionString);
            }
        }

        /// <summary>
        /// If user is prompted to allow the permission.
        /// </summary>
        public bool IsPrompted
        {
            get
            {
                return IsPermissionPrompted(PermissionString);
            }
        }

        /// <summary>
        /// If permission is restricted (requires signing permission).
        /// </summary>
        public bool IsRestricted
        {
            get
            {
                return IsPermissionRestricted(PermissionString);
            }
        }

        #region Functions

        /// <summary>
        /// Get permission for a member.
        /// </summary>
        /// <param name="member">The member to get permission for.</param>
        /// <returns>The permission of the member, or empty string if no permission attribute is avaliable.</returns>
        public static string GetPermission(System.Reflection.MemberInfo member)
        {
            foreach (var att in member.GetCustomAttributes(false))
            {
                if (att is RequiredPermissionAttribute)
                {
                    return (att as RequiredPermissionAttribute).PermissionString;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Convert string to permission.
        /// </summary>
        /// <param name="permission">The permission string to convert.</param>
        /// <param name="testVersion">Test the OS version for support.</param>
        /// <returns>The converted permission.</returns>
        public static Permission ConvertStringToPermission(string permission, bool testVersion = true)
        {
            if (permissionToInfo.ContainsKey(permission))
            {
                var info = permissionToInfo[permission];
                if (!testVersion || 
                    Util.IsCapableOfRunning(AvailableSinceAttribute.GetRequiredVersion(typeof(Permission).GetMember(info.Permission.ToString())[0]))) // If the OS supports it, then return it. Otherwise it will be unknown on that OS.
                {
                    return info.Permission;
                }
            }
            return Permission.Unknown;
        }

        /// <summary>
        /// Convert permission to string.
        /// </summary>
        /// <param name="permission">The permission to convert.</param>
        /// <returns>The permission string of the member, or empty string if no permission attribute is avaliable.</returns>
        public static string ConvertPermissionToString(Permission permission)
        {
            if(permissionToString.ContainsKey(permission))
            {
                return permissionToString[permission];
            }
            return string.Empty;
        }

        /// <summary>
        /// Get if permission causes a user prompt.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>true if the permission will cause a prompt, false if otherwise.</returns>
        public static bool IsPermissionPrompted(string permission)
        {
            if (permissionToInfo.ContainsKey(permission))
            {
                return permissionToInfo[permission].Prompted;
            }
            return false;
        }

        /// <summary>
        /// Get if permission is restricted and requires signing to use.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>true if the permission is restricted, false if otherwise.</returns>
        public static bool IsPermissionRestricted(string permission)
        {
            if (permissionToInfo.ContainsKey(permission))
            {
                return permissionToInfo[permission].Restricted;
            }
            return false;
        }

        #endregion
    }
}
