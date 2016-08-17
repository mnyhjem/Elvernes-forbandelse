using System.Web;
using Microsoft.AspNet.SignalR;

namespace ElvenCurse.Website.Hubs
{
    [Authorize]
    public class Chathub : Hub
    {
        public void Send(string name, string message)
        {
            
            Clients.All.addNewMessageToPage(HttpContext.Current.User.Identity.Name, message);
        }
    }
}