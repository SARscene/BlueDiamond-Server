using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueDiamond.DataModel
{
    [System.Diagnostics.DebuggerDisplay("Incident: {Name}({Description})")]
    public class Incident
    {
        public Incident()
        {
            IncidentID = Guid.NewGuid();
            Core = new Core();
            Opened = DateTime.UtcNow;
            Events = new List<Event>();
        }

        public static readonly Guid ROOT_INCIDENT = new Guid("{DD8DEAEE-9D7C-47DA-98C5-1783D09D5BA3}");

        /// <summary>
        /// Database ID
        /// </summary>
        public Guid IncidentID { get; set; }

        /// <summary>
        /// Core fields
        /// </summary>
        public Core Core { get; set; }

        #region properties

        [Display(Name = "Task #")]
        [Required(ErrorMessage="Task Number is required")]
        public int TaskNumber { get; set; }

        /// <summary>
        /// Arbitrary string data, suitable for deserialization
        /// </summary>
        public string Data { get; set; }

        [Display(Name = "RCMP #")]
        public string RCMPNumber { get; set; }

        [Display(Name = "BCAS #")]
        public string BCASNumber { get; set; }

        [Display(Name = "ASE #")]
        public string ASENumber { get; set; }

        /// <summary>
        /// A description of the sign-in location
        /// </summary>
        [Display(Name = "Sign In Location")]
        public string Location { get; set; }

        /// <summary>
        /// A description of the task
        /// </summary>
        [Display(Name = "Incident Description")]
        public string Description { get; set; }

        /// <summary>
        /// The task name
        /// </summary>
        [Required(ErrorMessage= "Incident Name is required")]
        [Display(Name = "Incident Name")]
        public string Name { get; set; }

        [Display(Name = "Started")]
        public DateTime Opened { get; set; }

        [Display(Name = "Finished")]
        public DateTime? Closed { get; set; }

        #endregion

        #region relationships

        public virtual ICollection<SignIn> SignIns { get; set; }

        #region Agency
        [Required(ErrorMessage = "Agency is required")]
        public Guid? AgencyID { get; set; }

        public virtual Agency Agency { get; set; }
        #endregion

        /// <summary>
        /// An incident has a collection of events
        /// </summary>
        public ICollection<Event> Events { get; set; }

        /// <summary>
        /// Tags for this incident
        /// </summary>
        public string TagString { get; set; }

        #endregion

        #region tags
        TagCollection m_tags;

        [NotMapped]
        public TagCollection Tags
        {
            get
            {
                if (m_tags == null)
                    m_tags = TagCollection.ToTags(TagString);
                return m_tags;
            }
            set
            {
                m_tags = value;
                TagString = m_tags.ToString();
            }
        }
        #endregion

        #region methods

        public void Open()
        {
            Closed = null;
        }

        public void Close() { Closed = DateTime.UtcNow; }

        public void Delete()
        {
            Core.IsDeleted = true;
            Core.Modified = DateTime.UtcNow;
        }

        public bool IsOpen { get { return !Closed.HasValue; } }

        public bool IsClosed { get { return Closed.HasValue; } }

        //[NotMapped]
        //public DateTime OpenedLocal
        //{
        //    get
        //    {
        //        return (Agency.TimeZone != null) ?
        //            TimeZoneInfo.ConvertTimeFromUtc(Opened, Agency.TimeZone) :
        //            Opened.ToLocalTime();
        //    }
        //    set
        //    {
        //        Opened = (Agency.TimeZone != null) ?
        //            TimeZoneInfo.ConvertTimeToUtc(value, Agency.TimeZone) :
        //            value;
        //    }
        //}

        //[NotMapped]
        //public DateTime? ClosedLocal
        //{
        //    get
        //    {
        //        return Closed.HasValue ?
        //            (Agency.TimeZone == null ? Closed.Value.ToLocalTime() : TimeZoneInfo.ConvertTimeFromUtc(Opened, Agency.TimeZone)) :
        //            Closed;
        //    }
        //}
        
        #endregion

        #region equality
        public bool Equals(Incident other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.IncidentID == other.IncidentID;
        }

        public static bool operator ==(Incident a, Incident b)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Equals((Incident)b);
        }

        public static bool operator !=(Incident a, Incident b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            Incident incident = obj as Incident;
            if (incident == null)
                return false;

            return (incident.IncidentID == this.IncidentID);
        }

        public override int GetHashCode()
        {
            return IncidentID.GetHashCode();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ?
                IncidentID.ToString() :
                string.Format("Incident: \"{0}\"", this.Name);
        }
        #endregion

    }
}