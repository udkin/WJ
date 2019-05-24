using Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJ.DAL
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public string UserLogin(string loginName)
        {
            string password = "";
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    var user = db.Queryable<WJ_T_User>().Where(p => p.User_LoginName == loginName).First();
                    if (user != null)
                    {
                        password = user.User_Password;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return password;
        }
    }
}
