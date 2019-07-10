using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WJ.API.Models
{
    public class OPSResultData
    {
        /// <summary>
        /// 成功标志，0：失败，1：成功
        /// </summary>
        public int success { set; get; }

        /// <summary>
        /// ，0：失败，1：成功
        /// </summary>
        public int code { set; get; }

        /// <summary>
        /// 返回JSON格式数据
        /// </summary>
        public dynamic data { set; get; }

        /// <summary>
        /// 异常信息，成功为空
        /// </summary>
        public string msg { set; get; }
    }
}