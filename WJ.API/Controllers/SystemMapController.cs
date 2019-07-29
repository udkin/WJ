using Newtonsoft.Json.Linq;
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
    public class SystemMapController : ApiBaseController
    {
        #region 获取应用（供下拉列表使用）
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public IHttpActionResult GetSystemMap(JObject data)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                var resultData = SystemMapService.Instance.GetMapValueList(data["Type"].ToString());
                SetSuccessResult(resultObj, resultData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion
    }
}
