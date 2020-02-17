using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class FileInfo
    {
        public Byte[] File { set; get; }
        public string Filename { set; get; }
    }
}