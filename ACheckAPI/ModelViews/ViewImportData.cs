using ACheckAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.ModelViews
{
    public class ViewImportData
    {
        public IFormFile FileImport { get; set; }
        public int countError = 0;
        public int countUpdate = 0;
        public int countInsert = 0;
        public int importSuccess = 0;
        public string fileName = "";

        public List<string> listError = new List<string>();
        public List<string> listWarning = new List<string>();
        public virtual List<Asset> lsAsset { get; set; }

        public ViewImportData()
        {
            lsAsset = new List<Asset>();
        }
    }
}
