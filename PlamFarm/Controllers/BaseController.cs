using Microsoft.AspNetCore.Mvc;

namespace PalmFarm.Controllers
{
    public class BaseController : Controller
    {
        public bool IsUserSessionValid()
        {
            if (HttpContext.Session.GetString("userName") == null)
                return false;
            return true;
        }

        
    }
}
