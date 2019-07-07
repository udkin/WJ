using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WJ.API.Models;
using WJ.Common;
using WJ.Entity;
using WJ.Service;

namespace WJ.API.Controllers
{
    [ApiAuthorize]
    public class UserAppController : ApiController
    {
        #region 获取用户系统应用列表
        /// <summary>
        /// 获取用户系统应用列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserAppList()
        {
            HttpResponseMessage redirectResponse = new HttpResponseMessage(HttpStatusCode.Moved);
            dynamic result = null;
            try
            {
                AuthInfo authInfo = this.RequestContext.RouteData.Values["access_token"] as AuthInfo;

                if (authInfo == null || DateTime.Now >= authInfo.TokenTimeLimit)
                {
                    result = new { code = 0, success = 0, data = "获取失败" };
                }
                else
                {
                    dynamic userAppList = UserAppService.Instance.GetUserAppDynamic(authInfo.UserId);
                    result = new { code = 0, success = 0, data = userAppList };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(result);
        }
        #endregion
    }
}
