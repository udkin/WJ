using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using WJ.Common;

namespace WJ.API.Models
{
    public class ApiBaseController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthInfo authInfo { set; get; }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            //初始化请求上下文
            base.Initialize(controllerContext);
            authInfo = this.RequestContext.RouteData.Values["access_token"] as AuthInfo;
        }

        public bool IsPropertyExist(dynamic data, string propertyname)
        {
            if (data is JObject)
                return data.Property(propertyname) != null;
            return data.GetType().GetProperty(propertyname) != null;
        }
    }
}