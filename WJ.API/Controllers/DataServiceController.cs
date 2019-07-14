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
        #region 外网统一访问接口
        /// <summary>
        /// 外网统一访问接口
        /// </summary>
        /// <param name="requestData"></param>
        /// <returnsJSON结果集</returns>
        [HttpGet, HttpPost]
        public dynamic GetData(dynamic requestData)
        {
            WJ_T_User userInfo = null;
            ResultModel resultObj = new ResultModel { Success = 0, Code = 0, ErrorMsg = "无效身份证凭证" };
            try
            {
                if (requestData.Action != null && requestData.BodyData != null)
                {
                    string action = requestData.Action.ToString().ToLower().Trim();
                    if (action != "userlogin" && requestData.Token != null)
                    {
                        string token = requestData.Token.ToString().Trim();
                        if (TokenService.Instance.CheckToken(token) == true)
                        {
                            userInfo = UserService.Instance.GetUserByToken(token);
                        }

                        if (userInfo == null)
                        {
                            return Json<dynamic>(resultObj);
                        }

                        TokenService.Instance.UpdateTokenTimeLimit(token);//更新Token有效时间
                    }

                    MethodInfo method = this.GetType().GetMethod(requestData.Action.ToString().Trim());
                    if (method != null)
                    {
                        if (action == "userlogin")
                        {
                            resultObj = (ResultModel)method.Invoke(this, new object[] { requestData.BodyData });
                        }
                        else
                        {
                            resultObj = (ResultModel)method.Invoke(this, new object[] { userInfo, requestData.BodyData });
                        }
                    }
                    else
                    {
                        resultObj.ErrorMsg = "请调用正确的方法";
                    }
                }
            }
            catch (Exception ex)
            {
                resultObj.ErrorMsg = ex.Message;
                LogHelper.ControllerErrorLog(ex.Message);
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
        public ResultModel UserLogin(dynamic requestData)
        {
            ResultModel resultObj = new ResultModel { Success = 0, Code = 0, ErrorMsg = "登录失败" };
            try
            {
                string userName = requestData.UserName.ToString().ToLower();
                string password = requestData.Password.ToString().ToLower();

                int userId = UserService.Instance.UserLogin(userName, password);

                if (userId > -1)
                {
                    // Token有效期
                    int tokenTimeLimit = SystemMapService.Instance.GetMapValueToInt("TokenTimeLimit");

                    string prefix = SystemMapService.Instance.GetMapValue("TokenPrefix1");
                    string token = prefix + Guid.NewGuid().ToString().Replace("-", "");

                    UserService.Instance.UpdateUserToken(userId, token);

                    WJ_T_Token tokenInfo = new WJ_T_Token();
                    tokenInfo.UserId = userId;
                    tokenInfo.Token_Ip = ((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                    tokenInfo.Token_Value = token;
                    tokenInfo.Token_CreateTime = DateTime.Now;
                    tokenInfo.Token_TimeLimit = DateTime.Now.AddSeconds(tokenTimeLimit);
                    TokenService.Instance.Add(tokenInfo);

                    SetSuccessResult(resultObj, new { Access_Token = token });
                }
            }
            catch (Exception ex)
            {
                resultObj.ErrorMsg = ex.Message;
                LogHelper.ControllerErrorLog(ex.Message);
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
        public ResultModel GetAppClassAndAppList(WJ_T_User userInfo, dynamic requestData)
        {
            ResultModel resultObj = new ResultModel { Success = 0, Code = 0, ErrorMsg = "获取用户APP列表失败" };
            try
            {
                var appClassList = UserAppClassService.Instance.GetList(p => p.UserId == userInfo.Id);
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
                        App_BrowserType = item.AppConfig_BrowserType,
                        App_Type = item.App_Type,
                        App_Flag = item.App_Flag
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
                SetSuccessResult(resultObj, userAppClassAndAppList);
            }
            catch (Exception ex)
            {
                resultObj.ErrorMsg = ex.Message;
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
        public ResultModel GetAppList(WJ_T_User userInfo, dynamic requestData)
        {
            ResultModel resultObj = new ResultModel { Success = 0, Code = 0, ErrorMsg = "获取用户APP列表失败" };
            try
            {
                dynamic userAppList = UserAppService.Instance.GetUserAppDynamic(userInfo.Id);
                SetSuccessResult(resultObj, userAppList);
            }
            catch (Exception ex)
            {
                resultObj.ErrorMsg = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }
            return resultObj;
        }
        #endregion

        #region 平台APP登录
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="requestData"></param>
        ///// <returns></returns>
        //public HttpResponseMessage PlatformLogin(WJ_T_User userInfo, string appId)
        //{
        //    var appInfo = AppService.Instance.GetAppLoginInfo(userInfo.Id, Convert.ToInt32(appId));
        //    HttpResponseMessage redirectResponse = new HttpResponseMessage(HttpStatusCode.Moved);
        //    if (appInfo == null)
        //    {
        //        redirectResponse.Headers.Location = new Uri("/404.html");
        //        return redirectResponse;
        //    }
        //    else
        //    {
        //        //var content = string.Format("UserAccount={0}&Password={1}", appInfo.LoginName, appInfo.Password);//登录名和密码
        //        var parameter = string.Format(appInfo.App_Paramater, appInfo.UserApp_LoginName, appInfo.UserApp_Password);//登录名和密码

        //        HttpWebRequest request = null;
        //        if (appInfo.App_Method.ToUpper() == "GET")
        //        {
        //            request = (HttpWebRequest)HttpWebRequest.Create(appInfo.App_LoginUrl + "?" + parameter);//访问登录页
        //            request.Method = appInfo.App_Method;
        //            request.ContentType = "application/x-www-form-urlencoded";
        //        }
        //        else
        //        {
        //            var buf = Encoding.UTF8.GetBytes(parameter);
        //            request = (HttpWebRequest)HttpWebRequest.Create(appInfo.App_LoginUrl);//访问登录页
        //            request.Method = appInfo.App_Method;
        //            request.ContentType = "application/x-www-form-urlencoded";
        //            request.ContentLength = buf.Length;
        //            request.CookieContainer = new CookieContainer();

        //            //向提交流中写入信息
        //            var writeStream = request.GetRequestStream();
        //            writeStream.Write(buf, 0, buf.Length);
        //            writeStream.Close();
        //            writeStream.Dispose();
        //        }

        //        HttpWebResponse response = request.GetResponse() as HttpWebResponse;//此句完成登录，无此句无法得到cookie
        //        //Stream stream = response.GetResponseStream();
        //        //StreamReader sr = new StreamReader(stream, Encoding.UTF8);
        //        //string str = sr.ReadToEnd();

        //        string cookie = request.Headers.GetValues("Cookie")[0];//提取Cookie中的Session信息
        //        response.Dispose();
        //        request.Abort();

        //        //HttpContext.Current.Response.Headers.Add("Cookie", session);
        //        //HttpContext.Current.Response.Redirect(appInfo.App_HomeUrl);//跳转首页

        //        //request = (HttpWebRequest)HttpWebRequest.Create(new Uri(appInfo.App_HomeUrl));//具体session才能访问的页
        //        //request.Headers.Add("Cookie", session);
        //        //response = request.GetResponse() as HttpWebResponse;
        //        //var resStream = new StreamReader(response.GetResponseStream());//取到返回值
        //        //string content = resStream.ReadToEnd();//显示返回值
        //        //resStream.Close();
        //        //resStream.Dispose();
        //        //response.Dispose();
        //        //request.Abort();

        //        //var response1 = new HttpResponseMessage();
        //        //response1.Content = new StringContent(content);
        //        //response1.Headers.Add("Cookie", session);
        //        //response1.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
        //        //return response1;

        //        redirectResponse.Headers.Location = new Uri(appInfo.App_HomeUrl);
        //        redirectResponse.Headers.Add("Cookie", cookie);
        //        //redirectResponse.Content = new StringContent(appInfo.App_HomeUrl);
        //        //redirectResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
        //        return redirectResponse;

        //        //HttpContext.Current.Response.Headers.Add("Cookie", cookie);
        //        //HttpContext.Current.Response.Redirect("appInfo.App_HomeUrl");
        //    }
        //}
        #endregion

        #region 用户方案
        /// <summary>
        /// 增加方案
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public ResultModel AddUserPlan(WJ_T_User userInfo, dynamic requestData)
        {
            ResultModel resultObj = new ResultModel { Success = 0, Code = 0, ErrorMsg = "创建用户方案失败" };
            try
            {
                if (UserPlanService.Instance.AddUserPlan(userInfo.Id, requestData))
                {
                    SetSuccessResult(resultObj);
                }
            }
            catch (Exception ex)
            {
                resultObj.ErrorMsg = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }
            return resultObj;
        }

        /// <summary>
        /// 更新方案
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public ResultModel UpdateUserPlanActivate(WJ_T_User userInfo, dynamic requestData)
        {
            ResultModel resultObj = new ResultModel { Success = 0, Code = 0, ErrorMsg = "更新用户方案失败" };
            try
            {
                int userPlanId = Convert.ToInt32(requestData.UserPlanId);
                if (UserPlanService.Instance.UpdateUserPlanActivate(userInfo.Id, userPlanId))
                {
                    SetSuccessResult(resultObj);
                }
            }
            catch (Exception ex)
            {
                resultObj.ErrorMsg = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }
            return resultObj;
        }

        /// <summary>
        /// 删除方案
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public ResultModel DeleteUserPlan(WJ_T_User userInfo, dynamic requestData)
        {
            ResultModel resultObj = new ResultModel { Success = 0, Code = 0, ErrorMsg = "删除用户方案失败" };
            try
            {
                if (UserPlanService.Instance.DeleteUserPlan(userInfo.Id, requestData))
                {
                    SetSuccessResult(resultObj);
                }
            }
            catch (Exception ex)
            {
                resultObj.ErrorMsg = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }
            return resultObj;
        }

        /// <summary>
        /// 获取方案
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public ResultModel GetUserPlanAndApp(WJ_T_User userInfo, dynamic requestData)
        {
            ResultModel resultObj = new ResultModel { Success = 0, Code = 0, ErrorMsg = "获取用户方案信息失败" };
            try
            {
                var userPlanList = UserPlanService.Instance.GetList(p => p.UserId == userInfo.Id);
                var userPlanIds = userPlanList.Select(p => p.Id).ToArray();
                var userPlanAppList = UserPlanAppService.Instance.GetList<WJ_V_UserPlanApp>(p => userPlanIds.Contains(p.UserPlanId));
                List<dynamic> planList = new List<dynamic>();
                Dictionary<int, List<dynamic>> planAppDict = new Dictionary<int, List<dynamic>>();

                foreach (var item in userPlanAppList)
                {
                    if (!planAppDict.ContainsKey(item.UserPlanId))
                    {
                        planAppDict.Add(item.UserPlanId, new List<dynamic>());
                    }

                    planAppDict[item.UserPlanId].Add(new
                    {
                        UserPlanAppId = item.AppId,
                        UserPlanApp_Name = item.App_Name,
                        UserPlanApp_Image = item.UserPlanApp_Image,
                        UserPlanApp_Location = item.UserPlanApp_Location,
                        UserPlanApp_Sort = item.UserPlanApp_Sort
                    });
                }

                foreach (var plan in userPlanList)
                {
                    planList.Add(new
                    {
                        UserPlanId = plan.Id,
                        UserPlan_Name = plan.UserPlan_Name,
                        UserPlan_Image = plan.UserPlan_Image,
                        UserPlan_Layout = plan.UserPlan_Layout,
                        UserPlan_Activate = plan.UserPlan_Activate,
                        UserPlanAppList = planAppDict[plan.Id]
                    });
                }
                SetSuccessResult(resultObj, planList);
            }
            catch (Exception ex)
            {
                resultObj.ErrorMsg = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }
            return resultObj;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public ResultModel UpdateUsePlan(WJ_T_User userInfo, dynamic requestData)
        {
            ResultModel resultObj = new ResultModel { Success = 0, Code = 0, ErrorMsg = "获取用户方案信息失败" };
            try
            {
                UserPlanService.Instance.UpdateUserPlanActivate(userInfo.Id, requestData);

                SetSuccessResult(resultObj, "");
            }
            catch (Exception ex)
            {
                resultObj.ErrorMsg = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }
            return resultObj;
        }

        #endregion
    }
}