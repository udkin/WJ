using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///操作员所属应用表

    ///</summary>
    [SugarTable("WJ_T_UserApp")]
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
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// Desc:用户ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int UserId { get; set; }

        /// <summary>
        /// Desc:APP分类ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int? AppClassId { get; set; }

        /// <summary>
        /// Desc:APPID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int AppId { get; set; }

        /// <summary>
        /// Desc:登录用户名
        /// Default:‘’
        /// Nullable:False
        /// </summary>           
        public string UserApp_LoginName { get; set; }

        /// <summary>
        /// Desc:密码
        /// Default:‘’
        /// Nullable:False
        /// </summary>           
        public string UserApp_Password { get; set; }

        /// <summary>
        /// Desc:排序
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int? UserApp_Sort { get; set; }

        /// <summary>
        /// Desc:状态，-1：删除，1：正常
        /// Default:1
        /// Nullable:False
        /// </summary>           
        public int UserApp_State { get; set; }
    }
}
