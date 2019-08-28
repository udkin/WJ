using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WJ.API.Models;
using WJ.Common;

namespace WJ.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // 获取网站配置信息到缓存中
            ConfigHelper.FillWebSiteConfig();
            ConfigHelper.FillSqlConfig();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            // 使api返回为json 
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }
    }
}
