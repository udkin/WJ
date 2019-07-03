using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Entity;

namespace WJ.Service
{
    public class DeptService : DbContext<WJ_T_Dept>
    {
        #region 单列模式
        private static DeptService _instance = null;

        private DeptService() { }

        public static DeptService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("DeptService")
                        if (_instance == null)
                            _instance = new DeptService();

                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetAllDeptList()
        {
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    //return db.Queryable<WJ_T_Dept>().Where(p => p.Dept_State == 1).ToList();
                    return db.Queryable<WJ_T_Dept>().Where(p => p.Dept_State == 1).Select(f => new { f.Id, f.Dept_Name }).ToList();
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.ErrorLog(ex.Message);
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
