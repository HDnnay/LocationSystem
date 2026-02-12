using Microsoft.AspNetCore.SignalR;

namespace LocationSystem.Api.Hubs
{
    public class MenuHub : Hub
    {
        public const string HubUrl = "/hub/menu";
        
        /// <summary>
        /// 加入用户组
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
        }
        
        /// <summary>
        /// 离开用户组
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
        }
    }
}