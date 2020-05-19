using ACheckAPI.Models;
using Microsoft.EntityFrameworkCore;
using shortid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACheckAPI.Common;
using ACheckAPI.ModelViews;
using Microsoft.AspNetCore.Http;

namespace ACheckAPI.Dao
{
    public class DaoAsset : DaoBase
    {
        public DaoAsset(TWG_ACHECKContext _context) : base(_context) { }
        public List<Asset> GetAll()
        {
            var result = (from asset in context.Asset
                          .Include(p => p.Image)
                          .Include(p => p.Assign)
                          .Include(p => p.DeptAsset)
                          join asset_cate in context.EavAttributeValue on asset.AssetId equals asset_cate.CategoryId
                          where asset_cate.AttributeGroup != null && asset_cate.AttributeGroup.Equals(EnumEAV.EAV_Type.AssetCategory.ToString())
                          join Cate in context.Category on asset_cate.EavId equals Cate.CategoryId
                          join asset_location in context.EavAttributeValue on asset.AssetId equals asset_location.CategoryId
                          where (asset_location.AttributeGroup ?? " ").Equals(EnumEAV.EAV_Type.AssetLocation.ToString())
                          join Area in context.Category on asset_location.EavId equals Area.CategoryId
                          into ps
                          where asset.Active == true
                          from p in ps.DefaultIfEmpty()
                          select new { asset, CategoryId = Cate == null ? "" : Cate.CategoryId, CategoryName = Cate == null ? "" : Cate.CategoryName, LocationID = p == null ? "" : p.CategoryId, LocationName = p == null ? "" : p.CategoryName }).AsEnumerable().Select(x =>
                          {
                              x.asset.CategoryID = x.CategoryId;
                              x.asset.CategoryName = x.CategoryName;
                              x.asset.LocationID = x.LocationID;
                              x.asset.LocationName = x.LocationName;
                              x.asset.Image = x.asset.Image.Where(p => p.Active == true).ToList();
                              x.asset.Assign = x.asset.Assign.Where(p => p.Active == true).ToList();
                              x.asset.DeptAsset = x.asset.DeptAsset.Where(p => p.Active == true).ToList();
                              return x.asset;
                          }).AsEnumerable().ToList();
            return result;
        }

        public List<Asset> GetAssetByCategoryId(string CategoryId)
        {
            var result = (from asset in context.Asset.Include(p => p.Image).Include(p => p.Assign).Include(p => p.DeptAsset)
                          join asset_cate in context.EavAttributeValue on asset.AssetId equals asset_cate.CategoryId
                          where asset_cate.AttributeGroup != null && asset_cate.EavId.Equals(CategoryId) && asset_cate.AttributeGroup.Equals(EnumEAV.EAV_Type.AssetCategory.ToString())
                          join Cate in context.Category on asset_cate.EavId equals Cate.CategoryId
                          join asset_location in context.EavAttributeValue on asset.AssetId equals asset_location.CategoryId
                          where (asset_location.AttributeGroup ?? " ").Equals(EnumEAV.EAV_Type.AssetLocation.ToString())
                          join Area in context.Category on asset_location.EavId equals Area.CategoryId
                          into ps
                          where asset.Active == true
                          from p in ps.DefaultIfEmpty()
                          select new { asset, CategoryId = Cate == null ? "" : Cate.CategoryId, CategoryName = Cate == null ? "" : Cate.CategoryName, LocationID = p == null ? "" : p.CategoryId, LocationName = p == null ? "" : p.CategoryName }).AsEnumerable().Select(x =>
                             {
                                 x.asset.CategoryID = x.CategoryId;
                                 x.asset.CategoryName = x.CategoryName;
                                 x.asset.LocationID = x.LocationID;
                                 x.asset.LocationName = x.LocationName;
                                 x.asset.Image = x.asset.Image.Where(p => p.Active == true).ToList();
                                 x.asset.Assign = x.asset.Assign.Where(p => p.Active == true).ToList();
                                 x.asset.DeptAsset = x.asset.DeptAsset.Where(p => p.Active == true).ToList();
                                 return x.asset;
                             }).AsEnumerable().ToList();
            return result;
        }

