using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///菜单表
    ///</summary>
    [SugarTable("WJ_T_Menu")]
    public partial class WJ_T_Menu
    {
        public WJ_T_Menu()
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
        /// Desc:菜单名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Menu_Name { get; set; }

        /// <summary>
        /// Desc:菜单图标
        /// Default:
        /// Nullable:True
        /// </summary>
        public string Menu_Ico { get; set; }

        /// <summary>
        /// Desc:控制器名称
        /// Default:
        /// Nullable:True
        /// </summary>
        public string Menu_Control { get; set; }

        /// <summary>
        /// Desc:菜单地址
        /// Default:
        /// Nullable:True
        /// </summary>
        public string Menu_Url { get; set; }

        /// <summary>
        /// Desc:菜单层级：0101
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Menu_Level { get; set; }

        /// <summary>
        /// Desc:0：目录 ，1：菜单
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int Menu_Type { get; set; }

        /// <summary>
        /// Desc:菜单顺序
        /// Default:
        /// Nullable:False
        /// </summary>
        public int Menu_Sort { get; set; }

        /// <summary>
        /// Desc:菜单可用状态，-1：废弃，1：正常
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int Menu_State { get; set; }
    }
}
