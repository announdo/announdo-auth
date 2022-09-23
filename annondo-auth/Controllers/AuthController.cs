using FirebaseAdmin.Auth;
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
    public async Task<IActionResult> Login([FromBody] string token)
    {
        var tickets = _db.Collection("tickets")!;
        var snapshot = await tickets.WhereEqualTo("phrase", token)
            .WhereEqualTo("type", "auth")
            .Limit(1)
            .GetSnapshotAsync();
        if (!snapshot.Any())
        {
            return Unauthorized();
        }

        var ticket = snapshot[0];
        var expiry = ticket.GetValue<Timestamp>("expiry").ToProto();
        var claims = new Dictionary<string, object>()
        {
            {
                "expiry", expiry.Seconds
            },
        };
        string authToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync($"tok-{Guid.NewGuid()}", claims);

        return Ok(authToken);
    }
}