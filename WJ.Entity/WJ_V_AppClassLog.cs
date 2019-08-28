using System;
using System.Linq;
using System.Text;

namespace WJ.Entity
{
    ///<summary>
    ///用户角色菜单视图
    ///</summary>
    public partial class WJ_V_AppClassStats
    {
        public WJ_V_AppClassStats()
        {
        }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppClassId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppClass_Name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public int App_Count { get; set; }
    }
}
