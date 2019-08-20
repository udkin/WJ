using System;
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
        /// Desc:应用图标
        /// Default:
        /// Nullable:False
        /// </summary>
        public string App_Icon { get; set; }

        /// <summary>
        /// Desc:应用类型，0：系统应用，1：定制应用
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int App_Type { get; set; }

        /// <summary>
        /// Desc:应用标识，供前端使用
        /// Default:
        /// Nullable:True
        /// </summary>
        public int? App_Flag { get; set; }

        /// <summary>
        /// Desc:应用登录URL
        /// Default:
        /// Nullable:True
        /// </summary>
        public string App_LoginUrl { get; set; }

        /// <summary>
        /// Desc:应用首页
        /// Default:
        /// Nullable:True
        /// </summary>
        public string App_HomeUrl { get; set; }

        /// <summary>
        /// Desc:登录请求方法
        /// Default:Get
        /// Nullable:True
        /// </summary>
        public string App_Method { get; set; }

        /// <summary>
        /// Desc:请求参数
        /// Default:
        /// Nullable:True
        /// </summary>
        public string App_Paramater { get; set; }

        /// <summary>
        /// Desc:提交HTML表单
        /// Default:
        /// Nullable:True
        /// </summary>
        public string App_Form { get; set; }

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
        /// Nullable:True
        /// </summary>
        public int? App_BrowserType { get; set; }

        /// <summary>
        /// Desc:排序号
        /// Default:
        /// Nullable:False
        /// </summary>
        public int App_Sort { get; set; }

        /// <summary>
        /// Desc:创建者ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int App_Creator { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>
        public DateTime App_CreateTime { get; set; }

        /// <summary>
        /// Desc:应用审核状态，0：待审核，1：审核
        /// </summary>
        public int App_AuditState { get; set; }

        /// <summary>
        /// Desc:应用状态，10：已新建，20：已维护，30：已删除，40：已上架，50：已下架
        /// </summary>
        public int App_State { get; set; }
    }
}
