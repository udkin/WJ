using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using WJ.Entity;
using WJ.Service;

namespace WJ.API
{
    /// <summary>
    /// AppLoginHandler 的摘要说明
    /// </summary>
    public class AppLoginHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            if (request.HttpMethod.ToLower() == "get")
            {
                if (!string.IsNullOrWhiteSpace(request["Token"]) && !string.IsNullOrWhiteSpace(request["AppId"]))
                {
                    if (TokenService.Instance.CheckToken(request["Token"].Trim()) == true)
                    {
                        WJ_T_User userInfo = UserService.Instance.GetUserByToken(request["Token"].Trim());
                        if (userInfo != null)
                        {
                            LoginApp(userInfo, request["AppId"].Trim());
                        }
                        else
                        {
                            context.Response.Redirect("/404.html");
                        }
                    }
                }
                else
                {
                    context.Response.Redirect("/404.html");
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void LoginApp(WJ_T_User userInfo, string appId)
        {
            var appInfo = AppService.Instance.GetAppLoginInfo(userInfo.Id, Convert.ToInt32(appId));
            HttpContext.Current.Response.ContentType = "text/html";
            if (appInfo == null)
            {
                HttpContext.Current.Response.Redirect("/404.html");
            }
            else
            {
                HttpContext.Current.Response.Write(string.Format(appInfo.AppConfig_Form, appInfo.UserApp_LoginName, appInfo.UserApp_Password));
            }
        }
    }
}