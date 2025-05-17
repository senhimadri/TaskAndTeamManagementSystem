using Microsoft.EntityFrameworkCore;
using TaskAndTeamManagementSystem.Application.Dtos.CommonDtos;

namespace TaskAndTeamManagementSystem.Application.Helpers.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> data, int pageNo, int pageSize)
    {
        pageNo = Math.Max(1, pageNo);
        pageSize = Math.Max(1, pageSize);

        int skippedRow = (pageNo - 1) * pageSize;
        data.Skip(skippedRow).Take(pageSize);

        return data;
    }

    public static async Task<GetPaginationResponse<T>> ToPaginatedResultAsync<T>(this IQueryable<T> data, int pageNo, int pageSize)
    {
        var totalCount = await data.CountAsync();
        var paginatedData = await data.Paginate(pageNo, pageSize).ToListAsync();

        return new GetPaginationResponse<T>(pageNo, pageSize, totalCount, paginatedData);

    }
}