        public Asset GetAssetByID(string AssetID)
        {
            var result = context.Asset.AsNoTracking().Where(p => p.Active == true && p.AssetId.Equals(AssetID))
                                                    .Include(p => p.Image).Include(p => p.Assign).AsEnumerable().FirstOrDefault();
            return result;
        }

        public List<Asset> AssetFilter(string AssetID, string AssetName)
        {
            if (string.IsNullOrEmpty(AssetID))
            {
                AssetID = "";
            }
            if (string.IsNullOrEmpty(AssetName))
            {
                AssetName = "";
            }
            var result = (from asset in context.Asset.Include(p => p.Image).Include(p => p.Assign).Include(p => p.DeptAsset)
                          join asset_cate in context.EavAttributeValue on asset.AssetId equals asset_cate.CategoryId
                          where asset_cate.AttributeGroup != null && asset_cate.AttributeGroup.Equals(EnumEAV.EAV_Type.AssetCategory.ToString())
                          join Cate in context.Category on asset_cate.EavId equals Cate.CategoryId
                          join asset_location in context.EavAttributeValue on asset.AssetId equals asset_location.CategoryId
                          where (asset_location.AttributeGroup ?? " ").Equals(EnumEAV.EAV_Type.AssetLocation.ToString())
                          join Area in context.Category on asset_location.EavId equals Area.CategoryId
                          into ps
                          where asset.AssetId.Contains(AssetID) && asset.AssetName.ToLower().Contains(AssetName.ToLower())
                          from p in ps.DefaultIfEmpty()
                          select new { asset, CategoryId = Cate == null ? "" : Cate.CategoryId, CategoryName = Cate == null ? "" : Cate.CategoryName, LocationID = p == null ? "" : p.CategoryId, LocationName = p == null ? "" : p.CategoryName }).AsEnumerable().Select(x =>
                          {
                              x.asset.CategoryID = x.CategoryId;
                              x.asset.CategoryName = x.CategoryName;
                              x.asset.LocationID = x.LocationID;
                              x.asset.LocationName = x.LocationName;
                              x.asset.Image = x.asset.Image.Where(p => p.Active == true).ToList();
                              x.asset.Assign = x.asset.Assign.Where(p => p.Active == true).ToList();
                              x.asset.DeptAsset = x.asset.DeptAsset.Where(p => p.Active == true).ToList();
                              return x.asset;
                          }).AsEnumerable().ToList();
            return result;
        }


