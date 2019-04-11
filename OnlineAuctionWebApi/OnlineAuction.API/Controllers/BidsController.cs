using System;
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
    [RoutePrefix("api")]
    [Authorize]
    public class BidsController : ApiController
    {
        private readonly IBidsService _bidsService;

        public BidsController(IBidsService bidsService)
        {
            _bidsService = bidsService;
        }

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

        [Authorize(Roles = "Admin")]
        [Route("users/{userId:int:min(1)}/bids")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBidsAsync(int userId)
        {
            var bids = await _bidsService.GetBidsByUserAsync(userId);
            if (!bids.Any())
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            return Ok(bids);
        }

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
