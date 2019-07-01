using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///方案模块布局表
    ///</summary>
    [SugarTable("WJ_T_PlanApp")]
    public partial class WJ_T_PlanApp
    {
        public WJ_T_PlanApp()
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
        /// Desc:方案ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int PlanId { get; set; }

        /// <summary>
        /// Desc:应用ID
        /// Default:
        /// Nullable:False
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// Desc:应用登录用户名
        /// Default:
        /// Nullable:False
        /// </summary>
        public string PlanApp_LoginUser { get; set; }

        /// <summary>
        /// Desc:应用登录密码
        /// Default:
        /// Nullable:False
        /// </summary>
        public string PlanApp_Password { get; set; }

        /// <summary>
        /// Desc:顺序号
        /// Default:
        /// Nullable:False
        /// </summary>
        public int PlanApp_Sort { get; set; }

    }
}
