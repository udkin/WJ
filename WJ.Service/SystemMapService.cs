using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Entity;

namespace WJ.Service
{
    public class SystemMapService
    {
        #region 单列模式
        private static SystemMapService _instance = null;

        private SystemMapService() { }

        public static SystemMapService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("SystemMapService")
                        if (_instance == null)
                            _instance = new SystemMapService();

                return _instance;
            }
        }
        #endregion

        public string GetMapValue(string mapType)
        {
            string value = string.Empty;
            try
            {
                using (SqlSugarClient db = DbHelper.GetInstance())
                {
                    value = db.Queryable<WJ_T_SystemMap>().Where(p => p.SystemMap_Type == mapType).Select(f => f.SystemMap_Value).First().ToString();
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.ErrorLog(ex.Message);
                Console.WriteLine(ex.Message);
            }
            return value;
        }
    }
}
