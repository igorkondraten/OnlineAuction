﻿using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.BLL.DTO
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}