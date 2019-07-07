using Sodao.FastSocket.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJ.DataService
{
    public class AlarmService
    {
        #region 单例模式
        private static AlarmService _instance = new AlarmService();

        public static AlarmService Instance { get { return _instance; } }

        /// <summary>
        /// 构造方法
        /// </summary>
        private AlarmService() { }
        #endregion

        #region 变量
        private string _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["Defalut"].ConnectionString;

        #endregion

        #region 方法
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            SqlDependency.Start(_connStr);

            SocketServerManager.Init();
            SocketServerManager.Start();
        }

        #region 初始化SQL数据库查询通知
        /// <summary>
        /// 初始化SQL数据库查询通知
        /// </summary>
        public void InitSqlDependency()
        {
            using (SqlConnection SqlConnection = new SqlConnection(_connStr))
            {
                SqlConnection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = SqlConnection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT Id,AppId,AppConfig_Name,AppConfig_Url,AppConfig_Method,AppConfig_DataType,AppConfig_Parameter,AppConfig_LoginName,AppConfig_Password,AppConfig_Cycle,AppConfig_State FROM WJ.dbo.WJ_T_AppConfig WHERE AppConfig_State = 1";

                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += Dependency_OnChange;

                    using (SqlDataReader sdr = command.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            string appId = sdr["AppId"].ToString();
                            string loginName = sdr["AppConfig_LoginName"].ToString();
                            System.Diagnostics.Debug.WriteLine("AppId:{0}\t LoginName:{1}", appId, loginName);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency dependency = sender as SqlDependency;
            dependency.OnChange -= Dependency_OnChange;
            if (e.Info != SqlNotificationInfo.Invalid)
            {
                InitSqlDependency();
            }
        }
        #endregion

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            SqlDependency.Stop(_connStr);

            
        }
        #endregion
    }
}