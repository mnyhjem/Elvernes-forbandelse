using System.Web.Mvc;

namespace ElvenCurse.Website.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AdminWorldconfiguration_default",
                "Admin/Worldconfiguration/{action}/{id}",
                new { action = "Index", controller= "Worldconfiguration", id = UrlParameter.Optional }
            );
        }
    }
}