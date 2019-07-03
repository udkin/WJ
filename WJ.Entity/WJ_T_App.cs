using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///系统应用表
    ///</summary>
    [SugarTable("WJ_T_App")]
    public partial class WJ_T_App
    {
        public WJ_T_App()
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
        /// Desc:应用分类ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppClassId { get; set; }

        /// <summary>
        /// Desc:应用名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string App_Name { get; set; }

        /// <summary>
        /// Desc:应用部门
        /// Default:
        /// Nullable:False
        /// </summary>
        public string App_DeptName { get; set; }

        /// <summary>
        /// Desc:应用图标
        /// Default:
        /// Nullable:False
        /// </summary>
        public string App_Image { get; set; }

        /// <summary>
        /// Desc:应用登录URL
        /// Default:
        /// Nullable:False
        /// </summary>
        public string App_LoginUrl { get; set; }

        /// <summary>
        /// Desc:应用首页
        /// Default:
        /// Nullable:False
        /// </summary>
        public string App_HomeUrl { get; set; }

        /// <summary>
        /// Desc:
        /// Default:Get
        /// Nullable:False
        /// </summary>
        public string App_Method { get; set; }

        /// <summary>
        /// Desc:登录用户名
        /// Default:
        /// Nullable:True
        /// </summary>
        public string App_LoginName { get; set; }

        /// <summary>
        /// Desc:登录密码
        /// Default:
        /// Nullable:True
        /// </summary>
        public string App_Password { get; set; }

        /// <summary>
        /// Desc:浏览器类型，1：IE，2：Chrome
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int App_BrowserType { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>
        public DateTime App_CreateTime { get; set; }

        /// <summary>
        /// Desc:操作者ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int App_OperatorId { get; set; }

        /// <summary>
        /// Desc:应用状态，-1：废弃，1：正常
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int App_State { get; set; }

    }
}
