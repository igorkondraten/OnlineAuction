using System.Collections.Generic;

namespace OnlineAuction.DAL.Entities
{
    /// <summary>
    /// Category entity.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Id of the category.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Name of the category.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Lots of the category.
        /// </summary>
        public virtual ICollection<Lot> Lots { get; set; }
    }
}
