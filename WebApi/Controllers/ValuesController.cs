using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Models;

namespace WebApi.Controllers
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]

    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public async Task<HttpResponseMessage> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            
            // Read the form data and return an async task.
            var task =await Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }

                    // This illustrates how to get the file names.
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                        Trace.WriteLine("Server file path: " + file.LocalFileName);
                    }

                    //getting file name and saving it to db
                    DetailsSavedToDb(provider);
                    
                    
                    return Request.CreateResponse(HttpStatusCode.OK);
                });
            
            
            return task;
        }

        private void DetailsSavedToDb(MultipartFormDataStreamProvider provider)
        {
            ProductContainer productContainer = new ProductContainer();
            productContainer.ProductImage = new Models.FileInfo();
            string filename=String.Empty, filepath=String.Empty;
            foreach (MultipartFileData file in provider.FileData)
            {
                filename = file.Headers.ContentDisposition.FileName;
                filepath = file.LocalFileName;
            }
            var model = provider.FormData["product"];
            var jsonObj = JObject.Parse(model);
            var product = JsonConvert.DeserializeObject<ProductDetails>(jsonObj.ToString());
            product.ProductImageFile = File.ReadAllBytes(filepath);
            product.ProductFileName = filename;
            using (var ctx = new MarketContext())
            {
                var stud = product;
                ctx.Products.Add(product);
                ctx.SaveChanges();
            }
           
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
