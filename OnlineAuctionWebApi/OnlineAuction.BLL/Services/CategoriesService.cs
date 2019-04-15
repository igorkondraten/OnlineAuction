using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Exceptions;
using OnlineAuction.BLL.Interfaces;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.BLL.Services
{
    /// <summary>
    /// Contains methods for managing categories.
    /// </summary>
    public class CategoriesService : ICategoriesService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Async method for getting all categories.
        /// </summary>
        /// <returns>The Task, containing collection of categories DTOs.</returns>
        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(x => Mapper.Map<Category, CategoryDTO>(x)).ToList();
        }

        /// <summary>
        /// Async method for creating category.
        /// </summary>
        /// <param name="category">The category DTO.</param>
        /// <returns>The Task, containing created category DTO.</returns>
        /// <exception cref="ArgumentNullException">Thrown if category is null.</exception>
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category is null.");
            var newCategory = Mapper.Map<CategoryDTO, Category>(category);
            _unitOfWork.Categories.Create(newCategory);
            await _unitOfWork.SaveAsync();
            return Mapper.Map<Category, CategoryDTO>(await _unitOfWork.Categories.GetAsync(newCategory.CategoryId));
        }

        /// <summary>
        /// Async method for updating category.
        /// </summary>
        /// <param name="category">The category DTO.</param>
        /// <returns>The Task.</returns>
        /// <exception cref="ArgumentNullException">Thrown if category is null.</exception>
        /// <exception cref="NotFoundException">Thrown if category not found in DB.</exception>
        public async Task EditCategoryAsync(CategoryDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category is null.");
            var oldCategory = await _unitOfWork.Categories.GetAsync(category.CategoryId);
            if (oldCategory == null)
                throw new NotFoundException("Category not found.");
            _unitOfWork.Categories.Update(Mapper.Map<CategoryDTO, Category>(category, oldCategory));
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Async method for deleting category.
        /// </summary>
        /// <param name="categoryId">The category ID.</param>
        /// <returns>The Task.</returns>
        /// <exception cref="NotFoundException">Thrown if category not found in DB.</exception>
        public async Task DeleteCategoryAsync(int categoryId)
        {
            if (await _unitOfWork.Categories.GetAsync(categoryId) == null)
                throw new NotFoundException("Category not found.");
            _unitOfWork.Categories.Delete(categoryId);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Async method for getting category by ID.
        /// </summary>
        /// <param name="categoryId">The category ID.</param>
        /// <returns>The Task, containing category DTO.</returns>
        public async Task<CategoryDTO> GetCategoryAsync(int categoryId)
        {
            return Mapper.Map<Category, CategoryDTO>(await _unitOfWork.Categories.GetAsync(categoryId));
        }

        #region IDisposable Support
        private bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
