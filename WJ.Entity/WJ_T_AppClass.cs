using System;
using System.Linq;
using System.Text;
using SqlSugar;

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
        public string AppClass_Image { get; set; }

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
        /// Desc:操作者
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppClass_Operator { get; set; }

        /// <summary>
        /// Desc:操作时间 
        /// Default:
        /// Nullable:False
        /// </summary>
        public DateTime AppClass_OperationTime { get; set; }

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

    }
}
