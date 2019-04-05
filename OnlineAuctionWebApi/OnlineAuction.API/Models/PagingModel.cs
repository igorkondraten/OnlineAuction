using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.API.Models
{
    public class PagingModel
    {
        [DefaultValue(10)]
        [Range(10, 100)]
        public int Limit { get; set; }
        [DefaultValue(0)]
        [Range(0, int.MaxValue)]
        public int Offset { get; set; }
    }
}