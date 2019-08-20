using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WJ.API.Models;
using WJ.Common;
using WJ.Entity;
using WJ.Service;

namespace WJ.API.Controllers
{
    public class AppController : ApiBaseController
    {
        #region 获取应用（供下拉列表使用）
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public IHttpActionResult GetApp(dynamic request)
        {
            ResultModel resultObj = GetResultInstance();
            try
            {
                var resultData = AppService.Instance.GetApp();
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

        #region 获取应用列表信息
        /// <summary>
        /// 获取应用列表信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetList(JObject data)
        {
            var resultObj = GetSearchResultInstance();

            try
            {
                int total = 0;
                var resultData = AppService.Instance.GetList(data, ref total);
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

        #region 获取应用详细信息
        /// <summary>
        /// 获取后台管理员列表信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetAppInfo(JObject data)
        {
            var resultObj = GetResultInstance();

            try
            {
                int appId = data["Id"].ToObject<int>();
                var resultData = AppService.Instance.GetAppInfo(appId);
                SetSuccessResult(resultObj);
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetAllAppClassAndApp(JObject data)
        {
            var resultObj = new ResultModel { Success = 0, Code = 0, ErrorMsg = "获取全部APP列表失败" };
            try
            {
                var appClassList = new DbContext<WJ_T_AppClass>().GetList(p => p.AppClass_State == 1);
                var appList = AppService.Instance.GetList(p => p.App_State == 1);

                foreach (var item in appClassList)
                {
                    item.AppList = appList.Where(p => p.AppClassId == item.Id).ToList();
                }

                SetSuccessResult(resultObj, appClassList);
            }
            catch (Exception ex)
            {
                resultObj.ErrorMsg = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
                LogHelper.DebugLog(ex.Message, LogType.Controller);
            }
            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 添加应用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Add(JObject data)
        {
            ResultModel resultObj = GetResultInstance("添加应用信息失败");

            try
            {
                string errorMsg = "";
                if (AppTempService.Instance.Add(UserInfo.Id, data, ref errorMsg))
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

        #region 更新应用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Update(JObject data)
        {
            var resultObj = GetResultInstance("更新应用信息失败");

            try
            {
                string errorMsg = "";
                if (AppService.Instance.Update(UserInfo.Id, data, ref errorMsg))
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

        #region 删除应用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Delete(JObject data)
        {
            var resultObj = GetResultInstance("申请删除应用失败");

            try
            {
                if (AppService.Instance.Delete(UserInfo.Id, data))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj);
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

        #region 上架应用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult AppUp(JObject data)
        {
            var resultObj = GetResultInstance("申请上架应用失败");

            try
            {
                var appId = data["Id"].ToObject<int>();
                if (AppService.Instance.AppUp(UserInfo.Id, appId))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj);
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

        #region 下架应用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult AppDown(JObject data)
        {
            var resultObj = GetResultInstance("申请下架应用失败");

            try
            {
                var appId = data["Id"].ToObject<int>();
                if (AppService.Instance.AppDown(UserInfo.Id, appId))
                {
                    SetSuccessResult(resultObj);
                }
                else
                {
                    SetFailResult(resultObj);
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
