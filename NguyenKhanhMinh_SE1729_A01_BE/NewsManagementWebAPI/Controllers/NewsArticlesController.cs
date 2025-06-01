using Microsoft.AspNetCore.Mvc;
using BusinessObjectsLayer.Entity;
using ServiceLayer;
using Microsoft.Extensions.Logging;

namespace NewsManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _service;
        private readonly ILogger<NewsArticlesController> _logger;

        public NewsArticlesController(INewsArticleService service, ILogger<NewsArticlesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/NewsArticles
        [HttpGet]
        public ActionResult<IEnumerable<NewsArticle>> GetNewsArticles()
        {
            _logger.LogInformation("GET request received for all news articles");
            var articles = _service.GetNewsArticles();
            return Ok(articles);
        }

        // GET: api/NewsArticles/5
        [HttpGet("{id}")]
        public ActionResult<NewsArticle> GetNewsArticle(string id)
        {
            _logger.LogInformation($"GET request received for news article with ID: {id}");
            var article = _service.GetNewsArticleById(id);

            if (article == null)
            {
                _logger.LogWarning($"News article with ID: {id} not found");
                return NotFound();
            }

            return Ok(article);
        }

        // PUT: api/NewsArticles/5
        [HttpPut("{id}")]
        public IActionResult PutNewsArticle(string id, NewsArticle newsArticle)
        {
            _logger.LogInformation($"PUT request received for news article with ID: {id}");
            
            if (id != newsArticle.NewsArticleId)
            {
                _logger.LogWarning("ID mismatch in PUT request");
                return BadRequest("ID mismatch");
            }

            try
            {
                _service.UpdateNewsArticle(id, newsArticle);
                _logger.LogInformation($"News article with ID: {id} successfully updated");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error updating news article: {ex.Message}");
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        // POST: api/NewsArticles
        [HttpPost]
        public ActionResult<NewsArticle> PostNewsArticle(NewsArticle newsArticle)
        {
            _logger.LogInformation("POST request received to create a new news article");
            
            try
            {
                _service.AddNewsArticle(newsArticle);
                _logger.LogInformation($"News article created with ID: {newsArticle.NewsArticleId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating news article: {ex.Message}");
                return Conflict(ex.Message);
            }

            return CreatedAtAction(nameof(GetNewsArticle), new { id = newsArticle.NewsArticleId }, newsArticle);
        }

        // DELETE: api/NewsArticles/5
        [HttpDelete("{id}")]
        public IActionResult DeleteNewsArticle(string id)
        {
            var existing = _service.GetNewsArticleById(id);
            if (existing == null)
                return NotFound();

            _service.RemoveNewsArticle(id);
            return NoContent();
        }
    }
}
