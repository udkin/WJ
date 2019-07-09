using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///方案信息表
    ///</summary>
    [SugarTable("WJ_T_UserPlan")]
    public partial class WJ_T_UserPlan
    {
        public WJ_T_UserPlan()
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
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Desc:方案名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string UserPlan_Name { get; set; }

        /// <summary>
        /// Desc:方案图标
        /// Default:
        /// Nullable:False
        /// </summary>
        public string UserPlan_Image { get; set; }

        /// <summary>
        /// Desc:方案布局：（行，列）1,3
        /// Default:
        /// Nullable:False
        /// </summary>
        public string UserPlan_Layout { get; set; }

        /// <summary>
        /// Desc:活动标志，0：不活动，1：活动
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int UserPlan_Activate { get; set; }

        /// <summary>
        /// Desc:方案状态，-1：废弃，1：正常
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int UserPlan_State { get; set; }

        /// <summary>
        /// Desc:方案状态，-1：废弃，1：正常
        /// Default:
        /// Nullable:False
        /// </summary>
        public DateTime UserPlan_CreateTime { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int UserPlan_UseCount { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public DateTime? UserPlan_LastTime { get; set; }
    }
}
