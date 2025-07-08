namespace TaskAndTeamManagementSystem.Domain;

public class BaseDomain<TKey> : IBaseDomain
{
    public TKey Id { get; set; } = default(TKey)!;
    public DateTimeOffset CreateAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDelete { get; set; }
}

public interface IBaseDomain
{
    DateTimeOffset CreateAt { get; set; }
    Guid? CreatedBy { get; set; }
    DateTimeOffset? UpdateAt { get; set; }
    Guid? UpdatedBy { get; set; }
    bool IsDelete { get; set; }
}