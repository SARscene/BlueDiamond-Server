using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BlueDiamond.DataModel;
using BlueDiamond.Utility;

namespace BlueDiamond.Models
{
    public class IncidentViewModel
    {
        public IncidentViewModel() { }

        static IncidentViewModel()
        {
            Mapper.CreateMap<Incident, IncidentViewModel>();
            Mapper.CreateMap<IncidentViewModel, Incident>();
        }

        public static IncidentViewModel Create(Incident incident)
        {
            var i = Mapper.Map<IncidentViewModel>(incident);
            i.Opened = incident.Opened.ToLocal(incident.Agency.TimeZone);
            i.Closed = incident.Closed.ToLocal(incident.Agency.TimeZone);
            return i;
        }

        public Incident ToIncident()
        {
            return Mapper.Map<Incident>(this);
        }
        public Guid IncidentID { get; set; }
        public Guid AgencyID{ get; set; }

        public Agency Agency { get; set; }

        public Core Core { get; set; }

        #region properties

        [Display(Name = "Task #")]
        [Required(ErrorMessage = "Task Number is required")]
        public int TaskNumber { get; set; }

        public string Data { get; set; }

        [Display(Name = "RCMP #")]
        public string RCMPNumber { get; set; }

        [Display(Name = "BCAS #")]
        public string BCASNumber { get; set; }

        [Display(Name = "ASE #")]
        public string ASENumber { get; set; }

        [Display(Name = "Sign In Location")]
        public string Location { get; set; }

        [Display(Name = "Incident Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Incident Name is required")]
        [Display(Name = "Incident Name")]
        public string Name { get; set; }

        [Display(Name = "Started")]
        public DateTime Opened { get; set; }

        [Display(Name = "Finished")]
        public DateTime? Closed { get; set; }

        #endregion

        public bool IsOpen { get { return this.Closed == null; } }
    }
}