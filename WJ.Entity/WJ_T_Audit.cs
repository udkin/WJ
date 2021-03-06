﻿using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///模块审批表
    ///</summary>
    [SugarTable("WJ_T_Audit")]
    public partial class WJ_T_Audit
    {
        public WJ_T_Audit()
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
        /// Desc:应用名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string App_Name { get; set; }

        /// <summary>
        /// Desc:应用ID
        /// Default:
        /// Nullable:True
        /// </summary>
        public int? AppId { get; set; }

        /// <summary>
        /// Desc:临时数据ID
        /// Default:
        /// Nullable:True
        /// </summary>
        public int? AppTempId { get; set; }

        /// <summary>
        /// Desc:申请人
        /// Default:
        /// Nullable:False
        /// </summary>
        public int Audit_Applicant { get; set; }

        /// <summary>
        /// Desc:申请时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>
        public DateTime Audit_ApplyTime { get; set; }

        /// <summary>
        /// Desc:申请类型，1：新建，2：修改，3：删除，4：上架，5：下架
        /// </summary>
        public int Audit_Type { get; set; }

        /// <summary>
        /// Desc:审批人
        /// Default:
        /// Nullable:True
        /// </summary>
        public int? Audit_Approver { get; set; }

        /// <summary>
        /// Desc:审核时间
        /// Default:
        /// Nullable:True
        /// </summary>
        public DateTime? Audit_Approval_Time { get; set; }

        /// <summary>
        /// Desc:审批结果，0：待审批，1：通过，2：驳回
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int Audit_State { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:
        /// Nullable:True
        /// </summary>
        public string Audit_Remark { get; set; }
    }
}
