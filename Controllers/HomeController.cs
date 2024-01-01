using Microsoft.AspNetCore.Mvc;
namespace homecloud.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("LoggedIn") != "1")
            return RedirectToAction("Index", "Login", new { area = "" });

        var email = HttpContext.Session.GetString("email");

        string cloudStorage = $"/home/david/{email}";

        if (!Directory.Exists(cloudStorage))
            Directory.CreateDirectory(cloudStorage);

        List<string> directories = Directory.GetDirectories(cloudStorage).ToList();
        List<string> files = Directory.GetFiles(cloudStorage).ToList();

        for (int i = 0; i < directories.Count(); i++)
            directories[i] = directories[i].Remove(0, directories[i].LastIndexOf("/") + 1);

        for (int i = 0; i < files.Count(); i++)
            files[i] = files[i].Remove(0, files[i].LastIndexOf("/") + 1);

        ViewData["folderDirectories"] = directories;
        ViewData["folderFiles"] = files;

        return View();
    }
}
