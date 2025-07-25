﻿namespace TaskAndTeamManagementSystem.Identity;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public double AccessTokenExpirationMinutes { get; set; }
    public double RefreshTokenExpirationDays { get; set; }
}
