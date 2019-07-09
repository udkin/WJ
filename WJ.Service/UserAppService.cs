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
    public class UserAppService : DbContext<WJ_T_UserApp>
    {
        #region 单列模式
        private static UserAppService _instance = null;

        private UserAppService() { }

        public static UserAppService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("UserAppService")
                        if (_instance == null)
                            _instance = new UserAppService();

                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public List<WJ_V_UserApp> GetUserAppList(int userId)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    //return db.Queryable<WJ_V_UserApp>().Where(p => p.UserId == userId).Select(f => new { f.AppClassId, f.AppId, f.App_Name, f.App_Image, f.App_BrowserType }).ToList();
                    return db.Queryable<WJ_V_UserApp>().Where(p => p.UserId == userId).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public dynamic GetUserAppDynamic(int userId)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Queryable<WJ_V_UserApp>().Where(p => p.UserId == userId).Select(f => new { f.AppClassId, f.AppId, f.App_Name, f.App_Image, f.AppConfig_BrowserType, f.App_Type, f.App_Flag }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
