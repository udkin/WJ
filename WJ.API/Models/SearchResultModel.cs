using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WJ.API.Models
{
    /// <summary>
    /// 外网接口返回结果类
    /// </summary>
    public class SearchResultModel : ResultModel
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { set; get; }
    }
}