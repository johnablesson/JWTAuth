using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuth.Controllers
{
    [ApiController]
    [Authorize]
    public class DataController : ControllerBase
    {
        [Route("Data/Get")]
        public string GetData()
        {
            var currentUser = HttpContext.User;
            var s = "";
            foreach (var item in currentUser.Claims)
            {
                s += item;
            }
            
            return s;
        }
    }
}