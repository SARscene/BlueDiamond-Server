using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlueDiamond.StorageModel;

namespace BlueDiamond.Controllers
{
    public class HomeController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var incidents = db
                .Incidents
                .Where(x => x.Closed == null)
                .Include(i => i.Agency)
                .OrderBy(x=>x.Opened);
            return View(await incidents.ToListAsync());
        }

        public ActionResult Time()
        { 
            return View();
        }

    }
}