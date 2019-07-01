using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WJ.API.Models;
using WJ.Service;

namespace WJ.API.Controllers
{
    [ApiAuthorize]
    public class AppController : ApiBaseController
    {
        /// <summary>
        /// 系统应用平台登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage PlatformLogin(int appId)
        {
            var appInfo = AppService.Instance.GetAppLoginInfo(authInfo.UserId, appId);
            //var content = string.Format("UserAccount={0}&Password={1}", appInfo.LoginName, appInfo.Password);//登录名和密码
            var parameter = string.Format("{0}&{1}", appInfo.LoginName, appInfo.Password);//登录名和密码

            HttpWebRequest request = null;
            if (appInfo.App_Method.ToUpper() == "GET")
            {
                request = (HttpWebRequest)HttpWebRequest.Create(appInfo.App_LoginUrl + "?" + parameter);//访问登录页
            }
            else
            {
                var buf = Encoding.UTF8.GetBytes(parameter);
                request = (HttpWebRequest)HttpWebRequest.Create(appInfo.App_LoginUrl);//访问登录页
                request.ContentLength = buf.Length;
                request.CookieContainer = new CookieContainer();

                //向提交流中写入信息
                var writeStream = request.GetRequestStream();
                writeStream.Write(buf, 0, buf.Length);
                writeStream.Close();
                writeStream.Dispose();
            }

            request.Method = appInfo.App_Method;
            request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;//此句完成登录，无此句无法得到cookie
            //Stream stream = response.GetResponseStream();
            //StreamReader resStream = new StreamReader(stream, Encoding.UTF8);
            //resStream.Close();
            //resStream.Dispose();
            request.Abort();

            string session = request.Headers.GetValues("Cookie")[0];//提取Cookie中的Session信息

            //HttpContext.Current.Response.Headers.Add("Cookie", session);
            //HttpContext.Current.Response.Redirect(appInfo.App_HomeUrl);//跳转首页

            HttpResponseMessage response1 = new HttpResponseMessage(HttpStatusCode.Moved);
            response1.Headers.Location = new Uri(appInfo.App_HomeUrl);
            response1.Headers.Add("Cookie", session);
            return response1;
        }
    }
}
