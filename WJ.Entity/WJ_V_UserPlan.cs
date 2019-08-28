using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJ.Entity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("WJ_V_UserPlan")]
    public partial class WJ_V_UserPlan
    {
        public WJ_V_UserPlan()
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
        public string User_Name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public string UserPlan_Name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>
        public string UserPlan_Icon { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public string UserPlan_Layout { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public int UserPlan_Sort { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public int UserPlan_Activate { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public string UserPlan_ActivateName { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public int UserPlan_State { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public DateTime UserPlan_CreateTime { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public int UserPlan_UseCount { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>
        public DateTime? UserPlan_LastTime { get; set; }
    }
}