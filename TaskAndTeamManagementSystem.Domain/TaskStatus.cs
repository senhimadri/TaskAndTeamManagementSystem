using System.ComponentModel.DataAnnotations;

namespace TaskAndTeamManagementSystem.Domain;

public enum ActivityStatus
{
    [Display(Name = "To Do")]
    ToDo = 1,
    [Display(Name = "In Progress")]
    InProgress = 2,
    [Display(Name = "Done")]
    Done = 3
}