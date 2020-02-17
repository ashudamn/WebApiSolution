using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Product")]
    public class ProductController : ApiController
    {
        [HttpPost]
        [Route("Register")]
        public async Task<HttpResponseMessage> PostProduct()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            // Read the form data and return an async task.
            var task = await Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }
                    DataAccess.AddProduct(provider);
                    return Request.CreateResponse(HttpStatusCode.OK);
                });
            return task;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<HttpResponseMessage> GetProducts()
        {
            var task = await Task<HttpResponseMessage>.Factory.StartNew(() => {
            //var jsonR = Json(DataAccess.GetAllProducts());
                return Request.CreateResponse(HttpStatusCode.OK, DataAccess.GetAllProducts());
            });
            var productList = DataAccess.GetAllProducts();
            //var jsonResponse = Json(productList);
            return task;
        }
        [HttpGet]
        [Route("GetProductDetailsById/{id}")]
        public IHttpActionResult GetProductDetailsById(int id)
        {
            ProductDetails product = DataAccess.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        [Route("GetCategories")]
        public async Task<HttpResponseMessage> GetCategories()
        {
            var task = await Task<HttpResponseMessage>.Factory.StartNew(() =>
              {
                  HttpResponseMessage httpResponse;
                  List<ProductCategory> categories = DataAccess.GetProductCategories();
                  if (categories == null)
                  {
                      httpResponse = Request.CreateResponse(HttpStatusCode.NotFound,"Categories Not found");
                  }
                  else
                  {
                      httpResponse = Request.CreateResponse(HttpStatusCode.OK,categories);
                  }
                  return httpResponse;
              });
            return task;
        }

    }
}
