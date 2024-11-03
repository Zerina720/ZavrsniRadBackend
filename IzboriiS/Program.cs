using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Extensions;
using IzboriiS.Data;
using IzboriiS.Data.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using IzboriiS.IService;
using IzboriiS.Service;
using IzboriiS;
using Microsoft.Extensions.DependencyInjection;
using MongoDbGenericRepository;

var builder = WebApplication.CreateBuilder(args);

var mongoConnectionString = "mongodb://localhost:27017";
var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase("Izbori");
var mongoDbContext = new MongoDbContextt(mongoClient);

// Kreiraj kolekcije ako već ne postoje
var collections = mongoDatabase.ListCollectionNames().ToList();
if (!collections.Contains("Izboris"))
{
    mongoDatabase.CreateCollection("Izboris"); // Uveri se da koristiš ispravan naziv
}
if (!collections.Contains("NosilacListes"))
{
    mongoDatabase.CreateCollection("NosilacListes"); // Uveri se da koristiš ispravan naziv
}
if (!collections.Contains("OdbijeniZahtevis"))
{
    mongoDatabase.CreateCollection("OdbijeniZahtevis"); // Uveri se da koristiš ispravan naziv
}
if (!collections.Contains("TipIzboras"))
{
    mongoDatabase.CreateCollection("TipIzboras"); // Uveri se da koristiš ispravan naziv
}
if (!collections.Contains("Users"))
{
    mongoDatabase.CreateCollection("Users"); // Uveri se da koristiš ispravan naziv
}
if (!collections.Contains("IzbornaListas"))
{
    mongoDatabase.CreateCollection("IzbornaListas"); // Uveri se da koristiš ispravan naziv
}

if (!collections.Contains("roles"))
{
    mongoDatabase.CreateCollection("roles"); // Kreira kolekciju za role
}

// Registracija MongoDb konteksta
builder.Services.AddSingleton<IMongoClient>(mongoClient);
builder.Services.AddSingleton<IMongoDbContext>(mongoDbContext);
builder.Services.AddScoped<MongoDbContextt>(); // Izaberi Scoped ili Singleton, ne oboje

// Konfigurisanje identiteta sa MongoDB
builder.Services.AddIdentity<User, AppRole>()
    .AddMongoDbStores<MongoDbContextt>(mongoDbContext)
    .AddDefaultTokenProviders();

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapping));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IIzboriService,IzboriService>();
builder.Services.AddScoped<IStranka, StrankaService>();
builder.Services.AddScoped<IIzbornaListaService, IzbornaListaService>();
builder.Services.AddScoped<INosilacLService,NosilacLService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = "http://localhost:5001",
        ValidIssuer = "https://localhost:5001",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("78fUjkyzfLz56gTK"))
    };
});


var app = builder.Build();

// Swagger konfiguracija
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
