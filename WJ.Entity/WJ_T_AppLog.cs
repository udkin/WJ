using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///日志表
    ///</summary>
    [SugarTable("WJ_T_AppLog")]
    public partial class WJ_T_AppLog
    {
        public WJ_T_AppLog()
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
        /// Desc:操作人
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppLog_UserId { get; set; }

        /// <summary>
        /// Desc:操作人姓名
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppLog_UserName { get; set; }

        /// <summary>
        /// Desc:操作用户类型名称
        /// Default:
        /// Nullable:True
        /// </summary>
        public string AppLog_UserTypeName { get; set; }

        /// <summary>
        /// Desc:应用分类ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppLog_AppClassId { get; set; }

        /// <summary>
        /// Desc:应用分类名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppLog_AppClassName { get; set; }

        /// <summary>
        /// Desc:操作应用ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppLog_AppId { get; set; }

        /// <summary>
        /// Desc:操作应用名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppLog_AppName { get; set; }

        /// <summary>
        /// Desc:登录账户
        /// Default:
        /// Nullable:True
        /// </summary>
        public string AppLog_LoginName { get; set; }

        /// <summary>
        /// Desc:密码
        /// Default:
        /// Nullable:True
        /// </summary>
        public string AppLog_Password { get; set; }

        /// <summary>
        /// Desc:操作描述
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppLog_Describe { get; set; }

        /// <summary>
        /// Desc:操作时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>
        public DateTime AppLog_Time { get; set; }
    }
}
