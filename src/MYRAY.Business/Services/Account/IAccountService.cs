using MYRAY.Business.DTOs;
using MYRAY.Business.DTOs.Account;
using MYRAY.Business.DTOs.Authentication;
using MYRAY.Business.Enums;

namespace MYRAY.Business.Services.Account;
/// <summary>
/// Interface for account service.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Get list of account with condition
    /// </summary>
    /// <param name="pagingDto">Object contains paging criteria.</param>
    /// <param name="sortingDto">Object contains sorting criteria</param>
    /// <param name="searchAccountDto">Object contains name of field filter</param>
    /// <returns></returns>
    public ResponseDto.CollectiveResponse<GetAccountDetail> GetAccounts(PagingDto pagingDto,
        SortingDto<AccountEnum.AccountSortCriteria> sortingDto, SearchAccountDto searchAccountDto);

    /// <summary>
    /// Get async an account by id
    /// </summary>
    /// <param name="id">ID of account.</param>
    /// <returns>An account</returns>
    public Task<GetAccountDetail> GetAccountByIdAsync(int? id);

    /// <summary>
    /// Get async an account by phone number
    /// </summary>
    /// <param name="phoneNumber">PhoneNumber</param>
    /// <returns>An account</returns>
    public Task<GetAccountDetail> GetAccountByPhoneNumberAsync(string? phoneNumber);
    
    /// <summary>
    /// Create an new account
    /// </summary>
    /// <param name="bodyDto">An object contains info to insert account</param>
    /// <returns>An Account</returns>
    public Task<GetAccountDetail> CreateAccountAsync(InsertAccountDto? bodyDto);
    
    /// <summary>
    /// Create an new account
    /// </summary>
    /// <param name="bodyDto">An object contains info to insert account</param>
    /// <returns>An Account</returns>
    public Task<GetAccountDetail> SignupAsync(SignupRequest? bodyDto);

    /// <summary>
    /// Change password of account
    /// </summary>
    /// <param name="id">Id of account</param>
    /// <param name="newPassword">New password to change</param>
    /// <returns>a updated account</returns>
    public Task<UpdateAccountDto> ChangPasswordAsync(int id, string newPassword);
    
    /// <summary>
    /// Update an existed account.
    /// </summary>
    /// <param name="updateAccountDto">Object contains input information account</param>
    /// <returns>An account</returns>
    public Task<UpdateAccountDto> UpdateAccountAsync(UpdateAccountDto updateAccountDto);
    
    /// <summary>
    /// Delete an existed account.
    /// </summary>
    /// <param name="id">Id of account</param>
    /// <returns>Task Async</returns>
    public Task<bool> DeleteAccountByIdAsync(int? id);
    
    /// <summary>
    /// Ban an existed account.
    /// </summary>
    /// <param name="id">Id of account</param>
    /// <returns></returns>
    public Task<bool> BanAccountByIdAsync(int? id);

    /// <summary>
    /// Top up balance an existed account.
    /// </summary>
    /// <param name="id">Id of account</param>
    /// <param name="topUp">Money to top up account</param>
    /// <param name="createBy">Who top up</param>
    /// <returns>An account</returns>
    public Task<GetAccountDetail> TopUpAccountByIdAsync(int? id, float topUp, int createBy);
}