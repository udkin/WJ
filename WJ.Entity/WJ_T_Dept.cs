using System;
using System.Linq;
using System.Text;

namespace Models
{
    ///<summary>
    ///部门表
    ///</summary>
    public partial class WJ_T_Dept
    {
           public WJ_T_Dept(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int iD {get;set;}

           /// <summary>
           /// Desc:部门名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Dept_Name {get;set;}

           /// <summary>
           /// Desc:部门全称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Dept_FullName {get;set;}

           /// <summary>
           /// Desc:部门图标
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Dept_Icon {get;set;}

           /// <summary>
           /// Desc:部门编号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Dept_Code {get;set;}

           /// <summary>
           /// Desc:部门级别，0101
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string Dept_Lever {get;set;}

           /// <summary>
           /// Desc:排序号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? Dept_SortNo {get;set;}

           /// <summary>
           /// Desc:部门状态，-1：删除，1：正常
           /// Default:1
           /// Nullable:True
           /// </summary>           
           public int? Dept_State {get;set;}

    }
}
