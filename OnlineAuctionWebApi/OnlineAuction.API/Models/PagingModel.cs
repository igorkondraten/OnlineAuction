using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.API.Models
{
    /// <summary>
    /// Pagination input model for slicing data to pages.
    /// </summary>
    public class PagingModel
    {
        /// <summary>
        /// Number of items which will be returned.
        /// </summary>
        [DefaultValue(10)]
        [Range(10, 100)]
        public int Limit { get; set; }

        /// <summary>
        /// Number of items which will be skipped.
        /// </summary>
        [DefaultValue(0)]
        [Range(0, int.MaxValue)]
        public int Offset { get; set; }
    }
}