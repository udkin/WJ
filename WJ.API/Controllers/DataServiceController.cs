using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using WJ.API.Models;
using WJ.Service;
using WJ.Common;
using WJ.Entity;

namespace WJ.API.Controllers
{
    public class DataServiceController : ApiBaseController
    {
        #region MyRegion

        #endregion

        #region 外网统一访问接口
        /// <summary>
        /// 外网统一访问接口
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        //public IHttpActionResult Login(dynamic request)
        public dynamic GetData([FromBody]dynamic requestData)
        {
            WJ_T_User userInfo = null;
            dynamic resultObj = new { Success = 0, Code = 1001, ResultData = "", ErrorMsg = "无效身份证凭证" };
            try
            {
                if (requestData.Action != null && requestData.BodyData != null)
                {
                    string action = requestData.Action.ToString().ToLower();
                    if (action != "userlogin" && requestData.Token != null)
                    {
                        string token = requestData.Token.ToString();
                        if (TokenService.Instance.CheckToken(token) == true)
                        {
                            userInfo = UserService.Instance.GetUserByToken(token);
                        }

                        if (userInfo == null)
                        {
                            return Json<dynamic>(resultObj);
                        }

                        TokenService.Instance.UpdateTokenTimeLimit(token);
                    }

                    MethodInfo method = this.GetType().GetMethod(requestData.Action.ToString());
                    if (action == "userlogin")
                    {
                        resultObj = method.Invoke(this, new object[] { requestData.BodyData });
                    }
                    else
                    {
                        resultObj = method.Invoke(this, new object[] { userInfo, requestData.BodyData });
                    }
                }
            }
            catch (Exception ex)
            {
                resultObj = new { Success = 0, Code = 0, ResultData = "", ErrorMsg = ex.Message };
                LogHelper.ErrorLog(ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 用户登录获取Token
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public dynamic UserLogin(dynamic requestData)
        {
            dynamic resultObj = new { Success = 0, Code = 0, ResultData = "", ErrorMsg = "登录失败" };
            try
            {
                string userName = requestData.UserName.ToString().ToLower();
                string password = requestData.Password.ToString().ToLower();

                int userId = UserService.Instance.UserLogin(userName, password);

                if (userId > -1)
                {
                    // Token有效期
                    int tokenTimeLimit = int.Parse(SystemMapService.Instance.GetMapValue("TokenTimeLimit"));

                    //AuthInfo authInfo = new AuthInfo()
                    //{
                    //    UserId = userId,
                    //    IsSuperAdmin = userId == 0,
                    //    CreateTime = DateTime.Now,
                    //    TokenTimeLimit = DateTime.Now.AddSeconds(tokenTimeLimit)
                    //    //,RoleMenu = UserService.Instance.GetUserControllerName(userId)
                    //};
                    //string token = JWTService.Instance.CreateToken("");
                    string prefix = SystemMapService.Instance.GetMapValue("TokenPrefix1");
                    string token = prefix + Guid.NewGuid().ToString().Replace("-", "");

                    UserService.Instance.UpdateUserToken(userId, token);

                    WJ_T_Token tokenInfo = new WJ_T_Token();
                    tokenInfo.UserId = userId;
                    tokenInfo.Token_Ip = ((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                    tokenInfo.Token_Value = token;
                    tokenInfo.Token_CreateTime = DateTime.Now;
                    tokenInfo.Token_TimeLimit = DateTime.Now.AddSeconds(tokenTimeLimit);
                    TokenService.Instance.Insert(tokenInfo);

                    resultObj = new { Success = 1, Code = 1, ResultData = new { Access_Token = token }, ErrorMsg = "" };
                }
            }
            catch (Exception ex)
            {
                resultObj = new { Success = 0, Code = 0, ResultData = "", ErrorMsg = ex.Message };
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }
            return resultObj;
        }
        #endregion

        #region 获取用户APP分类和APP列表
        /// <summary>
        /// 获取用户APP列表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public dynamic GetAppClassAndAppList(WJ_T_User userInfo, dynamic requestData)
        {
            dynamic resultObj = new { Success = 0, Code = 0, ResultData = "", ErrorMsg = "获取用户APP列表失败" };
            try
            {
                var appClassList = UserAppClassService.Instance.GetUserAppClass(userInfo.Id);
                var appList = UserAppService.Instance.GetUserAppList(userInfo.Id);
                List<dynamic> userAppClassAndAppList = new List<dynamic>();
                Dictionary<int, List<dynamic>> appDict = new Dictionary<int, List<dynamic>>();

                foreach (var item in appList)
                {
                    if (!appDict.ContainsKey(item.AppClassId))
                    {
                        appDict.Add(item.AppClassId, new List<dynamic>());
                    }

                    appDict[item.AppClassId].Add(new
                    {
                        AppId = item.AppId,
                        App_Name = item.App_Name,
                        App_Image = item.App_Image,
                        App_BrowserType = item.App_BrowserType
                    });
                }

                foreach (var appClass in appClassList)
                {
                    userAppClassAndAppList.Add(new
                    {
                        AppClassId = appClass.AppClassId,
                        AppClass_Name = appClass.AppClass_Name,
                        AppClass_Image = appClass.AppClass_Image,
                        AppList = appDict[appClass.AppClassId]
                    });
                }
                resultObj = new { Success = 1, Code = 1, ResultData = userAppClassAndAppList, ErrorMsg = "" };
            }
            catch (Exception ex)
            {
                resultObj = new { Success = 0, Code = 0, ResultData = "", ErrorMsg = ex.Message };
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }
            return resultObj;
        }
        #endregion

        #region 获取用户APP列表
        /// <summary>
        /// 获取用户APP列表
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public dynamic GetAppList(WJ_T_User userInfo, dynamic requestData)
        {
            dynamic resultObj = new { Success = 0, Code = 0, ResultData = "", ErrorMsg = "获取用户APP列表失败" };
            try
            {
                dynamic userAppList = UserAppService.Instance.GetUserAppDynamic(userInfo.Id);
                resultObj = new { Success = 1, Code = 1, ResultData = userAppList, ErrorMsg = "" };
            }
            catch (Exception ex)
            {
                resultObj = new { Success = 0, Code = 0, ResultData = "", ErrorMsg = ex.Message };
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }
            return resultObj;
        }
        #endregion

        #region 平台APP登录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public HttpResponseMessage PlatformLogin(WJ_T_User userInfo, string appId)
        {
            var appInfo = AppService.Instance.GetAppLoginInfo(userInfo.Id, Convert.ToInt32(appId));
            HttpResponseMessage redirectResponse = new HttpResponseMessage(HttpStatusCode.Moved);
            if (appInfo == null)
            {
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

                string cookie = request.Headers.GetValues("Cookie")[0];//提取Cookie中的Session信息
                response.Dispose();
                request.Abort();

                //HttpContext.Current.Response.Headers.Add("Cookie", session);
                //HttpContext.Current.Response.Redirect(appInfo.App_HomeUrl);//跳转首页

                //request = (HttpWebRequest)HttpWebRequest.Create(new Uri(appInfo.App_HomeUrl));//具体session才能访问的页
                //request.Headers.Add("Cookie", session);
                //response = request.GetResponse() as HttpWebResponse;
                //var resStream = new StreamReader(response.GetResponseStream());//取到返回值
                //string content = resStream.ReadToEnd();//显示返回值
                //resStream.Close();
                //resStream.Dispose();
                //response.Dispose();
                //request.Abort();

                //var response1 = new HttpResponseMessage();
                //response1.Content = new StringContent(content);
                //response1.Headers.Add("Cookie", session);
                //response1.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                //return response1;

                redirectResponse.Headers.Location = new Uri(appInfo.App_HomeUrl);
                redirectResponse.Headers.Add("Cookie", cookie);
                //redirectResponse.Content = new StringContent(appInfo.App_HomeUrl);
                //redirectResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                return redirectResponse;

                //HttpContext.Current.Response.Headers.Add("Cookie", cookie);
                //HttpContext.Current.Response.Redirect("appInfo.App_HomeUrl");
            }
        }

        public HttpResponseMessage AppLogin(WJ_T_User userInfo, dynamic requestData)
        {
            string appId = requestData.AppId.ToString();
            var appInfo = AppService.Instance.GetAppLoginInfo(userInfo.Id, Convert.ToInt32(appId));
            if (appInfo == null)
            {
                var redirectResponse = new HttpResponseMessage(HttpStatusCode.Moved);
                redirectResponse.Headers.Location = new Uri("/404.html");
                return redirectResponse;
            }
            else
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(string.Format(appInfo.App_Form, appInfo.UserApp_LoginName, appInfo.UserApp_Password), Encoding.UTF8, "text/html")
                };
                return result;
            }
        }
        #endregion
    }
}
