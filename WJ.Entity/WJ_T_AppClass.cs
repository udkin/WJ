using System;
using System.Linq;
using System.Text;
using SqlSugar;
using System.Collections.Generic;

namespace WJ.Entity
{
    ///<summary>
    ///应用分类表
    ///</summary>
    [SugarTable("WJ_T_AppClass")]
    public partial class WJ_T_AppClass
    {
        public WJ_T_AppClass()
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
        /// Desc:模块名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppClass_Name { get; set; }

        /// <summary>
        /// Desc:模块图标
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppClass_Icon { get; set; }

        /// <summary>
        /// Desc:排序号
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppClass_Sort { get; set; }

        /// <summary>
        /// Desc:创建者
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppClass_Creator { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:False
        /// </summary>
        public DateTime AppClass_CreateTime { get; set; }

        /// <summary>
        /// Desc:模块状态，-1：废弃，1：启用
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int AppClass_State { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppClass_Remark { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:
        /// Nullable:False
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<WJ_T_App> AppList { get; set; }
    }

}
