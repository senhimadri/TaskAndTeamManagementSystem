﻿namespace TaskAndTeamManagementSystem.Domain;

public class Team : BaseDomain<int>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
