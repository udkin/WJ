using System;
using System.Linq;
using System.Text;

namespace WJ.Entity
{
    ///<summary>
    ///方案信息表
    ///</summary>
    public partial class WJ_T_Plan
    {
        public WJ_T_Plan()
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
        /// Desc:方案名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Plan_Name { get; set; }

        /// <summary>
        /// Desc:方案布局：（行，列）1,3
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Plan_Ranks { get; set; }

        /// <summary>
        /// Desc:方案状态，-1：废弃，1：正常
        /// Default:1
        /// Nullable:False
        /// </summary>           
        public int Plan_State { get; set; }

    }
}
