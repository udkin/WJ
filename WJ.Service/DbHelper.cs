using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJ.Service
{
    public class DbHelper
    {
        /// <summary>
        /// Create SqlSugarClient
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Defalut"].ConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            //Print sql
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                System.Diagnostics.Debug.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
            };
            return db;
        }
    }
}
