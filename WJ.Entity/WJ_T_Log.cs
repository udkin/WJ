using System;
using System.Linq;
using System.Text;

namespace WJ.Entity
{
    ///<summary>
    ///日志表
    ///</summary>
    public partial class WJ_T_Log
    {
        public WJ_T_Log()
        {
        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Id { get; set; }

        /// <summary>
        /// Desc:操作人
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Log_UserId { get; set; }

        /// <summary>
        /// Desc:操作人姓名
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Log_UserName { get; set; }

        /// <summary>
        /// Desc:应用分类ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Log_AppClassId { get; set; }

        /// <summary>
        /// Desc:应用分类名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Log_AppClassName { get; set; }

        /// <summary>
        /// Desc:操作应用ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Log_AppId { get; set; }

        /// <summary>
        /// Desc:操作应用名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Log_App { get; set; }

        /// <summary>
        /// Desc:操作描述
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Log_Describe { get; set; }

        /// <summary>
        /// Desc:操作时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime Log_Time { get; set; }

    }
}
