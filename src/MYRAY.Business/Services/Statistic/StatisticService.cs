using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Repositories.Statistic;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Services.Statistic;

public class StatisticService : IStatisticService
{
    private readonly IMapper _mapper;
    private readonly IStatisticRepository _statisticRepository;

    public StatisticService(IMapper mapper, IStatisticRepository statisticRepository)
    {
        _mapper = mapper;
        _statisticRepository = statisticRepository;
    }

    public async Task<double> TotalMoney(int? moderatorId = null)
    {
        int? areaId = null;
        if (moderatorId != null)
        {
            DataTier.Entities.Area area = await _statisticRepository.GetAreaByModeratorId((int)moderatorId);
            areaId = area.Id;
        }
        return await _statisticRepository.TotalMoney(areaId);
        
    }

    public async Task<int> TotalJobPost(int? moderatorId = null)
    {
        int? areaId = null;
        if (moderatorId != null)
        {
            DataTier.Entities.Area area = await _statisticRepository.GetAreaByModeratorId((int)moderatorId);
            areaId = area.Id;
        }
        return _statisticRepository.TotalJobPosts(areaId).Count();
    }

    public async Task<int> TotalLandowner(int? moderatorId = null)
    {
        int? areaId = null;
        if (moderatorId != null)
        {
            DataTier.Entities.Area area = await _statisticRepository.GetAreaByModeratorId((int)moderatorId);
            areaId = area.Id;
        }
        return _statisticRepository.TotalLandowner(areaId).Count();
    }

    public async Task<int> TotalFarmer(int? moderatorId = null)
    {
        int? areaId = null;
        if (moderatorId != null)
        {
            DataTier.Entities.Area area = await _statisticRepository.GetAreaByModeratorId((int)moderatorId);
            areaId = area.Id;
        }
        return _statisticRepository.TotalFarmer(areaId).Count();
    }
}