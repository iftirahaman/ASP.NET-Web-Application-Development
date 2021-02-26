using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GBCSporting2021_TEC.Models;

namespace GBCSporting2021_TEC.Controllers
{
    [Authorize]
    public class IncidentsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Incidents
        [Authorize(Roles = "Admin, Technician")]
        public async Task<ActionResult> List()
        {
            var incidents = db.Incidents.Include(i => i.aCustomer)
                .Include(i => i.aProduct)
                .Include(i => i.aTechnician);
            return View(await incidents.ToListAsync());
        }

        // GET: Incidents/Details/5
        [Authorize(Roles = "Admin, Technician")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = await db.Incidents.FindAsync(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // GET: Incidents/Add
        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            ViewBag.Customer = new SelectList(db.Customers, "Id", "Fullname");
            ViewBag.Product = new SelectList(db.Products, "Id", "Name");
            ViewBag.Technician = new SelectList(db.Technicians, "Id", "Name");
            return View("AddEdit");
        }

        // POST: Incidents/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Add([Bind(Include = "Id,Title,Customer,Product,Technician,Description,DateOpened,DateClosed")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Incidents.Add(incident);
                await db.SaveChangesAsync();
                return RedirectToAction("List");
            }

            ViewBag.Customer = new SelectList(db.Customers, "Id", "Fullname", incident.Customer);
            ViewBag.Product = new SelectList(db.Products, "Id", "Name", incident.Product);
            ViewBag.Technician = new SelectList(db.Technicians, "Id", "Name", incident.Technician);
            return View("AddEdit", incident);
        }

        // GET: Incidents/Edit/5
        [Authorize(Roles = "Admin, Technician")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = await db.Incidents.FindAsync(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            ViewBag.Customer = new SelectList(db.Customers, "Id", "Fullname", incident.Customer);
            ViewBag.Product = new SelectList(db.Products, "Id", "Name", incident.Product);
            ViewBag.Technician = new SelectList(db.Technicians, "Id", "Name", incident.Technician);
            return View("AddEdit", incident);
        }

        // POST: Incidents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Technician")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Customer,Product,Technician,Description,DateOpened,DateClosed")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incident).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("List");
            }
            ViewBag.Customer = new SelectList(db.Customers, "Id", "Fullname", incident.Customer);
            ViewBag.Product = new SelectList(db.Products, "Id", "Name", incident.Product);
            ViewBag.Technician = new SelectList(db.Technicians, "Id", "Name", incident.Technician);
            return View("AddEdit", incident);
        }

        // GET: Incidents/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = await db.Incidents.FindAsync(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Incident incident = await db.Incidents.FindAsync(id);
            db.Incidents.Remove(incident);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
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
