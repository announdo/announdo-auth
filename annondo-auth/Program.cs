using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

if (!File.Exists("service_account.json"))
{
    Console.WriteLine($"Please download the service_account.json file from GCP and store it in the current folder! Expected directory {Environment.CurrentDirectory}");
    return;
}

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.Combine(Environment.CurrentDirectory, "service_account.json"), EnvironmentVariableTarget.Process);

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.GetApplicationDefault(),
});

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration["gcp-id"] is null or "")
{
    Console.WriteLine($"Please specify a \"gcp-id\" in appsettings.json.");
    return;
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(await FirestoreDb.CreateAsync(builder.Configuration["gcp-id"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
