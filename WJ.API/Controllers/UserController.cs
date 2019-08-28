using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using WJ.API.Models;
using WJ.Common;
using WJ.Entity;
using WJ.Service;

namespace WJ.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiAuthorize]
    //[AuthFilter]
    public class UserController : ApiBaseController
    {
        #region 用户登录、退出
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request">输入POST请求的JSON值{"UserName":"admin","Password":"123456"}</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public IHttpActionResult Login(JObject request)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                if (IsPropertyExist(request, "UserName") && IsPropertyExist(request, "Password"))
                {
                    string userName = request["UserName"].ToString().Trim().ToLower();
                    string password = request["Password"].ToString().Trim().ToLower();

                    int userId = UserService.Instance.UserLogin(userName, password);

                    if (userId > -1)
                    {
                        // Token有效期
                        int tokenTimeLimit = SystemMapService.Instance.GetMapValueToInt("TokenTimeLimit");

                        string prefix = SystemMapService.Instance.GetMapValue("TokenPrefix");
                        string token = prefix + Guid.NewGuid().ToString().Replace("-", "");

                        UserService.Instance.UpdateUserToken(userId, token);//更新用户Token

                        #region 保存Token信息
                        WJ_T_Token tokenInfo = new WJ_T_Token();
                        tokenInfo.UserId = userId;
                        tokenInfo.Token_Ip = ((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                        tokenInfo.Token_Value = token;
                        tokenInfo.Token_CreateTime = DateTime.Now;
                        tokenInfo.Token_TimeLimit = DateTime.Now.AddSeconds(tokenTimeLimit);
                        TokenService.Instance.Add(tokenInfo);
                        #endregion

                        SetSuccessResult(resultObj, new { Access_Token = token });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public IHttpActionResult LogOut(JObject request)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                string token = HttpContext.Current.Request.Headers["access_token"];
                if (!string.IsNullOrWhiteSpace(token) && UserService.Instance.LogOut(token))
                {
                    SetSuccessResult(resultObj);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 菜单
        #region 获取用户权限菜单
        /// <summary>
        /// 获取用户权限菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetUserRoleMenu()
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                List<dynamic> menuList = null;
                if (1 == UserInfo.Id)
                {
                    menuList = MenuService.Instance.GetSuperAdminMenu();
                }
                else
                {
                    menuList = MenuService.Instance.GetUserRoleMenu(UserInfo.Id);
                }
                SetSuccessResult(resultObj, menuList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
                resultObj.ErrorMsg = ex.Message;
            }

            return Json<dynamic>(resultObj);
        }
        #endregion
        #endregion

        #region 当前用户操作
        #region 获取当前用户信息
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetUserInfo()
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                SetSuccessResult(resultObj);
                resultObj.ResultData = UserInfo;
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                resultObj.ErrorMsg = ex.Message;
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 修改当前用户密码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult SetUserPassword(JObject data)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                string oldPassword = data["oldPassword"].ToString();
                string password = data["password"].ToString();
                if (UserInfo.User_Password.ToLower() != data["oldPassword"].ToString().Trim().ToLower())
                {
                    SetFailResult(resultObj, "当前密码不正确");
                }
                else
                {
                    string errorMsg = "";
                    if (UserService.Instance.UpdateEx(p => new WJ_T_User() { User_Password = password }, p => p.Id == UserInfo.Id && p.User_Password == oldPassword))
                    {
                        SetSuccessResult(resultObj);
                    }
                    else
                    {
                        SetFailResult(resultObj, errorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                resultObj.ErrorMsg = ex.Message;
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        
        #endregion

        #region 管理员
        #region 获取后台管理员列表信息
        /// <summary>
        /// 获取后台管理员列表信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetManagerList(JObject request)
        {
            var resultObj = GetSearchResultInstance();

            try
            {
                int total = 0;
                List<WJ_V_User> managerList = UserService.Instance.GetManagerList(request, ref total);
                SetSearchSuccessResult(resultObj, total, managerList);
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 添加管理员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult AddManager(JObject data)
        {
            ResultModel resultObj = GetResultInstance("添加管理员信息失败");

            try
            {
                string errorMsg = string.Empty;
                if (UserService.Instance.AddManager(data, ref errorMsg))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj, errorMsg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 更新管理员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult UpdateManager(JObject data)
        {
            ResultModel resultObj = GetResultInstance("更新管理员信息失败");

            try
            {
                string errorMsg = string.Empty;
                if (UserService.Instance.UpdateManager(data, ref errorMsg))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj, errorMsg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 删除管理员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult DeleteManager(JObject data)
        {
            ResultModel resultObj = GetResultInstance("删除管理员信息失败");

            try
            {
                int userId = data["Id"].ToObject<int>();
                string errorMsg = string.Empty;
                if (UserService.Instance.DeleteManager(userId, ref errorMsg))
                {
                    SetSuccessResult(resultObj);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 重置管理员密码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult ResetUserPassword(JObject data)
        {
            ResultModel resultObj = GetResultInstance("密码重置失败");
            try
            {
                int id = data["Id"].ToObject<int>();
                if( UserService.Instance.ResetManagerPassword(id))
                {
                    SetSuccessResult(resultObj);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                resultObj.ErrorMsg = ex.Message;
            }

            return Json<dynamic>(resultObj);
        }
        #endregion
        #endregion

        #region 操作用户
        #region 获取操作用户列表信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetUserList(JObject data)
        {
            var resultObj = GetSearchResultInstance();
            try
            {
                int total = 0;
                var resultData = UserService.Instance.GetUserList(data, ref total);
                SetSearchSuccessResult(resultObj, total, resultData);
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 添加操作用户
        /// <summary>
        /// 添加操作用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult AddUser(JObject data)
        {
            ResultModel resultObj = GetResultInstance("添加用户信息失败");

            try
            {
                string errorMsg = string.Empty;
                if (UserService.Instance.AddUser(data, ref errorMsg))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj, errorMsg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 删除操作用户
        /// <summary>
        /// 删除操作用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult DeleteUser(JObject data)
        {
            ResultModel resultObj = GetResultInstance("删除用户信息失败");

            try
            {
                var primaryList = ConvertStringToIntList(data["Id"].ToString());
                if (UserService.Instance.DeleteUser(primaryList))
                {
                    SetSuccessResult(resultObj);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 修改操作用户
        /// <summary>
        /// 修改操作用户
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult UpdateUser(JObject data)
        {
            ResultModel resultObj = GetResultInstance("更新用户信息失败");

            try
            {
                string errorMsg = string.Empty;
                if (UserService.Instance.UpdateUser(data, ref errorMsg))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj, errorMsg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion
        #endregion
    }
}