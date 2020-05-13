﻿using ACheckAPI.Models;
using Microsoft.EntityFrameworkCore;
using shortid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACheckAPI.Common;
using ACheckAPI.ModelViews;

namespace ACheckAPI.Dao
{
    public class DaoAsset :DaoBase
    {
        public DaoAsset(TWG_ACHECKContext _context) : base(_context) { }
        public List<Asset> GetAll()
        {
            var result = context.Asset.AsNoTracking().Where(p=>p.Active == true).Include(p=>p.Assign).AsEnumerable().ToList();
            return result;
        }

        public List<Asset> GetAssetByCategoryId(string CategoryId)
        {
            return context.Asset.AsNoTracking().Where(p => p.Active == true).AsEnumerable().ToList();
        }

        public Asset GetAssetByID(string AssetID)
        {
            var result = context.Asset.AsNoTracking().Where(p => p.Active == true && p.AssetId.Equals(AssetID)).Include(p => p.Assign).AsEnumerable().FirstOrDefault();
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
            var result = (from asset in context.Asset
                     join asset_cate in context.EavAttributeValue on asset.AssetId equals asset_cate.CategoryId
                     where asset_cate.AttributeGroup != null && asset_cate.AttributeGroup.Equals(EnumEAV.EAV_Type.AssetCategory.ToString())
                     join Cate in context.Category on asset_cate.EavId equals Cate.CategoryId
                     where asset.AssetId.Contains(AssetID) && asset.AssetName.ToLower().Contains(AssetName.ToLower())
                     select new { asset, Cate }).AsEnumerable().Select(x =>
                     {
                         x.asset.CategoryID = x.Cate.CategoryId;
                         x.asset.CategoryName = x.Cate.CategoryName;
                         return x.asset;
                     }).AsEnumerable().ToList();
            return result;
        }

        public int Add(ViewAsset viewAsset)
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
            var checkCode = this.CheckUniqueAssetCode(asset.AssetCode, asset.AssetId);
            if (checkCode)
            {
                asset.CreatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                asset.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                context.Asset.Add(asset);

                EavAttributeValue assetCategory = new EavAttributeValue();
                assetCategory.Guid = Guid.NewGuid().ToString().ToUpper();
                assetCategory.CategoryId = asset.AssetId;
                assetCategory.EavId = asset.CategoryID;
                assetCategory.AttributeGroup = EnumEAV.EAV_Type.AssetCategory.ToString();
                context.EavAttributeValue.Add(assetCategory);
                if (assign != null)
                {
                    assign.AssetId = asset.AssetId;
                    daoAssign.AssignAsset(assign);
                }
                if(deptAsset != null)
                {
                    deptAsset.AssetId = asset.AssetId;
                    daoDeptAsset.Add(deptAsset);
                }
                foreach (EavAttributeValue item in lsAttributeValue)
                {
                    item.Guid = Guid.NewGuid().ToString().ToUpper();
                    item.CategoryId = asset.AssetId;
                    item.AttributeGroup = EnumEAV.EAV_Type.AssetAttribute.ToString();
                    context.EavAttributeValue.Add(item);
                }
            }
            return context.SaveChanges();
        }

        public ViewAsset GetAssetByAssetID(string AssetID)
        {
            ViewAsset res = new ViewAsset();
            var a = (from asset in context.Asset
                     join asset_cate in context.EavAttributeValue on asset.AssetId equals asset_cate.CategoryId
                     where (asset_cate.AttributeGroup??" ").Equals(EnumEAV.EAV_Type.AssetCategory.ToString())
                     join Cate in context.Category on asset_cate.EavId equals Cate.CategoryId
                     where asset.AssetId.Equals(AssetID)
                     select new { asset, Cate }).AsEnumerable().Select(x =>
                     {
                         x.asset.CategoryID = x.Cate.CategoryId;
                         x.asset.CategoryName = x.Cate.CategoryName;
                         return x.asset;
                     }).AsEnumerable().FirstOrDefault();
            
            var AssetAttribute = context.EavAttributeValue.AsNoTracking().Where(p => p.Active == true && p.CategoryId.Equals(AssetID) && p.AttributeGroup.Equals(EnumEAV.EAV_Type.AssetAttribute.ToString())).Include(p => p.Eav).AsEnumerable().ToList();
            res.asset = a;
            res.EavAttributeValue = AssetAttribute;
            return res;
        }

        public int AddAsset(Asset asset, Assign assign)
        {
            string a  = EnumEAV.EAV_Type.Area.ToString();
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

        public int DeleteAsset(string assetID)
        {
            var entity = context.Asset.AsNoTracking().Where(p => p.AssetId.Equals(assetID)).FirstOrDefault();
            if (entity != null)
            {
                entity.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                entity.Active = false;
                context.Asset.Update(entity);
                var lsAssign = context.Assign.Where(p => p.AssetId.Equals(assetID) && p.Active == true).AsEnumerable().ToList();
                foreach(Assign item in lsAssign)
                {
                    item.UpdatedAt = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    item.Active = false;
                    context.Assign.Update(item);
                }
            }
            return context.SaveChanges();
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
