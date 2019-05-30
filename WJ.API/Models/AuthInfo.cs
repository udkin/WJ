using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WJ.API.Models
{
    /// <summary>
    /// 用户授权信息
    /// </summary>
    public class AuthInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserId { set; get; }
        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsSuperAdmin { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime TokenTimeLimit { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> RoleMenu { set; get; }
    }
}