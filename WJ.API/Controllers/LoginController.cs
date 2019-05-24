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
    public class LoginController : ApiController
    {
        [HttpPost]
        public dynamic Login([FromBody]dynamic request)
        {
            ResponseResult result = new ResponseResult();
            try
            {
                if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                {
                    result.Success = false;
                }
                else
                {
                    string password = UserService.Instance.UserLogin(request.UserName.ToString());
                    if (request.Password == password)
                    {
                        result.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}
