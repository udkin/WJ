using System;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///系统应用数据配置表
    ///</summary>
    [SugarTable("WJ_T_AppData")]
    public partial class WJ_T_AppData
    {
        public WJ_T_AppData()
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
        /// Desc:应用ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// Desc:数据名称
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppData_Name { get; set; }

        /// <summary>
        /// Desc:应用数据获取地址
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppData_Url { get; set; }

        /// <summary>
        /// Desc:数据类型，Json
        /// Default:
        /// Nullable:False
        /// </summary>
        public string AppData_DataType { get; set; }

        /// <summary>
        /// Desc:请求参数值
        /// Default:
        /// Nullable:True
        /// </summary>
        public string AppData_Parameter { get; set; }

        /// <summary>
        /// Desc:登录用户名
        /// Default:
        /// Nullable:True
        /// </summary>
        public string AppData_LoginName { get; set; }

        /// <summary>
        /// Desc:登录密码
        /// Default:
        /// Nullable:True
        /// </summary>
        public string AppData_Password { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>
        public DateTime AppData_CreateTime { get; set; }

        /// <summary>
        /// Desc:操作者ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppData_OperatorId { get; set; }

        /// <summary>
        /// Desc:循环周期时间单位秒
        /// Default:0
        /// Nullable:False
        /// </summary>
        public int AppData_Cycle { get; set; }

        /// <summary>
        /// Desc:报警有效时间范围，单位秒
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppData_AlarmTime { get; set; }

        /// <summary>
        /// Desc:状态，-1：废弃，1：正常
        /// Default:1
        /// Nullable:False
        /// </summary>
        public int AppData_State { get; set; }
    }
}