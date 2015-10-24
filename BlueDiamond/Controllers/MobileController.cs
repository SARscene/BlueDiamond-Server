using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using BlueDiamond.DataModel;
using BlueDiamond.StorageModel;
using BlueDiamond.Utility;

namespace BlueDiamond.Controllers
{
    [AllowCrossSiteJson]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MobileController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Handshake allows a mobile application to connect and get the current open incident
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Handshake()
        {
            try
            {
                //get the open Incident
                Incident incident = db.Incidents.ToList().FirstOrDefault(x => x.IsOpen);
                incident.Agency = null;
                incident.SignIns.Clear();
                if (incident == null)
                    return NotFound();
                return Ok(incident);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error Handshake:\r\n{0}", ex);
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Sign-in lets the mobile application "sign in" or register with the incident
        /// </summary>
        /// <param name="id">member ID - can be null</param>
        /// <param name="name">user name</param>
        /// <param name="phoneNumber">user's phone number</param>
        [HttpGet]
        [ResponseType(typeof(string))]
        public IHttpActionResult SignIn(Guid? id)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            string name = nvc["name"];
            string phoneNumber = nvc["phoneNnumber"];
            try
            {
                // get or create a member GUID
                if (!id.HasValue)
                    id = Guid.NewGuid();

                //get the open Incident
                Incident incident = db.Incidents.ToList().FirstOrDefault(x => x.IsOpen);
                if (incident == null)
                    return NotFound();

                // get or create a new member
                Member member =  db.Members.Find(id);
                if (member == null)
                {
                    member = new DataModel.Member()
                    {
                        MemberID = id.Value,
                        FirstName = name,
                        LastName = "",
                        PhoneNumber = phoneNumber,
                        EmailAddress = "email@email.com",
                        AgencyID = incident.AgencyID,
                    };
                    db.Members.Add(member);
                    db.SaveChanges();
                }

                // create a new signin
                SignIn si = new SignIn()
                {
                    IncidentID = incident.IncidentID,
                    MemberID = id.Value
                };

                db.SignIns.Add(si);

                // signal the sign in
                Helpers.SignalSignIn(incident.IncidentID, member.LastName, member.FirstName, si.SignedIn);

                // save changes
                db.SaveChanges();

                return Ok(id.Value.ToString());
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error sign in:\r\n{0}", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Upload a track to the current incident
        /// </summary>
        /// <param name="trackModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> UploadTrack(TrackModel trackModel)
        {
            try
            {
                if (trackModel == null && trackModel.Track == null)
                    return NotFound();

                if (string.IsNullOrEmpty(trackModel.MemberID))
                    return NotFound();

                // get or create a member GUID
                Guid memberGUID = Guid.Empty;
                if (string.IsNullOrEmpty(trackModel.MemberID))
                    memberGUID = Guid.NewGuid();
                else
                    memberGUID = new Guid(trackModel.MemberID);

                Member member = await db.Members.FindAsync(memberGUID);
                if (member == null)
                    return NotFound();

                trackModel.Track.TrackID = Guid.NewGuid();
                foreach (var trackPoint in trackModel.Track.Points)
                    trackPoint.TrackPointID = Guid.NewGuid();

                db.Tracks.Add(trackModel.Track);

                await db.SaveChangesAsync();

                return Ok();
            }
            catch(Exception ex)
            {
                Trace.TraceError("Error uploading track:\r\n{0}", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IEnumerable<Member> Members()
        {
            var members = db.Members.Include(m => m.Agency).ToList();
            return members;
        }

        [HttpGet]
        [ResponseType(typeof(Member))]
        public async Task<IHttpActionResult> Member(Guid? id)
        {
            if (id == null)
                return NotFound();
            Member memberModel = await db.Members.FindAsync(id);
            if (memberModel == null)
                return NotFound();
            return Ok(memberModel);
        }

        [HttpGet]
        [ResponseType(typeof(Track))]
        public IHttpActionResult Track(Guid? id)
        {
            if (id == null)
                return NotFound();
            Track track = db.Tracks.Where(x => x.TrackID == id.Value).Include(x => x.Points).FirstOrDefault();
            if (track == null)
                return NotFound();
            return Ok(track);
        }

        [HttpGet]
        [ResponseType(typeof(Track))]
        public async Task<IHttpActionResult> Tracks(Guid? incidentID)
        {
            Incident incident;
            if (incidentID.HasValue)
                incident = await db.Incidents.FindAsync(incidentID.Value);
            else
                incident = db.Incidents.ToList().FirstOrDefault(x => x.IsOpen);

            if (incident == null)
                return NotFound();

            var tracks = db.Tracks
                .Where(x => x.IncidentID == incident.IncidentID)
                .Include(t => t.Points);

            return Ok(tracks);
        }
    }

    public class TrackModel
    {
        public string MemberID { get; set; }
        public Track Track { get; set; }
    }
}