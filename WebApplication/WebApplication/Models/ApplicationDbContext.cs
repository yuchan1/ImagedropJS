using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication.Common;

namespace WebApplication.Models {

    // ★ユーザー情報をEntity Frameworkを用いて扱うために定義しています。ASP.NET Identityでユーザー情報をEFを用いて扱うためのIdentityDbContextクラスを継承します。
    // ★これにより、EFを使ってユーザー情報の操作を行えます。
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false) {

        }

        /// <summary>
        /// ASP.NET Identity 2 ユーザー情報関係の作成
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        /// <summary>
        /// Override SaveChanges: CreateAt、UpdatedAtの更新用
        /// </summary>
        /// <returns></returns>
        public async override Task<int> SaveChangesAsync() {
            var now = DateTime.Now;
            var entries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            
            foreach (var entry in entries) {
                if (entry.Entity.GetType().FullName != "WebApplication.Models.ApplicationUser" && entry.Entity.GetType().FullName != "WebApplication.Models.ApplicationRole") {
                    if (entry.State == EntityState.Added && entry.CurrentValues.PropertyNames.Contains("CreatedAt")) {
                        entry.Property("CreatedAt").CurrentValue = now;
                    }

                    entry.Property("UpdatedAt").CurrentValue = now;
                }
            }
            return await base.SaveChangesAsync();
        }
        
        /// <summary>
        /// Override OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Modelの小数桁変更など
            // modelBuilder.Entity<Name>().Property(e => e.WireSize).HasPrecision(18, 4);
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
    }
}