using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACheckAPI.Common;
using ACheckAPI.Dao;
using ACheckAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ACheckAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : BaseController
    {
        public AssetController(TWG_ACHECKContext tWG_ACHECKContext) : base(tWG_ACHECKContext) { }

        [HttpGet]
        [Route("GetAssetByID")]
        public ReturnObject GetAssetByID(string AssetID)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoAsset daoAsset = new DaoAsset(tWG_ACHECKContext);
                var result = daoAsset.GetAssetByAssetID(AssetID);
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
        [Route("AssetFilter")]
        public ReturnObject AssetFilter(string AssetID, string AssetName)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoAsset daoAsset = new DaoAsset(tWG_ACHECKContext);
                var result = daoAsset.AssetFilter(AssetID, AssetName);
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
        [Route("GetAsset")]
        public ReturnObject GetAsset()
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoAsset daoAsset = new DaoAsset(tWG_ACHECKContext);
                var result = daoAsset.GetAll();
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
        [Route("AddAsset")]

        public ReturnObject AddAsset(ModelViews.ViewAsset entity)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            using (var transaction = tWG_ACHECKContext.Database.BeginTransaction())
            {
                try
                {
                    DaoAsset daoAsset = new DaoAsset(tWG_ACHECKContext);
                    var result = daoAsset.AddAsset(entity.asset, entity.assign);
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
        [Route("Add")]
        public async Task<ReturnObject> Add()
        {
            IFormFileCollection formFiles = Request.Form.Files;
            ModelViews.ViewAsset entity = JsonConvert.DeserializeObject<ModelViews.ViewAsset>(Request.Form["entity"].ToString());
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            using (var transaction = tWG_ACHECKContext.Database.BeginTransaction())
            {
                try
                {
                    DaoAsset daoAsset = new DaoAsset(tWG_ACHECKContext);
                    int result = await daoAsset.Add(entity, formFiles);
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
        [Route("Update")]

        public async Task<ReturnObject> Update()
        {
            IFormFileCollection formFiles = Request.Form.Files;
            ModelViews.ViewAsset entity = JsonConvert.DeserializeObject<ModelViews.ViewAsset>(Request.Form["entity"].ToString());
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            using (var transaction = tWG_ACHECKContext.Database.BeginTransaction())
            {
                try
                {
                    DaoAsset daoAsset = new DaoAsset(tWG_ACHECKContext);
                    int result = await daoAsset.Update(entity, formFiles);
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


        public class Upload
        {
            public IFormFile File { get; set; }
            public string GroupImport { get; set; }
        }

        

        [HttpPost]
        [Route("UpdateAsset")]

        public ReturnObject UpdateAsset(ModelViews.ViewAsset entity)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            using (var transaction = tWG_ACHECKContext.Database.BeginTransaction())
            {
                try
                {
                    DaoAsset daoAsset = new DaoAsset(tWG_ACHECKContext);
                    var result = daoAsset.UpdateAsset(entity.asset, entity.assign);
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
        [Route("DeleteAsset")]

        public async Task<ReturnObject> DeleteAsset(string AssetId)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoAsset daoAsset = new DaoAsset(tWG_ACHECKContext);
                var result = await daoAsset.DeleteAsset(AssetId);
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