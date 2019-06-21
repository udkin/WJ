using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WJ.API.Models;
using WJ.Common;
using WJ.Service;

namespace WJ.API.Controllers
{
    [ApiAuthorize]
    public class DeptController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult GetAllDeptList(dynamic request)
        {
            dynamic result = new { code = 0, success = 1, msg = "获取部门信息失败" };
            try
            {
                var deptList = DeptService.Instance.GetAllDeptList();
                result = new { code = 0, success = 0, data = deptList };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(result);
        }
    }
}
