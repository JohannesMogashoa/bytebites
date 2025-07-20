namespace ByteBites.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; } // Or any other claim you want to extract
    bool IsAuthenticated { get; }
}