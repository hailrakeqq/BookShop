using System.Text;
using BookShop.API;
using BookShop.API.Authorization;
using BookShop.API.DIContainer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("Jwt").GetSection("Issuer").Value,
            ValidAudience = builder.Configuration.GetSection("Jwt").GetSection("Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt").GetSection("Key").Value))
        });
builder.Services.AddHttpContextAccessor();

builder.Services.AddCurrentUser();
builder.Services.AddTokenService();
builder.Services.AddLoginResponce();

builder.Services.AddUserService();
builder.Services.AddSellerService();
builder.Services.AddBookService();
builder.Services.AddFileService();
builder.Services.AddCommentService();

builder.Services.AddControllers();
builder.Services.AddTransient<ApplicationDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "V1",
        Title = "BookShop API",
        Description = "API for BookShop app"
    });
});

//create db collection block
var client = new MongoClient(builder.Configuration.GetSection("ConnectionString").Value);
var database = client.GetDatabase("BookShop");
var collectionNamesInDb = database.ListCollectionNames().ToList();

string[] collectionNames = { "Books", "Users", "Comments", "RefreshTokens",
                             "Sellerstats", "Users", "UsersLibrary", "UsersWishlist"};

foreach (var collection in collectionNames)
    if (!collectionNames.Contains(collection))
        database.CreateCollection($"{collection}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookShop API");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();