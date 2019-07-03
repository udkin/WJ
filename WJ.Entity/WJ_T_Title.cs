using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///职务表
    ///</summary>
    [SugarTable("WJ_T_Title")]
    public partial class WJ_T_Title
    {
        public WJ_T_Title()
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
        /// Desc:职务名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Title_Name { get; set; }

        /// <summary>
        /// Desc:职务全称
        /// Default:
        /// Nullable:True
        /// </summary>
        public string Title_FullName { get; set; }

        /// <summary>
        /// Desc:职务编号
        /// Default:
        /// Nullable:True
        /// </summary>
        public string Title_Code { get; set; }

        /// <summary>
        /// Desc:排序号
        /// Default:
        /// Nullable:False
        /// </summary>
        public int Title_Sort { get; set; }

        /// <summary>
        /// Desc:职务状态，-1：删除，1：正常
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int Title_State { get; set; }
    }
}
