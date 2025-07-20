using System.Security.Claims;
using ByteBites.Application.Common.Interfaces;

namespace ByteBites.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string? UserId
    {
        get
        {
            // For Auth0, the user ID is typically in the "sub" (subject) claim
            // or a custom claim like "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
            // Check your Auth0 token claims to confirm. "sub" is standard.
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ??
                   _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
        }
    }

    public string? UserName
    {
        get
        {
            // For Auth0, the user's name or preferred username might be in "name" or "preferred_username" claim
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue("name") ??
                   _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}