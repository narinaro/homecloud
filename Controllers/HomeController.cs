using Microsoft.AspNetCore.Mvc;
namespace homecloud.Controllers;

public class HomeController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HomeController(IHttpContextAccessor httpContextAccessor) {

        _httpContextAccessor = httpContextAccessor;
    }
    public IActionResult Index()
    {
        var LoggedIn = new CookieOptions();
        LoggedIn.Expires = DateTime.Now.AddDays(1);
        LoggedIn.IsEssential = true;

        _httpContextAccessor.HttpContext.Response.Cookies.Append("LoggedIn", "1", LoggedIn);

        
        string cloudStorage = @"/home/david/cloudStorage";

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
