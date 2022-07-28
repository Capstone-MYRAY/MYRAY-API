using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Enums;
using MYRAY.Business.Repositories.Interface;
using MYRAY.Business.Services.Statistic;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Statistic;

public class StatisticRepository : IStatisticRepository
{
    private readonly IDbContextFactory _contextFactory;
    private readonly IBaseRepository<DataTier.Entities.JobPost> _jobPostRepository;
    private readonly IBaseRepository<DataTier.Entities.Account> _accountRepository;
    private readonly IBaseRepository<DataTier.Entities.AreaAccount> _areaAccountRepository;
    private readonly IBaseRepository<DataTier.Entities.Area> _areaRepository;
    private readonly IBaseRepository<DataTier.Entities.Garden> _gardenRepository;
    private readonly IBaseRepository<DataTier.Entities.PaymentHistory> _paymentHistoryRepository;

    public StatisticRepository(IDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _accountRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Account>()!;
        _jobPostRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.JobPost>()!;
        _areaAccountRepository =
            _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.AreaAccount>()!;
        _areaRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Area>()!;
        _gardenRepository = _contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Garden>()!;
        _paymentHistoryRepository =
            contextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.PaymentHistory>()!;
    }

    public async Task<DataTier.Entities.Area> GetAreaByModeratorId(int moderatorId)
    {
        DataTier.Entities.Account? moderator = await _accountRepository.GetFirstOrDefaultAsync(a => a.RoleId == 2);
        if (moderator != null)
        {
            Expression<Func<DataTier.Entities.AreaAccount, object>> expArea = area => area.Area;
            DataTier.Entities.AreaAccount? areaAccount = await
                _areaAccountRepository.GetFirstOrDefaultAsync(aa => aa.AccountId == moderator.Id, new []{expArea});
            if (areaAccount == null)
                throw new Exception("No assign Area for moderator");
            return areaAccount.Area;
        }

        throw new Exception("Moderator is not existed");
    }

    public async Task<double> TotalMoney(int? areaId = null)
    {
        double? total = 0;
        if (areaId == null)
        {
            total = await _paymentHistoryRepository
                .Get(p => p.Status == (int?)PaymentHistoryEnum.PaymentHistoryStatus.Paid)
                .SumAsync(p => p.ActualPrice);
        }
        else
        {
            total = await _paymentHistoryRepository
                .Get(p => p.JobPost.Garden.Area.Id == areaId)
                .SumAsync(p => p.ActualPrice);
        }

        return total ?? 0;
    }

    public IQueryable<DataTier.Entities.JobPost> TotalJobPosts(int? areaId = null)
    {
        IQueryable<DataTier.Entities.JobPost> totalJobPost =
            _jobPostRepository.Get(j => areaId == null || j.Garden.Area.Id == areaId);
        return totalJobPost;
    }

    public IQueryable<DataTier.Entities.Account> TotalLandowner(int? areaId = null)
    {
        IQueryable<DataTier.Entities.Account> totalLandowner = null;
        if (areaId != null)
        {
            totalLandowner = _gardenRepository
                .Get(a => (areaId == null || a.AreaId == areaId)
                          && a.Account.RoleId == 3)
                .Select(a => a.Account).Distinct();
        } else
            totalLandowner = _accountRepository.Get(a => a.RoleId == 3);

        return totalLandowner;
    }

    public IQueryable<DataTier.Entities.Account> TotalFarmer(int? areaId = null)
    {
        IQueryable<DataTier.Entities.Account> totalFarmer = null;
        if (areaId != null)
        {
            totalFarmer = _gardenRepository
                .Get(a => (areaId == null || a.AreaId == areaId)
                          && a.Account.RoleId == 4)
                .Select(a => a.Account).Distinct();
        } else
            totalFarmer = _accountRepository.Get(a => a.RoleId == 4);
        return totalFarmer;
    }
}