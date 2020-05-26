using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.ModelViews
{
    public class ViewAssetResult
    {
        public int page = 1;
        public int pageCount = 0;
        public int total = 0;
        public int type = 1;
        public List<Models.Asset> data { get; set; }
        public ViewAssetResult()
        {
            data = new List<Models.Asset>();
        }
    }
}
