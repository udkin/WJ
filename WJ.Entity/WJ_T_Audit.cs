using System;
using System.Linq;
using System.Text;

namespace WJ.Entity
{
    ///<summary>
    ///模块审批表
    ///</summary>
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
        public int Id { get; set; }

        /// <summary>
        /// Desc:审批名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Audit_Name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int AppClassId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int AppId { get; set; }

        /// <summary>
        /// Desc:申请人
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int UserId { get; set; }

        /// <summary>
        /// Desc:申请类型，1：创建，2：修改，3：删除，4：上架，5：下架
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Audit_Apply_Type { get; set; }

        /// <summary>
        /// Desc:申请时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime Audit_Apply_Time { get; set; }

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
        /// Desc:审批结果，0：待审批，1：通过
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int Audit_Result { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Audit_Remark { get; set; }

    }
}
