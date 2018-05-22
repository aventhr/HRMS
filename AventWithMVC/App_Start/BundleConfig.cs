using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace AventWithMVC.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/sciptsJS").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/assets/css/style.css",
                        "~/assets/bootstrap/css/bootstrap.min.css",
                        "~/assets/css/media-queries.css",
                        "~/assets/css/form-elements.css",
                        "~/assets/bootstrap/css/bootstrap.min.css"));
            BundleTable.EnableOptimizations = true;
        }
    }
}