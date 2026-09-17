using Microsoft.AspNetCore.Mvc;

namespace CourseLab.PL.Controllers
{
    public class StateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetTheme(string theme)
        {
            if(theme != "White" && theme != "Black")
            {
                theme = "White";
            }

            CookieOptions op = new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            };

            HttpContext.Response.Cookies.Append("Theme", theme, op);
            return RedirectToAction("theme");
        }

        public IActionResult theme()
        {
            string theme = Request.Cookies["Theme"] ?? "White";
            return View(model: theme);
        }
    }
}
