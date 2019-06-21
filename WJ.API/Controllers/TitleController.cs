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
    /// 职务控制器
    /// </summary>
    [ApiAuthorize]
    public class TitleController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult GetAllTitleList(dynamic request)
        {
            dynamic result = new { code = 0, success = 1, msg = "获取职务信息失败" };
            try
            {
                var roleList = TitleService.Instance.GetAllTitleList();
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
