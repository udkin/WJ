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
    public class AuditController : ApiBaseController
    {
        #region 获取待审核信息列表
        /// <summary>
        /// 获取待审核信息列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetUnAuditList(JObject data)
        {
            var resultObj = GetSearchResultInstance();

            try
            {
                int total = 0;
                var resultData = AuditService.Instance.GetUnAuditList(data, ref total);
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

        #region 获取已审核信息列表
        /// <summary>
        /// 获取已审核信息列表
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult GetAuditList(JObject data)
        {
            var resultObj = GetSearchResultInstance();

            try
            {
                int total = 0;
                var resultData = AuditService.Instance.GetAuditList(data, ref total);
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

        #region 通过应用审核信息
        /// <summary>
        /// 通过应用审核信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult PassApp(JObject data)
        {
            var resultObj = GetResultInstance("通过审核失败");

            try
            {
                int auditId = data["Id"].ToObject<int>();
                if(AuditService.Instance.PassAudit(UserInfo.Id, auditId))
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

        #region 驳回应用审核信息
        /// <summary>
        /// 驳回应用审核信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult RejectApp(JObject data)
        {
            var resultObj = GetResultInstance("驳回审核失败");

            try
            {
                int auditId = data["Id"].ToObject<int>();
                if (AuditService.Instance.RejectAudit(UserInfo.Id, auditId))
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
