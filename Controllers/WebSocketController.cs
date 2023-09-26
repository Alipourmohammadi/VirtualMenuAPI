using System.Net;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;

// public class WebSocketController : ControllerBase
// {

//   [Route("/ws")]
//   public async Task Get()
//   {
//     if (HttpContext.WebSockets.IsWebSocketRequest)
//     {
//       using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

//     }
//     else
//     {
//       HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
//     }
//   }
// }