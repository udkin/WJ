﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using WJ.API.Models;

namespace WJ.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            #region 跨域配置
            //跨域配置
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            //var allowOrigins = ConfigurationManager.AppSettings["cors_allowOrigins"];
            //var allowHeaders = ConfigurationManager.AppSettings["cors_allowHeaders"];
            //var allowMethods = ConfigurationManager.AppSettings["cors_allowMethods"];
            //var globalCors = new EnableCorsAttribute(allowOrigins, allowHeaders, allowMethods);
            //config.EnableCors(globalCors); 
            #endregion

            //config.ParameterBindingRules.Insert(0, x =>
            //{
            //    if (x.ParameterType == typeof(dynamic))
            //    {
            //        return new FromUriOrBodyParameterBinding(x);
            //    }
            //    return null;
            //});

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
