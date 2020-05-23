using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ACheckAPI.Common;
using ACheckAPI.Dao;
using ACheckAPI.Models;
using ACheckAPI.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ACheckAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportDataController : BaseController
    {
        public ImportDataController(TWG_ACHECKContext tWG_ACHECKContext) : base(tWG_ACHECKContext) { }

        List<string> lstEmailAllSheet = new List<string>();
        List<string> listError = new List<string>();
        List<string> listWarning = new List<string>();
        //
        Dictionary<string, int> dict = new Dictionary<string, int>();
        Dictionary<string, string> dictMap = new Dictionary<string, string>()
        {
            { "MÃ TÀI SẢN","AssetCode"},
            { "TÊN TÀI SẢN","AssetName"},
            { "MÔ TẢ","Description"},
            { "ĐƠN VỊ TÍNH","Unit"},
            { "ĐƠN GIÁ","Price"},
            { "MODEL","Model"},
            { "XUẤT XỨ","Origin"},
            { "SỐ LƯỢNG","Quantity"},
            { "VỊ TRÍ","LocationID"},
            { "LOẠI TÀI SẢN","CategoryID"},
        };
        [HttpPost]
        [Route("ImportAsset")]
        public ReturnObject ImportAsset()
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            obj.value = 0;
            ViewImportData entity = new ViewImportData();
            IFormFile formFiles = Request.Form.Files.First();
            entity.FileImport = formFiles;
            listWarning = new List<string>();
            listError = new List<string>();
            if (entity.FileImport != null)
            {
                IFormFile file = entity.FileImport;
                if (file.Length > 0)
                {
                    entity.fileName = file.FileName;
                    string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                    ISheet sheet;
                    using (var stream = new FileStream(file.FileName, FileMode.Create))
                    {
                        int countUpdate = 0;
                        int countInsert = 0;
                        int countError = 0;
                        file.CopyTo(stream);
                        stream.Position = 0;
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        XSSFWorkbook hssfwbCheck = hssfwb;
                        if (hssfwb.NumberOfSheets > 0)
                        {
                            sheet = hssfwb.GetSheetAt(0);
                            var header = sheet.GetRow(5);
                            dict.Clear();
                            if (header != null)
                            {
                                Dictionary<string, int> dictHeaders = new Dictionary<string, int>();
                                var lstLines = new List<string>();
                                for (int j = 0; j < header.Cells.Count; j++)
                                {
                                    string key = header.Cells[j].StringCellValue.Trim().ToUpper();
                                    if (dictMap.ContainsKey(key))
                                    {
                                        dict.Add(dictMap[key], j);
                                    }
                                }
                            }
                            //Lấy dữ liệu từ dòng thứ 1
                            for (int rowIndex = 6; rowIndex < sheet.PhysicalNumberOfRows; rowIndex++)
                            {
                                // Lấy row hiện tại
                                var nowRow = sheet.GetRow(rowIndex);
                                if (nowRow.GetCell(1) != null && (nowRow.GetCell(1).CellType.ToString() != "Blank"))
                                {
                                    countInsert++;
                                    //Kiểm tra lỗi
                                    KiemTraLoi(ref nowRow);
                                    countError = listError.Count;
                                    //Nếu không có lỗi mới thực hiện tiếp
                                    if (countError <= 0)
                                    {
                                        var now = DateTime.Now.ToString("dd-MMM-yyyy");
                                        Asset asset = new Asset();
                                        asset.AssetCode = GetString(nowRow, "AssetCode").TrimEnd();
                                        asset.AssetName = GetString(nowRow, "AssetName").TrimEnd();
                                        asset.Description = GetString(nowRow, "Description").TrimEnd();
                                        asset.Unit = GetString(nowRow, "Unit").TrimEnd();
                                        asset.Price = GetInt(nowRow, "Price");
                                        asset.Quantity = GetInt(nowRow, "Quantity");
                                        asset.LocationID = GetString(nowRow, "LocationID").TrimEnd();
                                        asset.CategoryID = GetString(nowRow, "CategoryID").TrimEnd();
                                        
                                        entity.lsAsset.Add(asset);
                                    }
                                }
                                else
                                {
                                    listError.Add("File không có dữ liệu hoặc dữ liệu không đúng định dạng");
                                    break;
                                }
                            }
                            if(listError.Count == 0)
                            {
                                DaoAsset daoAsset = new DaoAsset(tWG_ACHECKContext);
                                int count = daoAsset.ImportData(entity.lsAsset);
                                if(count > 0)
                                {
                                    obj.value = count;
                                    obj.status = 1;
                                }
                                
                            }
                        }
                        System.IO.File.Delete(stream.Name);
                        entity.countError = countError;
                        entity.countUpdate = countUpdate;
                        entity.countInsert = countInsert;
                        entity.listError = listError;
                        entity.listWarning = listWarning;
                        obj.message = JsonConvert.SerializeObject(listError).ToString();
                    }
                }
            }
            return obj;
        }

        public string GetString(IRow row, string colName)
        {
            try
            {
                if (row.GetCell(dict[colName]).CellType.ToString() == "String")
                {
                    return row.GetCell(dict[colName]).StringCellValue.Trim() ?? "";
                }
                else if (row.GetCell(dict[colName]).CellType.ToString() == "Numeric" && !string.Equals(colName, "NgaySinh"))
                {
                    return row.GetCell(dict[colName]).NumericCellValue.ToString().Trim() ?? "";
                }
                else if (row.GetCell(dict[colName]).CellType.ToString() == "Numeric" && string.Equals(colName, "NgaySinh"))
                {
                    return row.GetCell(dict[colName]).ToString().Trim() ?? "";
                }
                else if (row.GetCell(dict[colName]).CellType.ToString() == "Formula")
                {
                    return row.GetCell(dict[colName]).StringCellValue.Trim() ?? "";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public int GetInt(IRow row, string colName)
        {
            try
            {
                if (row.GetCell(dict[colName]).CellType.ToString() == "String")
                {
                    return int.Parse(row.GetCell(dict[colName]).StringCellValue);
                }
                else if (row.GetCell(dict[colName]).CellType.ToString() == "Numeric")
                {
                    return int.Parse(row.GetCell(dict[colName]).NumericCellValue.ToString());
                }
                else if (row.GetCell(dict[colName]).CellType.ToString() == "Formula")
                {
                    return int.Parse(row.GetCell(dict[colName]).StringCellValue);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void KiemTraLoi(ref IRow row)
        {
            DaoAsset daoAsset = new DaoAsset(tWG_ACHECKContext);
            var sheet = row.Sheet;
            try
            {
                var AssetCode = GetString(row, "AssetCode");
                var AssetName = GetString(row, "AssetName");
                var Description = GetString(row, "Description");
                var Unit = GetString(row, "Unit");
                var Price = GetString(row, "Price");
                var Model = GetString(row, "Model");
                var Origin = GetString(row, "Origin");
                var Quantity = GetString(row, "Quantity");
                var CategoryID = GetString(row, "CategoryID");
                var LocationID = GetString(row, "LocationID");

                if (AssetCode != "" && AssetCode.Length > 150)
                {
                    listError.Add("Hàng thứ " + (row.RowNum + 1) + " - Asset Code '" + AssetCode + " có độ dài vượt quá độ dài cho phép.");
                }
                //if (AssetCode == "")
                //{
                //    listError.Add("Hàng thứ " + (row.RowNum + 1) + " - Asset Code không được để trống.");
                //}
                if (!string.IsNullOrEmpty(AssetCode))
                {
                    bool check = daoAsset.CheckUniqueAssetCode(AssetCode, "");
                    if (!check)
                    {
                        listError.Add("Hàng thứ " + (row.RowNum + 1) + " - Asset Code '" + AssetCode + " đã tồn tại.");
                    }
                }
                if (AssetName.Length > 200)
                {
                    listError.Add("Hàng thứ " + (row.RowNum + 1) + " - Tên tài sản '" + AssetCode + " có độ dài vượt quá độ dài cho phép.");
                }
                if (AssetName == "")
                {
                    listError.Add("Hàng thứ " + (row.RowNum + 1) + " - Tên tài sản không được để trống.");
                }
                if (CategoryID.Length > 200)
                {
                    listError.Add("Hàng thứ " + (row.RowNum + 1) + " - Loại tài sản '" + AssetCode + " có độ dài vượt quá độ dài cho phép.");
                }
                if (CategoryID == "")
                {
                    listError.Add("Hàng thứ " + (row.RowNum + 1) + " - Loại tài sản không được để trống.");
                }
                if (LocationID.Length > 200)
                {
                    listError.Add("Hàng thứ " + (row.RowNum + 1) + " - Vị trí tài sản '" + AssetCode + " có độ dài vượt quá độ dài cho phép.");
                }
                if (LocationID == "")
                {
                    listError.Add("Hàng thứ " + (row.RowNum + 1) + " - Vị trí tài sản không được để trống.");
                }
            }
            catch (Exception ex)
            {
                listError.Add(ex.Message.ToString());
            }

        }
    }
}