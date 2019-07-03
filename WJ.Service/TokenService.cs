using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Entity;

namespace WJ.Service
{
    public class TokenService : DbContext<WJ_T_Token>
    {
        #region 单列模式
        private static TokenService _instance = null;

        private TokenService() { }

        public static TokenService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("TokenService")
                        if (_instance == null)
                            _instance = new TokenService();

                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 注销所有用户Token
        /// </summary>
        /// <param name="userId"></param>
        public void LogoutUserToken(int userId)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    db.Updateable<WJ_T_Token>().SetColumns(p => new WJ_T_Token() { Token_State = 0 }).Where(p => p.UserId == userId && p.Token_State == 1).ExecuteCommand();
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.ErrorLog(ex.Message);
            }
        }
    }
}
