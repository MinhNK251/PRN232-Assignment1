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
using Microsoft.AspNetCore.SignalR;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.NewsArticlePages
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleRepo _newsArticleRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly ISystemAccountRepo _systemAccountRepo;
        private readonly ITagRepo _tagRepo;
        private readonly IHubContext<SignalrServer> _hubContext;
        private readonly ApiClient _apiClient;

        public EditModel(INewsArticleRepo newsArticleRepo, ICategoryRepo categoryRepo, ISystemAccountRepo systemAccountRepo, ITagRepo tagRepo, IHubContext<SignalrServer> hubContext, ApiClient apiClient)
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
        public List<int> SelectedTags { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Use API to get article
            NewsArticle = await _apiClient.GetAsync<NewsArticle>($"api/NewsArticles/{id}");

            if (NewsArticle == null)
            {
                return NotFound();
            }
            SelectedTags = NewsArticle.Tags.Select(t => t.TagId).ToList();
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

            // Update article via API
            await _apiClient.PutAsync($"api/NewsArticles/{NewsArticle.NewsArticleId}", NewsArticle);
            
            // Signal update to other clients
            await _hubContext.Clients.All.SendAsync("LoadData");
            
            return RedirectToPage("./Index");
        }

    }
}
