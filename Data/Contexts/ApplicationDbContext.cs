using Common.Utilities;
using Entities.Infrastructure;
using Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, long, IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public bool IgnorePreSaveChange { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntity).Assembly;

            // modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly, typeof(ApplicationDbContext).Assembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();
            // modelBuilder.AddPluralizingTableNameConvention();
           
            modelBuilder.Entity<User>(b =>
            {
                // Each User can have many UserClaims

                b.HasMany(e => e.UserRoles)
                    .WithOne(x => x.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
            modelBuilder.Entity<Role>(b =>
            {
                // Each User can have many UserClaims

                b.HasMany(e => e.UserRoles)
                    .WithOne(x => x.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            // modelBuilder.Entity<Transaction>()
            //     .Property(x => x.PaidAmount)
            //     .HasComputedColumnSql("[Amount] + [Tax] - [Discount]");
        }
    }
}