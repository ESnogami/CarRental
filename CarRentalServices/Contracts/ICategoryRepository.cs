using CarRentalServices.Entities;

namespace CarRentalServices.Contracts
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Gets all categories from the data source.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Category> GetCategories();

        /// <summary>
        /// Gets a category by its ID from the data source.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Category? GetCategoryById(int id);

        /// <summary>
        /// Inserts a new category into the data source.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        bool InsertCategory(Category category);

        /// <summary>
        ///  Updates an existing category in the data source.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        bool UpdateCategory(Category category);

        /// <summary>
        /// Deletes a category by its ID from the data source.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteCategory(int id);
    }
}