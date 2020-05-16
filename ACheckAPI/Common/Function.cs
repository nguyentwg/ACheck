using ACheckAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ACheckAPI.Common
{
    public class Function
    {
        private CloudBlockBlob GenerateCloudBlockBlobNhanVien(string duongdan)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(AppSetting.azureConfig.ConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(AppSetting.azureConfig.RootFolder);
            if (cloudBlobContainer.CreateIfNotExists())
            {
                cloudBlobContainer.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Constants.SubFolderAsset + duongdan);
            return cloudBlockBlob;
        }

        public List<Image> DangTaiHinhAnh(IFormFileCollection AssetImage, string AssetID)
        {
            List<Image> lsAssetImage = new List<Image>();
            try
            {
                foreach(IFormFile file in AssetImage)
                {
                    Image obj = new Image();
                    string image_name = AssetID + DateTime.Now.ToString("yyMMddhhmmss");
                    string file_name = AssetID + "/" + image_name + Path.GetExtension(file.FileName);
                    CloudBlockBlob cloudBlockBlob = GenerateCloudBlockBlobNhanVien(file_name);
                    cloudBlockBlob.UploadFromStream(file.OpenReadStream());
                    string path = cloudBlockBlob.Uri.AbsoluteUri + "?v=" + DateTime.Now.ToString("yyMMddhhmmss");
                    obj.Guid = Guid.NewGuid().ToString();
                    obj.OriginalName = file.FileName;
                    obj.ImageName = image_name + Path.GetExtension(file.FileName);
                    obj.Path = path;
                    obj.ReferenceId = AssetID;
                    lsAssetImage.Add(obj);
                }
                return lsAssetImage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //Ghi log va tra ve string
                return lsAssetImage;
            }
        }


    }
}
