using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace WJ.Entity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("WJ_T_Token")]
    public partial class WJ_T_Token
    {
        public WJ_T_Token()
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
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public string Token_Ip { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public DateTime Token_CreateTime { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        public DateTime Token_TimeLimit { get; set; }

        /// <summary>
        /// Desc:状态，-1：废弃，1：正常
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Token_State { get; set; }
    }
}
