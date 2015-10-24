using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Serialization;
using BlueDiamond.DataModel;
using BlueDiamond.StorageModel;
using BlueToque.GpxLib;
using BlueToque.GpxLib.GPX1_1;

namespace BlueDiamond.Controllers
{
    public class TracksController : Controller
    {
        public TracksController()
        {
            //Incident incident = db.Incidents.ToList().FirstOrDefault(x => x.IsOpen);

            //Track track = new Track()
            //{
            //    TrackID = Guid.NewGuid(),
            //    IncidentID = incident.IncidentID
            //};
            //track.Points.Add(new TrackPoint()
            //{
            //    TrackPointID = Guid.NewGuid(),
            //    Latitude = 49.50f,
            //    Longitude = 122.45f,
            //    Altitude = 100.0f,
            //    TimeStamp = DateTime.Now
            //});
            //db.Tracks.Add(track);
            //db.SaveChanges();
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

            ViewBag.IncidentID = incident.IncidentID;
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

        public async Task<ActionResult> DownloadAll(Guid? incidentID)
        {
            Incident incident;
            if (incidentID.HasValue)
                incident = await db.Incidents.FindAsync(incidentID.Value);
            else
                incident = db.Incidents.ToList().FirstOrDefault(x => x.IsOpen);

            if (incident == null)
                return HttpNotFound();

            var tracks = db.Tracks
                .Where(x => x.IncidentID == incident.IncidentID)
                .Include(t => t.Points)
                .ToList();

            string xmlString = CreateGPX(incident, tracks);

            // prompt the user for downloading, set to true to try to show the file inline
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("{0}.gpx", incident.IncidentID),
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(Encoding.UTF8.GetBytes(xmlString.ToString()), "application/gpx+xml");
        }

        private string CreateGPX(Incident incident, List<Track> tracks)
        {
            GpxClass gpx = new GpxClass()
            {
                creator = "Blue Diamond",
                trk = new trkTypeCollection()
            };

            gpx.metadata = new metadataType()
            {
                name = incident.Name,
            };

            foreach (var track in tracks)
            {
                trkType trk = new trkType()
                {
                    name = incident.Name,
                };

                trk.trkseg = new trksegTypeCollection();
                trksegType segment = new trksegType()
                {
                    trkpt = new wptTypeCollection()
                };

                trk.trkseg.Add(segment);

                foreach (var pt in track.Points)
                {
                    wptType wpt = new wptType()
                    {
                        lat = (decimal)pt.Latitude,
                        lon = (decimal)pt.Longitude,
                        ele = (decimal)pt.Altitude,
                        eleSpecified = true,
                        time = pt.TimeStamp,
                    };
                    segment.trkpt.Add(wpt);
                }

                gpx.trk.Add(trk);
            }

            try
            {
                var gpx11 = ToGpx1_1(gpx);
                XmlSerializer serializer = new XmlSerializer(gpx11.GetType());
                Utf8StringWriter sw = new Utf8StringWriter();
                serializer.Serialize(sw, gpx11);
                return sw.ToString();
            }
            catch (Exception ex)
            {
                Trace.TraceError("error serializing:\r\n{0}", ex);
                return null;
            }
        }

        internal BlueToque.GpxLib.GPX1_1.gpxType ToGpx1_1(GpxClass gpx)
        {
            return new BlueToque.GpxLib.GPX1_1.gpxType()
            {
                creator = gpx.creator,
                extensions = gpx.extensions,
                metadata = gpx.metadata,
                version = gpx.version,
                rte = gpx.rte == null ? new BlueToque.GpxLib.GPX1_1.rteTypeCollection() : new BlueToque.GpxLib.GPX1_1.rteTypeCollection(gpx.rte),
                trk = gpx.trk == null ? new BlueToque.GpxLib.GPX1_1.trkTypeCollection() : new BlueToque.GpxLib.GPX1_1.trkTypeCollection(gpx.trk),
                wpt = gpx.wpt == null ? new BlueToque.GpxLib.GPX1_1.wptTypeCollection() : new BlueToque.GpxLib.GPX1_1.wptTypeCollection(gpx.wpt),
            };

        }


    }
}