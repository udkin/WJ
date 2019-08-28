using Newtonsoft.Json.Linq;
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
    /// 
    /// </summary>
    public class AppClassController : ApiBaseController
    {
        #region 获取应用分类（供下拉列表使用）
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public IHttpActionResult GetAppClass(dynamic request)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                var resultData = AppClassService.Instance.GetAppClass();
                SetSuccessResult(resultObj, resultData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.ControllerErrorLog(ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 获取后台应用分类列表信息
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
                var resultData = AppClassService.Instance.GetList(data, ref total);
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

        #region 添加应用分类
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Add(JObject data)
        {
            ResultModel resultObj = GetResultInstance("添加应用分类信息失败");

            try
            {
                string errorMsg = "";
                if (AppClassService.Instance.Add(UserInfo.Id, data, ref errorMsg))
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

        #region 更新应用分类
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Update(JObject data)
        {
            ResultModel resultObj = GetResultInstance("更新应用分类信息失败");

            try
            {
                string errorMsg = "";
                if (AppClassService.Instance.Update(data, ref errorMsg))
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

        #region 删除应用分类
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Delete(JObject data)
        {
            ResultModel resultObj = GetResultInstance("删除应用分类信息失败");

            try
            {
                var primaryList = ConvertStringToIntList(data["Id"].ToString());
                string errorMsg = "";
                if (AppClassService.Instance.Delete(primaryList, ref errorMsg))
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
