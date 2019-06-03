using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WJ.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WJ.Common;
using Models;
using WJ.Service;

namespace WJ.API.Controllers
{
    [ApiAuthorize]
    public class LoginController : ApiController
    {
        public object Token { get; private set; }

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
            dynamic result = new { code = 0, success = 1, msg = "登录失败" };
            try
            {
                if (!string.IsNullOrWhiteSpace(request.UserName.ToString()) && !string.IsNullOrWhiteSpace(request.Password.ToString()))
                {
                    string userName = request.UserName.ToString().ToLower();
                    string password = request.Password.ToString().ToLower();

                    int userId = -1;
                    if ("superadmin" == userName && password == ConfigHelper.Instance.WebSiteConfig["SuperPassword"])
                    {
                        userId = 1;
                    }
                    else
                    {
                        userId = UserService.Instance.UserLogin(userName, password);
                    }

                    if (userId > -1)
                    {
                        int tokenTimeLimit = int.Parse(ConfigHelper.Instance.WebSiteConfig["TokenTimeLimit"]);

                        WJ_T_Token tokenInfo = new WJ_T_Token();
                        tokenInfo.UserId = userId;
                        tokenInfo.Token_Ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                        tokenInfo.Token_CreateTime = DateTime.Now;
                        tokenInfo.Token_TimeLimit = DateTime.Now.AddSeconds(tokenTimeLimit);

                        TokenService.Instance.Add(tokenInfo);

                        AuthInfo authInfo = new AuthInfo()
                        {
                            UserId = userId,
                            IsSuperAdmin = userId == 0,
                            CreateTime = tokenInfo.Token_CreateTime,
                            TokenTimeLimit = tokenInfo.Token_TimeLimit
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
            }

            return Json<dynamic>(result);
        }
    }
}
