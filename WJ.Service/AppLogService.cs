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

        #region MyRegion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userApp"></param>
        /// <returns></returns>
        public bool Add(WJ_V_UserApp userApp)
        {
            try
            {
                WJ_T_AppLog appLog = new WJ_T_AppLog();
                appLog.AppLog_UserId = userApp.UserId;
                appLog.AppLog_UserName = userApp.User_Name;
                appLog.AppLog_AppClassId = userApp.AppClassId;
                appLog.AppLog_AppClassName = userApp.AppClass_Name;
                appLog.AppLog_AppId = userApp.AppId;
                appLog.AppLog_AppName = userApp.App_Name;
                appLog.AppLog_LoginName = userApp.LoginName;
                appLog.AppLog_Password = userApp.Password;
                appLog.AppLog_Time = DateTime.Now;

                return Add(appLog);
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
            }

            return false;
        }
        #endregion
    }
}
