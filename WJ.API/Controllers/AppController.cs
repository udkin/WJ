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
using WJ.Common;
using WJ.Service;

namespace WJ.API.Controllers
{
    [ApiAuthorize]
    public class AppController : ApiBaseController
    {
        #region 登录系统应用
        /// <summary>
        /// 登录系统应用
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        public HttpResponseMessage PlatformLogin()
        {
            int appId = 1;
            AuthInfo authInfo = this.RequestContext.RouteData.Values["access_token"] as AuthInfo;
            var appInfo = AppService.Instance.GetAppLoginInfo(authInfo.UserId, appId);
            if (appInfo == null)
            {
                HttpResponseMessage redirectResponse = new HttpResponseMessage(HttpStatusCode.Moved);
                redirectResponse.Headers.Location = new Uri("/404.html");
                return redirectResponse;
            }
            else
            {
                //var content = string.Format("UserAccount={0}&Password={1}", appInfo.LoginName, appInfo.Password);//登录名和密码
                var parameter = string.Format(appInfo.App_Paramater, appInfo.UserApp_LoginName, appInfo.UserApp_Password);//登录名和密码

                HttpWebRequest request = null;
                if (appInfo.App_Method.ToUpper() == "GET")
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(appInfo.App_LoginUrl + "?" + parameter);//访问登录页
                    request.Method = appInfo.App_Method;
                    request.ContentType = "application/x-www-form-urlencoded";
                }
                else
                {
                    var buf = Encoding.UTF8.GetBytes(parameter);
                    request = (HttpWebRequest)HttpWebRequest.Create(appInfo.App_LoginUrl);//访问登录页
                    request.Method = appInfo.App_Method;
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = buf.Length;
                    request.CookieContainer = new CookieContainer();

                    //向提交流中写入信息
                    var writeStream = request.GetRequestStream();
                    writeStream.Write(buf, 0, buf.Length);
                    writeStream.Close();
                    writeStream.Dispose();
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;//此句完成登录，无此句无法得到cookie
                //Stream stream = response.GetResponseStream();
                //StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                //string str = sr.ReadToEnd();

                string session = request.Headers.GetValues("Cookie")[0];//提取Cookie中的Session信息
                response.Dispose();
                request.Abort();

                //HttpContext.Current.Response.Headers.Add("Cookie", session);
                //HttpContext.Current.Response.Redirect(appInfo.App_HomeUrl);//跳转首页

                request = (HttpWebRequest)HttpWebRequest.Create(new Uri(appInfo.App_HomeUrl));//具体session才能访问的页
                request.Headers.Add("Cookie", session);
                response = request.GetResponse() as HttpWebResponse;
                var resStream = new StreamReader(response.GetResponseStream());//取到返回值
                string content = resStream.ReadToEnd();//显示返回值
                resStream.Close();
                resStream.Dispose();
                response.Dispose();
                request.Abort();

                var response1 = new HttpResponseMessage();
                response1.Content = new StringContent(content);
                response1.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                return response1;

                //redirectResponse.Headers.Location = new Uri();
                //redirectResponse.Headers.Add("Cookie", session);
                //redirectResponse.Content = new StringContent(appInfo.App_HomeUrl);
                //redirectResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                //return redirectResponse;
            }
        }

        public string GetHtml(string url, string cookie)
        {
            var postUrl = "http://117.78.34.120:8007/main/mainback"; //?rnd=0." + DateTime.Now.Ticks;//此页需登录后才能访问
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(postUrl));//具体session才能访问的页
            request.Headers.Add("Cookie", cookie);
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            var resStream = new StreamReader(response.GetResponseStream());//取到返回值
            string content = resStream.ReadToEnd();//显示返回值
            resStream.Close();
            resStream.Dispose();
            response.Dispose();
            request.Abort();
            return content;

            //var currentRunPath = AppDomain.CurrentDomain.BaseDirectory;
            //var substringBin = currentRunPath.IndexOf("bin");
            //var path = currentRunPath.Substring(0, substringBin) + "Index.html";
            //var httpResponseMessage = new HttpResponseMessage();
            //httpResponseMessage.Content = new StringContent(File.ReadAllText(path), Encoding.UTF8);
            //httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            //return httpResponseMessage;
        }
        #endregion
    }
}
