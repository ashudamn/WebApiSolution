using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ProductContainer
    {
        public JObject ProductDetails { set; get; }
        public FileInfo ProductImage { set; get; }

    }
}