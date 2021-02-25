using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GBCSporting2021_TEC.Models { 

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual Technician Technician1 { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name, string description)
            : base(name)
        {
            this.Description = description;
        }

        public virtual string Description { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole
    {
        public ApplicationUserRole()
            : base()
        { }

        public ApplicationRole Role { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //public DbSet<ApplicationRole> Roles { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("ModelBuilder is NULL");
            }

            base.OnModelCreating(modelBuilder);


            //Define keys and relations
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
            modelBuilder.Entity<ApplicationUser>().HasMany((ApplicationUser u) => u.UserRoles);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");

            modelBuilder.Entity<Country>().HasMany(e => e.Customers).WithRequired(e => e.aCountry)
                .HasForeignKey(e => e.Country).WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>().HasMany(e => e.Incidents).WithRequired(e => e.aCustomer)
                .HasForeignKey(e => e.Customer).WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>().HasMany(e => e.Registrations).WithRequired(e => e.aCustomer)
                .HasForeignKey(e => e.Customer).WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>().HasMany(e => e.Incidents).WithRequired(e => e.aProduct)
                .HasForeignKey(e => e.Product).WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>().HasMany(e => e.Registrations).WithRequired(e => e.aProduct)
                .HasForeignKey(e => e.Product).WillCascadeOnDelete(false);

            modelBuilder.Entity<Technician>().HasMany(e => e.Incidents).WithRequired(e => e.aTechnician)
                .HasForeignKey(e => e.Technician).WillCascadeOnDelete(false);

        }

        public bool RoleExists(ApplicationRoleManager roleManager, string name)
        {
            return roleManager.RoleExists(name);
        }

        public bool CreateRole(ApplicationRoleManager _roleManager, string name, string description = "")
        {
            var idResult = _roleManager.Create(new ApplicationRole(name, description));
            return idResult.Succeeded;
        }

        public bool AddUserToRole(ApplicationUserManager _userManager, string userId, string roleName)
        {
            var idResult = _userManager.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }

        public void ClearUserRoles(ApplicationUserManager userManager, string userId)
        {
            var user = userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.UserRoles);
            foreach (ApplicationUserRole role in currentRoles)
            {
                userManager.RemoveFromRole(userId, role.Role.Name);
            }
        }

        public void RemoveFromRole(ApplicationUserManager userManager, string userId, string roleName)
        {
            userManager.RemoveFromRole(userId, roleName);
        }

        public void DeleteRole(ApplicationDbContext context, ApplicationUserManager userManager, string roleId)
        {
            var roleUsers = context.Users.Where(u => u.UserRoles.Any(r => r.RoleId == roleId));
            var role = context.Roles.Find(roleId);

            foreach (var user in roleUsers)
            {
                this.RemoveFromRole(userManager, user.Id, role.Name);
            }
            context.Roles.Remove(role);
            context.SaveChanges();
        }


        public bool Seed(ApplicationDbContext context)
        {
            #if DEBUG
            bool success = false;

            ApplicationRoleManager _roleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context));

            success = this.CreateRole(_roleManager, "Admin", "Global Access");
            if (!success == true) return success;

            success = this.CreateRole(_roleManager, "Technician", "Update incidents");
            if (!success == true) return success;

            // Create my debug (testing) objects here

            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            ApplicationUser user = new ApplicationUser();
            PasswordHasher passwordHasher = new PasswordHasher();

            user.UserName = "admin@sportspro.com";
            user.Email = "admin@sportspro.com";

            IdentityResult result = userManager.Create(user, "Admin@1234");

            success = this.AddUserToRole(userManager, user.Id, "Admin");
            if (!success) return success;

            return success;
            #endif
        }

        /// <summary>
        /// Context Initializer
        /// </summary>
        public class DropCreateAlwaysInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        public DbSet<RoleViewModel> RoleViewModels { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Technician> Technicians { get; set; }
    }
}