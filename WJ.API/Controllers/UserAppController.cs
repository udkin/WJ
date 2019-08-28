using Newtonsoft.Json.Linq;
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
    public class UserAppController : ApiBaseController
    {
        #region 获取用户APP列表
        /// <summary>
        /// 获取用户系统应用列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetUserAppList(JObject data)
        {
            var resultObj = GetSearchResultInstance();
            try
            {
                int total = 0;
                int userId = data["UserId"].ToObject<int>();
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();
                var resultData = UserAppService.Instance.DbInstance.Queryable<WJ_V_UserApp>().Where(p => p.UserId == userId).ToPageList(pageIndex, pageSize, ref total);
                //var resultData = UserAppService.Instance.GetList(data, ref total);
                SetSearchSuccessResult(resultObj, total, resultData);
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 设置用户APP信息
        /// <summary>
        /// 配置用户APP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult SetUserApp(JObject data)
        {
            ResultModel resultObj = GetResultInstance("设置用户APP信息失败");

            try
            {
                string errorMsg = string.Empty;
                if (UserAppService.Instance.SetUserApp(data, ref errorMsg))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj, errorMsg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 修改用户APP信息
        /// <summary>
        /// 修改用户APP信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult UpdateUserApp(JObject data)
        {
            ResultModel resultObj = GetResultInstance("更新用户APP信息失败");

            try
            {
                string errorMsg = string.Empty;
                if (UserAppService.Instance.Update(data, ref errorMsg))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj, errorMsg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion
    }
}
