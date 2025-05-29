using BusinessObjectsLayer.Models;
using RepositoriesLayer;

namespace ServiceLayer
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepo _repo;
        private readonly INewsArticleRepo _newsRepo;

        public SystemAccountService(ISystemAccountRepo repo, INewsArticleRepo newsRepo)
        {
            _repo = repo;
            _newsRepo = newsRepo;
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
            var articles = _newsRepo.GetNewsArticlesByCreatedBy(accountId);

            if (articles != null && articles.Count > 0)
            {
                throw new InvalidOperationException("This account has created news articles.");
            }

            _repo.RemoveAccount(accountId);
        }

        public void UpdateAccount(SystemAccount updatedAccount)
        {
            _repo.UpdateAccount(updatedAccount);
        }
    }
}