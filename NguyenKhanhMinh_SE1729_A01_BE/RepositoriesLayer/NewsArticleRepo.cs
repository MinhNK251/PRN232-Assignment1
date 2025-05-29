using BusinessObjectsLayer.Entity;
using DAOsLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoriesLayer
{
    public class NewsArticleRepo : INewsArticleRepo
    {
        public List<NewsArticle> GetNewsArticles()
            => NewsArticleDAO.Instance.GetNewsArticles();

        public List<NewsArticle> GetNewsArticlesByCreatedBy(int createdById)
            => NewsArticleDAO.Instance.GetNewsArticlesByCreatedBy(createdById);

        public NewsArticle? GetNewsArticleById(string articleId)
           => NewsArticleDAO.Instance.GetNewsArticleById(articleId);

        public List<NewsArticle> GetArticlesByTagId(int tagId)
            => NewsArticleDAO.Instance.GetArticlesByTagId(tagId);

        public List<NewsArticle> GetActiveNewsArticles()
            => NewsArticleDAO.Instance.GetActiveNewsArticles();

        public void AddNewsArticle(NewsArticle newsArticle)
            => NewsArticleDAO.Instance.AddNewsArticle(newsArticle);

        public void UpdateNewsArticle(string articleId, NewsArticle updatedArticle)
            => NewsArticleDAO.Instance.UpdateNewsArticle(articleId, updatedArticle);

        public void RemoveNewsArticle(string articleId)
        {
            var art = NewsArticleDAO.Instance.GetNewsArticleById(articleId);
            if (art != null && art.NewsStatus == true)
            {
                art.NewsStatus = false;
                NewsArticleDAO.Instance.UpdateNewsArticle(articleId, art);
            }
        }

        public void RemoveTagsByArticleId(string articleId)
            => NewsArticleDAO.Instance.RemoveTagsByArticleId(articleId);

        public List<NewsArticle> GetNewsArticlesByDateRange(DateTime startDate, DateTime endDate)
            => NewsArticleDAO.Instance.GetNewsArticlesByDateRange(startDate, endDate);
    }
}
