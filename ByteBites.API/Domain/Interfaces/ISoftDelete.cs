namespace ByteBites.API.Domain.Interfaces;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}