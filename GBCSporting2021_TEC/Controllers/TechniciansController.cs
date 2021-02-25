using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using GBCSporting2021_TEC.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Collections.Generic;

namespace GBCSporting2021_TEC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TechniciansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public TechniciansController()
        {
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        public TechniciansController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Technicians
        public async Task<ActionResult> List()
        {
            return View(await db.Technicians.ToListAsync());
        }

        // GET: Technicians/Add
        public ActionResult Add()
        {
            return View("AddEdit");
        }

        // POST: Technicians/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Include = "Id,Name,Email,Password,Phone")] Technician technician)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = technician.Email, Email = technician.Email };              
                var result = await UserManager.CreateAsync(user, technician.Password);
                if (result.Succeeded)
                {
                    technician.AspNetUser = user.Id;
                    if (db.AddUserToRole(UserManager, user.Id, "Technician"))
                    {
                        db.Technicians.Add(technician);
                        await db.SaveChangesAsync();

                        return RedirectToAction("List");
                    }
                }
            }

            return View("AddEdit", technician);
        }

        // GET: Technicians/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Technician technician = await db.Technicians.FindAsync(id);
            if (technician == null)
            {
                return HttpNotFound();
            }

            return View("AddEdit", technician);
        }

        // POST: Technicians/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Username,Password,Name,Email,Phone")] Technician technician)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(technician.AspNetUser);
                user.UserName = technician.Email;
                user.Email = technician.Email;

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    db.Entry(technician).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("List");
                }
            }
            return View("AddEdit", technician);
        }

        // GET: Technicians/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Technician technician = await db.Technicians.FindAsync(id);
            if (technician == null)
            {
                return HttpNotFound();
            }

            return View(technician);
        }

        // POST: Technicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            Technician technician = await db.Technicians.FindAsync(id);
            db.Technicians.Remove(technician);
            await db.SaveChangesAsync();

            var user = await UserManager.FindByIdAsync(technician.AspNetUser);
            var userLogins = user.Logins;
            var userRoles = await UserManager.GetRolesAsync(technician.AspNetUser);

            using (var transaction = db.Database.BeginTransaction())
            {
                foreach (var login in userLogins.ToList())
                {
                    await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if (userRoles.Count() > 0)
                {
                    foreach (var item in userRoles.ToList())
                    {
                        var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                await UserManager.DeleteAsync(user);
                transaction.Commit();                
            }
            
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
