using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.Common
{
    public class AzureConfig
    {
        public AzureConfig()
        {
        }

        public string ConnectionString { get; set; }
        public string RootFolder { get; set; }
        public string Domain { get; set; }
    }
}
