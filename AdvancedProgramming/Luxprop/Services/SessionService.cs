using Microsoft.JSInterop;

namespace Luxprop.Services
{
    public class SessionService
    {
        private readonly IJSRuntime _js;

        public SessionService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetUserAsync(int userId, string userName, string roleName)
        {
            await _js.InvokeVoidAsync("sessionStorage.setItem", "UserId", userId.ToString());
            await _js.InvokeVoidAsync("sessionStorage.setItem", "UserName", userName);
            await _js.InvokeVoidAsync("sessionStorage.setItem", "UserRole", roleName);
        }

        public async Task<string?> GetUserNameAsync()
        {
            return await _js.InvokeAsync<string>("sessionStorage.getItem", "UserName");
        }

        public async Task<string?> GetUserRoleAsync()
        {
            return await _js.InvokeAsync<string>("sessionStorage.getItem", "UserRole");
        }

        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("sessionStorage.clear");
        }
    }
}
