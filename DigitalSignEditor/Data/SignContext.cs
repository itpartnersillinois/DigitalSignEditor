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

        public DbSet<SignItem> SignItems { get; set; }
        public DbSet<SignPermission> SignPermissions { get; set; }
        public DbSet<Sign> Signs { get; set; }
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
            var signs = new List<Sign>
{
                new Sign { Name = "Sample Sign", Id = -1, IsActive = true, LastUpdated = DateTime.Now, Description = "Sample sign used for testing",
                    College = "College of Education", MinimumHeight = 600, MinimumWidth = 800, RatioHeight = 9, RatioWidth = 16 }
            };
            modelBuilder.Entity<Sign>().HasData(signs);

            Debug.WriteLine($"{id} context finishing initial setup.");
        }
    }
}