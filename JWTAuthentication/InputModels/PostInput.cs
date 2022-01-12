using JWTAuthentication.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.InputModels
{
    public class PostInput : Post
    {
        public new IFormFile File {get;set;}
    }
}
