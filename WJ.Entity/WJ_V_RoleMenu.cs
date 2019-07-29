using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("WJ_V_RoleMenu")]
    public partial class WJ_V_RoleMenu
    {
        public WJ_V_RoleMenu()
        {
        }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Menu_Name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Menu_Level { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Menu_Type { set; get; }
    }
}
