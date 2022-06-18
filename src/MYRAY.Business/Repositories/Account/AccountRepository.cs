using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Repositories.Interface;
using MYRAY.DataTier.Entities;

namespace MYRAY.Business.Repositories.Account;

/// <inheritdoc />
public class AccountRepository : IAccountRepository
{
    private readonly IBaseRepository<DataTier.Entities.Account>? _accountRepository;
    private readonly IBaseRepository<DataTier.Entities.PaymentHistory>? _paymentHistoryRepository;
    private readonly IDbContextFactory _dbContextFactory;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountRepository"/> class.
    /// </summary>
    /// <param name="dbContextFactory">Injection of <see cref="IDbContextFactory"/></param>
    public AccountRepository(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
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
                a.PhoneNumber.Equals(phoneNumber) && a.Status == (int?)AccountEnum.AccountStatus.Active);
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

    public async Task<DataTier.Entities.Account> CreateAccountAsync(DataTier.Entities.Account account)
    {
        await _accountRepository!.InsertAsync(account);

        await _dbContextFactory.SaveAllAsync();

        return account;
    }

    public async Task<DataTier.Entities.Account> UpdateAccountAsync(DataTier.Entities.Account account)
    {
        _accountRepository?.Modify(account);
        
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
        

        if (topUp < 0)
        {
            if (topUp * -1 > account.Balance)
            {
                throw new MException(StatusCodes.Status400BadRequest,
                    "Balance of account not enough for deduct money", nameof(topUp));
            }

            account.Balance -= (topUp * -1);
        }
        else
        {
            account.Balance += topUp;
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
            Balance = account.Balance,
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

        return account;
    }
}