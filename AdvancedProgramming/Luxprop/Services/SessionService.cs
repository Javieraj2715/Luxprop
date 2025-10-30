using Microsoft.JSInterop;

namespace Luxprop.Services
{
    public class SessionService
    {
        private readonly IJSRuntime _js;

        // ✅ Propiedades en memoria
        public int CurrentUserId { get; private set; }
        public string? CurrentUserName { get; private set; }
        public string? CurrentUserRole { get; private set; }

        public SessionService(IJSRuntime js)
        {
            _js = js;
        }

        // ✅ Guarda en sessionStorage y también en memoria
        public async Task SetUserAsync(int userId, string userName, string roleName)
        {
            CurrentUserId = userId;
            CurrentUserName = userName;
            CurrentUserRole = roleName;

            await _js.InvokeVoidAsync("sessionStorage.setItem", "UserId", userId.ToString());
            await _js.InvokeVoidAsync("sessionStorage.setItem", "UserName", userName);
            await _js.InvokeVoidAsync("sessionStorage.setItem", "UserRole", roleName);
        }

        // ✅ Carga desde sessionStorage a las propiedades locales
        public async Task LoadUserAsync()
        {
            var idString = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserId");
            if (int.TryParse(idString, out var id))
                CurrentUserId = id;

            CurrentUserName = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserName");
            CurrentUserRole = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserRole");
        }

        public async Task<string?> GetUserNameAsync()
        {
            if (string.IsNullOrEmpty(CurrentUserName))
                CurrentUserName = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserName");

            return CurrentUserName;
        }

        public async Task<string?> GetUserRoleAsync()
        {
            if (string.IsNullOrEmpty(CurrentUserRole))
                CurrentUserRole = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserRole");

            return CurrentUserRole;
        }

        public async Task<int> GetUserIdAsync()
        {
            if (CurrentUserId == 0)
            {
                var idString = await _js.InvokeAsync<string>("sessionStorage.getItem", "UserId");
                if (int.TryParse(idString, out var id))
                    CurrentUserId = id;
            }

            return CurrentUserId;
        }

        public async Task LogoutAsync()
        {
            CurrentUserId = 0;
            CurrentUserName = null;
            CurrentUserRole = null;
            await _js.InvokeVoidAsync("sessionStorage.clear");
        }

        // ✅ Ayuda rápida para verificar si hay sesión
        public bool IsAuthenticated => CurrentUserId > 0;
    }
}
