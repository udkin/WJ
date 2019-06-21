using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJ.Service
{
    public class ServiceBase
    {
        public bool Add(dynamic token)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    return db.Insertable<dynamic>(token).ExecuteCommandIdentityIntoEntity();
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.ErrorLog(ex.Message);
                return false;
            }
        }

        public bool Update(dynamic token)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    db.Updateable<dynamic>(token);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.ErrorLog(ex.Message);
                return false;
            }
        }

        #region 删除
        public bool Delete(dynamic token)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    db.Updateable<dynamic>(token);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.ErrorLog(ex.Message);
                return false;
            }
        }

        public bool Delete<T>(int id)
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    //db.Deleteable<T>().In(id).ExecuteCommand();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.ErrorLog(ex.Message);
                return false;
            }
        }
        #endregion
    }
}
