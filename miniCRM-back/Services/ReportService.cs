using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using miniCRM_back.Configs;
using miniCRM_back.Database;
using miniCRM_back.DTOs;
using miniCRM_back.Models;
using miniCRM_back.Services.Contracts;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace miniCRM_back.Services {
    public class ReportService : BaseService<TaskItem, TaskItemDto, TaskItemForCreationDto, TaskItemDto>, IReportService {
        public ReportService(IGenericRepository<TaskItem> repository, ILogger<BaseService<TaskItem, TaskItemDto, TaskItemForCreationDto, TaskItemDto>> logger, IMapper mapper) : base(repository, logger, mapper) {

        }

        public async Task<PagedResult<IEnumerable<ExpiredTaskItem>>> GetExpiredTasksAsync(PaginationParams paginationParams, bool includeBlankExpiredAt) {
            try {
                var (data, totalCount) = await repository.GetWithCustomQueryAsync(paginationParams, 
                    query => query.Where(x => (includeBlankExpiredAt && !x.ExpiredAt.HasValue) // tasks without deadline
                    || (x.ExpiredAt < DateTime.Now && !x.CompletedAt.HasValue) // incomplete tasks with expired deadline
                    || x.CompletedAt > x.ExpiredAt ) // completed tasks later than deadline
                    .Include(x=>x.User));

                var expired = data.Select(x => new ExpiredTaskItem {
                                                                    Id = x.Id,
                                                                    Name = x.Name,
                                                                    Details = x.Details,
                                                                    Percent = x.Percent,
                                                                    StartDate = x.StartDate,
                                                                    CompletedAt = x.CompletedAt,
                                                                    ExpiredAt = x.ExpiredAt,
                                                                    UserId = x.UserId,
                                                                    User = x.User,
                                                                }).ToList();

                var paginationMetadata = GetPaginationMetadata(paginationParams, totalCount);
                return PagedResult<IEnumerable<ExpiredTaskItem>>.Success(expired, paginationMetadata);
            } catch (Exception ex) {
                return PagedResult<IEnumerable<ExpiredTaskItem>>.Failure("InternalServerError", ex.Message);
            }
        }
    }
}
