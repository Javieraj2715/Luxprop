
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Luxprop.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public Task JoinConversation(string conversationId) =>
            Groups.AddToGroupAsync(Context.ConnectionId, conversationId);

        public Task SendMessage(string conversationId, string text) =>
            Clients.Group(conversationId).SendAsync("ReceiveMessage",
                new { from = Context.User?.Identity?.Name ?? "user", text, ts = DateTime.UtcNow });

        public Task NotifyAgentNewChat(string agentUserId, string conversationId, string clientName) =>
            Clients.User(agentUserId).SendAsync("NotifyNewChat",
                new { conversationId, clientName, ts = DateTime.UtcNow });
    }
}
