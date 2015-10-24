using System;
using System.Data.Entity;
using System.Diagnostics;
using BlueDiamond.DataModel;
using BlueDiamond.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlueDiamond.StorageModel
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbContextInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Incident> Incidents { get; set; }

        public DbSet<SignIn> SignIns { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Identifier> Identifiers { get; set; }

        public DbSet<Agency> Agencies { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        //public DbSet<Tag> Tags { get; set; }

        public DbSet<Event> Events { get; set; }

        public System.Data.Entity.DbSet<BlueDiamond.DataModel.Role> Roles1 { get; set; }

        //public DbSet<Role> Roles { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<TrackPoint> TrackPoints { get; set; }

    }

    public class ApplicationDbContextInitializer
         : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            try
            {
                base.Seed(context);

                var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                SecurityRoles.Register(
                    delegate(string name)
                    {
                        if (!rm.RoleExists(name))
                        {
                            var role = new IdentityRole(name);
                            rm.Create(role);
                        }
                    }
                );

                Agency rootAgency = new Agency()
                {
                    AgencyID = Agency.ROOT_AGENCY,
                    Name = "Admin",
                    Description = "System Administration",
                    Core = Core.Create("System")
                };
                context.Agencies.Add(rootAgency);

                Incident incident = new Incident()
                {
                    IncidentID = Incident.ROOT_INCIDENT,
                    Name = "Root",
                    Description = "System Administration Incident",
                    AgencyID = Agency.ROOT_AGENCY,
                    TaskNumber=0,
                    Opened = DateTime.UtcNow,
                    Closed= DateTime.UtcNow,
                    Core = Core.Create("System")
                };

                context.Incidents.Add(incident);
                context.SaveChanges();

                // create the administrator user
                var user = new ApplicationUser()
                {
                    UserName = "Administrator",
                    Agent = new Member()
                    {
                        MemberID = Member.SYSTEM_ADMINISTRATOR,
                        FirstName = "System",
                        LastName = "Administrator",
                        PhoneNumber="555-555-5555",
                        EmailAddress="michael.coyle@BlueToque.ca",
                        Core = Core.Create("System"),
                        AgencyID = Agency.ROOT_AGENCY
                    }
                };

                var result = um.Create(user, "administrator");
                um.AddToRole(user.Id, SecurityRoles.ADMINISTRATOR);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error:\r\n{0}", ex);
            }
        }
    }

    public static class SecurityRoles
    {
        public const string ADMINISTRATOR = "admin";
        public const string APPROVED = "approved";

        public static void Register(Action<string> action)
        {
            action(ADMINISTRATOR);
            action(APPROVED);
        }
    }

    public static class SecurityUsers
    {
        public const string ADMINISTRATOR = "administrator";
    }

}