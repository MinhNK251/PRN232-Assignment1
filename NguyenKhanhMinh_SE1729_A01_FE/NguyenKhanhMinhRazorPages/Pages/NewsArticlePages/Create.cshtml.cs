using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjectsLayer.Entity;
using RepositoriesLayer;
using Microsoft.AspNetCore.SignalR;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.NewsArticlePages
{
    public class CreateModel : PageModel
    {
        private readonly INewsArticleRepo _newsArticleRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly ISystemAccountRepo _systemAccountRepo;
        private readonly ITagRepo _tagRepo;
        private readonly IHubContext<SignalrServer> _hubContext;
        private readonly ApiClient _apiClient;

        public CreateModel(INewsArticleRepo newsArticleRepo, ICategoryRepo categoryRepo, ISystemAccountRepo systemAccountRepo, ITagRepo tagRepo, IHubContext<SignalrServer> hubContext, ApiClient apiClient)
        {
            _newsArticleRepo = newsArticleRepo;
            _categoryRepo = categoryRepo;
            _systemAccountRepo = systemAccountRepo;
            _tagRepo = tagRepo;
            _hubContext = hubContext;
            _apiClient = apiClient;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;
        [BindProperty]
        public List<int>? SelectedTags { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["CategoryId"] = new SelectList(_categoryRepo.GetCategories(), "CategoryId", "CategoryName");
            ViewData["Tags"] = new MultiSelectList(_tagRepo.GetTags(), "TagId", "TagName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            NewsArticle.CreatedDate = DateTime.Now;
            NewsArticle.ModifiedDate = DateTime.Now;
            
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToPage("/Login");
            }
            
            // Get account info (you might need to adjust this)
            var account = await _apiClient.GetAsync<SystemAccount>($"api/SystemAccounts/byEmail/{userEmail}");
            
            NewsArticle.CreatedById = account.AccountId;
            NewsArticle.UpdatedById = account.AccountId;
            
            // Handle tags if needed
            
            // Use API to create article
            await _apiClient.PostAsync<NewsArticle>("api/NewsArticles", NewsArticle);
            
            // Signal update to other clients
            await _hubContext.Clients.All.SendAsync("LoadData");
            
            return RedirectToPage("./Index");
        }
    }
}
