using BusinessObjectsLayer.Entity;
using DAOsLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoriesLayer
{
    public class CategoryRepo : ICategoryRepo
    {
        public Category? GetCategoryById(short categoryId)
            => CategoryDAO.Instance.GetCategoryById(categoryId);

        public List<Category> GetCategories()
            => CategoryDAO.Instance.GetCategories();

        public List<Category> GetActiveCategories()
            => CategoryDAO.Instance.GetActiveCategories();

        public void AddCategory(Category category)
            => CategoryDAO.Instance.AddCategory(category);

        public void UpdateCategory(short categoryId, Category category)
            => CategoryDAO.Instance.UpdateCategory(categoryId, category);

        public void RemoveCategory(short categoryId)
        {
            var cat = CategoryDAO.Instance.GetCategoryById(categoryId);
            if (cat != null && cat.IsActive == true)
            {
                cat.IsActive = false;
                CategoryDAO.Instance.UpdateCategory(categoryId, cat);
            }
        }
    }
}