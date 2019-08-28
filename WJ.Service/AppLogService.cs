using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Common;
using WJ.Entity;

namespace WJ.Service
{
    public class AppLogService : DbContext<WJ_T_AppLog>
    {
        #region 单列模式
        private static AppLogService _instance = null;

        private AppLogService() { }

        public static AppLogService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("AppLogService")
                        if (_instance == null)
                            _instance = new AppLogService();

                return _instance;
            }
        }
        #endregion

        #region 返回应用操作日志
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<WJ_T_AppLog> GetAppLogList(JObject data, ref int totalCount)
        {
            try
            {
                using (var db = DbInstance)
                {
                    int pageIndex = data["page"].ToObject<int>();
                    int pageSize = data["limit"].ToObject<int>();
                    string appClassName = data["AppClassName"] != null ? data["AppClassName"].ToString().Trim() : "";
                    string appName = data["AppName"] != null ? data["AppName"].ToString().Trim() : "";
                    string userName = data["UserName"] != null ? data["UserName"].ToString().Trim() : "";

                    return DbInstance.Queryable<WJ_T_AppLog>()
                    .WhereIF(!string.IsNullOrWhiteSpace(appClassName), p => p.AppLog_AppClassName.Contains(appClassName))
                    .WhereIF(!string.IsNullOrWhiteSpace(appName), p => p.AppLog_AppName.Contains(appName))
                    .WhereIF(!string.IsNullOrWhiteSpace(userName), p => p.AppLog_UserName.Contains(userName))
                    .OrderBy(p => p.AppLog_Time, OrderByType.Desc)
                    .ToPageList(pageIndex, pageSize, ref totalCount);
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion

        #region 返回首页最新应用操作日志
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<WJ_T_AppLog> GetTopAppLogList(int top)
        {
            try
            {
                return DbInstance.Queryable<WJ_T_AppLog>()
                .OrderBy(p => p.AppLog_Time, OrderByType.Desc)
                .Take(top)
                .ToList();
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion


        #region 获取应用访问图表数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public List<WJ_TP_AppLog> GetAppLogData(DateTime start, DateTime end, string scope)
        {
            try
            {
                string sql = "";
                if (scope == "month")
                {
                    sql = string.Format(@"select CONVERT(char(10),a.AppLog_Time,120) LogDate,COUNT(1) LogCount from WJ_T_AppLog a where a.AppLog_Time between '{0}' and '{1}'
                                          group by CONVERT(char(10), a.AppLog_Time, 120)", start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd") + " 23:59:59.999");
                }
                else
                {
                    sql = string.Format(@"select CONVERT(char(7),a.AppLog_Time,120) LogDate,COUNT(1) LogCount from WJ_T_AppLog a where a.AppLog_Time between '{0}' and '{1}'
                                          group by CONVERT(char(7), a.AppLog_Time, 120)", start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd") + " 23:59:59.999");
                }

                return new DbContext<WJ_TP_AppLog>().GetList(sql);
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
