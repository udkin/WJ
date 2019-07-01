using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("WJ_T_SystemMap")]
    public partial class WJ_T_SystemMap
    {
        public WJ_T_SystemMap()
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
        /// Nullable:True
        /// </summary>
        public string SystemMap_Table { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public string SystemMap_Type { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public string SystemMap_Name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public int SystemMap_Value { get; set; }

        /// <summary>
        /// Desc:排序号
        /// Default:
        /// Nullable:False
        /// </summary>
        public int SystemMap_Sort { get; set; }

        /// <summary>
        /// Desc:职务状态，-1：删除，1：正常
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int SystemMap_State { get; set; }

    }
}
