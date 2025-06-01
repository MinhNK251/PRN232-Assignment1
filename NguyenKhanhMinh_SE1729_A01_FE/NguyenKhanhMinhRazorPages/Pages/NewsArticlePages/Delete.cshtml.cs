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
using Microsoft.AspNetCore.SignalR;
using NguyenKhanhMinhRazorPages.Services;

namespace NguyenKhanhMinhRazorPages.Pages.NewsArticlePages
{
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleRepo _newsArticleRepo;
        private readonly ITagRepo _tagRepo;
        private readonly IHubContext<SignalrServer> _hubContext;
        private readonly ApiClient _apiClient;

        public DeleteModel(INewsArticleRepo newsArticleRepo, ITagRepo tagRepo, IHubContext<SignalrServer> hubContext, ApiClient apiClient)
        {
            _newsArticleRepo = newsArticleRepo;
            _tagRepo = tagRepo;
            _hubContext = hubContext;
            _apiClient = apiClient;
        }

        public NewsArticle NewsArticle { get; set; } = default!;
        public List<Tag> Tags { get; set; } = new List<Tag>();

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
            
            // Get tags for this article
            Tags = _tagRepo.GetTagsByNewsArticleId(id);
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Delete article via API
            await _apiClient.DeleteAsync($"api/NewsArticles/{id}");
            
            // Signal update to other clients
            await _hubContext.Clients.All.SendAsync("LoadData");
            
            return RedirectToPage("./Index");
        }
    }
}
