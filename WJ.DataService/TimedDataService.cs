using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WJ.Entity;
using WJ.Service;

namespace WJ.DataService
{
    public class TimedDataService
    {
        #region 单例模式
        private static TimedDataService _instance = new TimedDataService();

        public static TimedDataService Instance { get { return _instance; } }

        /// <summary>
        /// 构造方法
        /// </summary>
        private TimedDataService() { }
        #endregion

        #region 变量
        /// <summary>
        /// 
        /// </summary>
        private string _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["Defalut"].ConnectionString;
        /// <summary>
        /// 主线程
        /// </summary>
        private Thread mainThread = null;
        /// <summary>
        /// 循环访问Session
        /// </summary>
        private Thread sessionThread = null;
        /// <summary>
        /// 系统应用Session字典表
        /// </summary>
        private Dictionary<string, string> AppSessionDict = new Dictionary<string, string>();
        /// <summary>
        /// 系统应用接口列表
        /// </summary>
        private List<WJ_V_AppConfig> AppConfigList = new List<WJ_V_AppConfig>();
        /// <summary>
        /// 
        /// </summary>
        public AutoResetEvent mainEvent = new AutoResetEvent(false);
        /// <summary>
        /// 
        /// </summary>
        public AutoResetEvent callSessionEvent = new AutoResetEvent(false);
        #endregion

        #region 方法
        #region 启动、停止
        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            SqlDependency.Start(_connStr);
            InitSqlDependency();

            mainThread = new Thread(MainService);
            mainThread.IsBackground = true;
            mainThread.Start();

            sessionThread = new Thread(CycleCallSession);
            sessionThread.IsBackground = true;
            sessionThread.Start();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            SqlDependency.Stop(_connStr);

            if (mainThread != null)
            {
                mainThread.Abort();
                mainThread.DisableComObjectEagerCleanup();
            }

            if (sessionThread != null)
            {
                sessionThread.Abort();
                sessionThread.DisableComObjectEagerCleanup();
            }
        }
        #endregion

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
                            BatchLogin();
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
        /// 主服务
        /// </summary>
        protected void MainService()
        {
            BatchLogin();
            GetCycleAppConfig();
        }

        /// <summary>
        /// 批量登录获取对应Session
        /// </summary>
        protected void BatchLogin()
        {
            lock ("TimedData")
            {
                AppSessionDict.Clear();
                AppConfigList = AppConfigService.Instance.GetAppConfigList();

                foreach (var item in AppConfigList)
                {
                    if (AppSessionDict.ContainsKey(item.AppId + "_" + item.LoginName) == false)
                    {
                        string session = GetAppSession(item);
                        AppSessionDict.Add(item.AppId + "_" + item.LoginName, session);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetCycleAppConfig()
        {
            int minTimeSpan = 0;
            while (true)
            {
                lock ("TimedData")
                {
                    foreach (var AppConfig in AppConfigList)
                    {
                        if (AppConfig.RunTime == null || AppConfig.RunTime >= DateTime.Now)
                        {
                            if (AppConfig.RunTime == null)
                            {
                                AppConfig.RunTime = DateTime.Now;
                            }

                            GetHttpData(AppConfig);
                            AppConfig.RunTime.Value.AddSeconds(AppConfig.AppConfig_Cycle);
                        }
                    }
                }

                DateTime minTime = AppConfigList.Where(p => p.RunTime != null).Min(s => s.RunTime).Value;//获取最小的运行时间
                minTimeSpan = Convert.ToInt32((minTime - DateTime.Now).TotalMilliseconds);//获得与当前时间比较的差值毫秒数

                if (minTimeSpan > 0)
                {
                    mainEvent.WaitOne(minTimeSpan);
                    mainEvent.Reset();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AppConfig"></param>
        /// <returns></returns>
        public string GetHttpData(WJ_V_AppConfig AppConfig)
        {
            var postUrl = AppConfig.AppConfig_Url;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(postUrl);//具体session才能访问的页
            //request.ContentType = "application/x-www-form-urlencoded";
            string session = AppSessionDict[AppConfig.AppId + "_" + AppConfig.LoginName];

            if (AppConfig.App_Method.ToUpper() == "GET")
            {
                request = (HttpWebRequest)HttpWebRequest.Create(AppConfig.AppConfig_Url + "?" + AppConfig.AppConfig_Parameter);//访问登录页
                request.Method = AppConfig.App_Method;
                request.Headers.Add("Cookie", session);
            }
            else
            {
                var buf = Encoding.UTF8.GetBytes(AppConfig.AppConfig_Parameter);
                request = (HttpWebRequest)HttpWebRequest.Create(AppConfig.AppConfig_Url);//访问登录页
                request.Method = AppConfig.App_Method;
                request.Headers.Add("Cookie", session);
                request.ContentLength = buf.Length;

                //向提交流中写入信息
                var writeStream = request.GetRequestStream();
                writeStream.Write(buf, 0, buf.Length);
                writeStream.Close();
                writeStream.Dispose();
            }

            var resStream = new StreamReader(request.GetResponse().GetResponseStream());//取到返回值
            string result = resStream.ReadToEnd();//显示返回值
            resStream.Close();
            resStream.Dispose();
            request.Abort();

            return result;
        }

        /// <summary>
        /// 获取使用系统应用的Session
        /// </summary>
        /// <param name="AppConfig"></param>
        /// <returns></returns>
        public string GetAppSession(WJ_V_AppConfig AppConfig)
        {
            var content = string.Format("UserAccount={0}&Password={1}", AppConfig.LoginName, AppConfig.Password);//登录名和密码
            var buf = Encoding.UTF8.GetBytes(content);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(AppConfig.App_LoginUrl);//访问登录页
            request.Method = AppConfig.App_Method;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = buf.Length;
            request.CookieContainer = new CookieContainer();
            //res.CookieContainer = cookies;

            //向提交流中写入信息
            var writeStream = request.GetRequestStream();
            writeStream.Write(buf, 0, buf.Length);
            writeStream.Close();
            writeStream.Dispose();

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;//此句完成登录，无此句无法得到cookie
            Stream stream = response.GetResponseStream();
            StreamReader resStream = new StreamReader(stream, Encoding.UTF8);
            resStream.Close();
            resStream.Dispose();
            request.Abort();

            return request.Headers.GetValues("Cookie")[0];
        }

        /// <summary>
        /// 循环访问保持Session
        /// </summary>
        /// <returns></returns>
        public void CycleCallSession()
        {
            int cycleTime = SystemMapService.Instance.GetMapValueToInt("CycleCallSession") * 1000;
            while (true)
            {
                callSessionEvent.WaitOne(cycleTime);//暂停时间
                callSessionEvent.Reset();//重置状态，可以再次终止

                lock ("TimedData")
                {
                    foreach (var item in AppSessionDict.Keys)
                    {
                        //string appId = item.Split('_')[0];
                        //string loginName = item.Split('_')[0];
                        string session = AppSessionDict[item];

                        var AppConfig = AppConfigList.FirstOrDefault(p => p.AppId == 1);

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(AppConfig.App_HomeUrl);
                        request.Headers.Add("Cookie", session);
                        request.GetResponse();
                        request.Abort();
                    }
                }
            }
        }
        #endregion
    }
}
