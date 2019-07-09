using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WJ.API.Models
{
    /// <summary>
    /// 外网接口返回结果类
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 成功标志，0：失败，1：成功
        /// </summary>
        public int Success { set; get; }

        /// <summary>
        /// ，0：失败，1：成功
        /// </summary>
        public int Code { set; get; }

        /// <summary>
        /// 返回JSON格式数据
        /// </summary>
        public dynamic ResultData { set; get; }

        /// <summary>
        /// 异常信息，成功为空
        /// </summary>
        public string ErrorMsg { set; get; }
    }
}