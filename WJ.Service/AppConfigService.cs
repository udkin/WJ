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
    public class AppConfigService : DbContext<WJ_T_AppConfig>
    {
        #region 单列模式
        private static AppConfigService _instance = null;

        private AppConfigService() { }

        public static AppConfigService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("AppDataService")
                        if (_instance == null)
                            _instance = new AppConfigService();

                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 获取应用接口信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public List<WJ_V_AppConfig> GetAppConfigList()
        {
            try
            {
                using (var db = DbInstance)
                {
                    return db.Queryable<WJ_V_AppConfig>().Where(p => p.AppConfig_Cycle > 0).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appConfigId"></param>
        /// <returns></returns>
        public WJ_V_AppConfig GetAddData(int appId, int appConfigId)
        {
            try
            {
                using (var db = DbInstance)
                {
                    return db.Queryable<WJ_V_AppConfig>().Where(p => p.AppId == appId && p.AppConfigId == appConfigId).First();
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
