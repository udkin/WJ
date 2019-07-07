using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class UserController : ApiBaseController
    {
        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request">输入POST请求的JSON值{"UserName":"admin","Password":"123456"}</param>
        /// <returns></returns>
        //[ValidateInput(false)]
        [AllowAnonymous]
        //[AcceptVerbs("GET", "POST")]
        //[HttpPost]
        [HttpGet, HttpPost]
        //public IHttpActionResult Login(dynamic request)
        public IHttpActionResult Login(dynamic request)
        {
            //string userName = GetParamater("userName");
            //string password = GetParamater("password");
            dynamic result = new { code = 0, success = -1, msg = "登录失败" };
            try
            {
                if (IsPropertyExist(request, "UserName") && IsPropertyExist(request, "Password"))
                {
                    string userName = request.UserName.ToString().ToLower();
                    string password = request.Password.ToString().ToLower();

                    int userId = UserService.Instance.UserLogin(userName, password);

                    if (userId > -1)
                    {
                        // Token有效期
                        int tokenTimeLimit = int.Parse(SystemMapService.Instance.GetMapValue("TokenTimeLimit"));

                        AuthInfo authInfo = new AuthInfo()
                        {
                            UserId = userId,
                            IsSuperAdmin = userId == 0,
                            CreateTime = DateTime.Now,
                            TokenTimeLimit = DateTime.Now.AddSeconds(tokenTimeLimit)
                            //,RoleMenu = UserService.Instance.GetUserControllerName(userId)
                        };
                        string token = JWTService.Instance.CreateToken(authInfo);

                        WJ_T_Token tokenInfo = new WJ_T_Token();
                        tokenInfo.UserId = userId;
                        tokenInfo.Token_Ip = ((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                        tokenInfo.Token_CreateTime = authInfo.CreateTime;
                        tokenInfo.Token_TimeLimit = authInfo.TokenTimeLimit;
                        TokenService.Instance.Insert(tokenInfo);

                        result = new { code = 0, success = 0, data = new { access_token = token } };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(result);
        }
        #endregion

        #region 获取用户权限菜单
        /// <summary>
        /// 获取用户权限菜单
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult GetUserRoleMenu()
        {
            dynamic result = new { code = 0, success = 1, msg = "获取菜单失败" };
            try
            {
                AuthInfo authInfo = this.RequestContext.RouteData.Values["access_token"] as AuthInfo;

                if (authInfo == null || DateTime.Now >= authInfo.TokenTimeLimit)
                {
                    result = new { code = 1001 };
                }
                else
                {
                    List<dynamic> menuList = null;
                    if (1 == authInfo.UserId)
                    {
                        menuList = MenuService.Instance.GetSuperAdminMenu();
                    }
                    else
                    {
                        menuList = MenuService.Instance.GetUserRoleMenu(authInfo.UserId);
                    }
                    result = new { code = 0, success = 0, data = menuList };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(result);
        }
        #endregion

        #region 获取当前用户信息
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserInfo()
        {
            dynamic result = new { code = 0, success = 1, msg = "获取用户信息失败" };
            try
            {
                AuthInfo authInfo = this.RequestContext.RouteData.Values["access_token"] as AuthInfo;

                if (authInfo == null || DateTime.Now >= authInfo.TokenTimeLimit)
                {
                    result = new { code = 1001 };
                }
                else
                {
                    var user = UserService.Instance.GetById(authInfo.UserId);
                    result = new { code = 0, success = 0, data = user };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(result);
        }
        #endregion

        #region 获取后台管理员列表信息
        /// <summary>
        /// 获取后台管理员列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetManagerList(dynamic request)
        {
            dynamic result = new { code = 0, success = 1, msg = "获取用户信息失败" };
            try
            {
                AuthInfo authInfo = this.RequestContext.RouteData.Values["access_token"] as AuthInfo;

                if (authInfo == null || DateTime.Now >= authInfo.TokenTimeLimit)
                {
                    result = new { code = 1001 };
                }
                else
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
                    result = new { code = 0, success = 0, data = managerList };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(result);
        }
        #endregion

        #region 获取操作用户列表信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserList()
        {
            dynamic result = new { code = 0, success = 1, msg = "获取用户信息失败" };
            try
            {
                AuthInfo authInfo = this.RequestContext.RouteData.Values["access_token"] as AuthInfo;

                if (authInfo == null || DateTime.Now >= authInfo.TokenTimeLimit)
                {
                    result = new { code = 1001 };
                }
                else
                {
                    var menuList = UserService.Instance.GetUserList();
                    result = new { code = 0, success = 0, data = menuList };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(result);
        }
        #endregion
    }
}