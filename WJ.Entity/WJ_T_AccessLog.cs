using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///访问日志
    ///</summary>
    public partial class WJ_T_AccessLog
    {
        public WJ_T_AccessLog()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Id { get; set; }

        /// <summary>
        /// Desc:访问用户ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int AccessLog_UserId { get; set; }

        /// <summary>
        /// Desc:访问用户名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string AccessLog_UserName { get; set; }

        /// <summary>
        /// Desc:访问用户角色ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int AccessLog_UserRoleId { get; set; }

        /// <summary>
        /// Desc:访问用户角色名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string AccessLog_UserRoleName { get; set; }

        /// <summary>
        /// Desc:访问用户类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? AccessLog_UserType { get; set; }

        /// <summary>
        /// Desc:访问时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime AccessLog_Time { get; set; }

        /// <summary>
        /// Desc:访问方案ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int AccessLog_PlanId { get; set; }

        /// <summary>
        /// Desc:访问应用分类ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int AccessLog_AppClassId { get; set; }

        /// <summary>
        /// Desc:访问应用ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int AccessLog_AppId { get; set; }

    }
}
