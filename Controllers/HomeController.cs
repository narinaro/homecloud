using Microsoft.AspNetCore.Mvc;
namespace homecloud.Controllers;

public class HomeController : Controller
{
    private string? _email;
    private string? _cloudStorage;

    public IActionResult Index()
    {
        _email = HttpContext.Session.GetString("email");
        _cloudStorage = $"/home/david/cloudStorage/{_email}";

        if (HttpContext.Session.GetString("LoggedIn") != "1")
            return RedirectToAction("Index", "Login", new { area = "" });

        if (Request.Method == "POST")
            HandleActions();

        if (!Directory.Exists(_cloudStorage))
            Directory.CreateDirectory(_cloudStorage);

        List<string>? directories = Directory.GetDirectories(_cloudStorage).ToList();
        List<string>? files = Directory.GetFiles(_cloudStorage).ToList();

        for (int i = 0; i < directories.Count(); i++)
            directories[i] = directories[i].Remove(0, directories[i].LastIndexOf("/") + 1);

        for (int i = 0; i < files.Count(); i++)
            files[i] = files[i].Remove(0, files[i].LastIndexOf("/") + 1);

        ViewData["folderDirectories"] = directories;
        ViewData["folderFiles"] = files;

        return View();
    }

    private IActionResult HandleActions() {
        if (Request.Form["createDir"] == "1")
            Directory.CreateDirectory(_cloudStorage + "/" + Request.Form["dirName"]);
        else if (Request.Form["deleteDir"] == "1")
            Directory.Delete(_cloudStorage + "/" + Request.Form["dirName"], true);
        
        return RedirectToAction("Index", "Home", new { area = "" });
    }

}
