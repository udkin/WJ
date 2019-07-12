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
    public class TitleController : ApiBaseController
    {
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public IHttpActionResult GetAllTitleList(dynamic request)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                var titleList = TitleService.Instance.GetAllTitleList();
                SetSuccessResult(resultObj, titleList);
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
