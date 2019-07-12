using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Entity;

namespace WJ.Service
{
    public class AppClassService : DbContext<WJ_T_AppClass>
    {
        #region 单列模式
        private static AppClassService _instance = null;

        private AppClassService() { }

        public static AppClassService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("AppDataService")
                        if (_instance == null)
                            _instance = new AppClassService();

                return _instance;
            }
        }
        #endregion
    }
}
