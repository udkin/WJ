using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WJ.API.Models;
using WJ.Common;
using WJ.Service;

namespace WJ.API.Controllers
{
    [ApiAuthorize]
    public class UserController : ApiController
    {
        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request">输入POST请求的JSON值{"UserName":"","Password":""}</param>
        /// <returns></returns>
        //[ValidateInput(false)]
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult Login(dynamic request)
        {
            dynamic result = new { code = 0, success = -1, msg = "登录失败" };
            try
            {
                if (!string.IsNullOrWhiteSpace(request.UserName.ToString()) && !string.IsNullOrWhiteSpace(request.Password.ToString()))
                {
                    string userName = request.UserName.ToString().ToLower();
                    string password = request.Password.ToString().ToLower();

                    int userId = UserService.Instance.UserLogin(userName, password);

                    if (userId > -1)
                    {
                        int tokenTimeLimit = int.Parse(ConfigHelper.Instance.WebSiteConfig["TokenTimeLimit"]);

                        AuthInfo authInfo = new AuthInfo()
                        {
                            UserId = userId,
                            IsSuperAdmin = userId == 0,
                            CreateTime = DateTime.Now,
                            TokenTimeLimit = DateTime.Now.AddSeconds(tokenTimeLimit)
                            //,RoleMenu = UserService.Instance.GetUserControllerName(userId)
                        };
                        string token = JWTService.Instance.CreateToken(authInfo);
                        result = new { code = 0, success = 0, data = new { access_token = token } };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.Instance.Debuglog(ex.Message, "_Controllers.txt");
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
                        menuList = UserService.Instance.GetSuperAdminMenu();
                    }
                    else
                    {
                        menuList = UserService.Instance.GetUserRoleMenu(authInfo.UserId);
                    }
                    result = new { code = 0, success = 0, data = menuList };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.Instance.Debuglog(ex.Message, "_Controllers.txt");
            }

            return Json<dynamic>(result);
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取后台管理员列表
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
                    var menuList = UserService.Instance.GetManagerList();
                    result = new { code = 0, success = 0, data = menuList };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.Instance.Debuglog(ex.Message, "_Controllers.txt");
            }

            return Json<dynamic>(result);
        }
        #endregion

        #region 获取后台管理员列表信息
        /// <summary>
        /// 获取后台管理员列表信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetManagerList()
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
                    var menuList = UserService.Instance.GetManagerList();
                    result = new { code = 0, success = 0, data = menuList };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.Instance.Debuglog(ex.Message, "_Controllers.txt");
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
                LogHelper.Instance.Debuglog(ex.Message, "_Controllers.txt");
            }

            return Json<dynamic>(result);
        } 
        #endregion
    }
}
