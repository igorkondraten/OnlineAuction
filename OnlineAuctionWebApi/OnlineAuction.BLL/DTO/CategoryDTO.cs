using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.BLL.DTO
{
    /// <summary>
    /// Category data transfer object which contains information about the category.
    /// </summary>
    public class CategoryDTO
    {
        /// <summary>
        /// Id of the category.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Name of the category.
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
