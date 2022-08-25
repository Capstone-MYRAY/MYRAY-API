namespace MYRAY.Business.Repositories.Account;
/// <summary>
/// Interface for Account repository
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// Get async account by account's ID
    /// </summary>
    /// <param name="id">Id of account</param>
    /// <returns>An account or null if not existed.</returns>
    Task<DataTier.Entities.Account> GetAccountByIdAsync(int id);
    
    /// <summary>
    /// Get async account by phone 
    /// </summary>
    /// <param name="phoneNumber">Phone Number of Account</param>
    /// <returns>An account or null if not existed.</returns>
    Task<DataTier.Entities.Account?> GetAccountByPhoneAsync(string phoneNumber);

    /// <summary>
    /// Get async account by Role Id
    /// </summary>
    /// <param name="roleId">Id of role.</param>
    /// <returns>An account or null if not existed.</returns>
    Task<MYRAY.DataTier.Entities.Account> GetAccountByRoleIdAsync(int roleId);
    
    /// <summary>
    /// Get all account.
    /// </summary>
    /// <returns>List of account with condition.</returns>
    IQueryable<DataTier.Entities.Account> GetAccounts();

    /// <summary>
    /// Create a new account
    /// </summary>
    /// <param name="account">Account object to create.</param>
    /// <returns>A new account</returns>
    Task<DataTier.Entities.Account> CreateAccountAsync(DataTier.Entities.Account account, int? areaId);
    
    /// <summary>
    /// Update an exist account
    /// </summary>
    /// <param name="account">Account object to update</param>
    /// <returns>A new updated account</returns>
    Task<DataTier.Entities.Account> UpdateAccountAsync(DataTier.Entities.Account account, int? areaId);
    
    /// <summary>
    /// Delete an existed account
    /// </summary>
    /// <param name="id">Id of account</param>
    /// <returns>An account</returns>
    Task<DataTier.Entities.Account> DeleteAccountAsync(int id);
    
    /// <summary>
    /// Ban an existed account.
    /// </summary>
    /// <param name="id">Id of account</param>
    /// <returns>An account</returns>
    Task<DataTier.Entities.Account> BanAccountByIdAsync(int id);

    /// <summary>
    /// Unban an existed account .
    /// </summary>
    /// <param name="id">Id of account</param>
    /// <returns>An account.</returns>
    Task<MYRAY.DataTier.Entities.Account> UnbanAccountByIdAsync(int id);

    /// <summary>
    /// Top up balance for account
    /// </summary>
    /// <param name="id">ID of account</param>
    /// <param name="topUp">number of topup</param>
    /// <param name="createBy">Account Create</param>
    /// <param name="transactionId">TransactionId</param>
    /// <returns>An account</returns>
    Task<DataTier.Entities.Account> TopUpAccountByIdAsync(int id, float topUp, int createBy, string? transactionId = null);
}