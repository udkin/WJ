using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WJ.Service;

namespace WJ.DataService
{
    public class Common
    {
        #region 单列模式
        private static Common _instance = null;

        private Common() { }

        public static Common Instance
        {
            get
            {
                if (_instance == null)
                    lock ("Login")
                        if (_instance == null)
                            _instance = new Common();

                return _instance;
            }
        }
        #endregion

        public static string Cookie { set; get; }
        //public static CookieContainer CookieContainers { set; get; }

        public void SystemLogin()
        {
            string loginName = SystemMapService.Instance.GetMapValue("LoginName");
            string password = SystemMapService.Instance.GetMapValue("Password");

            var content = string.Format("UserAccount={0}&Password={1}", loginName, password);//登录名和密码
            var buf = Encoding.UTF8.GetBytes(content);

            var loginUrl = "http://117.78.34.120:8007/";//提交登录地址
            //var postUrl = "http://117.78.34.120:8007/BigData/GetMapData"; //?rnd=0." + DateTime.Now.Ticks;//此页需登录后才能访问

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(loginUrl);
            request.Method = "post";
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
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string str = sr.ReadToEnd();
        }
    }
}
