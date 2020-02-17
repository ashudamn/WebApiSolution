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
        public async Task<HttpResponseMessage> UserRegister([FromBody] string userDetail)
        {
            var task = await Task<HttpResponseMessage>.Factory.StartNew(()=> {
                //extract user details from request body
                var userData = JObject.Parse(userDetail);
                //save it to DB
                var userDetailModel = JsonConvert.DeserializeObject<UserDetails>(userData.ToString());
                DataAccess.AddUser(userDetailModel);
                return Request.CreateResponse(HttpStatusCode.OK,"User Created successfully");
            });
            return task;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<HttpResponseMessage> UserLogin([FromBody] string userLoginDetail)
        {
            var task = await Task<HttpResponseMessage>.Factory.StartNew(()=> {
                HttpResponseMessage response=null;
                if (DataAccess.DoesUserExists(JsonConvert.DeserializeObject<UserLoginDetails>(userLoginDetail)))
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
