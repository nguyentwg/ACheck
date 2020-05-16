using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ACheckAPI.Models;
using Microsoft.AspNetCore.Authorization;
using ACheckAPI.Common;
using ACheckAPI.Dao;

namespace ACheckAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : BaseController
    {
        public ImageController(TWG_ACHECKContext tWG_ACHECKContext) : base(tWG_ACHECKContext) { }

        [HttpPost]
        [Route("DeleteImage")]
        public async Task<ReturnObject> DeleteImage(string ImageID)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoImage daoImage = new DaoImage(tWG_ACHECKContext);
                var result = await daoImage.DeleteImage(ImageID);
                if (result > 0)
                {
                    obj.status = 1;
                }
                obj.value = result;
            }
            catch (Exception ex)
            {
                obj.status = -1;
                obj.message = ex.StackTrace;
            }
            return obj;
        }
    }
}