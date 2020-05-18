using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ACheckAPI.Common;
using ACheckAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ACheckAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        public DepartmentController(TWG_ACHECKContext tWG_ACHECKContext) : base(tWG_ACHECKContext) { }

        [HttpPost]
        [Route("GetListBranch")]
        public async Task<ReturnObject> GetListBranch()
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("lstMaChiNhanh", "");
                    StringContent content = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded");

                    using (var response = await httpClient.PostAsync(AppSetting.LAH_Hr_Api + "DanhSachChiNhanh", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (apiResponse != null)
                        {
                            var lsChiNhanh = JsonConvert.DeserializeObject<Object>(apiResponse);
                            obj.value = lsChiNhanh;
                        }
                        else
                        {
                            obj.value = null;
                        }
                        obj.status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                obj.status = -1;
                obj.message = ex.StackTrace;
            }
            return obj;
        }

        [HttpPost]
        [Route("GetListDepartment")]
        public async Task<ReturnObject> GetListDepartment(string BranchID)
        {
            ReturnObject obj = new ReturnObject();
            obj.status = -1;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded");
                    httpClient.DefaultRequestHeaders.Add("lstMaPhongBan", "");
                    using (var response = await httpClient.PostAsync(AppSetting.LAH_Hr_Api+ "DanhSachPhongBan", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if(apiResponse != null)
                        {
                            List<Department> lsDept = JsonConvert.DeserializeObject<List<Department>>(apiResponse);
                            if (!string.IsNullOrEmpty(BranchID))
                            {
                                lsDept = lsDept.Where(p => p.MaChiNhanh.Equals(BranchID)).AsEnumerable().ToList();
                            }
                            obj.value = lsDept;
                        }
                        else
                        {
                            obj.value = null;
                        }
                        obj.status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                obj.status = -1;
                obj.message = ex.StackTrace;
            }
            return obj;
        }
    }
}