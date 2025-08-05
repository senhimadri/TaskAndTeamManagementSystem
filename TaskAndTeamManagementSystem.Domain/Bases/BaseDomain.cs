namespace TaskAndTeamManagementSystem.Domain.Bases;

public class BaseDomain<TKey> : IBaseDomain
{
    public TKey Id { get; set; } = default!;
    public DateTimeOffset CreateAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDelete { get; set; }
}
