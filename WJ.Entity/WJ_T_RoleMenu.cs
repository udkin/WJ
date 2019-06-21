using System;
using System.Linq;
using System.Text;

namespace WJ.Entity
{
    ///<summary>
    ///角色菜单关系表
    ///</summary>
    public partial class WJ_T_RoleMenu
    {
        public WJ_T_RoleMenu()
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
        public int RoleId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int MenuId { get; set; }
    }
}
