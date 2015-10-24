using System;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlueDiamond.DataModel;
using BlueDiamond.Models;
using BlueDiamond.StorageModel;
using BlueDiamond.Utility;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Controls;

namespace BlueDiamond.Controllers
{
    public class IncidentsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var incidents = await db
                .Incidents
                .Where(x => !x.Core.IsDeleted)
                .Include(i => i.Agency)
                .OrderByDescending(x => x.Opened)
                .ToListAsync();
            return View(incidents);
        }

        public string GetUrl()
        {
            return string.Format("http://{0}:{1}/", GetIPAddress(), Request.Url.Port);
        }

        public FileContentResult Show(Guid id)
        {
            QrEncoder enc = new QrEncoder();
            QrCode code = enc.Encode(GetUrl());

            Renderer renderer = new Renderer(5);
            Image image = new Bitmap(256,256);
            using (Graphics graphics = Graphics.FromImage(image))
                renderer.Draw(graphics, code.Matrix);

            byte[] imageByte = imageToByteArray(image);
            string contentType = "image/png";

            return File(imageByte, contentType);
        }

        public byte[] imageToByteArray(Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
        public string GetIPAddress()
        {
            string myHost = System.Net.Dns.GetHostName();
            string ipAddress = System.Net.Dns.GetHostEntry(myHost).AddressList.FirstOrDefault(x=>x.AddressFamily== System.Net.Sockets.AddressFamily.InterNetwork).ToString();
            return ipAddress;
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Incident incident = await db.Incidents.FindAsync(id);
            if (incident == null)
                return HttpNotFound();
            return View(incident);
        }

        public ActionResult Create()
        {
            ViewBag.AgencyID = new SelectList(db.Agencies, "AgencyID", "Name");
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "IncidentID,Core,TaskNumber,RCMPNumber,BCASNumber,ASENumber,Location,Description,Name,Opened,Closed,AgencyID,Agency")] IncidentViewModel incidentModel)
        {
            Incident incident = incidentModel.ToIncident();
            if (ModelState.IsValid)
            {
                incident.Agency = await db.Agencies.FindAsync(incident.AgencyID);
                incident.Opened = DateTime.UtcNow;
                incident.IncidentID = Guid.NewGuid();
                incident.Core = Core.Create(User.Identity.Name);
                db.Incidents.Add(incident);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AgencyID = new SelectList(db.Agencies, "AgencyID", "Name", incident.AgencyID);
            return View(incident);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Incident incident = await db.Incidents.Include(x => x.Agency).FirstOrDefaultAsync(x => x.IncidentID == id);
            if (incident == null)
                return HttpNotFound();
            ViewBag.AgencyID = new SelectList(db.Agencies, "AgencyID", "Name", incident.AgencyID);
            return View(IncidentViewModel.Create(incident));
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "IncidentID,Core,TaskNumber,RCMPNumber,BCASNumber,ASENumber,Location,Description,Name,Opened,Closed,AgencyID")] IncidentViewModel incidentModel)
        {
            
            Incident incident = incidentModel.ToIncident();
            if (ModelState.IsValid)
            {
                incident.Agency = await db.Agencies.FindAsync(incident.AgencyID);
                incident.Opened = incidentModel.Opened.ToUtc(incident.Agency.TimeZone);
                incident.Closed = incidentModel.Closed.ToUtc(incident.Agency.TimeZone);

                incident.Core.Modify(User.Identity.Name);
                db.Entry(incident).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AgencyID = new SelectList(db.Agencies, "AgencyID", "Name", incident.AgencyID);
            return View(IncidentViewModel.Create(incident));
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Incident incident = await db.Incidents.FindAsync(id);
            if (incident == null)
                return HttpNotFound();
            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Incident incident = await db.Incidents.FindAsync(id);
            incident.Core.Delete(User.Identity.Name);
            //db.Incidents.Remove(incident);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Do sign-in form for incident with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> SignIn(Guid id, Guid? memberID)
        {
            if (memberID.HasValue)
            {
                SignIn si = new SignIn()
                {
                    IncidentID = id,
                    MemberID = memberID.Value
                };

                db.SignIns.Add(si);
                Member member = await db.Members.FindAsync(memberID);
                Helpers.SignalSignIn(id, member.LastName, member.FirstName, si.SignedIn);
                await db.SaveChangesAsync();
            }

            CheckInViewModel model = new CheckInViewModel();
            model.Incident = db.Incidents.Include(i => i.SignIns).FirstOrDefault(x => x.IncidentID == id);

            var signIns = db.SignIns
                .Where(x => x.IncidentID == id)
                .ToList();

            model.SignIns = db.Members.Select<Member, MemberSignIn>(x => new MemberSignIn() { Member = x }).ToList();

            foreach (var m in model.SignIns)
            {
                var si = signIns
                    .OrderByDescending(x => x.SignedIn)
                    .FirstOrDefault(x => x.MemberID == m.Member.MemberID);
                if (si != null)
                    m.SignIn = si;
            }
            return View(model);
        }

        public async Task<ActionResult> SignOut(Guid id, Guid memberID)
        {
            //retrieve the sign in record
            var incident = db
                .Incidents
                .Include(i => i.SignIns)
                .FirstOrDefault(x => x.IncidentID == id);

            var si = incident
                .SignIns
                .Where(x=>!x.SignedOut.HasValue)
                .FirstOrDefault(x => x.MemberID == memberID);
            
            if(si==null)
            {
                // return an error that the si was not found
            }

            si.SignedOut = DateTime.UtcNow;
            await db.SaveChangesAsync();
            
            return RedirectToAction("SignIn", new { id = id });
        }

        public async Task<ActionResult> Open(Guid id)
        {
            var incident = await db.Incidents.FindAsync(id);
            incident.Open();
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = id });
        }

        public async Task<ActionResult> Close(Guid id)
        {
            var incident = await db.Incidents.FindAsync(id);
            incident.Close();
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = id });
        }

        public async Task<ActionResult> SignInStatus(Guid id)
        {
            CheckInViewModel model = new CheckInViewModel();
            model.Incident = await db.Incidents.FindAsync(id);
            model.SignIns = await db
                .SignIns
                .Where(x => x.IncidentID == id)
                .Where(x => x.SignedOut == null)
                .Join(db.Members,
                    s => s.MemberID,
                    m => m.MemberID,
                    (s, m) => new MemberSignIn() { Member = m, SignIn = s })
                .ToListAsync();

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
