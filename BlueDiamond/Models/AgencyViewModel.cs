using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrueNorthX.DataModel;

namespace TrueNorthX.Models
{
    public class AgencyViewModel
    {
        public AgencyViewModel() { }

        public AgencyViewModel(Agency agency)
        {
            Agency = agency;
            TimeZone = agency.Tags.GetValue<TimeZoneInfo>("TimeZone", x => TimeZoneInfo.FindSystemTimeZoneById(x));
        }

        public Agency ToAgency()
        {
            this.Agency.Tags.Replace("TimeZone", TimeZone.Id);
            this.Agency.TagString = this.Agency.Tags.ToString();
            return Agency;
        }

        public Agency Agency { get; set; }

        public TimeZoneInfo TimeZone { get; set; }
    }
}