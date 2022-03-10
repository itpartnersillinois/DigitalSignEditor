using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalSignEditor.Data {

    public class DbContextFactory<TContext> where TContext : DbContext {
        private readonly IServiceProvider provider;

        public DbContextFactory(IServiceProvider provider) {
            this.provider = provider;
        }

        public DbContext CreateDbContext() {
            if (provider == null) {
                throw new InvalidOperationException($"You must configure an instance of IServiceProvider");
            }

            return ActivatorUtilities.CreateInstance<TContext>(provider);
        }
    }
}