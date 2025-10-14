using System.Security.Claims;

namespace Luxprop.Global.Identity
{
    public interface ICurrentUser
    {
        string? UserId { get; }
        bool IsInRole(string role);
        ClaimsPrincipal? Principal { get; }
    }
}
