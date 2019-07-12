using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Entity;

namespace WJ.Service
{
    public class AppDataService : DbContext<WJ_T_AppData>
    {
        #region 单列模式
        private static AppDataService _instance = null;

        private AppDataService() { }

        public static AppDataService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("AppDataService")
                        if (_instance == null)
                            _instance = new AppDataService();

                return _instance;
            }
        }
        #endregion
    }
}
