using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.WorkType;

public class WorkTypeRepository : IWorkTypeRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.WorkType> _workTypeRepository;

    public WorkTypeRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _workTypeRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.WorkType>()!;
    }


    public IQueryable<DataTier.Entities.WorkType> GetWorkTypes()
    {
        IQueryable<DataTier.Entities.WorkType> workTypes = _workTypeRepository.Get(w => w.Status != (int?)WorkTypeEnum.WorkTypeStatus.Inactive);
        return workTypes;
    }

    public async Task<DataTier.Entities.WorkType?> GetWorkTypeById(int id)
    {
        DataTier.Entities.WorkType? workType = await _workTypeRepository.GetFirstOrDefaultAsync(w => w.Id == id && w.Status != (int?)WorkTypeEnum.WorkTypeStatus.Inactive);
        return workType;
    }

    public async Task<DataTier.Entities.WorkType?> CreateWorkType(DataTier.Entities.WorkType workType)
    {
        await _workTypeRepository.InsertAsync(workType);
        await _contextFactory.SaveAllAsync();
        return workType;
    }

    public async Task<DataTier.Entities.WorkType?> UpdateWorkType(DataTier.Entities.WorkType workType)
    {
        _workTypeRepository.Update(workType);
        await _contextFactory.SaveAllAsync();
        return workType;
    }

    public async Task<DataTier.Entities.WorkType?> DeleteWorkType(int id)
    {
        DataTier.Entities.WorkType? workType = await _workTypeRepository.GetFirstOrDefaultAsync(w =>
            w.Id == id && w.Status != (int?)WorkTypeEnum.WorkTypeStatus.Inactive);
        if (workType == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "WorkType is not exist or deleted");
        }

        workType.Status = (int?)WorkTypeEnum.WorkTypeStatus.Inactive;
        await _contextFactory.SaveAllAsync();
        return workType;
    }
}