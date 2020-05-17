using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication.Common;
using WebApplication.Repositories;

namespace WebApplication {
    public class FileUploadsController : Controller {
        private readonly UnitOfWork unitOfWork;
        private readonly WriteLog writeLog;

        public FileUploadsController() {
            this.unitOfWork = new UnitOfWork();
            this.writeLog = new WriteLog();
            writeLog.Write();
        }

        // GET: /FileUploads/
        public async Task<ActionResult> Index() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file, long testId) {
            try {
                if (file == null) {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { Message = "ファイルが見つからないためアップロードは実行できません。" });
                }

                string contentType = file.ContentType;
                string fileName = Path.GetFileName(file.FileName.ToLower());
                string fileNameExtension = Path.GetExtension(fileName);

                // MIMEタイプ、拡張子チェック
                if (!(contentType == "image/jpeg" && fileNameExtension == ".jpg")) {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { Message = "jpg以外のファイルはアップロードは実行できません。" });
                }

                // ファイル容量チェック(1MBまで)
                if (file.ContentLength > 10000000) {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { Message = "1MBより大きいサイズのファイルはアップロードは実行できません。" });
                }

                // サーバーのディレクトリを準備
                // string directory = Path.Combine(Server.MapPath(@"\"), WebConfigurationManager.AppSettings["ImageDirectory"], testId.ToString(), "FileUploads");
                string directory = Path.Combine(Server.MapPath(@"\"), "Content\\images", testId.ToString(), "FileUploads");
                if (!Directory.Exists(directory)) {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(directory);
                }

                // ファイル名の変更、保存パスの取得
                DateTime now = DateTime.Now;
                fileName = now.ToString("yyyyMMdd-HHmmssfff") + fileNameExtension;
                fileName = fileName.ToLower();
                string filePath = Path.Combine(directory, fileName.ToLower());

                // ファイル名の重複チェック　※このチェックはほぼ実行されない
                if (System.IO.File.Exists(filePath)) {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { Message = "ファイルのアップロードに失敗しました。再度実行して下さい。" });
                }

                using (Bitmap bitmap = (Bitmap)Bitmap.FromStream(file.InputStream)) {
                    // 画像ファイルのExif情報を削除
                    foreach (var item in bitmap.PropertyItems) {
                        bitmap.RemovePropertyItem(item.Id);
                    }

                    // 画像がW1024×H768より大きい時はW1024×H768以内へリサイズ(Scaleの小さい方に合わせる)、変更がない場合もBitmapを経由させる
                    if (bitmap.Width > 1024 || bitmap.Height > 768) {
                        float widthScale = (float)(1024.0 / bitmap.Width);
                        float heightScale = (float)(768.0 / bitmap.Height);
                        float scale = (widthScale <= heightScale ? widthScale : heightScale);

                        int width = (int)(bitmap.Width * scale);
                        int height = (int)(bitmap.Height * scale);

                        // 小数の補正
                        if (widthScale <= heightScale) {
                            width = 1024;
                        } else {
                            height = 768;
                        }

                        Bitmap resizeBitmap = new Bitmap(bitmap, width, height);
                        resizeBitmap.Save(filePath);
                    } else {
                        Bitmap resizeBitmap = new Bitmap(bitmap, bitmap.Width, bitmap.Height);
                        resizeBitmap.Save(filePath);
                    }
                }

                // 画像パスを "/" でセット
                filePath = "/Content/images/" + testId.ToString() + "/FileUploads/" + fileName;

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { filePath = filePath });

            } catch (Exception e) {
                writeLog.Write(e.Message);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "ファイルのアップロードが実行できません。" });
        }
    }
}