using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DigitalSignEditor.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalSignEditor.Data {

    public class SignContext : DbContext {
        private Guid id;

        public SignContext(DbContextOptions<SignContext> options) : base(options) {
            id = Guid.NewGuid();
            Debug.WriteLine($"{id} context created.");
        }

        public DbSet<CalendarItem> CalendarItems { get; set; }
        public DbSet<SignPermission> SignPermissions { get; set; }
        public DbSet<Sign> Signs { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<StorageItem> StorageItems { get; set; }

        public override void Dispose() {
            Debug.WriteLine($"{id} context disposed.");
            base.Dispose();
        }

        public override ValueTask DisposeAsync() {
            Debug.WriteLine($"{id} context disposed async.");
            return base.DisposeAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            Debug.WriteLine($"{id} context starting initial setup.");
            var signs = new List<Sign> {
                new Sign { Name = "Sample Lobby Sign", Id = -1, IsActive = true, LastUpdated = DateTime.Now, Description = "Sample sign used for testing",
                    SignType = SignType.Lobby, TwitterHandle = "edILLINOIS", College = CollegeType.College_of_Education, Url = "edlobby",
                    MinimumHeight = 600, MinimumWidth = 800, RatioHeight = 9, RatioWidth = 16 },
                new Sign { Name = "Sample Title Sign", Id = -2, IsActive = true, LastUpdated = DateTime.Now, Description = "Sample sign used for testing",
                    SignType = SignType.SimpleWithTitle, TwitterHandle = "", College = CollegeType.College_of_Education, Url = "oleary",
                    MinimumHeight = 600, MinimumWidth = 800, RatioHeight = 9, RatioWidth = 16 },
                new Sign { Name = "Sample Image Sign", Id = -3, IsActive = true, LastUpdated = DateTime.Now, Description = "Sample sign used for testing",
                    SignType = SignType.Simple, TwitterHandle = "", College = CollegeType.Gies_College_of_Business, Url = "gies1055",
                    MinimumHeight = 600, MinimumWidth = 800, RatioHeight = 9, RatioWidth = 16 },
                new Sign { Name = "Sample Image Sign #2", Id = -4, IsActive = true, LastUpdated = DateTime.Now, Description = "Sample sign used for testing",
                    SignType = SignType.Simple, TwitterHandle = "", College = CollegeType.Gies_College_of_Business, Url = "gies1041",
                    MinimumHeight = 600, MinimumWidth = 800, RatioHeight = 9, RatioWidth = 16 }
            };
            modelBuilder.Entity<Sign>().HasData(signs);

            Debug.WriteLine($"{id} context finishing initial setup.");
        }
    }
}