using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjectsLayer.Entity;
using DAOsLayer;
using RepositoriesLayer;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.CategoryPages
{
    public class EditModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public EditModel(ApiClient apiClient)
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

                var categories = await _apiClient.GetAsync<List<Category>>("api/Categories");
                ViewData["ParentCategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
                return Page();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

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
                await _apiClient.PutAsync($"api/Categories/{Category.CategoryId}", Category);
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
                    ModelState.AddModelError("", $"Error updating category: {ex.Message}");
                    var categories = await _apiClient.GetAsync<List<Category>>("api/Categories");
                    ViewData["ParentCategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
                    return Page();
                }
            }
        }
    }
}
