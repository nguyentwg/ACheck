using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ACheckAPI.Models
{
    public partial class EavAttribute
    {
        public EavAttribute()
        {
            EavAttributeValue = new HashSet<EavAttributeValue>();
        }

        public string Guid { get; set; }
        public string Name { get; set; }
        public string Display { get; set; }
        public string Type { get; set; }
        public bool? Active { get; set; }
        public string AttributeGroup { get; set; }

        [JsonIgnore]
        public virtual ICollection<EavAttributeValue> EavAttributeValue { get; set; }
        
    }
}
