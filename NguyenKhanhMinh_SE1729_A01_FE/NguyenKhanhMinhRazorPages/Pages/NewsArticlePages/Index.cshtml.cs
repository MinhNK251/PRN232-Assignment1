using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjectsLayer.Entity;
using System.Text.Json;
using System.Text.Json.Serialization;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.NewsArticlePages
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IList<NewsArticle> NewsArticle { get; set; } = new List<NewsArticle>();
        
        [BindProperty(SupportsGet = true)]
        public string SearchTitle { get; set; }

        public IActionResult OnGet()
        {
            // Check authentication
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public async Task<JsonResult> OnGetLoadData(string? searchTitle)
        {
            try
            {
                var endpoint = string.IsNullOrEmpty(searchTitle) 
                    ? "api/NewsArticles" 
                    : $"api/NewsArticles?searchTitle={searchTitle}";
                
                var articles = await _apiClient.GetAsync<List<NewsArticle>>(endpoint);
                
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                return new JsonResult(new { articles }, options);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
