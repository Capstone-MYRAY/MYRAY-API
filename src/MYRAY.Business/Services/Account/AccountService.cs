using AutoMapper;
using Microsoft.AspNetCore.Http;
using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.DTOs.Authentication;
using MYRAY.Business.Enums;
using MYRAY.Business.Exceptions;
using MYRAY.Business.Helpers;
using MYRAY.Business.Helpers.Paging;
using MYRAY.Business.Repositories.Account;

namespace MYRAY.Business.Services.Account;
/// <summary>
/// Account Service class.
/// </summary>
public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly IAccountRepository _accountRepository;

    /// <summary>
    /// Initialize new instance of the <see cref="AccountService"/> class.
    /// </summary>
    /// <param name="mapper">Injection of <see cref="IMapper"/></param>
    /// <param name="accountRepository">Injection of <see cref="IAccountRepository"/></param>
    public AccountService(IMapper mapper, IAccountRepository accountRepository)
    {
        _mapper = mapper;
        _accountRepository = accountRepository;
    }
    /// <inheritdoc cref="IAccountService.GetAccounts"/>
    public ResponseDto.CollectiveResponse<GetAccountDetail> GetAccounts(PagingDto pagingDto, SortingDto<AccountEnum.AccountSortCriteria> sortingDto, SearchAccountDto searchAccountDto)
    {
        IQueryable<DataTier.Entities.Account> queryAccount = _accountRepository.GetAccounts();
        
        //--Apply Search
        queryAccount = queryAccount.GetWithSearch(searchAccountDto);

        //--Apply Sorting
        queryAccount = queryAccount.GetWithSorting(sortingDto.SortColumn.ToString(), sortingDto.OrderBy);

        //--Apply Paging
        ResponseDto.CollectiveResponse<GetAccountDetail> result =
            queryAccount.GetWithPaging<GetAccountDetail, DataTier.Entities.Account>(pagingDto, _mapper);

        return result;
    }
    
    /// <inheritdoc cref="IAccountService.GetAccountByIdAsync"/>
    public async Task<GetAccountDetail> GetAccountByIdAsync(int? id)
    {
        if (id == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "ID is not empty");
        }
   
        if (!int.TryParse(id.ToString(), out _))
        {
            throw new MException(StatusCodes.Status400BadRequest, "ID must be Number");
        }

        DataTier.Entities.Account queryAccount = await _accountRepository.GetAccountByIdAsync((int)id);

        GetAccountDetail accountDto = _mapper.Map<GetAccountDetail>(queryAccount);

        return accountDto;
    }

    public async Task<GetAccountDetail> GetAccountByPhoneNumberAsync(string? phoneNumber)
    {
        if (phoneNumber == null)
        {
            throw new MException(StatusCodes.Status400BadRequest, "Phone number is not empty");
        }
        
        DataTier.Entities.Account queryAccount = await _accountRepository.GetAccountByPhoneAsync((string)phoneNumber);

        GetAccountDetail accountDto = _mapper.Map<GetAccountDetail>(queryAccount);

        return accountDto;
    }

    /// <inheritdoc cref="IAccountService.CreateAccountAsync"/>
    public async Task<GetAccountDetail> CreateAccountAsync(InsertAccountDto? bodyDto)
    {
        if (bodyDto is null)
        {
            throw new MException(StatusCodes.Status400BadRequest, $"{nameof(InsertAccountDto)} is Null", nameof(bodyDto));
        }

        DataTier.Entities.Account newAccount = _mapper.Map<DataTier.Entities.Account>(bodyDto);
        newAccount.PhoneNumber = newAccount.PhoneNumber.ConvertVNPhoneNumber();
        Console.WriteLine(newAccount.PhoneNumber);
        newAccount = await _accountRepository.CreateAccountAsync(newAccount);

        GetAccountDetail newAccountDto = _mapper.Map<GetAccountDetail>(newAccount);

        return newAccountDto;
    }

    public async Task<GetAccountDetail> SignupAsync(SignupRequest? bodyDto)
    {
        InsertAccountDto insertAccountDto = _mapper.Map<InsertAccountDto>(bodyDto);
        return await CreateAccountAsync(insertAccountDto);
    }

    public async Task<UpdateAccountDto> ChangPasswordAsync(int id, string newPassword)
    {
        try
        {
            DataTier.Entities.Account account = await _accountRepository.GetAccountByIdAsync(id);
            account.Password = newPassword;
            account = await _accountRepository.UpdateAccountAsync(account);
            UpdateAccountDto updateAccountDto = _mapper.Map<UpdateAccountDto>(account);
            return updateAccountDto;
        }
        catch (Exception e)
        {
            throw new MException(StatusCodes.Status400BadRequest, e.Message, nameof(e.TargetSite.Name));
        }
    }

    /// <inheritdoc cref="IAccountService.UpdateAccountAsync"/>
    public async Task<UpdateAccountDto> UpdateAccountAsync(UpdateAccountDto bodyDto)
    {
        UpdateAccountDto updateAccountDto;
        try
        {
            if (bodyDto is null)
            {
                throw new MException(StatusCodes.Status400BadRequest, $"{nameof(UpdateAccountDto)} is Null");
            }
            DataTier.Entities.Account updateAccount = _mapper.Map<DataTier.Entities.Account>(bodyDto);
            updateAccount = await _accountRepository.UpdateAccountAsync(updateAccount);

            updateAccountDto = _mapper.Map<UpdateAccountDto>(updateAccount);
        }
        catch (MException e)
        {
            throw new MException(StatusCodes.Status400BadRequest, e.Message, nameof(e.TargetSite.Name));
        }

        return updateAccountDto;
    }

    /// <inheritdoc cref="IAccountService.DeleteAccountByIdAsync"/>
    public async Task<bool> DeleteAccountByIdAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                throw new MException(StatusCodes.Status400BadRequest, "Id is null");
            }
            DataTier.Entities.Account deleteAccount = await _accountRepository.DeleteAccountAsync((int)id);
             
            return true;
        }
        catch (Exception e)
        {
            throw new MException(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <inheritdoc cref="IAccountService.BanAccountByIdAsync"/>
    public async Task<bool> BanAccountByIdAsync(int? id)
    {
        try
        {
            if (id == null)
            {
                throw new MException(StatusCodes.Status400BadRequest, "Id is null");
            }

            DataTier.Entities.Account banAccount = await _accountRepository.BanAccountByIdAsync((int)id);

            return true;
        }
        catch (Exception e)
        {
            throw new MException(StatusCodes.Status400BadRequest, e.Message);
        }
    }

    /// <inheritdoc cref="IAccountService.TopUpAccountByIdAsync"/>
    public async Task<GetAccountDetail> TopUpAccountByIdAsync(int? id, float topUp)
    {
        try
        {
            if (id == null)
            {
                throw new MException(StatusCodes.Status400BadRequest, "Id is null");
            }

            DataTier.Entities.Account topUpAccount = await _accountRepository.TopUpAccountByIdAsync((int)id, topUp);
            GetAccountDetail result = _mapper.Map<GetAccountDetail>(topUpAccount);
            return result;
        }
        catch (Exception e)
        {
            throw new MException(StatusCodes.Status400BadRequest, e.Message);
        }
    }
}