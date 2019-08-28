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
    public class UserPlanController : ApiBaseController
    {
        #region 获取用户方案列表
        /// <summary>
        /// 获取用户系统应用列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetList(JObject data)
        {
            var resultObj = GetSearchResultInstance();
            try
            {
                int total = 0;
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();
                var resultData = UserPlanService.Instance.DbInstance.Queryable<WJ_V_UserPlan>().Where(p => p.UserPlan_State == 1)
                    .WhereIF(data["UserName"] != null && data["UserName"].ToString().Trim() != "", p => p.User_Name.Contains(data["UserName"].ToString()))
                    .WhereIF(data["PlanName"] != null && data["PlanName"].ToString().Trim() != "", p => p.UserPlan_Name.Contains(data["PlanName"].ToString()))
                    .OrderBy(p => p.UserPlan_CreateTime, SqlSugar.OrderByType.Desc)
                    .ToPageList(pageIndex, pageSize, ref total);
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
    }
}
