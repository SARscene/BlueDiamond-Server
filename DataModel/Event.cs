using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueDiamond.DataModel
{

    /// <summary>
    /// An event is the core data model of TrueNorth.EMIS
    /// It is the base class for all items that show up in activity streams.
    /// </summary>
    public class Event :
         IEquatable<Event>
    {
        public Event()
        {
            EventID = Guid.NewGuid();
            Core = new Core();
            Attachments = new List<Attachment>();
        }

        #region properties

        /// <summary>
        /// Unique ID
        /// </summary>
        public Guid EventID { get; set; }

        /// <summary>
        /// Core info about the event
        /// </summary>
        public Core Core { get; set; }

        #region incident
        /// <summary>
        /// What incident is this event a part of?
        /// </summary>
        public virtual Incident Incident { get; set; }

        public Guid IncidentID { get; set; }
        
        #endregion

        /// <summary>
        /// All Events have a text field
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Arbitrary data such as serialized XML
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Navigation property
        /// </summary>
        public virtual Event Parent { get; set; }

        /// <summary>
        /// Tags are "spike" | seperated values
        /// </summary>
        public string TagString { get; set; }

        /// <summary>
        /// Events have data attached
        /// </summary>
        public ICollection<Attachment> Attachments { get; set; }
        
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

        /// <summary>
        /// Delete this event
        /// </summary>
        public void Delete()
        {
            Core.IsDeleted = true;
            Core.Modified = DateTime.UtcNow;
        }

        #region equality
        public bool Equals(Event other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.EventID == other.EventID;
        }

        public static bool operator ==(Event a, Event b)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Equals((Event)b);
        }

        public static bool operator !=(Event a, Event b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            Event incident = obj as Event;
            if (incident == null)
                return false;

            return (incident.EventID == this.EventID);
        }

        public override int GetHashCode()
        {
            return EventID.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Event: \"{0}\"", this.EventID);
        }
        #endregion
    }

}
