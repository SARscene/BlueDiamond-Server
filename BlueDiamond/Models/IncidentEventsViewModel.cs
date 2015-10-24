using System.Collections.Generic;
using BlueDiamond.DataModel;

namespace BlueDiamond.Models
{
    public class IncidentEventsViewModel
    {
        public Incident Incident { get; set; }

        public Agency Agency { get; set; }

        public Member Agent { get; set; }

        public List<string> Categories { get; set; }

        public List<string> TextTags { get; set; }

        public List<Event> Events { get; set; }
    }
}