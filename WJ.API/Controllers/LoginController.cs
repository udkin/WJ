using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WJ.API.Models;
using WJ.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WJ.API.Controllers
{
    [Authorize]
    public class LoginController : ApiController
    {
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
                    if ("superadmin" == userName && password == WebSiteConfigService.Instance.WebSiteConfig["SuperPassword"])
                    {
                        userId = 0;
                    }
                    else
                    {
                        userId = UserService.Instance.UserLogin(userName, password);
                    }

                    if (userId > -1)
                    {
                        int tokenTimeLimit = int.Parse(WebSiteConfigService.Instance.WebSiteConfig["TokenTimeLimit"]);
                        
                        AuthInfo authInfo = new AuthInfo()
                        {
                            UserId = userId
                            ,IsSuperAdmin = userId == 0
                            ,CreateTime = DateTime.Now
                            ,TokenTimeLimit = DateTime.Now.AddSeconds(tokenTimeLimit)
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

        //[AllowAnonymous]
        //[HttpGet]
        //public IHttpActionResult Loginn(string userName, string password)
        //{
        //    dynamic result = new { code = 0, success = 1, msg = "登录失败" };
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(password))
        //        {
        //            int userId = UserService.Instance.UserLogin(userName, password);
        //            if (userId > -1)
        //            {
        //                AuthInfo authInfo = new AuthInfo()
        //                {
        //                    UserId = userId
        //                    ,IsSuperAdmin = userName == "SuperAdmin"
        //                    ,CreateTime = DateTime.Now
        //                    ,TimeOut = DateTime.Now.AddSeconds(7200)
        //                };
        //                string token = JWTService.Instance.CreateToken(authInfo);
        //                result = new { code = 0, success = 0, data = new { access_token = token } };
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    return Json<dynamic>(result);
        //}
    }
}
