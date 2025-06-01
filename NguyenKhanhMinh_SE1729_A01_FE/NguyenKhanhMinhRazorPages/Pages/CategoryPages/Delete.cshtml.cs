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
    public class DeleteModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public DeleteModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short id)
        {
            try
            {
                Category = await _apiClient.GetAsync<Category>($"api/Categories/{id}");
                if (Category == null)
                {
                    return NotFound();
                }
                return Page();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync(short id)
        {
            try
            {
                await _apiClient.DeleteAsync($"api/Categories/{id}");
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404") || ex.Message.Contains("NotFound"))
                {
                    return NotFound();
                }
                else
                {
                    // If there's an error (like a constraint violation), redisplay the page with an error
                    Category = await _apiClient.GetAsync<Category>($"api/Categories/{id}");
                    ModelState.AddModelError("", $"Error deleting category: {ex.Message}");
                    return Page();
                }
            }
        }
    }
}
