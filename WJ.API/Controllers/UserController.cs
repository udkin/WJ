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
                WJ_T_User userInfo = ControllerContext.RouteData.Values["UserInfo"] as WJ_T_User;

                List<dynamic> menuList = null;
                if (1 == userInfo.Id)
                {
                    menuList = MenuService.Instance.GetSuperAdminMenu();
                }
                else
                {
                    menuList = MenuService.Instance.GetUserRoleMenu(userInfo.Id);
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
                SetSuccessResult(resultObj, UserInfo);
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

        #region 管理员
        #region 获取后台管理员列表信息
        /// <summary>
        /// 获取后台管理员列表信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetManagerList(dynamic request)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                List<WJ_V_User> managerList = null;
                if (IsPropertyExist(request, "page") && IsPropertyExist(request, "limit"))
                {
                    int page = request.page;
                    int limit = request.limit;
                    managerList = UserService.Instance.GetManagerList(page, limit);
                }
                else
                {
                    managerList = UserService.Instance.GetManagerList();
                }
                //result = new { code = 0, success = 0, data = managerList };
                SetSuccessResult(resultObj, managerList);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult AddManager(JObject request)
        {
            ResultModel resultObj = GetResultInstance("添加用户信息失败");
            try
            {
                if (UserService.Instance.IsExits(p => p.User_LoginName == request["loginname"].ToString().Trim() && p.User_State == 1))
                {
                    SetFailResult(resultObj, "存在相同登录用户名");
                }
                else
                {
                    WJ_T_User user = new WJ_T_User();
                    user.User_LoginName = request["loginname"].ToString();
                    user.User_Password = request["password"].ToString();
                    user.DeptId = request["dept"].ToObject<int>();
                    user.TitleId = request["title"].ToObject<int>();
                    user.User_Name = request["username"].ToString();
                    user.User_Head = request["dept"].ToString();
                    user.User_Sex = request["dept"].ToObject<int>();
                    user.User_Phone = request["dept"].ToString();
                    user.User_Type = 1;
                    user.User_CreateTime = DateTime.Now;
                    user.User_State = 1;

                    int roleId = request["role"].ToObject<int>();

                    using (SqlSugar.SqlSugarClient db = DbHelper.GetInstance())
                    {
                        try
                        {
                            db.BeginTran();
                            int userId = UserService.Instance.Add(user, db);

                            if (userId > 0)
                            {
                                WJ_T_UserRole userRole = new WJ_T_UserRole();
                                userRole.UserId = userId;
                                userRole.RoleId = roleId;

                                if (UserRoleService.Instance.Add(userRole, db) > 0)
                                {
                                    db.CommitTran();
                                    SetSuccessResult(resultObj);
                                }
                            }
                            else
                            {
                                db.RollbackTran();
                            }
                        }
                        catch
                        {
                            db.RollbackTran();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
                resultObj.ErrorMsg = ex.Message;
            }

            return Json<dynamic>(resultObj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult UpdateManager(JObject request)
        {
            ResultModel resultObj = GetResultInstance("更新用户信息失败");
            try
            {
                WJ_T_User user = UserService.Instance.GetSingle(p => p.User_LoginName == request["loginname"].ToString().Trim());
                user.User_Password = request["password"].ToString();
                user.DeptId = request["dept"].ToObject<int>();
                user.TitleId = request["title"].ToObject<int>();
                user.User_Name = request["username"].ToString();
                user.User_Head = request["dept"].ToString();
                user.User_Sex = request["dept"].ToObject<int>();
                user.User_Phone = request["dept"].ToString();

                int roleId = request["role"].ToObject<int>();

                using (SqlSugar.SqlSugarClient db = DbHelper.GetInstance())
                {
                    try
                    {
                        db.BeginTran();
                        bool flag = UserService.Instance.Update(user, db);

                        if (flag)
                        {
                            WJ_T_UserRole userRole = new WJ_T_UserRole();
                            userRole.UserId = user.Id;
                            userRole.RoleId = roleId;

                            if (UserRoleService.Instance.Update(userRole, db))
                            {
                                db.CommitTran();
                                SetSuccessResult(resultObj);
                            }
                        }
                        else
                        {
                            db.RollbackTran();
                        }
                    }
                    catch
                    {
                        db.RollbackTran();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
                resultObj.ErrorMsg = ex.Message;
            }

            return Json<dynamic>(resultObj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult DeleteManager(JObject request)
        {
            ResultModel resultObj = GetResultInstance("更新用户信息失败");
            try
            {
                WJ_T_User user = UserService.Instance.GetSingle(p => p.User_LoginName == request["loginname"].ToString().Trim());

                int userId = request["id"].ToObject<int>();
                string loginName = request["loginname"].ToString().Trim();
                if (UserService.Instance.DeleteUser(user.Id))
                {
                    SetSuccessResult(resultObj);
                }
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

        #region 操作用户
        #region 获取操作用户列表信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetUserList()
        {
            ResultModel resultObj = GetResultInstance();
            //dynamic result = new { code = 0, success = 1, msg = "获取用户信息失败" };
            try
            {
                var menuList = UserService.Instance.GetUserList();
                //result = new { code = 0, success = 0, data = menuList };
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

        #region 添加操作用户

        #endregion

        #region 删除操作用户

        #endregion

        #region 修改操作用户
        public bool UpdateUser()
        {
            return true;
        }
        #endregion
        #endregion
    }
}