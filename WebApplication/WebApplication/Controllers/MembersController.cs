using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication.Common;
using WebApplication.Models;
using WebApplication.Repositories;
using Newtonsoft.Json;

namespace WebApplication.Controllers {
    public class MembersController : Controller {

        /*
         * View: Index
         * PartialView: Modal
         * Action: Gets, Create, Edit, Delete, Sort
         */

        private readonly UnitOfWork unitOfWork;
        private readonly WriteLog writeLog;

        public MembersController() {
            this.unitOfWork = new UnitOfWork();
            this.writeLog = new WriteLog();
            writeLog.Write();
        }

        // Index:
        // [Authorize(Roles = "SystemAdministrator,Administrator")]
        public async Task<ActionResult> Index() {
            return await Task.Run(() => {
                return View();
            });
        }

        // Modal: /Members/Modal
        // [Authorize(Roles = "SystemAdministrator,Administrator")]
        [HttpGet]
        [AjaxValidateAntiForgeryToken]
        [ValidateJsonXss]
        public async Task<ActionResult> Modal(int? id) {
            Member member = new Member();
            if (id != null) {
                member = await unitOfWork.MemberRepository.GetMemberByIdAsync(id);
            }

            return PartialView("Modal", member);
        }

        // GET: /Members/Gets
        public async Task<ActionResult> Gets() {
            var json = Json(await unitOfWork.MemberRepository.GetMembersAsync(), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        // POST: /Members/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Remarks,Order,IsDeleted,CreatedAt,UpdatedAt")] Member member) {
            try {
                if (ModelState.IsValid) {
                    Member r = await unitOfWork.MemberRepository.GetMemberByIdAsync(member.Id);
                    if (r != null) {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Message = "IDがすでに使用されています。" });
                    }

                    using (var dbContextTransaction = unitOfWork.ApplicationDbContext.Database.BeginTransaction()) {
                        await unitOfWork.MemberRepository.InsertAsync(member);
                        await unitOfWork.MemberRepository.SaveAsync();
                        await unitOfWork.MemberRepository.SortAsync(null);
                        dbContextTransaction.Commit();
                    }

                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { Message = "success" });
                }
            } catch (Exception e) {
                writeLog.Write(e.Message);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "データの新規登録に失敗しました。" });
        }

        // POST: /Members/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Remarks,Order,IsDeleted,CreatedAt,UpdatedAt,RowVersion")] Member member) {
            try {
                if (ModelState.IsValid) {
                    Member r = await unitOfWork.MemberRepository.GetMemberByIdAsync(member.Id);
                    int updatedAtOrder = (member.Order > r.Order ? 1 : 0);

                    if (r == null) {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Message = "IDが見つかりません。" });
                    }

                    using (var dbContextTransaction = unitOfWork.ApplicationDbContext.Database.BeginTransaction()) {
                        await unitOfWork.MemberRepository.UpdateAsync(member);
                        await unitOfWork.MemberRepository.SaveAsync();
                        await unitOfWork.MemberRepository.SortAsync(updatedAtOrder);
                        dbContextTransaction.Commit();
                    }

                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { Message = "success" });
                }
            } catch (Exception e) {
                writeLog.Write(e.Message);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "データの更新に失敗しました。画面を再度読み込みしてから実行して下さい。" });
        }

        // POST: /Members/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id) {
            try {
                if (ModelState.IsValid) {
                    Member member = await unitOfWork.MemberRepository.GetMemberByIdAsync(id);
                    if (member == null) {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(new { Message = "IDが見つかりません。" });
                    }
                    
                    using (var dbContextTransaction = unitOfWork.ApplicationDbContext.Database.BeginTransaction()) {
                        await unitOfWork.MemberRepository.DeleteAsync(member.Id);
                        await unitOfWork.MemberRepository.SaveAsync();
                        await unitOfWork.MemberRepository.SortAsync(null);
                        dbContextTransaction.Commit();
                    }
                
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { Message = "success" });
                }
            } catch (Exception e) {
                writeLog.Write(e.Message);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "データの削除に失敗しました。" });
        }

        // POST: /Members/Sort
        [HttpPost]
        [AjaxValidateAntiForgeryToken]
        [ValidateJsonXss]
        public async Task<ActionResult> Sort([Bind(Include = "Id,Name,Remarks,Order,IsDeleted,CreatedAt,UpdatedAt,RowVersion")] Member member) {
            try {
                if (ModelState.IsValid) {
                    Member r = await unitOfWork.MemberRepository.GetMemberByIdAsync(member.Id);
                    int updatedAtOrder = (member.Order > r.Order ? 1 : 0);

                    using (var dbContextTransaction = unitOfWork.ApplicationDbContext.Database.BeginTransaction()) {
                        await unitOfWork.MemberRepository.UpdateAsync(member);
                        await unitOfWork.MemberRepository.SaveAsync();
                        await unitOfWork.MemberRepository.SortAsync(updatedAtOrder);
                        dbContextTransaction.Commit();
                    }

                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(new { Message = "success" });
                }
            } catch (Exception e) {
                writeLog.Write(e.Message);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "データの順序変更に失敗しました。" });
        }
    }
}
