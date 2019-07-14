using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Entity;

namespace WJ.Service
{
    public class TitleService : DbContext<WJ_T_Title>
    {
        #region 单列模式
        private static TitleService _instance = null;

        private TitleService() { }

        public static TitleService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("TitleService")
                        if (_instance == null)
                            _instance = new TitleService();

                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetAllTitleList()
        {
            try
            {
                using (SqlSugarClient db = DbInstance)
                {
                    return db.Queryable<WJ_T_Title>().Where(p => p.Title_State == 1).OrderBy(p => p.Title_Sort).Select(f => new { f.Id, f.Title_Name }).ToList();
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
