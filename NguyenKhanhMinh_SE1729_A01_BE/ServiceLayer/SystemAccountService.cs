using BusinessObjectsLayer.Models;
using RepositoriesLayer;

namespace ServiceLayer
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepo _repo;

        public SystemAccountService(ISystemAccountRepo repo)
        {
            _repo = repo;
        }

        public void AddAccount(SystemAccount account)
        {
            _repo.AddAccount(account);
        }

        public SystemAccount? GetAccountByEmail(string email)
        {
            return _repo.GetAccountByEmail(email);
        }

        public SystemAccount? GetAccountById(short accountId)
        {
            return _repo.GetAccountById(accountId);
        }

        public List<SystemAccount> GetAccounts()
        {
            return _repo.GetAccounts();
        }

        public void RemoveAccount(short accountId)
        {
            _repo.RemoveAccount(accountId);
        }

        public void UpdateAccount(SystemAccount updatedAccount)
        {
            _repo.UpdateAccount(updatedAccount);
        }
    }
}