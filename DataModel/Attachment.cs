using System;

namespace BlueDiamond.DataModel
{
    /// <summary>
    /// A tag is a way of marking up events.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Attachment: {Name}:({Type})")]
    public class Attachment
    {
        public Attachment()
        {
            AttachmentID = Guid.NewGuid();
            Core = new Core();
            Type = this.GetType().Name;
        }

        /// <summary>
        /// Primary key
        /// </summary>
        public Guid AttachmentID { get; set; }

        /// <summary>
        /// Name of the attachment
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Arbitrary data such as XML 
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Attachment type
        /// </summary>
        public string Type { get; set; }

        #region event

        /// <summary>
        /// Foreign Key
        /// </summary>
        public Guid EventID { get; set; }

        /// <summary>
        /// The event this tag is on
        /// </summary>
        public virtual Event Event { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Core Core { get; set; }

    }
}
