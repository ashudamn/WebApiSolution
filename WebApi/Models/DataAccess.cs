using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebApi.Models
{
    public class DataAccess
    {
        internal static MarketContext marketContext;
        internal static void InitDbContext()
        {
            marketContext=marketContext!=null?marketContext : new MarketContext();
        }
        public static void AddUser(UserDetails userDetails)
        {
            InitDbContext();
            using (marketContext)
            {
                marketContext.Users.Add(userDetails);
                marketContext.SaveChanges();
            }
        }
        public static void AddProduct(ProductDetails productDetails)
        {
            InitDbContext();
            using (marketContext)
            {
                marketContext.Products.Add(productDetails);
                marketContext.SaveChanges();
            }
        }
        public static bool DoesUserExists(UserLoginDetails userLoginDetails)
        {
            bool found;
            using (marketContext)
            {
                var user = marketContext.Users.Where(u=>
                (u.Email==userLoginDetails.Email)
                &&(u.Password==userLoginDetails.Password)
                ).FirstOrDefault();
                found = user != null ? true : false;
            }
            return found;
        }
       public static void AddProduct(MultipartFormDataStreamProvider provider)
        {
            var model = provider.FormData["product"];
            var jsonObj = JObject.Parse(model);
            var product = JsonConvert.DeserializeObject<ProductDetails>(jsonObj.ToString());
            string filename = String.Empty, filepath = String.Empty;
            foreach (MultipartFileData file in provider.FileData)
            {
                filename = file.Headers.ContentDisposition.FileName;
                filepath = file.LocalFileName;
            }
            product.ProductImageFile = File.ReadAllBytes(filepath);
            product.ProductFileName = filename;
            using (var ctx = new MarketContext())
            {
                var stud = product;
                ctx.Products.Add(product);
                ctx.SaveChanges();
            }

        }
        public static List<ProductDetails> GetAllProducts()
        {
            List<ProductDetails> allProducts;
            using (var ctx = new MarketContext())
            {
                allProducts=ctx.Products.ToList<ProductDetails>();
            }
            return allProducts;
        }
    }
}