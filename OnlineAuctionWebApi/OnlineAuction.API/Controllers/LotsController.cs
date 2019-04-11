using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using OnlineAuction.API.Models;
using OnlineAuction.BLL.DTO;
using OnlineAuction.BLL.Interfaces;

namespace OnlineAuction.API.Controllers
{
    [RoutePrefix("api")]
    public class LotsController : ApiController
    {
        private readonly ILotsService _lotsService;

        public LotsController(ILotsService lotsService)
        {
            _lotsService = lotsService;
        }

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

        [Route("users/{userId:int:min(1)}/lots")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLotsByUserAsync(int userId, [FromUri] PagingModel model)
        {
            if (model == null || !ModelState.IsValid)
                model = new PagingModel() { Limit = 10, Offset = 0 };
            var (lots, totalCount) = await _lotsService.GetLotsByUserAsync(userId, model.Limit, model.Offset);
            if (!lots.Any())
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
            return Ok(new { lots, totalCount });
        }

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

        [Authorize(Roles = "Admin, Seller")]
        [Route("lots")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateLotAsync(LotDTO lot)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (lot.Image != null)
            {
                var path = WriteImageToFile(lot.Image);
                lot.ImageUrl = path;
            }
            lot.UserName = User.Identity.Name;
            var createdLot = await _lotsService.CreateLotAsync(lot);
            return Created(Request.RequestUri + "/" + createdLot?.LotId, createdLot);
        }
        
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
            if (lot.Image != null)
            {
                try
                {
                    lot.ImageUrl = WriteImageToFile(lot.Image);
                }
                catch (ExternalException e)
                {
                    return InternalServerError(e);
                }
            }
            lot.LotId = id;
            await _lotsService.EditLotAsync(lot);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("lots/{id:int:min(1)}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteLotAsync(int id)
        {
            await _lotsService.DeleteLotAsync(id);
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        private static string WriteImageToFile(byte[] arr)
        {
            var filename = $"Images/{Guid.NewGuid()}.";
            using (var img = Image.FromStream(new MemoryStream(arr)))
            {
                ImageFormat format;
                if (ImageFormat.Png.Equals(img.RawFormat))
                {
                    filename += "png";
                    format = ImageFormat.Png;
                }
                else if (ImageFormat.Gif.Equals(img.RawFormat))
                {
                    filename += "gif";
                    format = ImageFormat.Gif;
                }
                else if (ImageFormat.Jpeg.Equals(img.RawFormat))
                {
                    filename += "jpg";
                    format = ImageFormat.Jpeg;
                }
                else
                {
                    throw new ArgumentException("Invalid image format.");
                }
                var path = HttpContext.Current.Server.MapPath("~/" + filename);
                img.Save(path, format);
            }
            return $"/{filename}";
        }
    }
}
