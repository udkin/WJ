using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Entity;

namespace WJ.Service
{
    public class UserRoleService : DbContext<WJ_T_UserRole>
    {
        #region 单列模式
        private static UserRoleService _instance = null;

        private UserRoleService() { }

        public static UserRoleService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("UserRoleService")
                        if (_instance == null)
                            _instance = new UserRoleService();

                return _instance;
            }
        }
        #endregion
    }
}
