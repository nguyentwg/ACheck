using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ACheckAPI.Models
{
    public partial class EavAttributeValue
    {
        public string Guid { get; set; }
        public string EavId { get; set; }
        public string Value { get; set; }
        public bool? Active { get; set; }
        public string CategoryId { get; set; }
        public string AttributeGroup { get; set; }
        //[JsonIgnore]
        public virtual EavAttribute Eav { get; set; }
    }
}
