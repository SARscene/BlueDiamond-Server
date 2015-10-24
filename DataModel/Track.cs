using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using BlueToque.GpxLib;
using BlueToque.GpxLib.GPX1_1;

namespace BlueDiamond.DataModel
{
    public class Track
    {
        public Track()
        {
            TrackID = Guid.NewGuid();
            Core = new Core();
            Points = new List<TrackPoint>();
        }

        #region properties
        
        /// <summary>
        /// Unique ID
        /// </summary>
        public Guid TrackID { get; set; }

        /// <summary>
        /// Core info about the event
        /// </summary>
        public Core Core { get; set; }

        #endregion

        public string Name { get; set; }

        public Guid IncidentID { get; set; }

        public virtual Incident Incident{ get; set; }

        public ICollection<TrackPoint> Points { get; set; }

        public string ToXmlString()
        {
            GpxClass gpx = new GpxClass()
            {
                creator = "Blue Diamond",                
                trk = new trkTypeCollection()
            };

            trkType track = new trkType()
            {
                name = Name,
                
            };
            track.trkseg = new trksegTypeCollection();
            trksegType segment = new trksegType()
            {
                trkpt = new wptTypeCollection()
            };
            track.trkseg.Add(segment);

            foreach(var pt in this.Points)
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
            gpx.trk.Add(track);

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
                    Trace.TraceError("error serializing:\r\n{0}",ex);
                return null;
            }
            //return gpx.ToXml();
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

    public class Utf8StringWriter : StringWriter
    {
        // Use UTF8 encoding but write no BOM to the wire
        public override Encoding Encoding
        {
            get { return new UTF8Encoding(false); } // in real code I'll cache this encoding.
        }
    }

    /// <summary>
    /// Track point
    /// </summary>
    public class TrackPoint
    {
        public Guid TrackPointID { get; set; }

        public DateTime TimeStamp { get; set; }

        public float Latitude { get; set; }
        
        public float Longitude{ get; set; }
        
        public float Altitude { get; set; }
        
        public float Accuracy { get; set; }

    }

}
