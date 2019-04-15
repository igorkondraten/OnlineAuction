using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineAuction.BLL.DTO;

namespace OnlineAuction.BLL.Interfaces
{
    /// <summary>
    /// Interface for categories service.
    /// Contains methods for managing categories.
    /// </summary>
    public interface ICategoriesService : IDisposable
    {
        /// <summary>
        /// Async method for creating category.
        /// </summary>
        /// <param name="category">The category DTO.</param>
        /// <returns>The Task, containing created category DTO.</returns>
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);

        /// <summary>
        /// Async method for updating category.
        /// </summary>
        /// <param name="category">The category DTO.</param>
        /// <returns>The Task.</returns>
        Task EditCategoryAsync(CategoryDTO category);

        /// <summary>
        /// Async method for deleting category.
        /// </summary>
        /// <param name="categoryId">The category ID.</param>
        /// <returns>The Task.</returns>
        Task DeleteCategoryAsync(int categoryId);

        /// <summary>
        /// Async method for getting category by ID.
        /// </summary>
        /// <param name="categoryId">The category ID.</param>
        /// <returns>The Task, containing category DTO.</returns>
        Task<CategoryDTO> GetCategoryAsync(int categoryId);

        /// <summary>
        /// Async method for getting all categories.
        /// </summary>
        /// <returns>The Task, containing collection of categories DTOs.</returns>
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    }
}
