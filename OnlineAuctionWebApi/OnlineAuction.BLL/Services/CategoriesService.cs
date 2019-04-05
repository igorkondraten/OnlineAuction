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
    public class CategoriesService : ICategoriesService, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(x => Mapper.Map<Category, CategoryDTO>(x)).ToList();
        }

        public async Task CreateCategoryAsync(CategoryDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category is null.");
            _unitOfWork.Categories.Create(Mapper.Map<CategoryDTO, Category>(category));
            await _unitOfWork.SaveAsync();
        }

        public async Task EditCategoryAsync(CategoryDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category is null.");
            var oldCategory = await _unitOfWork.Categories.GetAsync(category.CategoryId);
            _unitOfWork.Categories.Update(Mapper.Map<CategoryDTO, Category>(category, oldCategory));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            if (await _unitOfWork.Categories.GetAsync(categoryId) == null)
                throw new NotFoundException("Category not found.");
            _unitOfWork.Categories.Delete(categoryId);
            await _unitOfWork.SaveAsync();
        }

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
