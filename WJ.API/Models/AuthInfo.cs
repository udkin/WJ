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
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 角色列表，可以用于记录该用户的角色,相当于claims的概念(如不清楚什么事claim，请google一下基于声明的权限控制)
        /// </summary>
        public List<string> Roles { get; set; }
        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool IsSuperAdmin { set; get; }
    }
}