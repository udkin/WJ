using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Entity;

namespace WJ.Service
{
    public class RoleService : DbContext<WJ_T_Role>
    {
        #region 单列模式
        private static RoleService _instance = null;

        private RoleService() { }

        public static RoleService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("RoleService")
                        if (_instance == null)
                            _instance = new RoleService();

                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetAllRoleList()
        {
            try
            {
                using (SqlSugarClient db = DbInstance)
                {
                    return db.Queryable<WJ_T_Role>().Where(p => p.Role_State == 1).OrderBy(p => p.Role_Sort).Select(f => new { f.Id, f.Role_Name }).ToList();
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.DbServiceLog(ex.Message);
                return null;
            }
        }
    }
}
