using Microsoft.AspNetCore.SignalR;

namespace MexxarTaskTracker.Api
{
    public class NotificationHub : Hub
    {
        public Task SendTaskReminder(string userId, string msg)
        {
            return Clients.User(userId).SendAsync("TaskReminder", msg);
        }
    }
}
