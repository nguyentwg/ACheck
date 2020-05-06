using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ACheckAPI.Models
{
    public partial class Category
    {
        public Category()
        {
            Asset = new HashSet<Asset>();
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

        [JsonIgnore]
        public virtual ICollection<Asset> Asset { get; set; }
    }
}
