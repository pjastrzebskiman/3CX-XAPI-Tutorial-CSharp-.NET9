using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _3CX_API_20.Models
{
    public class Groups
    {
        public bool AllowCallService { get; set; }
        public int AnswerAfter { get; set; }

        public RouteData BreakRoute { get; set; }
        public BreakTime BreakTime { get; set; }

        public List<string> CallHandlingMode { get; set; }

        public bool CallUsEnableChat { get; set; }
        public bool CallUsEnablePhone { get; set; }
        public bool CallUsEnableVideo { get; set; }
        public string CallUsRequirement { get; set; }
        public string ClickToCallId { get; set; }
        public string CurrentGroupHours { get; set; }

        public CustomOperator CustomOperator { get; set; }
        public string CustomPrompt { get; set; }
        public bool DisableCustomPrompt { get; set; }
        public bool GloballyVisible { get; set; }

        public List<GroupMember> GroupMember { get; set; }

        public bool HasMembers { get; set; }
        public RouteData HolidaysRoute { get; set; }
        public Hours Hours { get; set; }

        public int Id { get; set; }
        public bool IsDefault { get; set; }
        public string Language { get; set; }

        public DateTimeOffset? LastLoginTime { get; set; }

        public List<GroupMember> Members { get; set; }

        public string Name { get; set; }
        public string Number { get; set; }

        public List<OfficeHoliday> OfficeHolidays { get; set; }
        public RouteData OfficeRoute { get; set; }
        public RouteData OutOfOfficeRoute { get; set; }

        public DateTimeOffset? OverrideExpiresAt { get; set; }
        public bool OverrideHolidays { get; set; }

        public string PromptSet { get; set; }
        public Props Props { get; set; }

        public List<GroupRights> Rights { get; set; }

        public string TimeZoneId { get; set; }
        public string TranscriptionMode { get; set; }
        public string MemberName { get; set; }
    }
    public class GroupsResponse
    {
        public List<Groups> Value { get; set; }
    }

    public class RouteData
    {
        public bool IsPromptEnabled { get; set; }
        public string Prompt { get; set; }
        public Route Route { get; set; }
    }

    public class Route
    {
        public string External { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public List<string> Tags { get; set; }
        public string To { get; set; }
        public string Type { get; set; }
    }
    public class BreakTime
    {
        public bool IgnoreHolidays { get; set; }
        public List<Period> Periods { get; set; }
        public string Type { get; set; }
    }

    public class Period
    {
        public string DayOfWeek { get; set; }
        public string Start { get; set; }
        public string Stop { get; set; }
    }
    public class Hours
    {
        public bool IgnoreHolidays { get; set; }
        public List<Period> Periods { get; set; }
        public string Type { get; set; }
    }
    public class GroupMember
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
    public class GroupsMemberResponse
    {
        public List<GroupMember> Value { get; set; }
    }

    public class OfficeHoliday
    {
        public int Day { get; set; }
        public int DayEnd { get; set; }
        public string HolidayPrompt { get; set; }
        public int Id { get; set; }
        public bool IsRecurrent { get; set; }
        public int Month { get; set; }
        public int MonthEnd { get; set; }
        public string Name { get; set; }
        public string TimeOfEndDate { get; set; }
        public string TimeOfStartDate { get; set; }
        public int Year { get; set; }
        public int YearEnd { get; set; }
    }
    public class Props
    {
        public int DectMaxCount { get; set; }
        public string Fqdn { get; set; }
        public int LiveChatMaxCount { get; set; }
        public int OutboundRulesMaxCount { get; set; }
        public int PersonalContactsMaxCount { get; set; }
        public int PromptsMaxCount { get; set; }
        public string ResellerId { get; set; }
        public string ResellerName { get; set; }
        public int SbcMaxCount { get; set; }
        public string StartupLicense { get; set; }
        public string StartupOwnerEmail { get; set; }
        public DateTimeOffset? SubcriptionExpireDate { get; set; }
        public string Subscription { get; set; }
        public string SubscriptionType { get; set; }
        public string SystemNumberFrom { get; set; }
        public string SystemNumberTo { get; set; }
        public string TrunkNumberFrom { get; set; }
        public string TrunkNumberTo { get; set; }
        public int TrunksMaxCount { get; set; }
        public string UserNumberFrom { get; set; }
        public string UserNumberTo { get; set; }
    }

    public class CustomOperator
    {
        public string External { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public List<string> Tags { get; set; }
        public string To { get; set; }
        public string Type { get; set; }
    }


}
