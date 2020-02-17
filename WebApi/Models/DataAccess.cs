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
        
        public static UserDetails AddUser(UserDetails userDetails)
        {
            UserDetails user;
            using (marketContext=new MarketContext())
            {
                user=marketContext.Users.Add(userDetails);
                marketContext.SaveChanges();
            }
            return user;
        }
        public ProductDetails  AddProduct(ProductDetails productDetails)
        {
            ProductDetails productAdded ;
            using (marketContext= new MarketContext())
            {
                productAdded=marketContext.Products.Add(productDetails);
                marketContext.SaveChanges();
            }
            return productAdded;
        }
        public static bool DoesUserExists(UserLoginDetails userLoginDetails)
        {
            bool found;
            using (marketContext=new MarketContext())
            {
                var user = marketContext.Users.Where(u=>(u.Email==userLoginDetails.UserEmail)&&(u.Password==userLoginDetails.UserPassword)).FirstOrDefault();
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
            using (marketContext=new MarketContext())
            {
                var stud = product;
                marketContext.Products.Add(product);
                marketContext.SaveChanges();
            }

        }
        public static List<ProductDetails> GetAllProducts()
        {
           
            List<ProductDetails> allProducts;
            using (marketContext=new MarketContext())
            {
                allProducts = marketContext.Products.ToList<ProductDetails>();
            }
            return allProducts;
        }
        public static ProductDetails GetProduct(int id)
        {
            ProductDetails product = null;

            using (var context = new MarketContext())
            {
                product = context.Products
                    .Where(x => x.ProductId == id)
                    .FirstOrDefault();
            }
            return product;
        }
        public static List<ProductCategory> GetProductCategories()
        {
            List<ProductCategory> productCategories = null;
            using (var context=new MarketContext())
            {
                productCategories = context.ProductCategories.ToList<ProductCategory>();
            }
            if(productCategories==null|| productCategories.Count == 0)
            {
                return null;
            }
            return productCategories; 
        }
    }
}