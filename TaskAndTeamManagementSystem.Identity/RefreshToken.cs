namespace TaskAndTeamManagementSystem.Identity;

public class RefreshToken
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;
}
