using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Common;
using WJ.Entity;

namespace WJ.Service
{
    public class UesrPlanService : DbContext<WJ_T_UserPlan>
    {
        #region 单列模式
        private static UesrPlanService _instance = null;

        private UesrPlanService() { }

        public static UesrPlanService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("UesrPlanService")
                        if (_instance == null)
                            _instance = new UesrPlanService();

                return _instance;
            }
        }
        #endregion

        #region 获取用户方案列表信息
        /// <summary>
        /// 获取前台操作员列表信息
        /// </summary>
        /// <returns></returns>
        public List<WJ_V_User> GetUserList(JObject data, ref int totalCount)
        {
            try
            {
                int pageIndex = data["page"].ToObject<int>();
                int pageSize = data["limit"].ToObject<int>();
                string name = (data["User_Name"] == null ? "" : data["User_Name"].ToString().Trim());
                string telphone = (data["telphone"] == null ? "" : data["telphone"].ToString().Trim());

                var queryable = DbInstance.Queryable<WJ_V_User>().Where(p => p.User_Type > 2 && p.User_State == 1)
                    .WhereIF(!string.IsNullOrWhiteSpace(name), p => p.User_Name.Contains(name))
                    .WhereIF(!string.IsNullOrWhiteSpace(telphone), p => p.User_Name.Contains(telphone))
                    .OrderBy(p => p.User_Type)
                    .OrderBy(p => p.User_CreateTime)
                    .ToPageList(pageIndex, pageSize, ref totalCount);

                return queryable;
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
