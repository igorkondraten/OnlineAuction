using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineAuction.BLL.DTO;

namespace OnlineAuction.BLL.Interfaces
{
    public interface ICategoriesService : IDisposable
    {
        Task CreateCategoryAsync(CategoryDTO category);
        Task EditCategoryAsync(CategoryDTO category);
        Task DeleteCategoryAsync(int categoryId);
        Task<CategoryDTO> GetCategoryAsync(int categoryId);
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    }
}
