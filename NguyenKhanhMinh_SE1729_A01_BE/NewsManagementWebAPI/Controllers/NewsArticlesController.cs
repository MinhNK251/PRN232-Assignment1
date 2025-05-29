using Microsoft.AspNetCore.Mvc;
using BusinessObjectsLayer.Entity;
using ServiceLayer;

namespace NewsManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _service;

        public NewsArticlesController(INewsArticleService service)
        {
            _service = service;
        }

        // GET: api/NewsArticles
        [HttpGet]
        public ActionResult<IEnumerable<NewsArticle>> GetNewsArticles()
        {
            var articles = _service.GetNewsArticles();
            return Ok(articles);
        }

        // GET: api/NewsArticles/5
        [HttpGet("{id}")]
        public ActionResult<NewsArticle> GetNewsArticle(string id)
        {
            var article = _service.GetNewsArticleById(id);

            if (article == null)
                return NotFound();

            return Ok(article);
        }

        // PUT: api/NewsArticles/5
        [HttpPut("{id}")]
        public IActionResult PutNewsArticle(string id, NewsArticle newsArticle)
        {
            if (id != newsArticle.NewsArticleId)
                return BadRequest("ID mismatch");

            try
            {
                _service.UpdateNewsArticle(id, newsArticle);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        // POST: api/NewsArticles
        [HttpPost]
        public ActionResult<NewsArticle> PostNewsArticle(NewsArticle newsArticle)
        {
            try
            {
                _service.AddNewsArticle(newsArticle);
            }
            catch (Exception ex)
            {
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