using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ACheckAPI.Models;
using Microsoft.AspNetCore.Mvc.Filters;
//using TWG_HR_API;
using TWG_HR_AuthenticationService;

namespace ACheckAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        //protected APIRequest apiRequest;
        protected readonly TWG_ACHECKContext tWG_ACHECKContext;
        public BaseController(TWG_ACHECKContext tWG_ACHECKContext)
        {
            this.tWG_ACHECKContext = tWG_ACHECKContext;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //apiRequest = new APIRequest(HttpContext, "");
            //string url = string.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.Path);
            //ClientInfo clientInfo = apiRequest.auThenTication.GetClientInfo();
            //string result = apiRequest.LayThongTinDangNhap();
        }
    }
}