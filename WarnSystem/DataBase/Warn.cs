using Synapse.Api;
using System;

namespace WarnSystem.DataBase
{
    [API]
    public class Warn
    {
        public int Id { get; set; }
        public string Reason { get; set; }

        public DateTime WarnDate { get; set; }

        public DateTime ExpiratonDate { get; set; }


        public override string ToString() => $"Date: {WarnDate}, Reason: {Reason}, IsExpired: {DateTime.Now >= ExpiratonDate}";
    }
}
