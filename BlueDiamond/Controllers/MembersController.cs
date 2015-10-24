using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using CsvHelper.Configuration;
using BlueDiamond.DataModel;
using BlueDiamond.StorageModel;

namespace BlueDiamond.Controllers
{
    public class MembersController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Members
        public async Task<ActionResult> Index()
        {
            var members = db.Members.Include(m => m.Agency);
            return View(await members.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member memberModel = await db.Members.FindAsync(id);
            if (memberModel == null)
            {
                return HttpNotFound();
            }
            return View(memberModel);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            ViewBag.AgencyId = new SelectList(db.Agencies, "AgencyId", "Name");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MemberId,Core,FirstName,LastName,PhoneNumber,EmailAddress,AgencyID")] Member memberModel)
        {
            if (ModelState.IsValid)
            {
                memberModel.MemberID = Guid.NewGuid();
                db.Members.Add(memberModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AgencyId = new SelectList(db.Agencies, "AgencyId", "Name", memberModel.AgencyID);
            return View(memberModel);
        }

        // GET: Members/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member memberModel = await db.Members.FindAsync(id);
            if (memberModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgencyId = new SelectList(db.Agencies, "AgencyId", "Name", memberModel.AgencyID);
            return View(memberModel);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MemberId,Core,FirstName,LastName,PhoneNumber,EmailAddress,AgencyID")] Member memberModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AgencyId = new SelectList(db.Agencies, "AgencyId", "Name", memberModel.AgencyID);
            return View(memberModel);
        }

        // GET: Members/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member memberModel = await db.Members.FindAsync(id);
            if (memberModel == null)
            {
                return HttpNotFound();
            }
            return View(memberModel);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Member memberModel = await db.Members.FindAsync(id);
            db.Members.Remove(memberModel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public FileResult Export()
        {
            var members = db.Members.Include(m => m.Agency);
            MemoryStream ms = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms))
            {
                using (var csv = new CsvWriter(sw))
                {
                    csv.Configuration.RegisterClassMap<MemberModelCSVMap>();
                    csv.WriteHeader(typeof(Member));
                    foreach (var member in members)
                        csv.WriteRecord(member);

                    sw.Flush();
                    byte[] bytes = ms.GetBuffer();
                    return File(bytes, "text/csv", "MemberNames.csv");
                }
                //ms.Position = 0;
            }
        }

        public sealed class MemberModelCSVMap : CsvClassMap<Member>
        {
            public MemberModelCSVMap()
            {
                Map(x => x.MemberID);
                Map(x => x.FirstName);
                Map(x => x.LastName);
                Map(x => x.PhoneNumber);
                Map(x => x.EmailAddress);
                Map(x => x.RoleID);
                Map(x => x.AgencyID);
            }

        }

        public ActionResult Import()
        {
            ViewBag.AgencyId = new SelectList(db.Agencies, "AgencyId", "Name");
            return View();
        }

        void UpdateMember(Member model)
        {
            Member member = db.Members.Find(model.MemberID);
            if (member == null)
                return;
            member.Update(model);
            db.SaveChanges();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            if (file.ContentLength <=0)
            {
                ViewBag.Error = "Error: file upload failed";
                return View("Error");
            }

            //var fileName = Path.GetFileName(file.FileName);
            //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            //file.SaveAs(path);

            using (var sw = new StreamReader(file.InputStream))
            {
                var csv = new CsvReader(sw);
                csv.Configuration.RegisterClassMap<MemberModelCSVMap>();
                csv.Configuration.IgnoreReadingExceptions = true;

                foreach (var model in csv.GetRecords<Member>())
                {
                    if (model.AgencyID == Guid.Empty)
                        model.AgencyID = null;
                    if (model.RoleID == Guid.Empty)
                        model.RoleID = null;

                    if (model.MemberID == Guid.Empty)
                    {
                        var member = db.Members.FirstOrDefault(x => x.FirstName == model.FirstName && x.LastName == model.LastName);
                        if (member == null)
                        {
                            model.MemberID = Guid.NewGuid();
                            db.Members.Add(model);
                        }
                        else
                            member.Update(model);
                        db.SaveChanges();

                    }
                    else
                    {
                        // update the model
                        if (db.Members.Find(model.MemberID) != null)
                            UpdateMember(model);
                        else
                            db.Members.Add(model);
                    }
                }

            }


            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
