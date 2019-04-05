using System.Collections.Generic;

namespace OnlineAuction.DAL.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<Lot> Lots { get; set; }
    }
}
