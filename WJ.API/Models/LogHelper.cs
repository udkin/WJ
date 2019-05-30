using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WJ.API.Models
{
    public class LogHelper
    {
        #region 单列模式
        private static LogHelper _instance = null;

        private LogHelper() { }

        public static LogHelper Instance
        {
            get
            {
                if (_instance == null)
                    lock ("LogHelper")
                        if (_instance == null)
                            _instance = new LogHelper();

                return _instance;
            }
        }
        #endregion

        private object lockLog = new object(); //日志排他锁
        /// <summary>
        /// 调式日志，用于调式日志输出
        /// </summary>
        /// <param name="log"></param>
        internal void Debuglog(string log, string logname = "_Debuglog.txt")
        {
            lock (lockLog) //防止并发异常
            {
                try
                {
                    string LogName = DateTime.Now.ToString("yyyyMMdd") + logname; //按天日志
                    string logfile = HttpRuntime.AppDomainAppPath.ToString() + "log/" + LogName;
                    System.IO.StreamWriter sw = System.IO.File.AppendText(logfile);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
                    sw.WriteLine("---------------");
                    sw.Close();
                }
                catch
                {

                }
            }
        }
    }
}
