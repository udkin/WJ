using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///系统应用数据配置表
    ///</summary>
    [SugarTable("WJ_T_AppConfig")]
    public partial class WJ_T_AppConfig
    {
        public WJ_T_AppConfig()
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
        /// Desc:应用ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// Desc:应用登录URL
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppConfig_LoginUrl { get; set; }

        /// <summary>
        /// Desc:应用首页
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppConfig_HomeUrl { get; set; }

        /// <summary>
        /// Desc:登录请求方法
        /// Default:Get
        /// Nullable:False
        /// </summary>
        public string AppConfig_Method { get; set; }

        /// <summary>
        /// Desc:请求参数
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppConfig_Paramater { get; set; }

        /// <summary>
        /// Desc:请求参数
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppConfig_Form { get; set; }

        /// <summary>
        /// Desc:登录用户名
        /// Default:
        /// Nullable:True
        /// </summary>
        public string AppConfig_LoginName { get; set; }

        /// <summary>
        /// Desc:登录密码
        /// Default:
        /// Nullable:True
        /// </summary>
        public string AppConfig_Password { get; set; }

        /// <summary>
        /// Desc:浏览器类型，1：IE，2：Chrome
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int AppConfig_BrowserType { get; set; }
    }
}