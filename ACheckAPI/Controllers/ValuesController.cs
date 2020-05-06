using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ACheckAPI.Models;
using ACheckAPI.Dao;

namespace ACheckAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseController
    {
        public ValuesController(TWG_ACHECKContext tWG_ACHECKContext) : base(tWG_ACHECKContext) { }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            DaoAsset daoAsset = new DaoAsset(tWG_ACHECKContext);
            var a =daoAsset.GetAll();
            JsonResult jsonResult;
            jsonResult = Json(a);
            return jsonResult;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
