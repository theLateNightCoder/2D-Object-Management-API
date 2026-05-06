using individueelProject.Data;
using individueelProject.Repository.Environment2DRepo;
using individueelProject.Repository.Object2DRepo;
using individueelProject.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConnection = builder.Configuration["SqlConnectionString"];
builder.Services.AddSingleton<DapperDbContext>(provider => new DapperDbContext(sqlConnection));

builder.Services.AddTransient<IEnivronmentRepository, SqlEnvironment2DRepositroy>( );
builder.Services.AddTransient<IObjectRepository, SqlObject2DRepository>();

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddAuthorization();
 
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options => { 
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 10;
    

}).AddDapperStores(options => { options.ConnectionString = sqlConnection; });


builder.Services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme)
                            .Configure(options => { options.BearerTokenExpiration = TimeSpan.FromMinutes(60); });

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();


var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);


var app = builder.Build();


app.MapGet("/", () => $"The API is up . Connection string found: {(sqlConnectionStringFound ? " " : " ")}");


app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
    [FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.Unauthorized();
}).RequireAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.UseAuthorization();


app.MapControllers();

app.Run();
