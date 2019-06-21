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
    public class UserService
    {
        #region 单列模式
        private static UserService _instance = null;

        private UserService() { }

        public static UserService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("UserService")
                        if (_instance == null)
                            _instance = new UserService();

                return _instance;
            }
        }
        #endregion

        #region 验证用户登录信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public int UserLogin(string loginName, string password)
        {
            int userId = -1;
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    var user = db.Queryable<WJ_T_User>().Where(p => p.User_LoginName == loginName && p.User_State == 1).First();
                    if (user != null && user.User_Password == password)
                    {
                        return user.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex.Message);
                Console.WriteLine(ex.Message);
            }
            return userId;
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WJ_T_User GetUserInfo(int id)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Queryable<WJ_T_User>().Where(p => p.Id == id).First();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取后台管理员列表信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<WJ_V_User> GetManagerList(int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Queryable<WJ_V_User>().Where(p => p.User_Type == 1 && p.User_State == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取前台操作员列表信息
        /// <summary>
        /// 获取前台操作员列表信息
        /// </summary>
        /// <returns></returns>
        public List<WJ_V_User> GetUserList()
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Queryable<WJ_V_User>().Where(p => p.User_Type > 1 && p.User_State == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        #endregion
    }
}
