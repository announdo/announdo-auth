using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace annondo_auth.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    public AuthController(FirestoreDb db)
    {
        _db = db;
    }

    public FirestoreDb _db { get; }
    [HttpPost("login")]
    public IActionResult Login(string token)
    {
        
    }
}