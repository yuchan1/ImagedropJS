using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repositories {
    public interface IMemberRepository : IDisposable {

        Task<IEnumerable<Member>> GetMembersAsync();
        // Task<IEnumerable<Member>> FindAsync(string keyword);
        Task<Member> GetMemberByIdAsync(int? id);
        Task InsertAsync(Member company);
        Task UpdateAsync(Member company);
        Task DeleteAsync(int id);
        Task SaveAsync();

        Task<IEnumerable<Member>> GetSelectListsAsync();
        Task SortAsync(int? updatedAtOrder);
    }
}
