using System.Web;
using System.Web.Optimization;

namespace Gaia_App
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/login-jquery-js").Include(
                            "~/Scripts/jquery-2.1.4/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                                                                "~/Scripts/main.js",
                                                                "~/Scripts/gaia_js/fnGaia.js",
                                                                "~/Scripts/gaia_js/menu.js"));

            bundles.Add(new ScriptBundle("~/bundles/metro").Include("~/Scripts/metro.js"));

            bundles.Add(new ScriptBundle("~/bundles/login-metro-js").Include("~/Scripts/metro.js"));

            bundles.Add(new ScriptBundle("~/bundles/template").Include(
                                        "~/Scripts/metro.js"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                                                                        "~/Scripts/jquery.inputmask.*"
                                                                        //"~/Scripts/jquery.inputmask/inputmask.*"
                                                                        //, "~/Scripts/jquery.inputmask/jquery.inputmask.min.js"//,"~/Scripts/jquery.inputmask/phone-codes/phone.min.js"
                                                                        ));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include("~/Scripts/select2.js",
                                                                     "~/Scripts/es.js"));

            bundles.Add(new ScriptBundle("~/bundles/tooltip").Include("~/Scripts/tipped.js"));

            bundles.Add(new ScriptBundle("~/bundles/alertify").Include("~/Scripts/alertify.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/Scripts/jquery.base64.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/DataTables/dataTables.select.min.js",
                        "~/Scripts/DataTables/dataTables.buttons.min.js",
                        "~/Scripts/DataTables/buttons.flash.min.js",
                        "~/Scripts/DataTables/jszip.min.js",
                        "~/Scripts/DataTables/pdfmake.min.js",
                        "~/Scripts/DataTables/vfs_fonts.js",
                        "~/Scripts/DataTables/buttons.html5.min.js",
                        "~/Scripts/DataTables/buttons.print.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include("~/Scripts/ckeditor/ckeditor.js"));

            bundles.Add(new StyleBundle("~/Content/css/app").Include("~/Content/app/Layout.css"));

            bundles.Add(new StyleBundle("~/Content/css/login").Include("~/Content/app/signin.css"));

            bundles.Add(new StyleBundle("~/Content/css/template").Include(
                                        "~/Content/metro/metro.css",
                                        "~/Content/metro/metro-icons.css",
                                        "~/Content/metro/metro-responsive.css",
                                        "~/Content/metro/metro-schemes.css",
                                        "~/Content/metro/metro-colors.css"));

            bundles.Add(new StyleBundle("~/Content/css/login-metro-css").Include(
                                        "~/Content/metro/metro.css",
                                        "~/Content/metro/metro-icons.css",
                                        "~/Content/metro/metro-responsive.css",
                                        "~/Content/metro/metro-schemes.css",
                                        "~/Content/metro/metro-colors.css"));        

            bundles.Add(new StyleBundle("~/Content/css/select2").Include("~/Content/tools/select2.css"));

            bundles.Add(new StyleBundle("~/Content/css/tooltip").Include("~/Content/tools/tipped.css"));

            bundles.Add(new StyleBundle("~/Content/css/alertify").Include("~/Content/tools/alertify.css"));

            bundles.Add(new StyleBundle("~/Content/css/datatables").Include(
            "~/Content/DataTables/css/jquery.dataTables.css",
            "~/Content/DataTables/css/jquery.dataTables.min.css",
            "~/Content/DataTables/css/select.dataTables.min.css",
            "~/Content/DataTables/css/buttons.dataTables.min.css"));
        }
    }
}
