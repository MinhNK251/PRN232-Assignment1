using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjectsLayer.Entity;
using DAOsLayer;
using RepositoriesLayer;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.CategoryPages
{
    public class CreateModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public CreateModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get categories for dropdown
            var categories = await _apiClient.GetAsync<List<Category>>("api/Categories");
            ViewData["ParentCategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categories = await _apiClient.GetAsync<List<Category>>("api/Categories");
                ViewData["ParentCategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
                return Page();
            }

            try
            {
                await _apiClient.PostAsync<Category>("api/Categories", Category);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating category: {ex.Message}");
                var categories = await _apiClient.GetAsync<List<Category>>("api/Categories");
                ViewData["ParentCategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
                return Page();
            }
        }
    }
}
