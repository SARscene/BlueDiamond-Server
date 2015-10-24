using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlueDiamond.DataModel;
using BlueDiamond.Models;
using BlueDiamond.StorageModel;

namespace BlueDiamond.Controllers
{
    public class AgencyController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.Agencies.ToListAsync());
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Agency agencyModel = await db.Agencies.FindAsync(id);
            if (agencyModel == null)
                return HttpNotFound();
            return View(agencyModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AgencyID,Name,Description,TimeZone,TagString")] Agency agencyModel)
        {
            if (ModelState.IsValid)
            {
                agencyModel.AgencyID = Guid.NewGuid();
                agencyModel.Core = Core.Create(User.Identity.Name);
                db.Agencies.Add(agencyModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(agencyModel);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Agency agencyModel = await db.Agencies.FindAsync(id);
            if (agencyModel == null)
                return HttpNotFound();
            return View(agencyModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AgencyID,Name,Description,TimeZone,TagString")] Agency agencyModel)
        {
            if (ModelState.IsValid)
            {
                agencyModel.Core.Modify(User.Identity.Name);
                db.Entry(agencyModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(agencyModel);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Agency agencyModel = await db.Agencies.FindAsync(id);
            if (agencyModel == null)
                return HttpNotFound();
            return View(agencyModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Agency agencyModel = await db.Agencies.FindAsync(id);
            db.Agencies.Remove(agencyModel);
            await db.SaveChangesAsync();
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
