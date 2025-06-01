using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjectsLayer.Entity;
using DAOsLayer;
using RepositoriesLayer;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.SystemAccountPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IList<SystemAccount> SystemAccount { get;set; } = default!;

        public async Task OnGetAsync()
        {
            try
            {
                SystemAccount = await _apiClient.GetAsync<List<SystemAccount>>("api/SystemAccounts");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching accounts: {ex.Message}");
                SystemAccount = new List<SystemAccount>();
            }
        }
    }
}
