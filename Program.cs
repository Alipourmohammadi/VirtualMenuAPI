
using System.Net.WebSockets;
using VirtualMenuAPI.Data;
using Microsoft.EntityFrameworkCore;
// using VirtualMenuAPI.Handler;

var builder = WebApplication.CreateBuilder(args);
// builder.WebHost.UseUrls("http://192.168.1.161:5058");
// Add services to the container.
// builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddDbContext<DataContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
builder.Services.AddSingleton<WebSocketHandler>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

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
