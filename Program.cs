
//using System.Net.WebSockets;
using VirtualMenuAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VirtualMenuAPI.Models;
using VirtualMenuAPI.Services.CustomerService;
using VirtualMenuAPI.Data.Models.Users;
using VirtualMenuAPI.Services.AuthServices;
using VirtualMenuAPI.Services.BaristaServices;
using VirtualMenuAPI.Services.ManagerServices;
using VirtualMenuAPI.Services;
using VirtualMenuAPI.SSE.MiddleWare;
using VirtualMenuAPI.SSEMiddleware.CustomerSSE;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using VirtualMenuAPI.SSEMiddleware.BaristaSSE;
using VirtualMenuAPI.Data.Helpers;
using VirtualMenuAPI.Data.Inputs;
using VirtualMenuAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });


builder.Services.AddIdentityCore<Account>()
  .AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<DataContext>()
  .AddDefaultTokenProviders();
builder.Services.AddIdentityCore<Customer>()
  .AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<DataContext>()
  .AddDefaultTokenProviders();

//builder.Services.AddSingleton<WebSocketHandler>();
//Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBaristaService, BarsitaService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddSingleton<IOrderQueueService, OrderQueueService>();
builder.Services.AddSingleton<ICustomerSseHolder, CustomerHolder>();
builder.Services.AddSingleton<IBaristaSseHolder, BaristaHolder>();

//builder.Services.AddScoped<ICustomerAuthService>();

//builder.Services.AddIdentity<Customer, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
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
  options.TokenValidationParameters = tokenValidationParameters;
});
// builder.Services.AddSingleton<IOrderRepository, OrderRepository>();


builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

  // Add Bearer token authentication
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Description = "JWT Authorization header using the Bearer scheme.",
    Type = SecuritySchemeType.Http,
    Scheme = "bearer"
  });

  c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
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

//app.UseWebSockets();
//app.Use(async (context, next) =>
//{
//  if (context.WebSockets.IsWebSocketRequest)
//  {
//    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
//    var handler = app.Services.GetService<WebSocketHandler>() ?? throw new Exception();
//    // var handler = new WebSocketHandler();

//    // Now, you can use the shared instance of WebSocketHandler
//    handler.InitializeWebSocket(webSocket);
//    await handler.ReceiveDataAsync();
//  }
//  else
//  {
//    // throw new Exception("only websocket connections accepted");
//    await next();
//  }
//});


//if (app.Environment.IsDevelopment())
//{
  app.UseSwagger();
  app.UseSwaggerUI();
//}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapCustomerSseHolder("/sse/customer");
app.MapBaristaSseHolder("/sse/barista");
//app.UseHttpsRedirection();


app.MapControllers();
AppDbInitializer.SeedRolesToDb(app).Wait();
//await app.RunAsync();
app.Run();
