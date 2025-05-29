using BusinessObjectsLayer.Entity;

namespace ServiceLayer
{
    public interface ISystemAccountService
    {
        SystemAccount? GetAccountById(short accountId);
        SystemAccount? GetAccountByEmail(string email);
        List<SystemAccount> GetAccounts();
        void AddAccount(SystemAccount account);
        void UpdateAccount(SystemAccount updatedAccount);
        void RemoveAccount(short accountId);
    }
}