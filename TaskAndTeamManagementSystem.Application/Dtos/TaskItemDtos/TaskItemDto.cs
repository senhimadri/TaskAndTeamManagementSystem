using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;

public record CreateTaskItemPayload(string Title, 
                                    string? Description, 
                                    int Status, 
                                    DateTimeOffset? DueDate, 
                                    Guid AssignedUserId, 
                                    int TeamId) : ITaskItemDto;

public record UpdateTaskItemPayload(string Title,
                                    string? Description,
                                    int Status,
                                    DateTimeOffset? DueDate,
                                    Guid AssignedUserId,
                                    int TeamId) : ITaskItemDto;

public record GetTaskItemByIdResponse(string Title,
                                    string? Description,
                                    int Status,
                                    string StatusName,
                                    DateTimeOffset? DueDate,
                                    Guid AssignedUserId,
                                    string AssignedUserName,
                                    Guid CreatedById,
                                    string CreatedByName,   
                                    int TeamId,
                                    string TeamName);

public record GetTaskItemsListResponse( long Id,
                                        string Title,
                                        string Status, 
                                        string AssignedUserName,
                                        string CreatedByName,
                                        string TeamName);

public record TaskItemPaginationQuery(string? SearchText,int StatusId,
                                      Guid AssignedUserId,
                                      int TeamId,
                                      int PageNo,
                                      int PageSize,
                                      bool IsAscending,
                                      DateTimeOffset? FromDate,
                                      DateTimeOffset? ToDate);
