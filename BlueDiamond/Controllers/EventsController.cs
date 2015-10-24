using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BlueDiamond.DataModel;
using BlueDiamond.Models;
using BlueDiamond.StorageModel;
using System.Data.Entity;

namespace BlueDiamond.Controllers
{
    public class EventsController : BaseController
    {
        ApplicationDbContext m_db = new ApplicationDbContext();

        public UserManager<ApplicationUser> m_userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public ActionResult View(Guid? id)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToAction("Login", "Account");

            //ApplicationUser user = m_userManager.FindById(User.Identity.GetUserId());
            //if (user== null || user.Agent.Agency == null)
            //    return RedirectToAction("LoggedIn", "Home");

            Incident incident = GetIncident(id, null);
            IncidentEventsViewModel model = new IncidentEventsViewModel()
            {
                Incident = incident,
                //Agency = user.Agent.Agency,
                //Agent = user.Agent
            };

            return View(model);
        }

        public ActionResult RadioLog(Guid? id)
        {
            if(!id.HasValue)
                return RedirectToAction("Error");
            var incident =  m_db.Incidents.Include(x=>x.Events).Include("Events.Attachments").FirstOrDefault(x=>x.IncidentID==id);

            List<MessageData> log = incident
                .Events
                .Where(x => x.Data == TagTypes.LOG)
                .OrderByDescending(x=>x.Core.Created)
                .Select<Event, MessageData>(x => x.Attachments.OfType<MessageData>().FirstOrDefault())
                .ToList();
            
            ViewBag.IncidentID = id;
            
            RadioLogViewModel model = new RadioLogViewModel()
            {
                Incident = incident,
                Log = log,
                NewLog = new MessageData() { Data=id.ToString()}

            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> RadioLog(Guid id,[Bind(Include = "From,To,Message,Data,Core")]MessageData messageData)
        {
            //Guid id = new Guid(messageData.Data);
            messageData.AttachmentID = Guid.NewGuid();
            Event e = new Event()
            {
                IncidentID = id,
                Data = TagTypes.LOG,
                Core = Core.Create(User.Identity.Name)
            }.AddAttachment(messageData);

            if (ModelState.IsValid)
            {
                //agencyModel.AgencyID = Guid.NewGuid();
                m_db.Events.Add(e);
                await m_db.SaveChangesAsync();
            }

            return RedirectToAction("RadioLog", new { id = id });
        }

       

        /// <summary>
        /// If the incident ID is null, 
        /// * return the incident the user is logged into 
        /// * return the default incident
        /// otherwise return the incident that has the given ID
        /// </summary>
        /// <param name="incidentID"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private Incident GetIncident(Guid? incidentID,ApplicationUser user)
        {
            // find the User
            if (!incidentID.HasValue)
            {
                if (user == null)
                    return m_db.Incidents.Find(Incident.ROOT_INCIDENT);

                // figure out what Incident the user is signed in to
                var si = m_db.SignIns.Where(x => x.MemberID == user.Agent.MemberID && !x.SignedOut.HasValue).FirstOrDefault();
                return
                (si == null) ?
                    m_db.Incidents.Find(Incident.ROOT_INCIDENT) :
                    si.Incident;
            }
            else
            {
                return m_db.Incidents.Find(incidentID);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_userManager != null)
                    m_userManager.Dispose();
                m_userManager = null;

                if (m_db != null)
                    m_db.Dispose();
                m_db = null;
            }
            base.Dispose(disposing);
        }

    }
}