﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WJ.Common;
using WJ.Entity;

namespace WJ.Service
{
    public class AppService : DbContext<WJ_T_App>
    {
        #region 单列模式
        private static AppService _instance = null;

        private AppService() { }

        public static AppService Instance
        {
            get
            {
                if (_instance == null)
                    lock ("AppService")
                        if (_instance == null)
                            _instance = new AppService();

                return _instance;
            }
        }
        #endregion
    }
}
