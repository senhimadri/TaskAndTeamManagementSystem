using FluentValidation;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;

namespace TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos.Validator;

public class TaskItemDtoValidator: AbstractValidator<ITaskItemDto>
{
    private readonly IUnitOfWork _unitOfWork;
    public TaskItemDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Status)
           .Must(s => s == 1 || s == 2 || s == 3)
           .WithMessage("Status must be one of the following values: 1 (Todo), 2 (InProgress), or 3 (Done).");


        RuleFor(x => x.DueDate)
            .Must(d => d == null || d > DateTimeOffset.UtcNow)
            .WithMessage("Due date must be in the future.");

        //RuleFor(x => x.AssignedUserId)
        //    .NotEmpty().WithMessage("Assigned user is required.")
        //    .MustAsync(async (assignedUserId, cancellationToken) =>
        //        await _unitOfWork.UserRepository.IsAnyAsync(u => u.Id == assignedUserId))
        //    .WithMessage("Assigned user does not exist.");

        RuleFor(x => x.TeamId)
            .NotEqual(0).WithMessage("Team ID must be a non-zero value.")
            .MustAsync(async (teamId, cancellationToken) =>
                            await _unitOfWork.TeamRepository.IsAnyAsync(u => u.Id == teamId))
            .WithMessage("Team does not exist.");
    }
}
