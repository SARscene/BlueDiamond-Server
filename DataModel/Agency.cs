using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueDiamond.DataModel
{
    [System.Diagnostics.DebuggerDisplay("Agency: {Name}({Description})")]
    public class Agency
    {
        public Agency()
        {
            AgencyID = Guid.NewGuid();
            Core = new Core();
        }

        public static readonly Guid ROOT_AGENCY = new Guid("{7055FCFB-1D2A-4723-BE20-2A8745038D4C}");

        public Guid AgencyID { get; set; }

        public Core Core { get; set; }

        [Display(Name="Agency Name")]
        public string Name { get; set; }

        [Display(Name = "Short Description")]
        public string Description { get; set; }

        /// <summary>
        /// Tags for this incident
        /// </summary>
        public string TagString { get; set; }

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

        [NotMapped]
        public TimeZoneInfo TimeZone
        {
            get 
            {
                return this
                    .Tags
                    .GetValue<TimeZoneInfo>("TimeZone", x => TimeZoneInfo.FindSystemTimeZoneById(x));
            }
            set 
            {
                if (value == null)
                    this.Tags.RemoveByName("TimeZone");
                else
                    this.Tags.Replace("TimeZone", value.Id);
                TagString = Tags.ToString();
            }

        }
    }
}