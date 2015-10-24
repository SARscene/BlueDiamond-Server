using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueDiamond.DataModel;

namespace BlueDiamond.Models
{
    public class RadioLogViewModel
    {
        public Incident Incident { get; set; }

        public List<MessageData> Log { get; set; }
        public MessageData NewLog{ get; set; }
    }
}