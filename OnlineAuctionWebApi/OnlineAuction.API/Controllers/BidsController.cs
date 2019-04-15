using System;
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
    /// Bids controller.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class BidsController : ApiController
    {
        private readonly IBidsService _bidsService;

        public BidsController(IBidsService bidsService)
        {
            _bidsService = bidsService;
        }

        /// <summary>
        /// Get bids by lot ID.
        /// </summary>
        /// <param name="lotId">The lot ID.</param>
        /// <returns>200 - at least 1 bid found; 204 - no bids found; 404 - lot not found.</returns>
        [AllowAnonymous]
        [Route("lots/{lotId:int:min(1)}/bids")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBidsByLotAsync(int lotId)
        {
            var bids = await _bidsService.GetBidsByLotAsync(lotId);
            if (!bids.Any())
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            return Ok(bids);
        }

        /// <summary>
        /// Get bids by user profile ID.
        /// </summary>
        /// <param name="userProfileId">User profile ID.</param>
        /// <returns>200 - at least 1 bid found; 204 - no bids found; 404 - user not found.</returns>
        [Authorize(Roles = "Admin")]
        [Route("users/{userProfileId:int:min(1)}/bids")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBidsAsync(int userProfileId)
        {
            var bids = await _bidsService.GetBidsByUserAsync(userProfileId);
            if (!bids.Any())
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            return Ok(bids);
        }

        /// <summary>
        /// Get bid by ID.
        /// </summary>
        /// <param name="id">Bid ID.</param>
        /// <returns>200 - bid found; 404 - bid not found.</returns>
        [AllowAnonymous]
        [Route("bids/{id:int:min(1)}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBidAsync(int id)
        {
            var bid = await _bidsService.GetBidAsync(id);
            if (bid == null)
                return NotFound();
            return Ok(bid);
        }

        /// <summary>
        /// Create new bid on lot.
        /// </summary>
        /// <param name="lotId">Lot ID.</param>
        /// <param name="bid">New bid.</param>
        /// <returns>400 - validation failed; 201 - bid created.</returns>
        [Route("lots/{lotId:int:min(1)}/bids")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateBidAsync(int lotId, BidDTO bid)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            bid.Price = Math.Round(bid.Price, 2);
            bid.PlacedUserName = User.Identity.Name;
            bid.LotId = lotId;
            var createdBid = await _bidsService.CreateBidAsync(bid);
            return Created(Request.RequestUri + "/" + createdBid?.LotId, createdBid);
        }

        /// <summary>
        /// Delete bid by ID.
        /// </summary>
        /// <param name="id">Bid ID.</param>
        /// <returns>404 - bid not found; 204 - bid deleted.</returns>
        [Authorize(Roles = "Admin")]
        [Route("bids/{id:int:min(1)}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteBidAsync(int id)
        {
            await _bidsService.DeleteBidAsync(id);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }
    }
}
