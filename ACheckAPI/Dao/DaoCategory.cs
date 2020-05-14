using ACheckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using shortid;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ACheckAPI.ModelViews;
using ACheckAPI.Common;

namespace ACheckAPI.Dao
{
    public class DaoCategory : DaoBase
    {
        public DaoCategory(TWG_ACHECKContext _context) : base(_context) { }

        public List<Category> GetCategories()
        {
            return context.Category.AsNoTracking().Where(p => p.Active == true).AsEnumerable().ToList();
        }

        public List<Category> Get()
        {
            return context.Category.AsNoTracking().Where(p => p.Active == true).Include(p=>p.EavAttributeValue).ThenInclude(x=>x.Eav).AsEnumerable().ToList();
        }

        public ViewCategory GetCategoriesByID(string CategoryId)
        {
            DaoAsset daoAsset = new DaoAsset(context);
            ViewCategory result = new ViewCategory();
            result.lsSubCategory = context.Category.AsNoTracking().Where(p => p.Active == true && p.ParentId.Equals(CategoryId))
                                                   .Include(p => p.EavAttributeValue).ThenInclude(x => x.Eav).AsEnumerable().ToList();

            result.lsAsset = daoAsset.GetAssetByCategoryId(CategoryId);
            result.countAsset = result.lsAsset.Count();
            result.countSubCategory = result.lsSubCategory.Count();
            return result;
        }
        public ViewCategory GetCategoriesByGroupID(string GroupId)
        {
            DaoAsset daoAsset = new DaoAsset(context);
            ViewCategory result = new ViewCategory();
            result.lsSubCategory = context.Category.AsNoTracking().Where(p => p.Active == true && p.CategoryType.Equals(GroupId))
                                                   .Include(p => p.EavAttributeValue).ThenInclude(x => x.Eav).AsEnumerable().ToList();
            
            result.countSubCategory = result.lsSubCategory.Count();
            return result;
        }

        public async Task<int> Add(ViewAddCategory entity)
        {
            Category category = entity.Category;
            List<EavAttributeValue> lsAttributeValue = entity.EavAttributeValue;
            DaoEAVAttribute daoEAVAttribute = new DaoEAVAttribute(context);
            category.CategoryId = this.GenerateCategoryID();
            bool checkCode = this.CheckUniqueCategoryCode(category.Code, category.CategoryId);
            bool checkID = this.CheckUniqueCategoryID(category.CategoryId);
            if (checkCode && checkID)
            {
                category.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                category.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Category.Add(category);
                foreach (EavAttributeValue item in lsAttributeValue)
                {
                    var eavGroup = context.EavAttribute.Where(p => p.Guid.Equals(item.EavId)).Select(p=>p.AttributeGroup).FirstOrDefault();
                    item.Guid = Guid.NewGuid().ToString().ToUpper();
                    item.CategoryId = category.CategoryId;
                    item.AttributeGroup = eavGroup;
                    context.EavAttributeValue.Add(item);
                }
            }
            return await context.SaveChangesAsync();
        }

        public async Task<int> Update(ViewAddCategory entity)
        {
            Category category = entity.Category;
            List<EavAttributeValue> lsAttributeValue = entity.EavAttributeValue;
            DaoEAVAttribute daoEAVAttribute = new DaoEAVAttribute(context);
            bool checkCode = this.CheckUniqueCategoryCode(category.Code, category.CategoryId);
            Category categoryUpdate = context.Category.Where(p => p.CategoryId.Equals(category.CategoryId)).FirstOrDefault();
            category.CopyPropertiesTo<Category>(categoryUpdate);
            if (checkCode)
            {
                categoryUpdate.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Category.Update(categoryUpdate);
                foreach (EavAttributeValue item in lsAttributeValue)
                {
                    if (string.IsNullOrEmpty(item.Guid))
                    {
                        item.Guid = Guid.NewGuid().ToString().ToUpper();
                        item.CategoryId = category.CategoryId;
                        item.AttributeGroup = category.CategoryType;
                        context.EavAttributeValue.Add(item);
                    }
                    else
                    {
                        var entityDB = context.EavAttributeValue.Where(p => p.Guid.Equals(item.Guid)).FirstOrDefault();
                        if(entityDB != null)
                        {
                            item.CopyPropertiesTo<EavAttributeValue>(entityDB);
                            entityDB.AttributeGroup = category.CategoryType;
                            context.EavAttributeValue.Update(entityDB);
                        }
                    }
                }
            }
            return await context.SaveChangesAsync();
        }

        public int AddCategory(Category category)
        {
            category.CategoryId = this.GenerateCategoryID();
            bool checkCode = this.CheckUniqueCategoryCode(category.Code, category.CategoryId);
            bool checkID = this.CheckUniqueCategoryID(category.CategoryId);
            if(checkCode && checkID)
            {
                category.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                category.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Category.Add(category);
            }
            return context.SaveChanges();
        }


        public int UpdateCategory(Category category)
        {
            var entity = context.Category.AsNoTracking().Where(p => p.CategoryId.Equals(category.CategoryId)).FirstOrDefault();
            bool checkCode = this.CheckUniqueCategoryCode(category.Code, category.CategoryId);
            if (checkCode)
            {
                category.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Category.Update(category);
            }
            return context.SaveChanges();
        }

        public async Task<int> DeleteCategory(string CategoryId)
        {
            var entity = context.Category.Where(p => p.CategoryId.Equals(CategoryId)).FirstOrDefault();
            if (entity != null)
            {
                entity.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                entity.Active = false;
                context.Category.Update(entity);
            }
            return await context.SaveChangesAsync();
        }

        public async Task<int> ArrangeCategory(List<ModelViews.ViewArrangeCategory> lsEntity)
        {
            foreach(ModelViews.ViewArrangeCategory item in lsEntity)
            {
                var category = context.Category.Where(p => p.CategoryId.Equals(item.Category_ID) && p.Active == true).AsEnumerable().FirstOrDefault();
                if (category != null)
                {
                    category.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    category.No = item.No;
                    context.Category.Update(category);
                }
            }
            return await context.SaveChangesAsync();
        }

        public string GenerateCategoryID()
        {
            string today = DateTime.Now.ToString("yyMM");
            string id = ShortId.Generate(true, false, 10).ToUpper();
            string result;
            result = today + "/" + id;
            return result;
        }

        public bool CheckUniqueCategoryCode(string Code, string categoryID)
        {
            int count = context.Category.AsNoTracking().Where(p => p.Active == true && p.Code.Equals(Code) && p.CategoryId != categoryID).AsEnumerable().Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckUniqueCategoryID(string ID)
        {
            int count = context.Category.AsNoTracking().Where(p => p.Active == true && p.CategoryId.Equals(ID)).AsEnumerable().Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
