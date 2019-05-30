using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WJ.API.Models;
using WJ.DAL;

namespace WJ.API.Controllers
{
    [ApiAuthorizeAttribute]
    public class UserController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetUserRoleMenu()
        {
            dynamic result = new { code = 0, success = 1, msg = "获取菜单异常" };
            try
            {
                AuthInfo authInfo = this.RequestContext.RouteData.Values["access_token"] as AuthInfo;

                if (authInfo == null || authInfo.TokenTimeLimit >= DateTime.Now)
                {
                    result = new { code = 1001 };
                }
                else
                {
                    List<dynamic> menuList = null;
                    if (0 == authInfo.UserId)
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
                LogHelper.Instance.Debuglog(ex.Message, "_Controllers.txtss");
            }

            return Json<dynamic>(result);
        }
    }
}
