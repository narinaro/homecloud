using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
namespace homecloud.Controllers;

public class LoginController : Controller {
    private MySqlConnection _connection;

    public LoginController(MySqlConnection connection) {
        _connection = connection;
    }

    public IActionResult Index() {
        if (Request.Method == "POST") {
            string? email = Request.Form["email"];
            string? password = Request.Form["password"];

            if (checkCreds())
                return RedirectToAction("Index", "Home", new { area = "" });

            return RedirectToAction("Index", "Login", new { area = "" });
        }
        return View();
    }
    
    public bool checkCreds() {
        _connection.Open();
        var command = new MySqlCommand($"select idUsers from homecloud.Users where email = '{Request.Form["email"]}' and password = '{Request.Form["password"]}'", _connection);

        MySqlDataReader rdr = command.ExecuteReader();

        if(rdr.Read()) {
            Console.WriteLine(rdr[0]);
            rdr.Close();
            return true;
        } else {
            rdr.Close();
            return false;
        }


    }
}
