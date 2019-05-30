﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using WJ.DAL;

namespace WJ.API.Models
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        //重写基类的验证方式，加入我们自定义的Ticket验证
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //前端请求api时会将token存放在名为"auth"的请求头中
            var authHeader = from h in actionContext.Request.Headers where h.Key == "access_token" select h.Value.FirstOrDefault();
            if (authHeader != null && string.IsNullOrWhiteSpace(authHeader.FirstOrDefault()) == false)
            {
                string token = authHeader.FirstOrDefault();
                //对token进行解密
                AuthInfo authInfo = JWTService.Instance.DecodeToken(token);
                if (authInfo != null)
                {
                    if (0 == authInfo.UserId)
                    {
                        actionContext.RequestContext.RouteData.Values.Add("access_token", authInfo);
                        base.IsAuthorized(actionContext);
                    }
                    else
                    {
                        string controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName.ToLower();
                        List<string> controllerList = UserService.Instance.GetUserControllerName(authInfo.UserId);

                        // 有访问控制器权继续处理，否则返回401
                        if (controllerList.Contains(controllerName))
                        {
                            actionContext.RequestContext.RouteData.Values.Add("access_token", authInfo);
                            base.IsAuthorized(actionContext);
                        }
                        else
                        {
                            HandleUnauthorizedRequest(actionContext);
                        }
                    }
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                // 允许匿名访问继续访问
                if (isAnonymous)
                {
                    base.OnAuthorization(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
        }

        /// <summary>
        /// 重写实现处理授权失败时返回json，用于前端无权限跳转首页使用
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            // 403
            response.StatusCode = HttpStatusCode.Forbidden;
            response.Content = new StringContent(JsonConvert.SerializeObject(new { code = 1001 }), Encoding.UTF8, "application/json");
        }

        #region MyRegion
        //protected override bool IsAuthorized(HttpActionContext actionContext)
        //{
        //    //前端请求api时会将token存放在名为"auth"的请求头中
        //    var authHeader = from h in actionContext.Request.Headers where h.Key == "access_token" select h.Value.FirstOrDefault();
        //    if (authHeader != null)
        //    {
        //        string token = authHeader.FirstOrDefault();
        //        if (!string.IsNullOrEmpty(token))
        //        {
        //            try
        //            {
        //                //对token进行解析获取用户授权信息
        //                AuthInfo authInfo = JWTService.Instance.DecodeToken(token);
        //                if (authInfo != null)
        //                {
        //                    //将用户信息存放起来，供后续调用
        //                    actionContext.RequestContext.RouteData.Values.Add("access_token", authInfo);
        //                }
        //            }
        //            catch
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return true;
        //} 
        #endregion
    }
}