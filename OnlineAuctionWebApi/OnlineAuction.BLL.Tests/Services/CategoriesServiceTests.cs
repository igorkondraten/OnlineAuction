using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Exceptions;
using OnlineAuction.BLL.Interfaces;
using OnlineAuction.BLL.Services;
using OnlineAuction.DAL.Entities;
using OnlineAuction.DAL.Interfaces;

namespace OnlineAuction.BLL.Tests.Services
{
    [TestFixture]
    public class CategoriesServiceTests
    {
        private ICategoriesService _service;
        private Mock<IUnitOfWork> _mockUnitWork;
        private List<Category> _categories;

        [OneTimeSetUp]
        public void Init()
        {
            _mockUnitWork = new Mock<IUnitOfWork>();
            _service = new CategoriesService(_mockUnitWork.Object);
            _categories = new List<Category>()
            {
                new Category() { CategoryId = 1, Name = "Category 1" },
                new Category() { CategoryId = 2, Name = "Category 3" },
                new Category() { CategoryId = 3, Name = "Category 3" }
            };
        }

        [Test]
        public async Task GetAllCategoriesAsync_GetAllCategories_3CategoriesReturned()
        {
            _mockUnitWork.Setup(x => x.Categories.GetAllAsync()).ReturnsAsync(_categories);

            var results = await _service.GetAllCategoriesAsync();

            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task CreateCategoryAsync_CreateNewCategory_CreatedCategoryReturned()
        {
            var newCategory = new CategoryDTO() { CategoryId = 4, Name = "New" };
            _mockUnitWork.Setup(x => x.Categories.Create(It.IsAny<Category>()));
            _mockUnitWork.Setup(x => x.Categories.GetAsync(newCategory.CategoryId))
                .ReturnsAsync(new Category() {CategoryId = 4, Name = "New"});

            var result = await _service.CreateCategoryAsync(newCategory);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.CategoryId, Is.EqualTo(newCategory.CategoryId));
            _mockUnitWork.Verify(x => x.Categories.Create(It.Is<Category>(y => y.CategoryId == newCategory.CategoryId)), Times.Once);
        }

        [Test]
        public async Task EditCategoryAsync_EditExistingCategory_CategoryUpdated()
        {
            var newCategory = new CategoryDTO() { CategoryId = 4, Name = "New" };
            var oldCategory = new CategoryDTO() { CategoryId = 4, Name = "Old" };
            _mockUnitWork.Setup(x => x.Categories.GetAsync(oldCategory.CategoryId))
                .ReturnsAsync(new Category() {CategoryId = 4, Name = "Old"});
            _mockUnitWork.Setup(x => x.Categories.Update(It.IsAny<Category>()));

            await _service.EditCategoryAsync(newCategory);

            _mockUnitWork.Verify(x => x.Categories.Update(It.Is<Category>(y => y.Name == newCategory.Name)));
        }

        [Test]
        public async Task EditCategoryAsync_EditNotExistingCategory_NotFoundExceptionThrown()
        {
            var newCategory = new CategoryDTO() { CategoryId = 10, Name = "New" };
            var oldCategory = new CategoryDTO() { CategoryId = 4, Name = "Old" };
            _mockUnitWork.Setup(x => x.Categories.GetAsync(oldCategory.CategoryId)).ReturnsAsync((Category) null);
            _mockUnitWork.Setup(x => x.Categories.Update(It.IsAny<Category>()));

            Assert.ThrowsAsync<NotFoundException>(() => _service.EditCategoryAsync(newCategory), "Category not found.");
        }

        [Test]
        public async Task DeleteCategoryAsync_DeleteExistingCategory_CategoryDeleted()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.Categories.GetAsync(id)).ReturnsAsync(_categories.Find(x => x.CategoryId == id));
            _mockUnitWork.Setup(x => x.Categories.Delete(id));

            await _service.DeleteCategoryAsync(id);

            _mockUnitWork.Verify(x => x.Categories.Delete(id), Times.Once);
        }

        [Test]
        public async Task DeleteCategoryAsync_DeleteNotExistingCategory_NotFoundExceptionThrown()
        {
            var id = 10;
            _mockUnitWork.Setup(x => x.Categories.GetAsync(id)).ReturnsAsync(_categories.Find(x => x.CategoryId == id));
            _mockUnitWork.Setup(x => x.Categories.Delete(id));

            Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteCategoryAsync(id));
        }

        [Test]
        public async Task GetCategoryAsync_GetCategoryWithId1_CategoryReturned()
        {
            var id = 1;
            _mockUnitWork.Setup(x => x.Categories.GetAsync(id)).ReturnsAsync(_categories[0]);

            var result = await _service.GetCategoryAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.CategoryId, Is.EqualTo(id));
        }
    }
}