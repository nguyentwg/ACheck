using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACheckAPI.Models
{
    public partial class Category
    {
        public Category()
        {
            AssetCategory = new HashSet<AssetCategory>();
            EavAttributeValue = new HashSet<EavAttributeValue>();
        }

        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public string Level { get; set; }
        public string CreatedAt { get; set; }
        public string Creater { get; set; }
        public string UpdatedAt { get; set; }
        public string Updater { get; set; }
        public bool? Active { get; set; }
        public int? No { get; set; }
        public string Path { get; set; }
        public string CategoryType { get; set; }

        [NotMapped]
        public int CountSubCategory { get; set; }
        [NotMapped]
        public int CountAsset { get; set; }

        public virtual ICollection<AssetCategory> AssetCategory { get; set; }
        public virtual ICollection<EavAttributeValue> EavAttributeValue { get; set; }
    }
}
