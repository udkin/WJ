using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WJ.Common
{
    public enum LogType
    {
        Debug,
        Error,
        Service,
        Controller,
        SiteConfig,
        SQL
    }

    public class LogHelper
    {
        /// <summary>
        /// 调式日志，用于调式日志输出
        /// </summary>
        /// <param name="log"></param>
        /// <param name="type"></param>
        public static void DebugLog(string log, LogType type = LogType.Debug)
        {
            lock ("Debug") //防止并发异常
            {
                try
                {
                    string LogName = DateTime.Now.ToString("yyyyMMdd") + string.Format("_{0}.txt", type.ToString()); //按天日志
                    string logfile = HttpRuntime.AppDomainAppPath.ToString() + "log\\" + LogName;
                    System.IO.StreamWriter sw = System.IO.File.AppendText(logfile);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff ") + log);
                    sw.WriteLine("---------------");
                    sw.Close();
                }
                catch { }
            }
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="log"></param>
        public static void DbServiceLog(string log)
        {
#if DEBUG
            WriteLine("【数据操作异常】：" + log);
#else
            DebugLog(log, LogType.Service);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public static void ControllerErrorLog(string log)
        {
#if DEBUG
            WriteLine("【控制器异常】：" + log);
#else
            DebugLog(log, LogType.Controller);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public static void DbSqlLog(string log)
        {
            if (ConfigHelper.WebSiteConfig["WriteSql"] == "true")
            {
                DebugLog(log, LogType.SQL);
            }
            else
            {
                WriteLine("【SQL脚本】：" + log);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public static void WriteLine(string log)
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff ") + log);
            System.Diagnostics.Debug.WriteLine("");
        }
    }
}
