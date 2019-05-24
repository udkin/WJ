﻿using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///角色表
    ///</summary>
    public partial class WJ_T_Role
    {
           public WJ_T_Role(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Id {get;set;}

           /// <summary>
           /// Desc:角色名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Role_Name {get;set;}

           /// <summary>
           /// Desc:操作者
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int Role_Operator {get;set;}

           /// <summary>
           /// Desc:操作时间 
           /// Default:
           /// Nullable:False
           /// </summary>           
           public DateTime Role_OperationTime {get;set;}

           /// <summary>
           /// Desc:角色可用 状态，-1：废弃，1：正常
           /// Default:1
           /// Nullable:False
           /// </summary>           
           public int Role_State {get;set;}

    }
}