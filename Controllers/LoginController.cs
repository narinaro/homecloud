using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
namespace homecloud.Controllers;

public class LoginController : Controller
{
    private MySqlConnection _connection;

    public LoginController(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<IActionResult> Index()
    {
        await _connection.OpenAsync();
        await using var command = new MySqlCommand("insert into homecloud.Users (idUsers, username, email, password) values (38, 'lkjf', 'ldaöjfk', 'dklaöf')", _connection);
        await command.ExecuteScalarAsync();
        return await Task.Run(() => View());

        if (HttpContext.Request.Method == "POST") {

            LoginCreds user = new LoginCreds();
            user.username = Request.Form["user"];
            user.email = Request.Form["email"];
            user.password = Request.Form["password"];

            RedirectToAction("Index", "Login", new { area = "" });
        }

    }
}
