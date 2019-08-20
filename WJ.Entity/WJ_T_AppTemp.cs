using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///系统应用临时信息
    ///</summary>
    [SugarTable("WJ_T_AppTemp")]
    public partial class WJ_T_AppTemp
    {
        public WJ_T_AppTemp()
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
        /// Desc:应用ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? AppId { get; set; }

        /// <summary>
        /// Desc:应用名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string AppTemp_Name { get; set; }

        /// <summary>
        /// Desc:应用图标
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string AppTemp_Icon { get; set; }

        /// <summary>
        /// Desc:应用类型，0：系统应用，1：定制应用
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int AppTemp_Type { get; set; }

        /// <summary>
        /// Desc:应用标识，供前端使用
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? AppTemp_Flag { get; set; }

        /// <summary>
        /// Desc:应用登录URL
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string AppTemp_LoginUrl { get; set; }

        /// <summary>
        /// Desc:应用首页
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string AppTemp_HomeUrl { get; set; }

        /// <summary>
        /// Desc:登录请求方法
        /// Default:Get
        /// Nullable:True
        /// </summary>           
        public string AppTemp_Method { get; set; }

        /// <summary>
        /// Desc:请求参数
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string AppTemp_Paramater { get; set; }

        /// <summary>
        /// Desc:提交HTML表单
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string AppTemp_Form { get; set; }

        /// <summary>
        /// Desc:登录用户名
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string AppTemp_LoginName { get; set; }

        /// <summary>
        /// Desc:登录密码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string AppTemp_Password { get; set; }

        /// <summary>
        /// Desc:浏览器类型，1：IE，2：Chrome
        /// Default:1
        /// Nullable:True
        /// </summary>           
        public int? AppTemp_BrowserType { get; set; }

        /// <summary>
        /// Desc:排序号
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int AppTemp_Sort { get; set; }

        /// <summary>
        /// Desc:创建者ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int AppTemp_Creator { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime AppTemp_CreateTime { get; set; }

        /// <summary>
        /// Desc:操作类型，0：新建，1：修改，2：删除，3：上架，4：下架
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int AppTemp_DataType { get; set; }

        /// <summary>
        /// Desc:临时数据状态，0：待审核，1：审核完成
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int AppTemp_State { get; set; }
    }
}