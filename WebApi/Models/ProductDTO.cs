using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ProductDTO
    {
        public int ProductId { set; get; }
        public string ProductCategory { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductModelName { get; set; }
        public bool AcceptTerms { get; set; }
    }
}