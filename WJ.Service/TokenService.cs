using Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJ.Service
{
    public class TokenService
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

        public bool Add(WJ_T_Token token)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Insertable<WJ_T_Token>(token).ExecuteCommandIdentityIntoEntity();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(WJ_T_Token token)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    db.Updateable<WJ_T_Token>(token);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
