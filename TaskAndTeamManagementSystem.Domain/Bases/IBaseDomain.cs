namespace TaskAndTeamManagementSystem.Domain.Bases;

public interface IBaseDomain
{
    DateTimeOffset CreateAt { get; set; }
    Guid? CreatedBy { get; set; }
    DateTimeOffset? UpdateAt { get; set; }
    Guid? UpdatedBy { get; set; }
    bool IsDelete { get; set; }
}