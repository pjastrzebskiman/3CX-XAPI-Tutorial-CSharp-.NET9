using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class Users
    {
        public string AccessPassword { get; set; }
        public bool AllowLanOnly { get; set; }
        public bool AllowOwnRecordings { get; set; }
        public string AuthID { get; set; }
        public string AuthPassword { get; set; }
        public string Blfs { get; set; }

        public BreakTime BreakTime { get; set; }

        public bool CallScreening { get; set; }
        public bool CallUsEnableChat { get; set; }
        public bool CallUsEnablePhone { get; set; }
        public bool CallUsEnableVideo { get; set; }
        public string CallUsRequirement { get; set; }
        public string ClickToCallId { get; set; }
        public string ContactImage { get; set; }
        public string CurrentProfileName { get; set; }
        public string DeskphonePassword { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string EmergencyAdditionalInfo { get; set; }
        public string EmergencyLocationId { get; set; }
        public bool Enable2FA { get; set; }
        public bool Enabled { get; set; }
        public bool EnableHotdesking { get; set; }
        public string FirstName { get; set; }

        public List<ForwardingException> ForwardingExceptions { get; set; }
        public List<ForwardingProfile> ForwardingProfiles { get; set; }

        public bool GoogleCalendarEnabled { get; set; }
        public bool GoogleContactsEnabled { get; set; }
        public bool GoogleSignInEnabled { get; set; }

        public List<Greeting> Greetings { get; set; }

        public List<UserGroup> Groups { get; set; }

        public bool HideInPhonebook { get; set; }
        public string HotdeskingAssignment { get; set; }

        public Hours Hours { get; set; }

        public int Id { get; set; }
        public bool Internal { get; set; }
        public bool IsRegistered { get; set; }
        public string Language { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public bool MS365CalendarEnabled { get; set; }
        public bool MS365ContactsEnabled { get; set; }
        public bool MS365SignInEnabled { get; set; }
        public bool MS365TeamsEnabled { get; set; }
        public bool MyPhoneAllowDeleteRecordings { get; set; }
        public bool MyPhoneHideForwardings { get; set; }
        public bool MyPhonePush { get; set; }
        public bool MyPhoneShowRecordings { get; set; }
        public string Number { get; set; }

        public List<string> OfficeHoursProps { get; set; }

        public string OutboundCallerID { get; set; }
        public bool PbxDeliversAudio { get; set; }

        public List<Phone> Phones { get; set; }

        public bool PinProtected { get; set; }
        public int PinProtectTimeout { get; set; }
        public int PrimaryGroupId { get; set; }
        public string PromptSet { get; set; }
        public string QueueStatus { get; set; }
        public bool RecordCalls { get; set; }
        public bool RecordEmailNotify { get; set; }
        public bool RecordExternalCallsOnly { get; set; }
        public bool Require2FA { get; set; }
        public bool SendEmailMissedCalls { get; set; }
        public string SIPID { get; set; }
        public string SRTPMode { get; set; }

        public List<string> Tags { get; set; }

        public string TranscriptionMode { get; set; }
        public bool VMDisablePinAuth { get; set; }
        public string VMEmailOptions { get; set; }
        public bool VMEnabled { get; set; }
        public string VMPIN { get; set; }
        public bool VMPlayCallerID { get; set; }
        public string VMPlayMsgDateTime { get; set; }
        public bool WebMeetingApproveParticipants { get; set; }
        public string WebMeetingFriendlyName { get; set; }
    }
    public class UsersResponse
    {
        public List<Users> Value { get; set; }
    }



  
    public class ForwardingException
    {
        public string CallerId { get; set; }
        public Destination Destination { get; set; }
        public Hours Hours { get; set; }
        public int Id { get; set; }
    }

    public class Destination
    {
        public string External { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public List<string> Tags { get; set; }
        public string To { get; set; }
        public string Type { get; set; }
    }

   
    public class ForwardingProfile
    {
        public bool AcceptMultipleCalls { get; set; }
        public AvailableRoute AvailableRoute { get; set; }
        public AwayRoute AwayRoute { get; set; }
        public bool BlockPushCalls { get; set; }
        public string CustomMessage { get; set; }
        public string CustomName { get; set; }
        public bool DisableRingGroupCalls { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int NoAnswerTimeout { get; set; }
        public bool OfficeHoursAutoQueueLogOut { get; set; }
        public bool RingMyMobile { get; set; }
    }

    public class AvailableRoute
    {
        public bool BusyAllCalls { get; set; }
        public Destination BusyExternal { get; set; }
        public Destination BusyInternal { get; set; }

        public bool NoAnswerAllCalls { get; set; }
        public Destination NoAnswerExternal { get; set; }
        public Destination NoAnswerInternal { get; set; }

        public bool NotRegisteredAllCalls { get; set; }
        public Destination NotRegisteredExternal { get; set; }
        public Destination NotRegisteredInternal { get; set; }
    }

    public class AwayRoute
    {
        public bool AllHoursExternal { get; set; }
        public bool AllHoursInternal { get; set; }
        public Destination External { get; set; }
        public Destination Internal { get; set; }
    }

    
    public class Greeting
    {
        public string DisplayName { get; set; }
        public string Filename { get; set; }
        public string Type { get; set; }
    }

   
    public class UserGroup
    {
        public bool CanDelete { get; set; }
        public int GroupId { get; set; }
        public GroupRights GroupRights { get; set; }
        public int Id { get; set; }
        public string MemberName { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public GroupRights Rights { get; set; }
        public List<string> Tags { get; set; }
        public string TranscriptionMode { get; set; }
        public string Type { get; set; }
    }

    public class Phone
    {
        public int Id { get; set; }
        public string Interface { get; set; }
        public string MacAddress { get; set; }
        public string Name { get; set; }
        public string ProvisioningLinkExt { get; set; }
        public string ProvisioningLinkLocal { get; set; }
        public PhoneSettings Settings { get; set; }
        public string TemplateName { get; set; }
    }

 
    public class PhoneSettings
    {
        public bool AllowCustomQueueRingtones { get; set; }
        public string Backlight { get; set; }
        public List<string> Codecs { get; set; }
        public string CustomLogo { get; set; }
        public List<CustomQueueRingtone> CustomQueueRingtones { get; set; }
        public string DateFormat { get; set; }
        public string Firmware { get; set; }
        public string FirmwareLang { get; set; }
        public bool IsLogoCustomizable { get; set; }
        public bool IsSBC { get; set; }
        public LlDpInfo LlDpInfo { get; set; }
        public int LocalRTPPortEnd { get; set; }
        public int LocalRTPPortStart { get; set; }
        public int LocalSipPort { get; set; }
        public string LogoDescription { get; set; }
        public List<string> LogoFileExtensionAllowed { get; set; }
        public bool OwnBlfs { get; set; }
        public string PhoneLanguage { get; set; }
        public string PowerLed { get; set; }
        public string ProvisionExtendedData { get; set; }
        public string ProvisionType { get; set; }
        public string QueueRingTone { get; set; }
        public string RemoteSpmHost { get; set; }
        public int RemoteSpmPort { get; set; }
        public string RingTone { get; set; }
        public string SbcName { get; set; }
        public string ScreenSaver { get; set; }
        public string Secret { get; set; }
        public string Srtp { get; set; }
        public string TimeFormat { get; set; }
        public string TimeZone { get; set; }
        public List<VlanInfo> VlanInfos { get; set; }
        public string XferType { get; set; }
    }

    public class CustomQueueRingtone
    {
        public string Queue { get; set; }
        public string Ringtone { get; set; }
    }

    public class LlDpInfo
    {
        public bool Configurable { get; set; }
        public bool Value { get; set; }
    }

    public class VlanInfo
    {
        public bool Configurable { get; set; }
        public bool Enabled { get; set; }
        public int Priority { get; set; }
        public bool PriorityConfigurable { get; set; }
        public int PriorityMax { get; set; }
        public int PriorityMin { get; set; }
        public string Type { get; set; }
        public int VlanId { get; set; }
        public int VlanIdMax { get; set; }
        public int VlanIdMin { get; set; }
    }


}