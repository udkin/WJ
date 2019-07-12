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
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.ErrorLog(ex.Message);
            }
        }

        /// <summary>
        /// 检查Token有效性
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool CheckToken(string token)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Ado.GetString(string.Format(@"select COUNT(1) from WJ_T_Token where Id in (select MAX(id) from WJ_T_Token 
                                                        where UserId in (select Id from WJ_T_User where User_Token = '{0}'))
                                                        and Token_TimeLimit >= GETDATE() and Token_Value = '{0}'", token)) == "1";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.ErrorLog(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public void UpdateTokenTimeLimit(string token)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    db.Ado.ExecuteCommand(string.Format("update WJ_T_Token set Token_TimeLimit = DATEADD(S,(select CAST(SystemMap_Value as int) from WJ_T_SystemMap where SystemMap_Type = 'TokenTimeLimit'),GETDATE()) where Token_Value = '{0}'", token));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.ErrorLog(ex.Message);
            }
        }
    }
}
