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
                        WJ_T_User userInfo = UserService.Instance.GetSingle(p => p.User_Token == request["Token"].Trim() && p.User_State == 1);
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
            var userApp = new DbContext<WJ_V_UserApp>().GetSingle(p => p.UserId == userInfo.Id && p.AppId == Convert.ToInt32(appId));
            //var appInfo = AppService.Instance.GetAppLoginInfo(userInfo.Id, Convert.ToInt32(appId));
            HttpContext.Current.Response.ContentType = "text/html";
            if (userApp == null)
            {
                HttpContext.Current.Response.Redirect("/404.html");
            }
            else
            {
                HttpContext.Current.Response.Write(string.Format(userApp.App_Form, userApp.LoginName, userApp.Password));

                WJ_T_AppLog appLog = new WJ_T_AppLog();
                appLog.AppLog_UserId = userApp.UserId;
                appLog.AppLog_UserName = userApp.User_Name;
                appLog.AppLog_UserTypeName = userApp.User_TypeName;
                appLog.AppLog_AppClassId = userApp.AppClassId;
                appLog.AppLog_AppClassName = userApp.AppClass_Name;
                appLog.AppLog_AppId = userApp.AppId;
                appLog.AppLog_AppName = userApp.App_Name;
                appLog.AppLog_LoginName = userApp.LoginName;
                appLog.AppLog_Password = userApp.Password;
                appLog.AppLog_Time = DateTime.Now;

                AppLogService.Instance.Add(appLog); // 记录用户访问APP信息s
            }
        }
    }
}