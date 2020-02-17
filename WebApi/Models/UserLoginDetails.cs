using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class UserLoginDetails
    {
        public string UserEmail { set; get; }
        public string UserPassword { set; get; }
    }
}