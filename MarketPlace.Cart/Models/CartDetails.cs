﻿using MarketPlace.Cart.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;

namespace MarketPlace.Cart.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}