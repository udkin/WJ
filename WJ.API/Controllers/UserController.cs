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
    //[AuthFilter]
    public class UserController : ApiBaseController
    {
        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request">输入POST请求的JSON值{"UserName":"admin","Password":"123456"}</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public IHttpActionResult Login([FromBody]dynamic request)
        {
            OPSResultData resultObj = new OPSResultData { success = 0, code = 0, msg = "" };
            //dynamic result = new { code = 0, success = -1, msg = "登录失败" };
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

                        SetSuccessOpsResult(resultObj, new { Access_Token = token });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
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
            OPSResultData resultObj = new OPSResultData { success = 1, code = 1, msg = "" };
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
                SetSuccessOpsResult(resultObj, menuList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
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
            OPSResultData resultObj = new OPSResultData { success = 1, code = 1, msg = "" };
            //dynamic result = new { code = 0, success = 1, msg = "获取用户信息失败" };
            try
            {
                WJ_T_User userInfo = ControllerContext.RouteData.Values["UserInfo"] as WJ_T_User;
                SetSuccessOpsResult(resultObj, userInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 获取后台管理员列表信息
        /// <summary>
        /// 获取后台管理员列表信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetManagerList([FromBody]dynamic request)
        {
            OPSResultData resultObj = new OPSResultData { success = 1, code = 1, msg = "" };
            //dynamic result = new { code = 0, success = 1, msg = "获取用户信息失败" };
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
                SetSuccessOpsResult(resultObj, managerList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 获取操作用户列表信息
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetUserList()
        {
            OPSResultData resultObj = new OPSResultData { success = 1, code = 1, msg = "" };
            //dynamic result = new { code = 0, success = 1, msg = "获取用户信息失败" };
            try
            {
                var menuList = UserService.Instance.GetUserList();
                //result = new { code = 0, success = 0, data = menuList };
                SetSuccessOpsResult(resultObj, menuList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion
    }
}