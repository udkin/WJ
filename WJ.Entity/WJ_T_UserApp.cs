using System;
using System.Linq;
using System.Text;

namespace WJ.Entity
{
    ///<summary>
    ///操作员所属应用表

    ///</summary>
    public partial class WJ_T_UserApp
    {
        public WJ_T_UserApp()
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
        public int UserId { get; set; }

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
        public int AppId { get; set; }

    }
}
