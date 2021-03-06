﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACheckAPI.Common;
using ACheckAPI.Dao;
using ACheckAPI.Models;
using ACheckAPI.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ACheckAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/")]
    [ApiController]
    public class CategoryController : BaseController
    {
        public CategoryController(TWG_ACHECKContext tWG_ACHECKContext) : base(tWG_ACHECKContext) { }
        
        [HttpGet]
        [Route("GetAll")]
        public ReturnObject GetAll()
        {
            ReturnObject obj = new ReturnObject();
            try
            {
                DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
                var result = daoCategory.GetCategories();
                obj.status = 1;
                obj.value = result;
            }
            catch(Exception ex)
            {
                obj.status = -1;
                obj.message = ex.StackTrace;
            }
            return obj;
        }

        [HttpGet]
        [Route("Get")]
        public ReturnObject Get()
        {
            ReturnObject obj = new ReturnObject();
            try
            {
                DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
                var result = daoCategory.Get();
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
        [Route("GetCategoriesByKeyWord")]
        public ReturnObject GetCategoriesByKeyWord(string keyWord)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                object result = new object();
                DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
                if (!string.IsNullOrEmpty(keyWord))
                {
                    result = daoCategory.GetCategoriesByKeyWord(keyWord);
                }
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
        [Route("GetCategoryByID")]
        public ReturnObject GetCategoryByID(string CategoryId)
        {
            ReturnObject obj = new ReturnObject();
            try
            {
                DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
                var lsCategory = daoCategory.GetCategoriesByID(CategoryId);
                obj.status = 1;
                obj.value = lsCategory;
            }
            catch(Exception ex)
            {
                obj.status = -1;
                obj.message = ex.StackTrace;
            }
            return obj;
        }

        [HttpGet]
        [Route("GetCategoryByGroupID")]
        public ReturnObject GetCategoryByGroupID(string GroupId)
        {
            ReturnObject obj = new ReturnObject();
            try
            {
                DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
                var lsCategory = daoCategory.GetCategoriesByGroupID(GroupId);
                obj.status = 1;
                obj.value = lsCategory;
            }
            catch (Exception ex)
            {
                obj.status = -1;
                obj.message = ex.StackTrace;
            }
            return obj;
        }

        //[HttpPost]
        //[Route("AddCategory")]

        //public ReturnObject AddCategory(Category entity)
        //{
        //    ReturnObject obj = new ReturnObject();
        //    obj.status = -1;
        //    try
        //    {
        //        DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
        //        var result = daoCategory.AddCategory(entity);
        //        if (result > 0)
        //        {
        //            obj.status = 1;
        //        }
        //        obj.value = result;
        //    }
        //    catch (Exception ex)
        //    {
        //        obj.status = -1;
        //        obj.message = ex.StackTrace;
        //    }
        //    return obj;
        //}

        [HttpPost]
        [Route("Add")]
        public async Task<ReturnObject> Add(ViewAddCategory entity)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
                var result = await daoCategory.Add(entity);
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

        [HttpPost]
        [Route("Update")]
        public async Task<ReturnObject> Update(ViewAddCategory entity)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
                var result = await daoCategory.Update(entity);
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

        //[HttpPost]
        //[Route("UpdateCategory")]

        //public ReturnObject UpdateCategory(Category entity)
        //{
        //    ReturnObject obj = new ReturnObject();
        //    obj.status = -1;
        //    try
        //    {
        //        DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
        //        var result = daoCategory.UpdateCategory(entity);
        //        if (result > 0)
        //        {
        //            obj.status = 1;
        //        }
        //        obj.value = result;
        //    }
        //    catch (Exception ex)
        //    {
        //        obj.status = -1;
        //        obj.message = ex.StackTrace;
        //    }
        //    return obj;
        //}


        [HttpPost]
        [Route("DeleteCategory")]

        public async Task<ReturnObject> UpdateCategory(string CategoryId)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
                int result = await daoCategory.DeleteCategory(CategoryId);
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
        

        [HttpPost]
        [Route("ArrangeCategory")]

        public async Task<ReturnObject> ArrangeCategory(List<ModelViews.ViewArrangeCategory> lsentity)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
                int result = await daoCategory.ArrangeCategory(lsentity);
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

        [HttpPost]
        [Route("CheckCodeCategory")]

        public ReturnObject CheckCategoryCode(string CategoryCode)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                DaoCategory daoCategory = new DaoCategory(tWG_ACHECKContext);
                var result = daoCategory.CheckUniqueCategoryCode(CategoryCode, "");
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
    }
}