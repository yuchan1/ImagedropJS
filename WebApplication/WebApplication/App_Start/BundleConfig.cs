using System.Web;
using System.Web.Optimization;

namespace WebApplication {
    public class BundleConfig {
        // バンドルの詳細については、http://go.microsoft.com/fwlink/?LinkId=301862  を参照してください
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Ajax通信用に追加
            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax*"));

            // javascript追加 --- start ---
            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                        "~/Scripts/chartjs/chart.js"));

            bundles.Add(new ScriptBundle("~/bundles/encoding").Include(
                        "~/Scripts/encoding/encoding.js"));

            bundles.Add(new ScriptBundle("~/bundles/fetch").Include(
                        "~/Scripts/fetch/fetch2.0.4.js"));

            bundles.Add(new ScriptBundle("~/bundles/promise").Include(
                        "~/Scripts/promise/promise-7.0.4.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                        "~/Scripts/select2/select2.js"));

            bundles.Add(new ScriptBundle("~/bundles/tabulator").Include(
                        "~/Scripts/tabulator/tabulator.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker").Include(
                        "~/Scripts/bootstrap-datepicker/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/imagedrop").Include(
                        "~/Scripts/imagedrop/imagedrop.js"));

            // bundles.Add(new ScriptBundle("~/bundles/flatpickr").Include(
            //            "~/Scripts/flatpickr/flatpickr-ja.js"));

            // bundles.Add(new ScriptBundle("~/bundles/jquery-datepicker").Include(
            //            "~/Scripts/jquery-datepicker/jquery.ui.datepicker-ja.js"));

            // 開発と学習には、Modernizr の開発バージョンを使用します。次に、実稼働の準備が
            // できたら、http://modernizr.com にあるビルド ツールを使用して、必要なテストのみを選択します。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            
            // CSS追加 ---  start ---
            bundles.Add(new StyleBundle("~/Content/bootstrap-datepicker").Include(
                        "~/Content/bootstrap-datepicker/bootstrap-datepicker3.css"));

            bundles.Add(new StyleBundle("~/Content/select2").Include(
                        "~/Content/select2/select2.css"));

            bundles.Add(new StyleBundle("~/Content/tabulator").Include(
                        "~/Content/tabulator/tabulator.css"));

            bundles.Add(new StyleBundle("~/Content/imagedrop").Include(
                        "~/Content/imagedrop/imagedrop.css"));

            bundles.Add(new StyleBundle("~/Content/Login").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/Login/main.css"));

            // 画面固有javascript ---  start ---
            bundles.Add(new ScriptBundle("~/bundles/views/Members/index").Include(
                        "~/Scripts/views/Members/index.js"));

            // bundles.Add(new ScriptBundle("~/bundles/views/FileUploads/index").Include(
            //             "~/Scripts/views/FileUploads/index.js"));

        }
    }
}
