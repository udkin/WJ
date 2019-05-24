using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///用户角色关联表
    ///</summary>
    public partial class WJ_T_UserRole
    {
           public WJ_T_UserRole(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int UserId {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int RoleId {get;set;}

    }
}
