using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.Models
{
    public partial class Image
    {
        public string Guid { get; set; }
        public string ReferenceId { get; set; }
        public string OriginalName { get; set; }
        public string ImageName { get; set; }
        public string Path { get; set; }
        public string PathThumbNail { get; set; }
        public bool? Active { get; set; }
        [JsonIgnore]
        public virtual Asset Reference { get; set; }
        [JsonIgnore]
        public virtual Category ReferenceNavigation { get; set; }

        internal static IDisposable FromStream(MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }
    }
}
