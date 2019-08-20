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
    public class UserAppClassService : DbContext<WJ_V_UserAppClass>
    {
        #region 单列模式
        private static UserAppClassService _instance = null;

        private UserAppClassService() { }

        public static UserAppClassService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("UserAppClassService")
                        if (_instance == null)
                            _instance = new UserAppClassService();

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
        public List<WJ_V_UserAppClass> GetUserAppClass(int userId)
        {
            try
            {
                using (var db = DbInstance)
                {
                    return db.Queryable<WJ_V_UserAppClass>().Where(p => p.UserId == userId).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
    }
}
