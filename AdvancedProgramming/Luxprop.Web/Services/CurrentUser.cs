using System.Security.Claims;

public interface ICurrentUser
{
    string? UserId { get; }          // si usas Identity
    int? UsuarioId { get; }          // tu id interno (tabla Usuario)
    bool IsInRole(string role);
}

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;
    public CurrentUser(IHttpContextAccessor http) => _http = http;

    ClaimsPrincipal? P => _http.HttpContext?.User;

    public string? UserId => P?.FindFirstValue(ClaimTypes.NameIdentifier);
    public int? UsuarioId
    {
        get
        {
            // Preferimos un claim "UsuarioId"; si no existe, intenta NameIdentifier parseado
            var v = P?.FindFirst("UsuarioId")?.Value ?? P?.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(v, out var id) ? id : null;
        }
    }

    public bool IsInRole(string role) => P?.IsInRole(role) == true;
}
