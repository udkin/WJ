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
    public class RoleController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult GetAllRoleList(dynamic request)
        {
            dynamic result = new { code = 0, success = 1, msg = "获取角色信息失败" };
            try
            {
                var roleList = RoleService.Instance.GetAllRoleList();
                result = new { code = 0, success = 0, data = roleList };
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
