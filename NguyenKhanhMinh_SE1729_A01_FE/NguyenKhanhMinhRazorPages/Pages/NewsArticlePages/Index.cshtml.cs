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
using RepositoriesLayer;

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

        public async Task<IActionResult> OnGetAsync()
        {
            // Check authentication
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                // Use API client instead of repository
                var endpoint = string.IsNullOrEmpty(SearchTitle)
                    ? "api/NewsArticles"
                    : $"api/NewsArticles?searchTitle={SearchTitle}";
                
                NewsArticle = await _apiClient.GetAsync<List<NewsArticle>>(endpoint);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error fetching articles: {ex.Message}");
                NewsArticle = new List<NewsArticle>();
            }

            return Page();
        }
    }
}
