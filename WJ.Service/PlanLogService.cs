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
    public class PlanLogService : DbContext<WJ_T_PlanLog>
    {
        #region 单列模式
        private static PlanLogService _instance = null;

        private PlanLogService() { }

        public static PlanLogService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("PlanLogService")
                        if (_instance == null)
                            _instance = new PlanLogService();

                return _instance;
            }
        }
        #endregion

        #region 返回方案操作日志
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<WJ_T_PlanLog> GetPlanLogList(JObject data, ref int totalCount)
        {
            try
            {
                using (var db = DbInstance)
                {
                    int pageIndex = data["page"].ToObject<int>();
                    int pageSize = data["limit"].ToObject<int>();
                    string planName = data["PlanName"] != null ? data["PlanName"].ToString().Trim() : "";
                    string userName = data["UserName"] != null ? data["UserName"].ToString().Trim() : "";

                    return DbInstance.Queryable<WJ_T_PlanLog>()
                    .WhereIF(!string.IsNullOrWhiteSpace(planName), p => p.PlanLog_PlanIdName.Contains(planName))
                    .WhereIF(!string.IsNullOrWhiteSpace(userName), p => p.PlanLog_UserName.Contains(userName))
                    .OrderBy(p => p.PlanLog_Time, OrderByType.Desc)
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
    }
}
