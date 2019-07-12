using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using WJ.Common;
using WJ.Entity;

namespace WJ.API.Models
{
    public class ApiBaseController : ApiController
    {
        #region 属性
        private WJ_T_User _userInfo= null;
        /// <summary>
        /// 
        /// </summary>
        public WJ_T_User UserInfo
        {
            get
            {
                if (_userInfo == null)
                    lock ("GetUser")
                        if (_userInfo == null)
                            _userInfo = ControllerContext.RouteData.Values["UserInfo"] as WJ_T_User;
                return _userInfo;
            }
        }
        #endregion


        protected override void Initialize(HttpControllerContext controllerContext)
        {
            //初始化请求上下文
            base.Initialize(controllerContext);

            //UserInfo = this.RequestContext.RouteData.Values["UserInfo"] as WJ_T_User;
        }

        public bool IsPropertyExist(dynamic data, string propertyname)
        {
            if (data is JObject)
                return data.Property(propertyname) != null;
            return data.GetType().GetProperty(propertyname) != null;
        }

        public dynamic ConvertInputStream()
        {
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "GET")
            {
                Stream stream = HttpContext.Current.Request.InputStream;
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                string input = sr.ReadToEnd();
                return JsonConvert.DeserializeObject(input);
            }
            else
            {
                return null;
            }
        }

        public ResultModel GetResultInstance(string msg = "")
        {
            return new ResultModel { Success = 0, Code = 0, ErrorMsg = msg };
        }


        public void SetSuccessResult(ResultModel resultObj, dynamic resultData = null)
        {
            resultObj.Success = 1;
            resultObj.Code = 1;
            resultObj.ErrorMsg = "";
            resultObj.ResultData = resultData;
        }

        public void SetFailResult(ResultModel resultObj, string errorMsg = "")
        {
            resultObj.Success = 0;
            resultObj.Code = 1;
            resultObj.ErrorMsg = errorMsg;
        }
    }
}