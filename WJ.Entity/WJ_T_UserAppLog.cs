using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///使用APP日志
    ///</summary>
    [SugarTable("WJ_T_UserAppLog")]
    public partial class WJ_T_UserAppLog
    {
        public WJ_T_UserAppLog()
        {
        }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
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
        public int AppId { get; set; }

        /// <summary>
        /// Desc:使用次数
        /// Default:
        /// Nullable:False
        /// </summary>
        public int UserAppLog_UseCount { get; set; }

        /// <summary>
        /// Desc:最后一次使用时间
        /// Default:DateTime.Now
        /// Nullable:True
        /// </summary>
        public DateTime? UserAppLog_LastTime { get; set; }
    }
}
