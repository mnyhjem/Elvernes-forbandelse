using System.Web.Mvc;

namespace ElvenCurse.Website.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }
    }
}