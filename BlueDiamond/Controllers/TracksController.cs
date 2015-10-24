using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlueDiamond.DataModel;
using BlueDiamond.StorageModel;

namespace BlueDiamond.Controllers
{
    public class TracksController : Controller
    {
        public TracksController()
        {
            Incident incident = db.Incidents.ToList().FirstOrDefault(x => x.IsOpen);

            Track track = new Track()
            {
                TrackID = Guid.NewGuid(),
                IncidentID = incident.IncidentID
            };
            track.Points.Add(new TrackPoint()
            {
                TrackPointID = Guid.NewGuid(),
                Latitude = 49.50f,
                Longitude = 122.45f,
                Altitude = 100.0f,
                TimeStamp = DateTime.Now
            });
            db.Tracks.Add(track);
            db.SaveChanges();
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Track
        public async Task<ActionResult> Index(Guid? incidentID)
        {
            Incident incident ;
            if(incidentID.HasValue)
                incident = await db.Incidents.FindAsync(incidentID.Value);
            else
                incident =  db.Incidents.ToList().FirstOrDefault(x => x.IsOpen);

            if (incident == null)
                return HttpNotFound();

            var tracks = db.Tracks
                .Where(x=>x.IncidentID==incident.IncidentID)
                .Include(t=>t.Points);

            return View(tracks);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return HttpNotFound();
            Track track = db.Tracks.Where(x => x.TrackID == id.Value).Include(x => x.Points).FirstOrDefault();
            if (track == null)
                return HttpNotFound();
            return View(track);
}

        public ActionResult Download(Guid? id)
        {  
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Track track = db.Tracks.Where(x => x.TrackID == id.Value).Include(x => x.Points).FirstOrDefault();
            if (track == null)
                return HttpNotFound();

            string xmlString = track.ToXmlString();

            // prompt the user for downloading, set to true to try to show the file inline
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("{0}.gpx", track.TrackID),
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(Encoding.UTF8.GetBytes(xmlString.ToString()), "application/gpx+xml");
        }

    }
}