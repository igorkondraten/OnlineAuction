using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Interfaces;

namespace OnlineAuction.API.Controllers
{
    /// <summary>
    /// Categories controller.
    /// </summary>
    [RoutePrefix("api/categories")]
    [Authorize]
    public class CategoriesController : ApiController
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns>200 - at least 1 category found; 204 - no categories found.</returns>
        [AllowAnonymous]
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCategoriesAsync()
        {
            var categories = await _categoriesService.GetAllCategoriesAsync();
            if (!categories.Any())
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            return Ok(categories);
        }

        /// <summary>
        /// Get category by id.
        /// </summary>
        /// <param name="id">Category ID.</param>
        /// <returns>200 - category found; 404 - category not found.</returns>
        [AllowAnonymous]
        [Route("{id:int:min(1)}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCategoryAsync(int id)
        {
            var category = await _categoriesService.GetCategoryAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        /// <summary>
        /// Create category.
        /// </summary>
        /// <param name="category">New category.</param>
        /// <returns>400 - validation failed, 201 - category created.</returns>
        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateCategoryAsync(CategoryDTO category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdCategory = await _categoriesService.CreateCategoryAsync(category);
            return Created(Request.RequestUri + "/" + createdCategory?.CategoryId, createdCategory);
        }

        /// <summary>
        /// Edit category.
        /// </summary>
        /// <param name="id">Category ID.</param>
        /// <param name="category">Category.</param>
        /// <returns>400 - validation failed; 200 - category updated; 404 - category not found.</returns>
        [Authorize(Roles = "Admin")]
        [Route("{id:int:min(1)}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditCategoryAsync(int id, CategoryDTO category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _categoriesService.EditCategoryAsync(category);
            return Ok();
        }

        /// <summary>
        /// Delete category by ID.
        /// </summary>
        /// <param name="id">Category ID.</param>
        /// <returns>404 - category not found; 204 - category deleted.</returns>
        [Authorize(Roles = "Admin")]
        [Route("{id:int:min(1)}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCategoryAsync(int id)
        {
            await _categoriesService.DeleteCategoryAsync(id);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }
    }
}
