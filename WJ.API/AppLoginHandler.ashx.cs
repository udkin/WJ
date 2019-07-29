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
                        WJ_T_User userInfo = new DbContext<WJ_T_User>().GetSingle(p => p.User_Token == request["Token"].Trim() && p.User_State == 1);
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
            var appInfo = new DbContext<WJ_V_UserApp>().GetSingle(p => p.UserId == userInfo.Id && p.AppId == Convert.ToInt32(appId));
            //var appInfo = AppService.Instance.GetAppLoginInfo(userInfo.Id, Convert.ToInt32(appId));
            HttpContext.Current.Response.ContentType = "text/html";
            if (appInfo == null)
            {
                HttpContext.Current.Response.Redirect("/404.html");
            }
            else
            {
                HttpContext.Current.Response.Write(string.Format(appInfo.App_Form, appInfo.LoginName, appInfo.Password));
            }
        }
    }
}