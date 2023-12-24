using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
namespace homecloud.Controllers;

public class LoginController : Controller {
    private MySqlConnection _connection;

    public LoginController(MySqlConnection connection) {
        _connection = connection;
    }

    public async Task<IActionResult> Index() {

        if (HttpContext.Request.Method == "POST") {
            var result = AsyncContext.Run(checkCreds);

            

            return await Task.Run(() => RedirectToAction("Index", "Login", new { area = "" }));
        }

        return  await Task.Run(() => View());

    }
    
    public async Task<bool> checkCreds() {
        await _connection.OpenAsync();
        await using var command = new MySqlCommand("select idUsers from homecloud.Users where username = '{Request.Form['user']}' and email = '{Request.Form['email']}' and password = '{Request.Form['password']}'", _connection);

        MySqlDataReader rdr = command.ExecuteReader();

        if(rdr.Read()) {
            Console.WriteLine(rdr[0]);
            rdr.Close();
            return await Task.Run(() => true);
        } else {
            rdr.Close();
            return await Task.Run(() => false);
        }


    }
}
