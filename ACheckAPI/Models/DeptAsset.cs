using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ACheckAPI.Models
{
    public partial class DeptAsset
    {
        public string Guid { get; set; }
        public string DeptId { get; set; }
        public string AssetId { get; set; }
        public string Supporter { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public bool? Active { get; set; }
        public string CreatedAt { get; set; }
        public string Creater { get; set; }
        public string UpdatedAt { get; set; }
        public string Updater { get; set; }
        [JsonIgnore]
        public virtual Asset Asset { get; set; }
    }
}
