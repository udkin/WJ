﻿using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///用户表
    ///</summary>
    [SugarTable("WJ_T_User")]
    public partial class WJ_T_User
    {
        public WJ_T_User()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Desc:用户登录名
        /// Default:
        /// Nullable:False
        /// </summary>
        public string User_LoginName { get; set; }

        /// <summary>
        /// Desc:用户密码
        /// Default:
        /// Nullable:False
        /// </summary>
        public string User_Password { get; set; }

        /// <summary>
        /// Desc:姓名
        /// Default:
        /// Nullable:False
        /// </summary>
        public string User_Name { get; set; }

        /// <summary>
        /// Desc:头像
        /// Default:
        /// Nullable:True
        /// </summary>
        public string User_Head { get; set; }

        /// <summary>
        /// Desc:性别，1：男，2：女
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int User_Sex { get; set; }

        /// <summary>
        /// Desc:手机号
        /// Default:
        /// Nullable:True
        /// </summary>
        public string User_Phone { get; set; }

        /// <summary>
        /// Desc:部门
        /// Default:
        /// Nullable:False
        /// </summary>
        public int DeptId { get; set; }

        /// <summary>
        /// Desc:职务
        /// Default:
        /// Nullable:False
        /// </summary>
        public int TitleId { get; set; }

        /// <summary>
        /// Desc:用户类型，1：管理员，2：主屏操作员，3：子屏操作员
        /// Default:2
        /// Nullable:False
        /// </summary>
        public int User_Type { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>
        public DateTime User_CreateTime { get; set; }

        /// <summary>
        /// Desc:用户可用状态，-1：废弃，1：正常
        /// Default:2
        /// Nullable:False
        /// </summary>
        public int User_State { get; set; }

        /// <summary>
        /// Desc:用户Token
        /// Default:
        /// Nullable:True
        /// </summary>
        public string User_Token { get; set; }

    }
}
