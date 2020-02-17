using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ProductDetails
    {
        [Key]
        public int ProductId { set; get; }
        public string ProductCategory { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public bool AcceptTerms { get; set; }
        public Byte[] ProductImageFile { get; set; }
        public string ProductFileName { get; set; }
    }
}