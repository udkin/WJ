﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WJ.API.Models;
using WJ.Common;
using WJ.Service;

namespace WJ.API.Controllers
{
    /// <summary>
    /// 职务控制器
    /// </summary>
    [ApiAuthorize]
    public class TitleController : ApiBaseController
    {
        #region 获取职务（供下拉列表使用）
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public IHttpActionResult GetTitle(dynamic request)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                var resultData = TitleService.Instance.GetTitle();
                SetSuccessResult(resultObj, resultData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 获取后台职务列表信息
        /// <summary>
        /// 获取后台管理员列表信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetList(JObject data)
        {
            var resultObj = GetSearchResultInstance();

            try
            {
                int total = 0;
                var resultData = TitleService.Instance.GetList(data, ref total);
                SetSearchSuccessResult(resultObj, total, resultData);
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 添加职务
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Add(JObject data)
        {
            ResultModel resultObj = GetResultInstance("添加职务信息失败");

            try
            {
                string errorMsg = "";
                if (TitleService.Instance.Add(data, ref errorMsg))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj, errorMsg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 更新职务
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Update(JObject data)
        {
            ResultModel resultObj = GetResultInstance("更新职务信息失败");

            try
            {
                string errorMsg = "";
                if (TitleService.Instance.Update(data, ref errorMsg))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj, errorMsg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 删除职务
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Delete(JObject data)
        {
            ResultModel resultObj = GetResultInstance("删除职务信息失败");

            try
            {
                var primaryList = ConvertStringToIntList(data["Id"].ToString());
                string errorMsg = "";
                if (TitleService.Instance.Delete(primaryList, ref errorMsg))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj, errorMsg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion
    }
}
