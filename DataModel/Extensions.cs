
namespace BlueDiamond.DataModel
{
    public static class Extensions
    {

        /// <summary>
        /// Fluent add tag to event
        /// </summary>
        /// <param name="e"></param>
        /// <param name="tag1"></param>
        /// <returns></returns>
        public static Event AddAttachment(this Event e, Attachment data)
        {
            e.Attachments.Add(data);
            return e;
        }

    }
}
