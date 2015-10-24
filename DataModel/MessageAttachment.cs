
namespace BlueDiamond.DataModel
{
    /// <summary>
    /// A chat event from one agent to another
    /// containing simple text
    /// Other information can be attached via tags
    /// </summary>
    public class MessageData : Attachment
    {
        /// <summary>
        /// Who the message is from
        /// </summary>
        public virtual string From { get; set; }

        /// <summary>
        /// Who the message is to
        /// This can be null if the message is to everyone
        /// </summary>
        public virtual string To { get; set; }

        /// <summary>
        /// A string message
        /// </summary>
        public string Message { get; set; }

    }
}
