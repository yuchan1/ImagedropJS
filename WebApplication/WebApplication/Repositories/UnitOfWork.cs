using System;
using WebApplication.Models;

namespace WebApplication.Repositories {
    public class UnitOfWork : IDisposable {
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        public ApplicationDbContext ApplicationDbContext {
            get {
                return this.context;
            }
        }

        private IMemberRepository memberRepository;

        public IMemberRepository MemberRepository {
            get {
                return this.memberRepository ?? (this.memberRepository = new MemberRepository(context));
            }
        }

        public void Save() {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
