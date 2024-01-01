using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Net.Http.Headers;
namespace homecloud.Controllers;

public class LoginController : Controller {
    private MySqlConnection _connection;
    private IHttpContextAccessor _httpContextAccessor;

    public LoginController(MySqlConnection connection, IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
        _connection = connection;
    }

    public IActionResult Index() {
        if (Request.Method == "POST") {
            if (checkCreds())
                return RedirectToAction("Index", "Home", new { area = "" });
            return RedirectToAction("Index", "Login", new { area = "" });
        }

        if (HttpContext.Session.GetString("LoggedIn") == "1")
            return RedirectToAction("Index", "Home", new { area = "" });
        return View();
    }
    
    public bool checkCreds() {
        _connection.Open();
        var command = new MySqlCommand($"select idUsers from homecloud.Users where email = '{Request.Form["email"]}' and password = '{Request.Form["password"]}'", _connection);

        MySqlDataReader rdr = command.ExecuteReader();

        if(rdr.Read()) {
            SetSession();
            Console.WriteLine(rdr[0]);
            rdr.Close();
            return true;
        } else {
            rdr.Close();
            return false;
        }
    }

    public void SetSession() {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("email"))) {
            HttpContext.Session.SetString("email", Request.Form["email"]);
            HttpContext.Session.SetString("password", Request.Form["password"]);
            HttpContext.Session.SetString("LoggedIn", "1");
        }
    }
}
