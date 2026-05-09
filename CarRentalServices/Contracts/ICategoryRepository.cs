using CarRentalServices.Entities;

namespace CarRentalServices.Contracts
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();

        Category? GetCategoryById(int id);

        bool InsertCategory(Category category);

        bool UpdateCategory(Category category);

        bool DeleteCategory(int id);
    }
}