        public async Task<int> Add(ViewAsset viewAsset, IFormFileCollection formFileCollection)
        {
            DaoAssign daoAssign = new DaoAssign(context);
            DaoDeptAsset daoDeptAsset = new DaoDeptAsset(context);
            Asset asset = new Asset();
            Assign assign = new Assign();
            DeptAsset deptAsset = new DeptAsset();
            asset = viewAsset.asset;
            assign = viewAsset.assign;
            deptAsset = viewAsset.DeptAsset;
            List<EavAttributeValue> lsAttributeValue = new List<EavAttributeValue>();
            lsAttributeValue = viewAsset.EavAttributeValue;
            asset.AssetId = this.GenerateAssetID();
            if (string.IsNullOrEmpty(asset.AssetCode))
            {
                asset.AssetCode = asset.AssetId;
            }
            var checkCode = this.CheckUniqueAssetCode(asset.AssetCode, asset.AssetId);
            if (checkCode)
            {
                asset.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                asset.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Asset.Add(asset);

                if (asset.CategoryID != null)
                {
                    EavAttributeValue cate = new EavAttributeValue();
                    cate.Guid = Guid.NewGuid().ToString().ToUpper();
                    cate.CategoryId = asset.AssetId;
                    cate.EavId = asset.CategoryID;
                    cate.AttributeGroup = EnumEAV.EAV_Type.AssetCategory.ToString();
                    context.EavAttributeValue.Add(cate);
                }
                if (asset.LocationID != null)
                {
                    EavAttributeValue cate = new EavAttributeValue();
                    cate.Guid = Guid.NewGuid().ToString().ToUpper();
                    cate.CategoryId = asset.AssetId;
                    cate.EavId = asset.LocationID;
                    cate.AttributeGroup = EnumEAV.EAV_Type.AssetLocation.ToString();
                    context.EavAttributeValue.Add(cate);
                }
                if (assign != null)
                {
                    assign.AssetId = asset.AssetId;
                    daoAssign.AssignAsset(assign);
                }
                if (deptAsset != null)
                {
                    deptAsset.AssetId = asset.AssetId;
                    daoDeptAsset.Add(deptAsset);
                }
                if(lsAttributeValue != null)
                {
                    foreach (EavAttributeValue item in lsAttributeValue)
                    {
                        item.Guid = Guid.NewGuid().ToString().ToUpper();
                        item.CategoryId = asset.AssetId;
                        item.AttributeGroup = EnumEAV.EAV_Type.AssetAttribute.ToString();
                        context.EavAttributeValue.Add(item);
                    }
                }
                List<Image> lsAssetImage = new List<Image>();
                lsAssetImage = new Function().DangTaiHinhAnh(formFileCollection, asset.AssetId);
                context.Image.AddRange(lsAssetImage);
            }
            return await context.SaveChangesAsync();
        }

        public async Task<int> Update(ViewAsset viewAsset, IFormFileCollection formFileCollection)
        {
            DaoAssign daoAssign = new DaoAssign(context);
            DaoDeptAsset daoDeptAsset = new DaoDeptAsset(context);
            Asset asset = new Asset();
            Assign assign = new Assign();
            DeptAsset deptAsset = new DeptAsset();
            asset = viewAsset.asset;
            assign = viewAsset.assign;
            deptAsset = viewAsset.DeptAsset;
            List<EavAttributeValue> lsAttributeValue = new List<EavAttributeValue>();
            lsAttributeValue = viewAsset.EavAttributeValue;
            var checkCode = this.CheckUniqueAssetCode(asset.AssetCode, asset.AssetId);
            if (checkCode)
            {
                var assetDB = context.Asset.Where(p => p.AssetId.Equals(asset.AssetId)).FirstOrDefault();
                asset.CopyPropertiesTo<Asset>(assetDB);
                assetDB.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Asset.Update(assetDB);
                if (assign != null)
                {
                    assign.AssetId = asset.AssetId;
                    daoAssign.AssignAsset(assign);
                }
                if (deptAsset != null)
                {
                    deptAsset.AssetId = asset.AssetId;
                    daoDeptAsset.Add(deptAsset);
                }
                foreach (EavAttributeValue item in lsAttributeValue)
                {
                    if (!string.IsNullOrEmpty(item.Guid))
                    {
                        context.EavAttributeValue.Update(item);
                    }
                    else
                    {
                        item.Guid = Guid.NewGuid().ToString().ToUpper();
                        item.CategoryId = asset.AssetId;
                        item.AttributeGroup = !string.IsNullOrEmpty(item.Value) ? EnumEAV.EAV_Type.AssetAttribute.ToString() : EnumEAV.EAV_Type.AssetCategory.ToString();
                        context.EavAttributeValue.Add(item);
                    }
                }
                List<Image> lsAssetImage = new List<Image>();
                lsAssetImage = new Function().DangTaiHinhAnh(formFileCollection, asset.AssetId);
                context.Image.AddRange(lsAssetImage);
            }
            return await context.SaveChangesAsync();
        }

