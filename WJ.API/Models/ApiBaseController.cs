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
    /// <summary>
    /// API基类
    /// </summary>
    [ApiAuthorize]
    public class ApiBaseController : ApiController
    {
        #region 属性
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public WJ_T_User UserInfo
        {
            get
            {
                return ControllerContext.RouteData.Values["UserInfo"] as WJ_T_User;
            }
        }
        #endregion


        protected override void Initialize(HttpControllerContext controllerContext)
        {
            //初始化请求上下文
            base.Initialize(controllerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public bool IsPropertyExist(dynamic data, string propertyname)
        {
            if (data is JObject)
                return data.Property(propertyname) != null;
            return data.GetType().GetProperty(propertyname) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JObject ConvertInputStream()
        {
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "GET")
            {
                Stream stream = HttpContext.Current.Request.InputStream;
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                string input = sr.ReadToEnd();
                return JObject.Parse(input);
            }
            else
            {
                return null;
            }
        }

        #region 反馈结果信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ResultModel GetResultInstance(string msg = "")
        {
            return new ResultModel { Success = 0, Code = 1, ErrorMsg = msg };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public SearchResultModel GetSearchResultInstance(string msg = "")
        {
            return new SearchResultModel { Success = 0, Code = 1, Total = 0, ErrorMsg = msg };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultObj"></param>
        /// <param name="resultData"></param>
        public void SetSuccessResult(ResultModel resultObj, dynamic resultData = null)
        {
            resultObj.Success = 1;
            resultObj.Code = 1;
            resultObj.ErrorMsg = "";
            resultObj.ResultData = resultData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultObj"></param>
        /// <param name="total"></param>
        /// <param name="resultData"></param>
        public void SetSuccessAdminResult(SearchResultModel resultObj, int total, dynamic resultData = null)
        {
            SetSuccessResult(resultObj, resultData);
            resultObj.Total = total;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultObj"></param>
        /// <param name="errorMsg"></param>
        public void SetFailResult(ResultModel resultObj, string errorMsg = "")
        {
            resultObj.Success = 0;
            if (!string.IsNullOrWhiteSpace(errorMsg))
                resultObj.ErrorMsg = errorMsg;
        }
        #endregion

        #region 字符串转换成数字列表
        /// <summary>
        /// 字符串转换成数字列表
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public List<int> ConvertStringToIntList(string content)
        {
            var ids = content.TrimStart(',').TrimEnd(',').Split(',');

            var primaryList = new List<int>();
            foreach (var item in ids)
            {
                primaryList.Add(Convert.ToInt32(item));
            }
            return primaryList;
        }
        #endregion
    }
}