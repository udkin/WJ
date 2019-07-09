using System;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///系统应用表
    ///</summary>
    [SugarTable("WJ_T_App")]
    public partial class WJ_T_App
    {
        public WJ_T_App()
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
        /// Desc:应用分类ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppClassId { get; set; }

        /// <summary>
        /// Desc:应用名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string App_Name { get; set; }

        /// <summary>
        /// Desc:应用部门
        /// Default:
        /// Nullable:False
        /// </summary>
        public string App_DeptName { get; set; }

        /// <summary>
        /// Desc:应用图标
        /// Default:
        /// Nullable:False
        /// </summary>
        public string App_Image { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>
        public DateTime App_CreateTime { get; set; }

        /// <summary>
        /// Desc:操作者ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int App_OperatorId { get; set; }

        /// <summary>
        /// Desc:应用状态，-1：废弃，1：正常
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int App_State { get; set; }
    }
}
