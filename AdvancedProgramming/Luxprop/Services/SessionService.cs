using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Luxprop.Services
{
    public class SessionService
    {
        private readonly IJSRuntime _js;

        public SessionService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetUserAsync(int userId, string userName)
        {
            await _js.InvokeVoidAsync("sessionStorage.setItem", "UserId", userId.ToString());
            await _js.InvokeVoidAsync("sessionStorage.setItem", "UserName", userName);
        }

        public async Task<string?> GetUserNameAsync()
        {
            return await _js.InvokeAsync<string>("sessionStorage.getItem", "UserName");
        }

        public async Task<int?> GetUserIdAsync()
        {
            var id = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserId");
            return int.TryParse(id, out var userId) ? userId : null;
        }

        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("sessionStorage.clear");
        }
    }
}
