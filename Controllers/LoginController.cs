using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
namespace homecloud.Controllers;

public class LoginController : Controller {
    private MySqlConnection _connection;
    private IHttpContextAccessor _httpContextAccessor;
    private string? _email;
    private string? _password;

    public LoginController(MySqlConnection connection, IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
        _connection = connection;
    }

    public IActionResult Index() {
        if (Request.Method == "POST") {
            _email = Request.Form["email"];
            _password = Request.Form["password"];

            // email or password not filled -> login page
            if (_email == "" || _password == "")
                return RedirectToAction("Index", "Login", new { area = "" });

            // check if email password combination exists in db ? login : home
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
        var command = new MySqlCommand($"select idUsers from homecloud.Users where email = '{_email}' and password = '{_password}'", _connection);

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
            HttpContext.Session.SetString("email", _email);
            HttpContext.Session.SetString("password", _password);
            HttpContext.Session.SetString("LoggedIn", "1");
        }
    }
}
