namespace TaskAndTeamManagementSystem.Domain;

public class BaseDomain<TKey> 
{
    public TKey Id { get; set; } = default(TKey)!;
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset? UpdateAt { get; set; }
    public bool IsDelete { get; set; }
}