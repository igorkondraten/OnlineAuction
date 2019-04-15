using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OnlineAuction.API.Models;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Interfaces;

namespace OnlineAuction.API.Controllers
{
    /// <summary>
    /// Lots controller.
    /// </summary>
    [RoutePrefix("api")]
    public class LotsController : ApiController
    {
        private readonly ILotsService _lotsService;

        public LotsController(ILotsService lotsService)
        {
            _lotsService = lotsService;
        }

        /// <summary>
        /// Get all lots or search by keywords with pagination.
        /// </summary>
        /// <param name="model">Pagination model.</param>
        /// <param name="query">Optional lot full name or it's part.</param>
        /// <returns>200 - at least 1 lot found; 204 - no lots found.</returns>
        [AllowAnonymous]
        [Route("lots")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLotsAsync([FromUri] PagingModel model, string query = null)
        {
            if (model == null || !ModelState.IsValid)
                model = new PagingModel() { Limit = 10, Offset = 0 };
            var (lots, totalCount) = query == null
                ? await _lotsService.GetAllLotsAsync(model.Limit, model.Offset)
                : await _lotsService.FindLotsAsync(query, model.Limit, model.Offset);
            if (!lots.Any())
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            return Ok(new { lots, totalCount });
        }

        /// <summary>
        /// Get lots by user profile ID with pagination.
        /// </summary>
        /// <param name="userProfileId">USer profile ID.</param>
        /// <param name="model">Pagination model.</param>
        /// <returns>200 - at least 1 lot found; 204 - no lots found.</returns>
        [Route("users/{userProfileId:int:min(1)}/lots")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLotsByUserAsync(int userProfileId, [FromUri] PagingModel model)
        {
            if (model == null || !ModelState.IsValid)
                model = new PagingModel() { Limit = 10, Offset = 0 };
            var (lots, totalCount) = await _lotsService.GetLotsByUserAsync(userProfileId, model.Limit, model.Offset);
            if (!lots.Any())
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            return Ok(new { lots, totalCount });
        }

        /// <summary>
        /// Get lot by ID.
        /// </summary>
        /// <param name="id">Lot ID.</param>
        /// <returns>200 - lot found; 404 - lot not found.</returns>
        [AllowAnonymous]
        [Route("lots/{id:int:min(1)}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLotAsync(int id)
        {
            var lot = await _lotsService.GetLotAsync(id);
            if (lot == null)
                return NotFound();
            return Ok(lot);
        }

        /// <summary>
        /// Create new lot.
        /// </summary>
        /// <param name="lot">New lot.</param>
        /// <returns>201 - lot created; 400 - lot validation failed.</returns>
        [Authorize(Roles = "Admin, Seller")]
        [Route("lots")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateLotAsync(LotDTO lot)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            lot.UserName = User.Identity.Name;
            var createdLot = await _lotsService.CreateLotAsync(lot);
            return Created(Request.RequestUri + "/" + createdLot?.LotId, createdLot);
        }
        
        /// <summary>
        /// Update lot.
        /// </summary>
        /// <param name="id">Lot ID.</param>
        /// <param name="lot">Lot.</param>
        /// <returns>200 - lot updated; 400 - lot validation failed; 404 - lot not found; 403 - authorization failed.</returns>
        [Authorize(Roles = "Admin, Seller")]
        [Route("lots/{id:int:min(1)}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditLotAsync(int id, LotDTO lot)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var oldLot = await _lotsService.GetLotAsync(id);
            if (oldLot == null)
                return NotFound();
            // Not admin user can edit only his own lot.
            if (!User.IsInRole("Admin") && oldLot.UserName != User.Identity.Name)
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Forbidden));
            lot.LotId = id;
            await _lotsService.EditLotAsync(lot);
            return Ok();
        }

        /// <summary>
        /// Delete lot.
        /// </summary>
        /// <param name="id">Lot ID.</param>
        /// <returns>404 - lot not found; 204 - lot deleted.</returns>
        [Authorize(Roles = "Admin")]
        [Route("lots/{id:int:min(1)}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteLotAsync(int id)
        {
            await _lotsService.DeleteLotAsync(id);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }
    }
}
