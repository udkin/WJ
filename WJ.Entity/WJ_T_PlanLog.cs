using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///访问日志
    ///</summary>
    [SugarTable("WJ_T_PlanLog")]
    public partial class WJ_T_PlanLog
    {
        public WJ_T_PlanLog()
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
        /// Desc:访问用户ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int PlanLog_UserId { get; set; }

        /// <summary>
        /// Desc:访问用户名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string PlanLog_UserName { get; set; }

        /// <summary>
        /// Desc:访问方案ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int PlanLog_PlanId { get; set; }

        /// <summary>
        /// Desc:访问方案名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string PlanLog_PlanIdName { get; set; }

        /// <summary>
        /// Desc:访问时间
        /// Default:
        /// Nullable:False
        /// </summary>
        public DateTime PlanLog_Time { get; set; }
    }
}
