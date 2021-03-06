﻿using System.Web;
using System.Web.Optimization;
using ElvenCurse.Website.Concrete;

namespace ElvenCurse.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/frameworks").Include(
                "~/Scripts/jquery.signalR-2.2.1.min.js",
                //"~/Sitescriptes/phaser2.5.0/phaser.js",
                //"~/Sitescriptes/phaser2.5.0/phaser-tiled.js"
                //"~/Sitescriptes/phaser2.6.1/phaser.js"
                "~/Sitescriptes/phaser2.7.3/phaser.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/elvencurse").Include(
                "~/Sitescriptes/ElvenCurse/Game.js",
                "~/Sitescriptes/ElvenCurse/Booter.js",
                "~/Sitescriptes/ElvenCurse/StatePreloader.js",
                "~/Sitescriptes/ElvenCurse/StateGameplay.js",
                "~/Sitescriptes/ElvenCurse/Player.js",
                "~/Sitescriptes/ElvenCurse/Nameplate.js",
                "~/Sitescriptes/ElvenCurse/OtherPlayer.js",
                "~/Sitescriptes/ElvenCurse/Interactiveobjects/InteractiveObject.js",
                "~/Sitescriptes/ElvenCurse/EntityPortraitplate.js",
                "~/Sitescriptes/ElvenCurse/Actionbar.js",
                "~/Sitescriptes/ElvenCurse/Worldsectionnameplate.js",
                "~/Sitescriptes/ElvenCurse/Npcs/NpcBase.js",
                "~/Sitescriptes/ElvenCurse/Npcs/Wolf.js",
                "~/Sitescriptes/ElvenCurse/Npcs/Elfhunter.js",
                "~/Sitescriptes/ElvenCurse/Npcs/Bunny.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            var styles = new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/bootwatch-theme.css");

            styles.Orderer = new AsDefinedBundleOrderer();
            bundles.Add(styles);

            
        }
    }
}
