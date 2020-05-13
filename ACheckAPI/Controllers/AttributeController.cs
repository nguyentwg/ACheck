using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ACheckAPI.Models;
using ACheckAPI.Common;
using ACheckAPI.Dao;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ACheckAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : BaseController
    {
        public AttributeController(TWG_ACHECKContext tWG_ACHECKContext) : base(tWG_ACHECKContext) { }

        [HttpGet]
        [Route("GetAttribute")]
        public ReturnObject GetAttribute()
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoEAVAttribute daoEAVAttribute = new DaoEAVAttribute(tWG_ACHECKContext);
                var result = daoEAVAttribute.GetAll();
                obj.status = 1;
                obj.value = result;
            }
            catch (Exception ex)
            {
                obj.status = -1;
                obj.message = ex.StackTrace;
            }
            return obj;
        }

        [HttpGet]
        [Route("GetAttributeByGroup")]
        public ReturnObject GetAttributeByGroup(string AttributeByGroup)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoEAVAttribute daoEAVAttribute = new DaoEAVAttribute(tWG_ACHECKContext);
                var result = daoEAVAttribute.GetAttributeByGroup(AttributeByGroup);
                obj.status = 1;
                obj.value = result;
            }
            catch (Exception ex)
            {
                obj.status = -1;
                obj.message = ex.StackTrace;
            }
            return obj;
        }

        [HttpPost]
        [Route("AddAttribute")]
        public async Task<ReturnObject> AddAttribute(EavAttribute entity)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            using (var transaction = tWG_ACHECKContext.Database.BeginTransaction())
            {
                try
                {
                    DaoEAVAttribute daoEAVAttribute = new DaoEAVAttribute(tWG_ACHECKContext);
                    int result = await daoEAVAttribute.AddEavAttribute(entity);
                    if (result > 0)
                    {
                        obj.status = 1;
                    }
                    obj.value = result;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    obj.status = -1;
                    obj.message = ex.StackTrace;
                }
            }
            return obj;
        }

        [HttpPost]
        [Route("UpdateAttribute")]
        public async Task<ReturnObject> UpdateAttribute(EavAttribute entity)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            using (var transaction = tWG_ACHECKContext.Database.BeginTransaction())
            {
                try
                {
                    DaoEAVAttribute daoEAVAttribute = new DaoEAVAttribute(tWG_ACHECKContext);
                    int result = await daoEAVAttribute.UpdateEavAttribute(entity);
                    if (result > 0)
                    {
                        obj.status = 1;
                    }
                    obj.value = result;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    obj.status = -1;
                    obj.message = ex.StackTrace;
                }
            }
            return obj;
        }

        [HttpPost]
        [Route("DeleteAttribute")]
        public async Task<ReturnObject> DeleteAttribute(string id)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            using (var transaction = tWG_ACHECKContext.Database.BeginTransaction())
            {
                try
                {
                    DaoEAVAttribute daoEAVAttribute = new DaoEAVAttribute(tWG_ACHECKContext);
                    int result = await daoEAVAttribute.DeleteEavAttribute(id);
                    if (result > 0)
                    {
                        obj.status = 1;
                    }
                    obj.value = result;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    obj.status = -1;
                    obj.message = ex.StackTrace;
                }
            }
            return obj;
        }

    }
}
