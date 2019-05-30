using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJ.DAL
{
    public class TokenService
    {
        #region 单列模式
        private static TokenService _instance = null;

        private TokenService() { }

        public TokenService Instance
        {
            get
            {
                if (null == _instance)
                    lock ("TokenService")
                        if (null == _instance)
                            _instance = new TokenService();

                return _instance;
            }
        } 
        #endregion
    }
}
