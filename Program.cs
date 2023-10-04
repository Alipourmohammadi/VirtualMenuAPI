
using System.Net.WebSockets;
using VirtualMenuAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VirtualMenuAPI.Models;
using VirtualMenuAPI.Services.CustomerService;
using VirtualMenuAPI.Authentication;

var builder = WebApplication.CreateBuilder(args);
// builder.WebHost.UseUrls("http://192.168.1.161:5058");

builder.Services.AddDbContext<DataContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
builder.Services.AddSingleton<WebSocketHandler>();


builder.Services.AddIdentity<Customer, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
var tokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWT:Secret").Value!)),

    ValidateIssuer = true,
    ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,

    ValidateAudience = false,

    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWT:Secret").Value!)),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,

        ValidateAudience = false,

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    }; ;
});
// builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

//Services
builder.Services.AddScoped<ICustomerService>();
builder.Services.AddScoped<ICustomerAuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add this in the ConfigureServices method of Startup.cs
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

// Add this in the Configure method of Startup.cs, before app.UseEndpoints

var app = builder.Build();

app.UseWebSockets();
app.Use(async (context, next) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var handler = app.Services.GetService<WebSocketHandler>() ?? throw new Exception();
        // var handler = new WebSocketHandler();

        // Now, you can use the shared instance of WebSocketHandler
        handler.InitializeWebSocket(webSocket);
        await handler.ReceiveDataAsync();
    }
    else
    {
        // throw new Exception("only websocket connections accepted");
        await next();
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors();

await app.RunAsync();
// app.Run();
