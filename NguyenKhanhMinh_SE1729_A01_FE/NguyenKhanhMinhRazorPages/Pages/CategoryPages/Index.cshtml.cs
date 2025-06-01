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

namespace NguyenKhanhMinhRazorPages.Pages.CategoryPages
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            try
            {
                Category = await _apiClient.GetAsync<List<Category>>("api/Categories");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                Category = new List<Category>();
            }
        }
    }
}
