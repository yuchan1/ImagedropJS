using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repositories {
    public class MemberRepository : IMemberRepository, IDisposable {
        private readonly ApplicationDbContext context;

        public MemberRepository(ApplicationDbContext context) {
            this.context = context;
            // this.context.Database.Log = (log) => System.Diagnostics.Debug.WriteLine(log);
        }

        public async Task<IEnumerable<Member>> GetMembersAsync() {
            return await context.Members
                .OrderBy(e => e.Order).ToListAsync();
        }

        /* No use
        public async Task<IEnumerable<Member>> FindAsync(string keyword) {
            return await context.Members
                .Where(e => e.MemberName.Contains(keyword))
                .OrderBy(e => e.Order).ToListAsync();
        }
         */

        public async Task<Member> GetMemberByIdAsync(int? id) {
            return await context.Members.FindAsync(id);
        }

        public async Task InsertAsync(Member member) {
            await Task.Run(() => {
                context.Members.Add(member);
            });
        }

        public async Task UpdateAsync(Member member) {
            Member r = await context.Members.FindAsync(member.Id);
            r.Name = member.Name;
            r.Remarks = member.Remarks;
            r.Order = member.Order;
            r.IsDeleted = member.IsDeleted;
            r.CreatedAt = member.CreatedAt;
            r.UpdatedAt = member.UpdatedAt;
            r.RowVersion = member.RowVersion;
            context.Members.Attach(r);
            context.Entry(r).State = EntityState.Modified;
        }

        public async Task DeleteAsync(int id) {
            Member member = await context.Members.FindAsync(id);
            member.IsDeleted = true;                // context.Members.Remove(member);
            context.Members.Attach(member);
            context.Entry(member).State = EntityState.Modified;          
        }

        public async Task SaveAsync() {
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Member>> GetSelectListsAsync() {
            return await context.Members
                .Where(e => !e.IsDeleted)
                .OrderBy(e => e.Order).ToListAsync();
        }

        public async Task SortAsync(int? updatedAtOrder) {
            await Task.Run(() => {
                string sql = @"UPDATE [dbo].[M_Members]
                    SET [Order] = B.[Order]
                    FROM [dbo].[M_Members] AS A
                    LEFT OUTER JOIN (
                        SELECT [Id], ROW_NUMBER() OVER (
                            ORDER BY
                                [Order] ASC,  
	                            CASE WHEN {0} = 'ASC' THEN [UpdatedAt] ELSE null END ASC, 
	                            CASE WHEN {0} = 'DESC' THEN [UpdatedAt] ELSE null END DESC
                        ) AS 'Order' 
                        FROM [dbo].[M_Members]
                    ) AS B 
                    ON A.[Id] = B.[Id]";

                context.Database.ExecuteSqlCommand(sql, updatedAtOrder == 1 ? "ASC" : "DESC");
            });
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) context.Dispose();
            }

            this.disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}