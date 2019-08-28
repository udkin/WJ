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
    public class HomeController : ApiBaseController
    {
        #region 获取应用分类访问统计数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetAppLogStats(JObject data)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                string sql = ConfigHelper.SqlConfig["AppLogStats"];
                var appClassList = new DbContext<WJ_V_AppStats>().GetList(sql);

                int total = 0;
                foreach (var item in appClassList)
                {
                    total += item.App_Count;
                }

                var resultData = new { Total = total, AppClassList = appClassList };
                SetSuccessResult(resultObj, resultData);
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 获取应用分类访问统计数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetAppStats(dynamic request)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                string sql = ConfigHelper.SqlConfig["AppStats"];
                var appList = new DbContext<WJ_V_AppStats>().GetList(sql);

                int total = 0;
                foreach (var item in appList)
                {
                    total += item.App_Count;
                }

                var resultData = new { Total = total, AppClassList = appList };
                SetSuccessResult(resultObj, resultData);
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 获取应用访问数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetTopAppLogList(JObject data)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                int top = data["limit"].ToObject<int>();
                var appLogList = AppLogService.Instance.GetTopAppLogList(top);
                SetSuccessResult(resultObj, appLogList);
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 获取应用访问数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetTopAuditList(JObject data)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                int top = data["limit"].ToObject<int>();
                var appLogList = AuditService.Instance.GetTopAuditList(top);
                SetSuccessResult(resultObj, appLogList);
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 获取应用访问图表数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetAppLogData(JObject data)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                string scope = data["LogType"] != null ? data["LogType"].ToString().Trim().ToLower() : "month";
                DateTime start = scope == "month" ? DateTime.Now.Date.AddMonths(-1).AddDays(1) : DateTime.Now.Date.AddYears(-1).AddDays(1);
                DateTime end = DateTime.Now.Date;
                DateTime startBefore = scope == "month" ? start.AddMonths(-1) : start.AddYears(-1);
                DateTime endBefore = scope == "month" ? end.AddMonths(-1) : end.AddYears(-1);

                var appLogList = AppLogService.Instance.GetAppLogData(start, end, scope);
                var appLogBeforeList = AppLogService.Instance.GetAppLogData(startBefore, endBefore, scope);

                List<string> dateList = new List<string>();//统计的日期数据
                List<int> logList = new List<int>();//当前时间APP访问数据
                List<int> logBeforeList = new List<int>();//前一个时间APP访问数据
                int appLogTotal = 0;//近期APP访问总数
                int appLogBeforeTotal = 0;//前期APP访问总数

                int userLogTotal = TokenService.Instance.GetUserLogCount(start, end);//近期用户访问总数
                int userLogBeforeTotal = TokenService.Instance.GetUserLogCount(startBefore, endBefore);//前期用户访问总数

                string dateType = scope == "month" ? "yyyy-MM-dd" : "yyyy-MM";

                DateTime s = start;
                string date = "";
                string beforeDate = "";

                while (s <= end)
                {
                    date = s.ToString(dateType);
                    beforeDate = scope == "month" ? s.AddMonths(-1).ToString(dateType) : s.AddYears(-1).ToString(dateType);

                    dateList.Add(s.ToString(dateType));
                    var logDay = appLogList.FirstOrDefault(p => p.LogDate == date);
                    logList.Add(logDay != null ? logDay.LogCount : 0);

                    logDay = appLogBeforeList.FirstOrDefault(p => p.LogDate == beforeDate);
                    logBeforeList.Add(logDay != null ? logDay.LogCount : 0);

                    if (scope == "month")
                    {
                        s = s.AddDays(1);
                    }
                    else
                    {
                        s = s.AddMonths(1);
                    }
                }

                appLogTotal = logList.Count > 0 ? logList.Sum() : 0;
                appLogBeforeTotal = logBeforeList.Count > 0 ? logBeforeList.Sum() : 0;

                int appLogChain = 0;
                int userLogChain = 0;

                string appLogDesc = "";
                string userLogDesc = "";

                if (appLogTotal == 0)
                {
                    appLogDesc = "本期无数据";
                }
                else if (appLogBeforeTotal == 0)
                {
                    appLogDesc = "上期无数据";
                }
                else
                {
                    appLogChain = appLogTotal * 100 / appLogBeforeTotal - 100;
                }

                if (userLogTotal == 0)
                {
                    userLogDesc = "本期无数据";
                }
                else if (userLogBeforeTotal == 0)
                {
                    userLogDesc = "上期无数据";
                }
                else
                {
                    userLogChain = userLogTotal * 100 / userLogBeforeTotal - 100;
                }

                SetSuccessResult(resultObj, new { AppLogTotal = appLogTotal, AppLogBeforeTotal = appLogBeforeTotal, AppLogDesc = appLogDesc, AppLogChain = appLogChain
                                                , UserLogTotal = userLogTotal, UserLogBeforeTotal = userLogBeforeTotal, UserLogDesc = userLogDesc, UserLogChain = userLogChain
                                                , DateList = dateList, AppLogList = logList, AppLogBeforeList = logBeforeList });
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion
    }
}
