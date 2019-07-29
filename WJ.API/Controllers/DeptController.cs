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
    /// 部门控制器
    /// </summary>
    [ApiAuthorize]
    public class DeptController : ApiBaseController
    {
        #region 获取部门（供下拉列表使用）
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, HttpPost]
        public IHttpActionResult GetDept(JObject data)
        {
            ResultModel resultObj = GetResultInstance();

            try
            {
                var deptList = DeptService.Instance.GetDept();
                SetSuccessResult(resultObj, deptList);
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 获取后台部门列表信息
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
                var deptList = DeptService.Instance.GetDeptList(data, ref total);
                SetSuccessAdminResult(resultObj, total, deptList);
            }
            catch (Exception ex)
            {
                LogHelper.ControllerErrorLog(ex.Message);
                SetFailResult(resultObj, ex.Message);
            }

            return Json<dynamic>(resultObj);
        }
        #endregion

        #region 添加部门
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Add(JObject data)
        {
            ResultModel resultObj = GetResultInstance("添加部门信息失败");

            try
            {
                string errorMsg = "";
                if (DeptService.Instance.Add(data, ref errorMsg))
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

        #region 更新部门
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Update(JObject data)
        {
            ResultModel resultObj = GetResultInstance("更新部门信息失败");

            try
            {
                string errorMsg = "";
                if (DeptService.Instance.Update(data, ref errorMsg))
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

        #region 删除部门
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IHttpActionResult Delete(JObject data)
        {
            ResultModel resultObj = GetResultInstance("删除部门信息失败");

            try
            {
                var primaryList = ConvertStringToIntList(data["Id"].ToString());
                string errorMsg = "";
                if (DeptService.Instance.Delete(primaryList, ref errorMsg))
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
