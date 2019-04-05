using System.Threading.Tasks;
using System.Web.Http;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Interfaces;

namespace OnlineAuction.API.Controllers
{
    [RoutePrefix("api/categories")]
    [Authorize]
    public class CategoriesController : ApiController
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [AllowAnonymous]
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCategoriesAsync()
        {
            var categories = await _categoriesService.GetAllCategoriesAsync();
            return Ok(categories);
        }

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

        [Authorize(Roles = "Admin")]
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateCategoryAsync(CategoryDTO category)
        {
            if (category == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _categoriesService.CreateCategoryAsync(category);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("{id:int:min(1)}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditCategoryAsync(int id, CategoryDTO category)
        {
            if (category == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _categoriesService.EditCategoryAsync(category);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("{id:int:min(1)}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCategoryAsync(int id)
        {
            await _categoriesService.DeleteCategoryAsync(id);
            return Ok();
        }
    }
}
