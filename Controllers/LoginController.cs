using Microsoft.AspNetCore.Mvc;
namespace homecloud.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        if (HttpContext.Request.Method == "POST") {

            LoginCreds user = new LoginCreds();
            user.username = Request.Form["user"];
            user.email = Request.Form["email"];
            user.password = Request.Form["password"];

            return RedirectToAction("Index", "Login", new { area = "" });
        }

        return View();
    }
}
