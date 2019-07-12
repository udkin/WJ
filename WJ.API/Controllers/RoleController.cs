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
    /// <summary>
    /// 
    /// </summary>
    [ApiAuthorize]
    public class RoleController : ApiBaseController
    {
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public IHttpActionResult GetAllRoleList(dynamic request)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                var roleList = RoleService.Instance.GetAllRoleList();
                SetSuccessResult(resultObj, roleList);
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