        public Asset GetAssetByAssetID(string AssetID)
        {
            Asset res = new Asset();
            res = (from asset in context.Asset.Include(p => p.Image).Include(p=>p.Assign).Include(p=>p.DeptAsset)
                            join asset_cate in context.EavAttributeValue on asset.AssetId equals asset_cate.CategoryId
                            where (asset_cate.AttributeGroup ?? " ").Equals(EnumEAV.EAV_Type.AssetCategory.ToString())
                            join Cate in context.Category on asset_cate.EavId equals Cate.CategoryId
                            join asset_location in context.EavAttributeValue on asset.AssetId equals asset_location.CategoryId
                            where (asset_location.AttributeGroup ?? " ").Equals(EnumEAV.EAV_Type.AssetLocation.ToString())
                            join Area in context.Category on asset_location.EavId equals Area.CategoryId
                            into ps
                            where asset.AssetId.Equals(AssetID)
                            from p in ps.DefaultIfEmpty()
                            select new { asset, CategoryId = Cate == null ? "" : Cate.CategoryId, CategoryName = Cate == null ? "" : Cate.CategoryName, LocationID = p == null ? "" : p.CategoryId, LocationName = p == null ? "" : p.CategoryName }).AsEnumerable().Select(x =>
                            {
                                x.asset.CategoryID = x.CategoryId;
                                x.asset.CategoryName = x.CategoryName;
                                x.asset.LocationID = x.LocationID;
                                x.asset.LocationName = x.LocationName;
                                x.asset.Image = x.asset.Image.Where(p => p.Active == true).ToList();
                                x.asset.Assign = x.asset.Assign.Where(p => p.Active == true).ToList();
                                x.asset.DeptAsset = x.asset.DeptAsset.Where(p => p.Active == true).ToList();
                                return x.asset;
                            }).AsEnumerable().FirstOrDefault();
            return res;
        }

        public int AddAsset(Asset asset, Assign assign)
        {
            string a = EnumEAV.EAV_Type.Area.ToString();
            DaoAssign daoAssign = new DaoAssign(context);
            asset.AssetId = this.GenerateAssetID();
            var checkCode = this.CheckUniqueAssetCode(asset.AssetCode, asset.AssetId);
            if (checkCode)
            {
                asset.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                asset.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Asset.Add(asset);
                if (assign != null)
                {
                    assign.AssetId = asset.AssetId;
                    daoAssign.AssignAsset(assign);
                }
            }
            return context.SaveChanges();
        }

        public int UpdateAsset(Asset asset, Assign assign)
        {
            DaoAssign daoAssign = new DaoAssign(context);
            var entity = context.Asset.AsNoTracking().Where(p => p.AssetId.Equals(asset.AssetId)).FirstOrDefault();
            bool checkCode = this.CheckUniqueAssetCode(asset.AssetCode, asset.AssetId);
            if (checkCode)
            {
                asset.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Asset.Update(asset);
                if (assign != null)
                {
                    assign.AssetId = asset.AssetId;
                    daoAssign.AssignAsset(assign);
                }
            }
            return context.SaveChanges();
        }

        public async Task<int> DeleteAsset(string assetID)
        {
            var entity = context.Asset.AsNoTracking().Where(p => p.AssetId.Equals(assetID)).FirstOrDefault();
            if (entity != null)
            {
                entity.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                entity.Active = false;
                context.Asset.Update(entity);
                var lsAssign = context.Assign.Where(p => p.AssetId.Equals(assetID) && p.Active == true).AsEnumerable().ToList();
                foreach (Assign item in lsAssign)
                {
                    item.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    item.Active = false;
                    context.Assign.Update(item);
                }
            }
            return await context.SaveChangesAsync();
        }

        public string GenerateAssetID()
        {
            string today = DateTime.Now.ToString("yyMM");
            string id = ShortId.Generate(true, false, 10).ToUpper();
            string result;
            result = today + "/" + id;
            return result;
        }

        public bool CheckUniqueAssetCode(string AssetCode, string AssetId)
        {
            int count = context.Asset.AsNoTracking().Where(p => p.Active == true && p.AssetCode.Equals(AssetCode) && p.AssetId != AssetId).AsEnumerable().Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckUniqueAssetID(string ID)
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
