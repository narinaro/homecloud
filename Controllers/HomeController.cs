using Microsoft.AspNetCore.Mvc;
namespace homecloud.Controllers;

public class HomeController : Controller
{
    private string? _email;
    private string? _cloudStorage;

    public IActionResult Index() {
        _email = HttpContext.Session.GetString("email");
        _cloudStorage = $"/home/david/cloudStorage/{_email}";

        // Check if user is logged in
        if (!CheckSession())
            return RedirectToAction("Index", "Login", new { area = "" });
            
        // Check if request is post -> Action
        if (Request.Method == "POST")
            HandleActions();
        
        // Create dir for user if not already exists
        if (!Directory.Exists(_cloudStorage))
            Directory.CreateDirectory(_cloudStorage);

        // Output files & dirs
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

    // handle the different actions
    private IActionResult HandleActions() {
        if (Request.Form["createDir"] == "1")
            Directory.CreateDirectory(_cloudStorage + "/" + Request.Form["dirName"]); // dirName is checked for illegal chars in "CreateDirectory"
        else if (Request.Form["deleteDir"] == "1")
            Directory.Delete(_cloudStorage + "/" + Request.Form["dirName"], true);
        
        return RedirectToAction("Index", "Home", new { area = "" });
    }

    // Check if user is logged in
    private bool CheckSession() {
        if (HttpContext.Session.GetString("LoggedIn") != "1")
            return false;
        return true;
    }

}
