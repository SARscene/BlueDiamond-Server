using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BlueDiamond.DataModel;
using BlueDiamond.StorageModel;
using BlueDiamond.Utility;

namespace BlueDiamond.Controllers
{
    [AllowCrossSiteJson]
    public class MobileController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public  IHttpActionResult Handshake()
        {
            //get the open Incident
            Incident incident =  db.Incidents.ToList().FirstOrDefault(x => x.IsOpen);
            incident.Agency = null;
            incident.SignIns.Clear();
            if (incident == null)
                return NotFound();
            return Ok(incident);
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

        public async Task<IHttpActionResult> SignIn(string memberID, string name)
        {
            // get or create a member GUID
            Guid memberGUID  = Guid.Empty;
            if (string.IsNullOrEmpty(memberID))
                memberGUID = Guid.NewGuid();
            else
                memberGUID = new Guid(memberID);

            //get the open Incident
            Incident incident = db.Incidents.ToList().FirstOrDefault(x => x.IsOpen);
            if (incident == null)
                return NotFound();

            // get or create a new member
            Member member = await db.Members.FindAsync(memberID);
            if (member == null)
            {
                member = new DataModel.Member()
                {
                    MemberID = memberGUID,
                    FirstName = name,
                    LastName = "",
                    PhoneNumber = "5555555555",
                    EmailAddress = "email@email.com",
                    AgencyID = incident.AgencyID,
                };
                db.Members.Add(member);
                await db.SaveChangesAsync();
            }

            // create a new signin
            SignIn si = new SignIn()
                {
                    IncidentID = incident.IncidentID,
                    MemberID = memberGUID
                };

            db.SignIns.Add(si);

            // signal the sign in
            Helpers.SignalSignIn(incident.IncidentID, member.LastName, member.FirstName, si.SignedIn);
            
            // save changes
            await db.SaveChangesAsync();

            return Ok(memberGUID);
        }

        [HttpPost]
        public async Task<IHttpActionResult> UploadTrack(TrackModel trackModel)
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

        [HttpGet]
        [ResponseType(typeof(Track))]
        public IHttpActionResult Track(Guid? id)
        {
            Track track = new Track()
            {
            };
            track.Points.Add(new TrackPoint()
                {
                    Latitude = 180,
                    Longitude = 90,
                    Altitude = 100,
                    TimeStamp = DateTime.Now
                });
            return Ok(track);
            //if (id == null)
            //    return NotFound();
            //Track track = db.Tracks.Where(x => x.TrackID == id.Value).Include(x => x.Points).FirstOrDefault();
            //if (track == null)
            //    return NotFound();
            //return Ok(track);
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