using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.AreaAccount;
using MYRAY.Business.Repositories.Interface;
using MYRAY.Business.Services.Notification;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Account;

/// <inheritdoc />
public class AccountRepository : IAccountRepository
{
    private readonly IBaseRepository<DataTier.Entities.Account>? _accountRepository;
    private readonly IBaseRepository<DataTier.Entities.PaymentHistory>? _paymentHistoryRepository;
    private readonly IAreaAccountRepository _areaAccountRepository;
    private readonly IDbContextFactory _dbContextFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountRepository"/> class.
    /// </summary>
    /// <param name="dbContextFactory">Injection of <see cref="IDbContextFactory"/></param>
    /// <param name="areaAccountRepository">Injection of see <see cref="IAreaAccountRepository"/></param>
    public AccountRepository(IDbContextFactory dbContextFactory, IAreaAccountRepository areaAccountRepository)
    {
        _dbContextFactory = dbContextFactory;
        _areaAccountRepository = areaAccountRepository;
        _accountRepository = _dbContextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.Account>();
        _paymentHistoryRepository = _dbContextFactory.GetContext<MYRAYContext>().GetRepository<DataTier.Entities.PaymentHistory>();
    }

    
    public async Task<DataTier.Entities.Account> GetAccountByIdAsync(int id)
    {
        Expression<Func<DataTier.Entities.Account, object>> expression = account => account.Role;
        DataTier.Entities.Account queryAccount = 
            await _accountRepository.GetFirstOrDefaultAsync(a => a.Id == id && a.Status == (int?)AccountEnum.AccountStatus.Active, 
                new []{expression});
        if (queryAccount == null)
        {
            return null;
        }
        
        return queryAccount;
    }

    public async Task<DataTier.Entities.Account> GetAccountByPhoneAsync(string phoneNumber)
    {
        DataTier.Entities.Account queryAccount =
            await _accountRepository.GetFirstOrDefaultAsync(a =>
                a.PhoneNumber.Equals(phoneNumber));
        if (queryAccount == null)
        {
            return null;
        }

        return queryAccount;
    }

    public async Task<DataTier.Entities.Account> GetAccountByRoleIdAsync(int roleId)
    { 
        DataTier.Entities.Account queryAccount = await 
            _accountRepository.GetFirstOrDefaultAsync(a => a.RoleId == roleId && a.Status == (int?)AccountEnum.AccountStatus.Active);
        if (queryAccount == null)
        {
            return null;
        }

        return queryAccount;
    }

    public IQueryable<DataTier.Entities.Account> GetAccounts()
    {
        IQueryable<DataTier.Entities.Account> queryAccount =
            _accountRepository!.Get(a => a.Status == (int?)AccountEnum.AccountStatus.Active);
        return queryAccount;
    }

    public async Task<DataTier.Entities.Account> CreateAccountAsync(DataTier.Entities.Account account, int? areaId)
    {
        await _accountRepository!.InsertAsync(account);
        
        if (areaId != null)
        {
            await _areaAccountRepository.DeleteAreaAccountByArea((int)areaId);
            await _areaAccountRepository.DeleteAreaAccountByAccount(account.Id);

            account.AreaAccounts = new List<DataTier.Entities.AreaAccount>() { new(){ AreaId = (int)areaId, AccountId = account.Id} };
            // await _areaAccountRepository.CreateAreaAccount(area.Id, (int)moderatorId);
        }

        await _dbContextFactory.SaveAllAsync();

        return account;
    }

    public async Task<DataTier.Entities.Account> UpdateAccountAsync(DataTier.Entities.Account account, int? areaId)
    {
        _accountRepository?.Modify(account);
        
        if (areaId != null)
        {
            await _areaAccountRepository.DeleteAreaAccountByArea((int)areaId);
            await _areaAccountRepository.DeleteAreaAccountByAccount(account.Id);

            // account.AreaAccounts = new List<DataTier.Entities.AreaAccount>() { new(){ AreaId = (int)areaId, AccountId = account.Id} };
            await _areaAccountRepository.CreateAreaAccount((int)areaId, account.Id);
        }
        
        await _dbContextFactory.SaveAllAsync();

        return account;
    }

    public async Task<DataTier.Entities.Account> DeleteAccountAsync(int id)
    {
        DataTier.Entities.Account account = _accountRepository.GetById(id);
        if (account == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Account is not existed", nameof(id));
        }

        account.Status = (int)AccountEnum.AccountStatus.Inactive;
        _accountRepository?.Update(account);

        await _dbContextFactory.SaveAllAsync();

        return account;
    }

    public async Task<DataTier.Entities.Account> BanAccountByIdAsync(int id)
    {
        DataTier.Entities.Account account = _accountRepository.GetById(id);
        if (account == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Account is not existed", nameof(id));
        }

        account.Status = (int)AccountEnum.AccountStatus.Banned;
        _accountRepository?.Update(account);

        await _dbContextFactory.SaveAllAsync();

        return account;
    }

    public async Task<DataTier.Entities.Account> TopUpAccountByIdAsync(int id, float topUp, int createBy)
    {
        DataTier.Entities.Account account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Account is not existed", nameof(id));
        }

        DataTier.Entities.PaymentHistory lastPayment = await  _paymentHistoryRepository.Get(p => p.BelongedId == account.Id)
            .OrderByDescending(p => p.CreatedDate)
            .FirstOrDefaultAsync();
        
        float balanceAfterPending = (float)(lastPayment == null ? account.Balance : lastPayment.Balance);

        if (topUp < 0)
        {
            if (topUp * -1 > account.Balance)
            {
                throw new MException(StatusCodes.Status400BadRequest,
                    "Balance of account not enough for deduct money", nameof(topUp));
            }

            account.Balance -= (topUp * -1);
            balanceAfterPending -= (topUp * -1);
        }
        else
        {
            account.Balance += topUp;
            balanceAfterPending += topUp;
        }
        
        _accountRepository?.Update(account);
        //-- New payment
        DataTier.Entities.PaymentHistory newPayment = new DataTier.Entities.PaymentHistory
        {
            CreatedBy = createBy,
            Status = (int?)PaymentHistoryEnum.PaymentHistoryStatus.Paid,
            CreatedDate = DateTime.Now,
            ActualPrice = topUp,
            BalanceFluctuation =  topUp,
            Balance = balanceAfterPending,
            EarnedPoint = 0,
            UsedPoint = 0,
            BelongedId = account.Id,
            Message =  (topUp > 0 ? "Nạp " : "Rút " ) + "tiền vào tài khoản",
            JobPostPrice = 0,
            PointPrice = 0
        };
        //-- End Add Payment

        await _paymentHistoryRepository.InsertAsync(newPayment);
        
        await _dbContextFactory.SaveAllAsync();

        // Sent noti
        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"type", "topUp"},
            {"paymentHistoryId", newPayment.Id.ToString()}
        };
        await PushNotification.SendMessage(id.ToString(), "Nạp tiền thành công", $"Bạn đã nạp thành công {topUp}VND", data);

        return account;
    }
}