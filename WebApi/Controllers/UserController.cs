using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Models;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("Register")]
        public async Task<HttpResponseMessage> UserRegister(JObject userDetail)
        {
            var task = await Task<HttpResponseMessage>.Factory.StartNew(()=> {
                var userDetailModel = JsonConvert.DeserializeObject<UserDetails>(userDetail.ToString());
                HttpResponseMessage httpResponse;
                var user= DataAccess.AddUser(userDetailModel);
                httpResponse =user!=null? Request.CreateResponse(HttpStatusCode.Created, "User Created successfully"): Request.CreateResponse(HttpStatusCode.InternalServerError, "Something went wrong"); ;
                return httpResponse;
            });
            return task;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<HttpResponseMessage> UserLogin(JObject userDetailsJson)
        {
            var task = await Task<HttpResponseMessage>.Factory.StartNew(()=> {
                HttpResponseMessage response=null;
                UserLoginDetails userDetails = JsonConvert.DeserializeObject<UserLoginDetails>(userDetailsJson.ToString());
                if (DataAccess.DoesUserExists(userDetails))
                {
                    response= Request.CreateResponse(HttpStatusCode.OK, "User Login successfully");
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound, "User Not Found");
                }
                return response;
            });
            return task;
        }
    }
}
