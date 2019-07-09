using System;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///方案模块APP列表
    ///</summary>
    [SugarTable("WJ_T_UserPlanApp")]
    public partial class WJ_T_UserPlanApp
    {
        public WJ_T_UserPlanApp()
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
        /// Desc:方案ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int UserPlanId { get; set; }

        /// <summary>
        /// Desc:应用ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// Desc:应用位置号，0_0，0_1
        /// Default:
        /// Nullable:False
        /// </summary>
        public string UserPlanApp_Location { get; set; }

        /// <summary>
        /// Desc:状态，-1：删除，1：正常
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int UserPlanApp_State { get; set; }
    }
}
