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
    public class DeptController : ApiBaseController
    {
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public IHttpActionResult GetAllDeptList(dynamic request)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                var deptList = DeptService.Instance.GetAllDeptList();
                SetSuccessResult(resultObj, deptList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(resultObj);
        }
    }
}
