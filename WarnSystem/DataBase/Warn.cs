using Synapse.Api;
using System;

namespace WarnSystem.DataBase
{
    [API]
    public class Warn
    {
        public int Id { get; set; }

        public string StaffUserId { get; set; }

        public string StaffNickName { get; set; }

        public string Reason { get; set; }

        public DateTime WarnDate { get; set; }

        public DateTime ExpiratonDate { get; set; }

        public override string ToString() => $"[{WarnDate}] for Reason {Reason} by {StaffNickName}; IsExpired: {DateTime.Now >= ExpiratonDate}";
    }
}
