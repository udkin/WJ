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
    public class AppService : DbContext<WJ_T_App>
    {
        #region 单列模式
        private static AppService _instance = null;

        private AppService() { }

        public static AppService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("AppService")
                        if (_instance == null)
                            _instance = new AppService();

                return _instance;
            }
        }
        #endregion

        public WJ_V_UserApp GetAppLoginInfo(int userId, int appId)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Queryable<WJ_V_UserApp>().Where(p => p.UserId == userId && p.AppId == appId).First();
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
