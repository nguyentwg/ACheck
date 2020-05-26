using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.Common
{
    public class Constants
    {
        //ROOT FOLDER FROM AZURE [RootFolder]: [dev/]|[test/]|[production/]
        public const string SubFolderAsset = "Acheck/Asset/";

        public const int PageSize = 5;
        public const int PageSizeAdmin = 50;
        public const int DisplayPageNum = 5; //nên là số lẻ
    }
}
